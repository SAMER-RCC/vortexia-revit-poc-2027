namespace Vortexia.Core.Commands
{
    /// <summary>
    /// Represents the result of a command execution.
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// Gets the status of the command execution.
        /// </summary>
        public CommandStatus Status { get; private set; }

        /// <summary>
        /// Gets the message associated with the result.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the data returned by the command.
        /// </summary>
        public object? Data { get; private set; }

        /// <summary>
        /// Gets the exception if the command failed.
        /// </summary>
        public Exception? Exception { get; private set; }

        /// <summary>
        /// Gets the timestamp when the result was created.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        private CommandResult(CommandStatus status, string message, object? data = null, Exception? exception = null)
        {
            Status = status;
            Message = message;
            Data = data;
            Exception = exception;
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Creates a successful command result.
        /// </summary>
        public static CommandResult Success(string message, object? data = null)
        {
            return new CommandResult(CommandStatus.Success, message, data);
        }

        /// <summary>
        /// Creates a failed command result.
        /// </summary>
        public static CommandResult Failure(string message, Exception? exception = null)
        {
            return new CommandResult(CommandStatus.Failed, message, exception: exception);
        }

        /// <summary>
        /// Creates a canceled command result.
        /// </summary>
        public static CommandResult Canceled(string message)
        {
            return new CommandResult(CommandStatus.Canceled, message);
        }

        /// <summary>
        /// Creates a pending command result.
        /// </summary>
        public static CommandResult Pending(string message)
        {
            return new CommandResult(CommandStatus.Pending, message);
        }

        public override string ToString()
        {
            return $"[{Status}] {Message}";
        }
    }

    /// <summary>
    /// Represents the status of a command execution.
    /// </summary>
    public enum CommandStatus
    {
        /// <summary>
        /// The command is pending execution.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The command is currently executing.
        /// </summary>
        Running = 1,

        /// <summary>
        /// The command executed successfully.
        /// </summary>
        Success = 2,

        /// <summary>
        /// The command execution failed.
        /// </summary>
        Failed = 3,

        /// <summary>
        /// The command execution was canceled.
        /// </summary>
        Canceled = 4
    }
}
