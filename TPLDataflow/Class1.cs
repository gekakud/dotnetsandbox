using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Timers;
using Astea.DC.Core.Communication;

namespace Astea.DC.Communication
{
    public class DataEventBuffer : IDataEventBuffer, IDisposable
    {
        private Timer _triggerTimer;
        private DataEventEqualityComparer _comparer;
        private BatchBlock<IDataEvent> _batchBlock;
        private TransformBlock<IEnumerable<IDataEvent>, IEnumerable<IDataEvent>> _distinctBlock;

        public DataEventBuffer()
        {
            _comparer = new DataEventEqualityComparer();

            _batchBlock = new BatchBlock<IDataEvent>(500);
            _distinctBlock = new TransformBlock<IEnumerable<IDataEvent>, IEnumerable<IDataEvent>>(p_events =>
            {
                var result = p_events.Distinct(_comparer);

                _triggerTimer.Stop();
                _triggerTimer.Start();

                return result;
            });

            _batchBlock.LinkTo(_distinctBlock);

            _triggerTimer = new Timer(10 * 1000) { AutoReset = true };
            _triggerTimer.Elapsed += (p_sender, p_args) => _batchBlock.TriggerBatch();
            _triggerTimer.Start();
        }

        protected virtual IEnumerable<IDataEvent> Transform(IEnumerable<IDataEvent> p_events)
        {
            return p_events.Distinct(_comparer);
        }

        public bool Buffer(IDataEvent p_dataEvent)
        {
            return _batchBlock.Post(p_dataEvent);
        }

        public Task<bool> BufferAsync(IDataEvent p_dataEvent)
        {
            return _batchBlock.SendAsync(p_dataEvent);
        }

        public IEnumerable<IDataEvent> Receive()
        {
            return _distinctBlock.Receive();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool p_disposing)
        {
            if (p_disposing)
            {
                if (_triggerTimer != null)
                {
                    _triggerTimer.Stop();
                    _triggerTimer.Dispose();
                }
                _triggerTimer = null;

                if (_batchBlock != null)
                    _batchBlock.Complete();
                _batchBlock = null;

                if (_distinctBlock != null)
                    _distinctBlock.Complete();
                _distinctBlock = null;

                _comparer = null;
            }
        }

        ~DataEventBuffer()
        {
            Dispose(false);
        }
    }
}
