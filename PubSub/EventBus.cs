using System;
using System.Threading.Tasks;

namespace PubSub
{
    internal class EventBus : IEventBusPublisher, IEventBusRegistrator, IDisposable
    {
        private readonly IEventPublisher m_eventPublisher;
        private readonly ISubscriberStore m_store;

        private bool m_disposing;

        public EventBus()
        {
            m_disposing = false;
            m_store = new EventHandlerStore();
            int consumerAmount = 5;
            m_eventPublisher = new ProducerConsumerPublisher(consumerAmount, m_store);
        }

        private Task<bool> Publish(Type eventType, IEventData eventData)
        {
            return m_eventPublisher.Post(new Tuple<Type, IEventData>(eventType, eventData));
        }

        public Task<bool> Publish<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            return Publish(typeof(TEventData), eventData);
        }

        private IDisposable Subscribe(Type eventType, IEventHandler handler)
        {
            return m_store.Subscribe(eventType, handler);
        }
        public IDisposable Subscribe<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
            return Subscribe(typeof(TEventData), handler);
        }

        public IDisposable Subscribe<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            return Subscribe(typeof(TEventData), new ActionEventHandler<TEventData>(action));
        }

        private void Unsubscribe(Type eventType, IEventHandler handler)
        {
            m_store.Unsubscribe(eventType, handler);
        }

        public void Unsubscribe<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
            Type eventType = typeof(TEventData);
            Unsubscribe(eventType, handler);
        }

        public void Unsubscribe<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            m_store.Unsubscribe(action);
        }
        public void UnsubscribeAll<TEventData>() where TEventData : IEventData
        {
            UnsubscribeAll(typeof(TEventData));
        }

        private void UnsubscribeAll(Type eventType)
        {
            m_store.UnsubscribeAll(eventType);
        }

        public void Dispose()
        {
            if (m_disposing)
            {
                return;
            }

            m_disposing = true;

            Dispose(m_disposing);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                m_store.Dispose();
                m_eventPublisher.Dispose();
            }
        }
    }
}