using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using Timer = System.Timers.Timer;

namespace TPLDataflow
{
    public enum TypeOfServerAction
    {
        Get,
        Set,
        Update,
        Find
    }

    public class ServerAction
    {
        public TypeOfServerAction ActionType { get; set; }
        public string Payload { get; set; }
        public Guid RequestId { get; set; }
    }

    public class Program
    {
        private Timer _timer;
        private static void Main()
        {         
            DataflowExecutor d = new DataflowExecutor(new Configs{BufferSize = 2,UIBufferSize = 3,DelayTime = 2});

            var i = 0;
            while (true)
            {
                d.PutDataIntoPipe(i);
                var ttt = DummyActionsGenerator.GetServerActions();
                i++;
            Thread.Sleep(500);
            }
        }
    }

    public static class DummyActionsGenerator
    {
        public static List<ServerAction> GetServerActions()
        {
            var list = new List<ServerAction>();
            var rnd = new Random(4321);
            var numOfActions = rnd.Next(1, 10);

            for (var i = 0; i < numOfActions; i++)
            {
                var randActType = (TypeOfServerAction)rnd.Next(0, (int) TypeOfServerAction.Find);

                list.Add(new ServerAction
                {
                    ActionType = randActType,
                    Payload = "bla",
                    RequestId = Guid.NewGuid()
                });
            }

            return list;
        }
    }

    internal class DataflowManager
    {
        
    }

    public class DataflowExecutor : IDisposable
    {
        private BatchBlock<int> _bufferBatchBlock;
        private  TransformBlock<IEnumerable<int>, string> _buildTransformBlock;
        private  BatchBlock<string> _dataStoreBatchBlock;
        private BatchBlock<string> _itemsToUpdateUI;
        private  TransformBlock<IEnumerable<int>, IEnumerable<int>> _mergeTransformBlock;
        public int UpdateTime = 1;
        private Timer _timer;

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }
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