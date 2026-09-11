namespace Vortexia.Tests
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using Vortexia.Core;
    using Vortexia.Core.Commands;
    using Vortexia.Core.Events;
    using Xunit;

    /// <summary>
    /// Tests for the Master Controller.
    /// </summary>
    public class MasterControllerTests
    {
        private readonly Mock<ILogger<MasterController>> _loggerMock;
        private readonly IEventBus _eventBus;
        private readonly MasterController _controller;

        public MasterControllerTests()
        {
            _loggerMock = new Mock<ILogger<MasterController>>();
            _eventBus = new EventBus();
            _controller = new MasterController(_loggerMock.Object, _eventBus);
        }

        [Fact]
        public void RegisterCommand_WithValidCommand_ShouldRegister()
        {
            // Arrange
            var command = new TestCommand();

            // Act
            _controller.RegisterCommand(command);

            // Assert
            var commands = _controller.GetAvailableCommands();
            Assert.Contains(commands, c => c.Name == "TestCommand");
        }

        [Fact]
        public void RegisterCommand_WithNullCommand_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _controller.RegisterCommand(null!));
        }

        [Fact]
        public void GetAvailableCommands_ShouldReturnRegisteredCommands()
        {
            // Arrange
            var command1 = new TestCommand();
            var command2 = new TestCommand { Name = "TestCommand2" };
            _controller.RegisterCommand(command1);
            _controller.RegisterCommand(command2);

            // Act
            var commands = _controller.GetAvailableCommands();

            // Assert
            Assert.Equal(2, commands.Count());
        }

        [Fact]
        public void GetCommandsByCategory_ShouldReturnFilteredCommands()
        {
            // Arrange
            var command = new TestCommand();
            _controller.RegisterCommand(command);

            // Act
            var commands = _controller.GetCommandsByCategory("General");

            // Assert
            Assert.Single(commands);
            Assert.Equal("TestCommand", commands.First().Name);
        }

        [Fact]
        public async Task ExecuteCommandAsync_WithValidCommand_ShouldSucceed()
        {
            // Arrange
            var command = new TestCommand();
            _controller.RegisterCommand(command);

            // Act
            var result = await _controller.ExecuteCommandAsync(command);

            // Assert
            Assert.Equal(CommandStatus.Success, result.Status);
        }

        [Fact]
        public async Task ExecuteCommandAsync_WithNullCommand_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.ExecuteCommandAsync(null!));
        }

        [Fact]
        public async Task ExecuteCommandAsync_ShouldRecordExecutionInHistory()
        {
            // Arrange
            var command = new TestCommand();

            // Act
            await _controller.ExecuteCommandAsync(command);

            // Assert
            var history = _controller.ExecutionHistory;
            Assert.Single(history);
            Assert.Equal("TestCommand", history.First().CommandName);
            Assert.Equal(CommandStatus.Success, history.First().Result.Status);
        }

        [Fact]
        public async Task ExecuteCommandAsync_WithFailingCommand_ShouldRecordFailure()
        {
            // Arrange
            var command = new FailingTestCommand();

            // Act
            var result = await _controller.ExecuteCommandAsync(command);

            // Assert
            Assert.Equal(CommandStatus.Failed, result.Status);
            Assert.NotNull(result.Exception);
        }

        [Fact]
        public void UnregisterCommand_WithRegisteredCommand_ShouldUnregister()
        {
            // Arrange
            var command = new TestCommand();
            _controller.RegisterCommand(command);

            // Act
            var result = _controller.UnregisterCommand("TestCommand");

            // Assert
            Assert.True(result);
            Assert.Empty(_controller.GetAvailableCommands());
        }

        [Fact]
        public void UnregisterCommand_WithUnregisteredCommand_ShouldReturnFalse()
        {
            // Act
            var result = _controller.UnregisterCommand("NonExistentCommand");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EventBus_ShouldPublishCommandStartedEvent()
        {
            // Arrange
            var eventRaised = false;
            var listener = new TestEventListener(() => eventRaised = true);
            _eventBus.Subscribe<CommandStartedEvent>(listener);
            var command = new TestCommand();

            // Act
            await _controller.ExecuteCommandAsync(command);

            // Assert
            Assert.True(eventRaised);
        }
    }

    /// <summary>
    /// Test implementation of a command.
    /// </summary>
    internal class TestCommand : CommandBase
    {
        public override string Name => "TestCommand";
        public override string Description => "A test command";
        public override string Category => "General";

        public override async Task<CommandResult> ExecuteAsync()
        {
            await Task.Delay(10);
            return CommandResult.Success("Test command executed successfully");
        }
    }

    /// <summary>
    /// Test implementation of a failing command.
    /// </summary>
    internal class FailingTestCommand : CommandBase
    {
        public override string Name => "FailingTestCommand";
        public override string Description => "A test command that fails";

        public override async Task<CommandResult> ExecuteAsync()
        {
            await Task.Delay(10);
            throw new InvalidOperationException("This command is designed to fail");
        }
    }

    /// <summary>
    /// Test event listener.
    /// </summary>
    internal class TestEventListener : IEventListener
    {
        private readonly Action _callback;

        public TestEventListener(Action callback)
        {
            _callback = callback;
        }

        public Task OnEventAsync(IEvent @event)
        {
            _callback();
            return Task.CompletedTask;
        }
    }
}
