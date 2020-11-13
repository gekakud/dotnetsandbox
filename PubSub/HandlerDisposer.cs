using System;

namespace PubSub
{
    internal class HandlerDisposer : IDisposable
    {
        private readonly ISubscriberStore m_store;
        private readonly Type m_eventType;
        private readonly IEventHandler m_handler;

        public HandlerDisposer(ISubscriberStore store, Type eventType, IEventHandler handler)
        {
            m_store = store;
            m_eventType = eventType;
            m_handler = handler;
        }

        public void Dispose()
        {
            m_store.Unsubscribe(m_eventType, m_handler);
        }
    }
}