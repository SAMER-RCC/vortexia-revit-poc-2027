namespace Vortexia.Core.Events
{
    /// <summary>
    /// Defines the interface for events in the Vortexia system.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        string EventType { get; }

        /// <summary>
        /// Gets the timestamp when the event was created.
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets the data associated with the event.
        /// </summary>
        object? Data { get; }

        /// <summary>
        /// Gets the source that triggered the event.
        /// </summary>
        string Source { get; }
    }

    /// <summary>
    /// Provides a base implementation for events.
    /// </summary>
    public abstract class EventBase : IEvent
    {
        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        public abstract string EventType { get; }

        /// <summary>
        /// Gets the timestamp when the event was created.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the data associated with the event.
        /// </summary>
        public object? Data { get; protected set; }

        /// <summary>
        /// Gets the source that triggered the event.
        /// </summary>
        public string Source { get; protected set; }

        protected EventBase(string source, object? data = null)
        {
            Source = source;
            Data = data;
            Timestamp = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"[{EventType}] {Source} - {Timestamp:yyyy-MM-dd HH:mm:ss.fff}";
        }
    }

    /// <summary>
    /// Event raised when a command starts executing.
    /// </summary>
    public class CommandStartedEvent : EventBase
    {
        public override string EventType => "CommandStarted";

        public CommandStartedEvent(string commandName, object? data = null)
            : base(commandName, data)
        {
        }
    }

    /// <summary>
    /// Event raised when a command completes execution.
    /// </summary>
    public class CommandCompletedEvent : EventBase
    {
        public override string EventType => "CommandCompleted";

        public CommandCompletedEvent(string commandName, object? data = null)
            : base(commandName, data)
        {
        }
    }

    /// <summary>
    /// Event raised when a command fails.
    /// </summary>
    public class CommandFailedEvent : EventBase
    {
        public override string EventType => "CommandFailed";

        public CommandFailedEvent(string commandName, Exception exception)
            : base(commandName, exception)
        {
        }
    }

    /// <summary>
    /// Event raised when an error occurs in the system.
    /// </summary>
    public class ErrorOccurredEvent : EventBase
    {
        public override string EventType => "ErrorOccurred";

        public ErrorOccurredEvent(string source, Exception exception)
            : base(source, exception)
        {
        }
    }
}
