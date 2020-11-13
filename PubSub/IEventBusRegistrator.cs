using System;

namespace PubSub
{
    public interface IEventBusRegistrator
    {
        /// <summary>
        /// Subscribe to an event. 
        /// Same (given) action is used for all event occurrences.
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        IDisposable Subscribe<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        /// Subscribe to an event. 
        /// Same (given) instance of the handler is used for all event occurrences.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="handler">Object to handle the event</param>
        IDisposable Subscribe<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

        /// <summary>
        /// UnSubscribes from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="action"></param>
        void Unsubscribe<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        /// Unsubscribes from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="handler">Handler object that is registered before</param>
        void Unsubscribe<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

        /// <summary>
        /// Unsubscribe all event handlers of given event type.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        void UnsubscribeAll<TEventData>() where TEventData : IEventData;
    }
}