namespace Vortexia.Core
{
    using Microsoft.Extensions.Logging;
    using Vortexia.Core.Commands;
    using Vortexia.Core.Events;

    /// <summary>
    /// Defines the interface for the Master Controller.
    /// </summary>
    public interface IMasterController
    {
        /// <summary>
        /// Executes a command asynchronously.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>A task that represents the asynchronous command execution.</returns>
        Task<CommandResult> ExecuteCommandAsync(ICommand command);

        /// <summary>
        /// Gets all available commands.
        /// </summary>
        /// <returns>A collection of available commands.</returns>
        IEnumerable<ICommand> GetAvailableCommands();

        /// <summary>
        /// Gets commands by category.
        /// </summary>
        /// <param name="category">The category of commands to retrieve.</param>
        /// <returns>A collection of commands in the specified category.</returns>
        IEnumerable<ICommand> GetCommandsByCategory(string category);

        /// <summary>
        /// Registers a command.
        /// </summary>
        /// <param name="command">The command to register.</param>
        void RegisterCommand(ICommand command);

        /// <summary>
        /// Unregisters a command.
        /// </summary>
        /// <param name="commandName">The name of the command to unregister.</param>
        bool UnregisterCommand(string commandName);

        /// <summary>
        /// Gets the event bus for subscribing to events.
        /// </summary>
        IEventBus EventBus { get; }

        /// <summary>
        /// Gets the execution history.
        /// </summary>
        IReadOnlyList<CommandExecutionRecord> ExecutionHistory { get; }
    }

    /// <summary>
    /// Represents a record of command execution.
    /// </summary>
    public class CommandExecutionRecord
    {
        /// <summary>
        /// Gets the name of the executed command.
        /// </summary>
        public string CommandName { get; set; } = string.Empty;

        /// <summary>
        /// Gets the result of the execution.
        /// </summary>
        public CommandResult Result { get; set; } = null!;

        /// <summary>
        /// Gets the execution duration.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets the execution timestamp.
        /// </summary>
        public DateTime ExecutedAt { get; set; }
    }

    /// <summary>
    /// The Master Controller - orchestrates all command execution and event handling.
    /// </summary>
    public class MasterController : IMasterController
    {
        private readonly ILogger<MasterController> _logger;
        private readonly Dictionary<string, ICommand> _registeredCommands = new();
        private readonly List<CommandExecutionRecord> _executionHistory = new();
        private readonly IEventBus _eventBus;
        private readonly object _lockObject = new();
        private const int MaxHistorySize = 1000;

        /// <summary>
        /// Initializes a new instance of the MasterController.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="eventBus">The event bus instance.</param>
        public MasterController(
            ILogger<MasterController> logger,
            IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            
            _logger.LogInformation("Master Controller initialized");
        }

        /// <summary>
        /// Gets the event bus for subscribing to events.
        /// </summary>
        public IEventBus EventBus => _eventBus;

        /// <summary>
        /// Gets the execution history.
        /// </summary>
        public IReadOnlyList<CommandExecutionRecord> ExecutionHistory
        {
            get
            {
                lock (_lockObject)
                {
                    return _executionHistory.AsReadOnly();
                }
            }
        }

        /// <summary>
        /// Executes a command asynchronously.
        /// </summary>
        public async Task<CommandResult> ExecuteCommandAsync(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            _logger.LogInformation("Executing command: {CommandName}", command.Name);

            var startTime = DateTime.UtcNow;
            CommandResult result;

            try
            {
                // Validate the command
                if (!command.Validate())
                {
                    result = CommandResult.Failure($"Command validation failed: {command.Name}");
                    _logger.LogWarning("Command validation failed: {CommandName}", command.Name);
                    await _eventBus.PublishAsync(new CommandFailedEvent(command.Name, new InvalidOperationException("Validation failed")));
                    RecordExecution(command.Name, result, DateTime.UtcNow - startTime);
                    return result;
                }

                // Publish command started event
                await _eventBus.PublishAsync(new CommandStartedEvent(command.Name));

                // Execute the command
                result = await command.ExecuteAsync();

                // Publish command completed event
                await _eventBus.PublishAsync(new CommandCompletedEvent(command.Name, result.Data));

                _logger.LogInformation(
                    "Command executed successfully: {CommandName} - {Message}",
                    command.Name,
                    result.Message);
            }
            catch (Exception ex)
            {
                result = CommandResult.Failure($"Command execution failed: {ex.Message}", ex);
                _logger.LogError(ex, "Command execution failed: {CommandName}", command.Name);
                await _eventBus.PublishAsync(new CommandFailedEvent(command.Name, ex));
            }
            finally
            {
                var duration = DateTime.UtcNow - startTime;
                RecordExecution(command.Name, result, duration);
            }

            return result;
        }

        /// <summary>
        /// Gets all available commands.
        /// </summary>
        public IEnumerable<ICommand> GetAvailableCommands()
        {
            lock (_lockObject)
            {
                return _registeredCommands.Values.ToList();
            }
        }

        /// <summary>
        /// Gets commands by category.
        /// </summary>
        public IEnumerable<ICommand> GetCommandsByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentNullException(nameof(category));
            }

            lock (_lockObject)
            {
                return _registeredCommands.Values
                    .Where(cmd => cmd.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        /// <summary>
        /// Registers a command.
        /// </summary>
        public void RegisterCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            lock (_lockObject)
            {
                if (_registeredCommands.ContainsKey(command.Name))
                {
                    _logger.LogWarning("Command already registered: {CommandName}", command.Name);
                    return;
                }

                _registeredCommands[command.Name] = command;
                _logger.LogInformation(
                    "Command registered: {CommandName} ({Category})",
                    command.Name,
                    command.Category);
            }
        }

        /// <summary>
        /// Unregisters a command.
        /// </summary>
        public bool UnregisterCommand(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
            {
                throw new ArgumentNullException(nameof(commandName));
            }

            lock (_lockObject)
            {
                var result = _registeredCommands.Remove(commandName);
                if (result)
                {
                    _logger.LogInformation("Command unregistered: {CommandName}", commandName);
                }
                return result;
            }
        }

        /// <summary>
        /// Records a command execution in the history.
        /// </summary>
        private void RecordExecution(string commandName, CommandResult result, TimeSpan duration)
        {
            lock (_lockObject)
            {
                var record = new CommandExecutionRecord
                {
                    CommandName = commandName,
                    Result = result,
                    Duration = duration,
                    ExecutedAt = DateTime.UtcNow
                };

                _executionHistory.Add(record);

                // Maintain history size limit
                if (_executionHistory.Count > MaxHistorySize)
                {
                    _executionHistory.RemoveRange(0, _executionHistory.Count - MaxHistorySize);
                }
            }
        }
    }
}
