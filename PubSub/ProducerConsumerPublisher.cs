using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PubSub
{
    internal class ProducerConsumerPublisher: IEventPublisher
    {
        private readonly BlockingCollection<Task<bool>> m_blockingCollection;
        private readonly CancellationTokenSource m_cancellationTokenSource;
        private readonly Task[] m_consumers;
        private readonly ISubscriberStore m_store;

        private bool m_disposing;

        public ProducerConsumerPublisher(int consumerAmount, ISubscriberStore store)
        {
            m_disposing = false;
            m_store = store;
            m_cancellationTokenSource = new CancellationTokenSource();
            m_blockingCollection = new BlockingCollection<Task<bool>>();
            m_consumers = new Task[consumerAmount];
            StartConsumers(consumerAmount);
        }

        private void StartConsumers(int consumerAmount)
        {
            for (int i = 0; i < consumerAmount; i++)
            {
                m_consumers[i] = Task.Factory.StartNew(() =>
                {
                    PostEvents(m_cancellationTokenSource.Token);
                }, m_cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        private void PostEvents(CancellationToken token)
        {
            foreach (Task<bool> task in m_blockingCollection.GetConsumingEnumerable())
            {
                if (token.IsCancellationRequested)
                {
                    //s_logger.Info("Post Event was canceled");
                    return;
                }

                task.RunSynchronously();//run on consumer thread
            }
        }

        public Task<bool> Post(Tuple<Type, IEventData> item)
        {
            if (m_blockingCollection.IsAddingCompleted)
            {
                //s_logger.Warn("this blocking collection has been marked completed for adding can not publish event {0}", new object[] { item.Item2 });
                return Task.FromResult(false);
            }

            Task<bool> task = new Task<bool>(() =>
            {
                //s_logger.Info("Post event type {0}", new object[] { item.Item1 });

                bool success = true;

                foreach (IEventHandler handler in m_store.GetHandlers(item.Item1))
                {
                    success &= handler.HandleEvent(item.Item2, item.Item1);
                }

                return success;
            });

            m_blockingCollection.Add(task);

            return task;
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
                try
                {
                    m_blockingCollection.CompleteAdding();
                    m_cancellationTokenSource.Cancel();
                    Task.WaitAll(m_consumers);
                    m_blockingCollection.Dispose();
                }
                catch (Exception e)
                {
                    //s_logger.Warn(e,"failed to dispose");
                }
            }
        }
    }
}