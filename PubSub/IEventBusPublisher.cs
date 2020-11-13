using System.Threading.Tasks;

namespace PubSub
{
    public interface IEventBusPublisher
    {
        /// <summary>
        /// Publish an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="eventData">Related data for the event</param>
        Task<bool> Publish<TEventData>(TEventData eventData) where TEventData : IEventData;
    }
}