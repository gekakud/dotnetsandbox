using System;
using System.Collections.Generic;
using System.Linq;

namespace PubSub
{
    internal class EventHandlerStore : ISubscriberStore
    {
        private readonly object m_locker;
        private readonly Dictionary<Type, List<IEventHandler>> m_eventHandlers;

        private bool m_disposing;

        public EventHandlerStore()
        {
            m_disposing = false;
            m_locker = new object();
            m_eventHandlers = new Dictionary<Type, List<IEventHandler>>();
        }

        public IEnumerable<IEventHandler> GetHandlers(Type eventType)
        {
            var eventHandlers = new List<IEventHandler>();

            lock (m_locker)
            {
                foreach (var handlerFactory in m_eventHandlers
                    .Where(hf => ShouldPublishEventForHandler(eventType, hf.Key)))
                {
                    eventHandlers.AddRange(handlerFactory.Value);
                }

                return eventHandlers.ToArray();
            }
        }

        private static bool ShouldPublishEventForHandler(Type eventType, Type handlerType)
        {
            //    Should trigger same type      Should trigger for inherited types
            return handlerType == eventType || handlerType.IsAssignableFrom(eventType);
        }

        public IDisposable Subscribe(Type eventType, IEventHandler handler)
        {
            lock (m_locker)
            {
                if (m_eventHandlers.ContainsKey(eventType) == false)
                {
                    m_eventHandlers[eventType] = new List<IEventHandler>();
                }

                m_eventHandlers[eventType].Add(handler);

                return new HandlerDisposer(this, eventType, handler);
            }
        }

        public void Unsubscribe(Type eventType, IEventHandler handler)
        {
            lock (m_locker)
            {
                if (m_eventHandlers.ContainsKey(eventType) == false)
                {
                    return;
                }

                m_eventHandlers[eventType].Remove(handler);
            }
        }
        public void Unsubscribe<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            Type eventType = typeof(TEventData);

            lock (m_locker)
            {
                if (m_eventHandlers.ContainsKey(eventType) == false)
                {
                    return;
                }

                m_eventHandlers[eventType].RemoveAll(
                    handler =>
                    {
                        var actionHandler = handler as ActionEventHandler<TEventData>;
                        if (actionHandler == null)
                        {
                            return false;
                        }

                        return actionHandler.Action == action;
                    });
            }
        }

        public void UnsubscribeAll(Type eventType)
        {
            lock (m_locker)
            {
                if (m_eventHandlers.ContainsKey(eventType) == false)
                {
                    return;
                }

                m_eventHandlers.Remove(eventType);
            }
        }

        public void Dispose()
        {
            if (m_disposing) return;

            m_disposing = true;

            Dispose(m_disposing);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (m_disposing)
            {
                lock (m_locker)
                {
                    m_eventHandlers.Clear();
                }
            }
        }
    }
}