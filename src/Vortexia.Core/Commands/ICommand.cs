namespace Vortexia.Core.Commands
{
    /// <summary>
    /// Defines the interface for a command that can be executed.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the unique name of the command.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a human-readable description of the command.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the category of the command.
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Validates the command before execution.
        /// </summary>
        /// <returns>True if the command is valid; otherwise, false.</returns>
        bool Validate();

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous command execution.</returns>
        Task<CommandResult> ExecuteAsync();
    }

    /// <summary>
    /// Provides a base implementation for commands.
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// Gets the unique name of the command.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets a human-readable description of the command.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the category of the command.
        /// </summary>
        public virtual string Category => "General";

        /// <summary>
        /// Validates the command before execution.
        /// </summary>
        public virtual bool Validate()
        {
            return true;
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        public abstract Task<CommandResult> ExecuteAsync();

        public override string ToString()
        {
            return $"{Name} - {Description}";
        }
    }
}
