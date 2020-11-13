using System;
using System.Collections.Generic;

namespace PubSub
{
    internal interface ISubscriberStore : IDisposable
    {
        IEnumerable<IEventHandler> GetHandlers(Type eventType);
        IDisposable Subscribe(Type eventType, IEventHandler handler);
        void Unsubscribe(Type eventType, IEventHandler handler);
        void Unsubscribe<TEventData>(Action<TEventData> action) where TEventData : IEventData;
        void UnsubscribeAll(Type eventType);
    }
    
    public interface IEventHandler
    {

    }

    public interface IEventHandler<in TEventData> : IEventHandler
    {
        void HandleEvent(TEventData eventData);
    }
    
    public interface IEventData
    {
    }
}