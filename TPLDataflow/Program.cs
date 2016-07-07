using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace TPLDataflow
{
    public class Program
    {
        private static void Main()
        {
        }
    }

    public class DataflowExecutor
    {
        private readonly BatchBlock<int> _bufferBatchBlock;
        private readonly TransformBlock<IEnumerable<int>, string> _buildTransformBlock;
        private readonly BatchBlock<string> _dataStoreBatchBlock;
        private BatchBlock<string> _itemsToUpdateUI;
        private readonly TransformBlock<IEnumerable<int>, IEnumerable<int>> _mergeTransformBlock;

        public DataflowExecutor(Configs p_configs)
        {
            _bufferBatchBlock = new BatchBlock<int>(p_configs.BufferSize);

            _mergeTransformBlock =
                new TransformBlock<IEnumerable<int>, IEnumerable<int>>(
                    (Func<IEnumerable<int>, IEnumerable<int>>) MergeStuff);
            _buildTransformBlock =
                new TransformBlock<IEnumerable<int>, string>((Func<IEnumerable<int>, string>) BuildStuff);

            _dataStoreBatchBlock = new BatchBlock<string>(p_configs.BufferSize);
            _itemsToUpdateUI = new BatchBlock<string>(p_configs.UIBufferSize);

            _bufferBatchBlock.LinkTo(_mergeTransformBlock);
            _mergeTransformBlock.LinkTo(_buildTransformBlock);

            //CON
            var linkOptions = new DataflowLinkOptions {PropagateCompletion = true};
            _buildTransformBlock.LinkTo(_dataStoreBatchBlock, linkOptions, p_p => p_p.Length > 5);
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