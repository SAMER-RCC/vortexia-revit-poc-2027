namespace Vortexia.Core.Events
{
    /// <summary>
    /// Defines the interface for event subscribers/listeners.
    /// </summary>
    public interface IEventListener
    {
        /// <summary>
        /// Handles an event asynchronously.
        /// </summary>
        /// <param name="event">The event to handle.</param>
        /// <returns>A task that represents the asynchronous event handling.</returns>
        Task OnEventAsync(IEvent @event);
    }

    /// <summary>
    /// Defines the interface for the event bus (publisher).
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Subscribes a listener to events of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of event to subscribe to.</typeparam>
        /// <param name="listener">The listener to subscribe.</param>
        void Subscribe<T>(IEventListener listener) where T : IEvent;

        /// <summary>
        /// Unsubscribes a listener from events of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of event to unsubscribe from.</typeparam>
        /// <param name="listener">The listener to unsubscribe.</param>
        void Unsubscribe<T>(IEventListener listener) where T : IEvent;

        /// <summary>
        /// Publishes an event to all subscribed listeners.
        /// </summary>
        /// <param name="event">The event to publish.</param>
        /// <returns>A task that represents the asynchronous publication.</returns>
        Task PublishAsync(IEvent @event);

        /// <summary>
        /// Gets the number of subscribers for a specific event type.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <returns>The number of subscribers.</returns>
        int GetSubscriberCount<T>() where T : IEvent;
    }

    /// <summary>
    /// Default implementation of the event bus.
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<IEventListener>> _subscribers = new();
        private readonly object _lockObject = new();

        public void Subscribe<T>(IEventListener listener) where T : IEvent
        {
            lock (_lockObject)
            {
                var eventType = typeof(T);
                if (!_subscribers.ContainsKey(eventType))
                {
                    _subscribers[eventType] = new List<IEventListener>();
                }

                if (!_subscribers[eventType].Contains(listener))
                {
                    _subscribers[eventType].Add(listener);
                }
            }
        }

        public void Unsubscribe<T>(IEventListener listener) where T : IEvent
        {
            lock (_lockObject)
            {
                var eventType = typeof(T);
                if (_subscribers.ContainsKey(eventType))
                {
                    _subscribers[eventType].Remove(listener);
                }
            }
        }

        public async Task PublishAsync(IEvent @event)
        {
            List<IEventListener>? listeners = null;

            lock (_lockObject)
            {
                var eventType = @event.GetType();
                if (_subscribers.ContainsKey(eventType))
                {
                    listeners = new List<IEventListener>(_subscribers[eventType]);
                }
            }

            if (listeners != null)
            {
                var tasks = listeners.Select(listener => listener.OnEventAsync(@event));
                await Task.WhenAll(tasks);
            }
        }

        public int GetSubscriberCount<T>() where T : IEvent
        {
            lock (_lockObject)
            {
                var eventType = typeof(T);
                return _subscribers.ContainsKey(eventType) ? _subscribers[eventType].Count : 0;
            }
        }
    }
}
