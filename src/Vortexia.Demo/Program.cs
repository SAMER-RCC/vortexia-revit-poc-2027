using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vortexia.Core;
using Vortexia.Core.Commands;

namespace Vortexia.Demo
{
    /// <summary>
    /// Demo application showcasing the Vortexia Master Controller.
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            // Setup dependency injection
            var services = new ServiceCollection();

            // Add logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            // Add Vortexia Core services
            services.AddVortexiaCore();

            var serviceProvider = services.BuildServiceProvider();

            // Get the logger and controller
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            var controller = serviceProvider.GetRequiredService<IMasterController>();

            logger.LogInformation("=== Vortexia Revit POC 2027 - Demo Application ===");
            logger.LogInformation("");

            // Create and register sample commands
            var helloWorldCommand = new HelloWorldCommand();
            var greetingCommand = new GreetingCommand("User");

            controller.RegisterCommand(helloWorldCommand);
            controller.RegisterCommand(greetingCommand);

            logger.LogInformation("Registered {Count} commands", controller.GetAvailableCommands().Count());
            logger.LogInformation("");

            // Execute commands
            logger.LogInformation("Executing HelloWorld command...");
            var result1 = await controller.ExecuteCommandAsync(helloWorldCommand);
            logger.LogInformation("Result: {Status} - {Message}", result1.Status, result1.Message);
            logger.LogInformation("");

            logger.LogInformation("Executing Greeting command...");
            var result2 = await controller.ExecuteCommandAsync(greetingCommand);
            logger.LogInformation("Result: {Status} - {Message}", result2.Status, result2.Message);
            logger.LogInformation("");

            // Display execution history
            logger.LogInformation("=== Execution History ===");
            foreach (var record in controller.ExecutionHistory)
            {
                logger.LogInformation(
                    "• {CommandName} - {Status} ({Duration}ms)",
                    record.CommandName,
                    record.Result.Status,
                    record.Duration.TotalMilliseconds);
            }

            logger.LogInformation("");
            logger.LogInformation("Demo completed successfully!");

            // Dispose
            await serviceProvider.DisposeAsync();
        }
    }

    /// <summary>
    /// Sample HelloWorld command.
    /// </summary>
    public class HelloWorldCommand : CommandBase
    {
        public override string Name => "HelloWorld";
        public override string Description => "Displays a hello world message";
        public override string Category => "Demo";

        public override async Task<CommandResult> ExecuteAsync()
        {
            await Task.Delay(100); // Simulate work
            return CommandResult.Success("مرحباً بك في Vortexia Revit POC 2027! 🎉");
        }
    }

    /// <summary>
    /// Sample Greeting command.
    /// </summary>
    public class GreetingCommand : CommandBase
    {
        private readonly string _name;

        public GreetingCommand(string name)
        {
            _name = name;
        }

        public override string Name => "Greeting";
        public override string Description => "Displays a personalized greeting";
        public override string Category => "Demo";

        public override async Task<CommandResult> ExecuteAsync()
        {
            await Task.Delay(100); // Simulate work
            return CommandResult.Success($"مرحباً يا {_name}! 👋");
        }
    }
}
