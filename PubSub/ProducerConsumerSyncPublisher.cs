using System;
using System.Threading.Tasks;

namespace PubSub
{
    internal class ProducerConsumerSyncPublisher : IEventPublisher
    {
        private readonly ISubscriberStore m_store;

        public ProducerConsumerSyncPublisher(ISubscriberStore store)
        {
            m_store = store;
        }

        public Task<bool> Post(Tuple<Type, IEventData> item)
        {
            bool success = true;
            foreach (IEventHandler handler in m_store.GetHandlers(item.Item1))
            {
                success &= EventHandlerExtention.HandleEvent(handler, item.Item2, item.Item1);
            }

            return Task.FromResult(success);
        }

        public void Dispose()
        {
            // nothing to do here
        }
    }
}