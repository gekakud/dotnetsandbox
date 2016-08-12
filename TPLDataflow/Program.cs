using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using System.Timers;
using Timer = System.Timers.Timer;

namespace TPLDataflow
{
    public class Program
    {
        private Timer _t;
        private static void Main()
        {         
            DataflowExecutor d = new DataflowExecutor(new Configs{BufferSize = 2,UIBufferSize = 3,DelayTime = 2});

            var i = 0;
            while (true)
            {
                d.PutDataIntoPipe(i);
                i++;
            Thread.Sleep(500);
            }
        }
    }

    public class DataflowExecutor
    {
        private BatchBlock<int> _bufferBatchBlock;
        private  TransformBlock<IEnumerable<int>, string> _buildTransformBlock;
        private  BatchBlock<string> _dataStoreBatchBlock;
        private BatchBlock<string> _itemsToUpdateUI;
        private  TransformBlock<IEnumerable<int>, IEnumerable<int>> _mergeTransformBlock;
        public int UpdateTime = 1;
        private Timer _timer;

        public void PutDataIntoPipe(int p_d)
        {
            _bufferBatchBlock.Post(p_d);
        }

        private bool CreateAndLinkBlocks(Configs p_configs)
        {
            try
            {
                _bufferBatchBlock = new BatchBlock<int>(p_configs.BufferSize);

                _mergeTransformBlock =
                    new TransformBlock<IEnumerable<int>, IEnumerable<int>>(
                        (Func<IEnumerable<int>, IEnumerable<int>>)MergeStuff);
                _buildTransformBlock =
                    new TransformBlock<IEnumerable<int>, string>((Func<IEnumerable<int>, string>)BuildStuff);

                _dataStoreBatchBlock = new BatchBlock<string>(p_configs.BufferSize);
                _itemsToUpdateUI = new BatchBlock<string>(p_configs.UIBufferSize);

                _bufferBatchBlock.LinkTo(_mergeTransformBlock);
                _mergeTransformBlock.LinkTo(_buildTransformBlock);

                //CON
                var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
                _buildTransformBlock.LinkTo(_dataStoreBatchBlock, linkOptions, p_p => p_p.Length > 5);
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        public DataflowExecutor(Configs p_configs)
        {
            if (CreateAndLinkBlocks(p_configs))
            {
                SetTimeOutForBuffer(p_configs.DelayTime);
            }
        }

        private void SetTimeOutForBuffer(int p_delayTime)
        {
            UpdateTime = p_delayTime;
            _timer = new Timer(UpdateTime*1000){AutoReset = true};
            _timer.Elapsed += (x, y) => { _bufferBatchBlock.TriggerBatch(); };
            _timer.Start();
        }

        private IEnumerable<int> MergeStuff(IEnumerable<int> p_data)
        {
            Console.WriteLine("Thread {0} is {1}", Thread.CurrentThread.ManagedThreadId, "Merging");
            Thread.Sleep(20);
            return new object() as IEnumerable<int>;
        }

        private string BuildStuff(IEnumerable<int> p_data)
        {
            Console.WriteLine("Thread {0} is {1}", Thread.CurrentThread.ManagedThreadId, "Building");
            Thread.Sleep(20);

            return "";
        }
    }

    public interface ICustomType
    {
        int Val { get; set; }
    }

    public class Configs
    {
        public int BufferSize { get; set; }
        public int UIBufferSize { get; set; }
        public int DelayTime { get; set; }
        public int UIupdateTime { get; set; }
    }
}