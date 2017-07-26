using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLDataflow
{
    public class Program
    {
        private static void Main()
        {
            var sim = new Simulator();
            Task.Factory.StartNew(sim.StartSimulator,TaskCreationOptions.LongRunning);

            Console.ReadKey();
        }
    }

    public class Simulator
    {
        private static ICommunicationService _comService;
        private DataflowManager _dfManager;

        public Simulator()
        {
            //set event provider
            _comService = new DummyActionsGenerator();
            _dfManager = new DataflowManager(_comService);
        }

        public void StartSimulator()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            while (true)
            {
                //simulate posting new requests to server dataflow
                _dfManager.PostRequestsToBuffer();
                
                var delay = rnd.Next(1000, 10000);
                Thread.Sleep(delay);
            }
        }
    }
}