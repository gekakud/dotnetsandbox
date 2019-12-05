using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Events
{
    internal class Program
    {
        //Events enable a class or object to notify other classes or objects when something of interest occurs.
        //The class that sends (or raises) the event is called the publisher
        //and the classes that receive (or handle) the event are called subscribers.
        private static void Main()
        {
            Console.WriteLine("Main");

            DataProcessor dataProcessor = new DataProcessor();//publisher
            ProcessingStatusNotifier progressTracker = new ProcessingStatusNotifier();//subscriber

            dataProcessor.DataProcessingStarted += progressTracker.Started;
            dataProcessor.DataProcessingInProgress += progressTracker.InProgress;
            dataProcessor.DataProcessingFinished += progressTracker.Finished;

            dataProcessor.ProcessSomeData("some long text to process");
            Console.ReadKey();
        }
    }

    //simulates UI
    internal class ProcessingStatusNotifier
    {
        public void Started()
        {
            Console.WriteLine("Current status: STARTED");
        }

        public void InProgress(int percentage)
        {
            Console.WriteLine("Current status: IN PROGRESS - {0}% done", percentage);
        }

        public void Finished()
        {
            Console.WriteLine("Current status: FINISHED");
        }
    }

    internal class DataProcessor
    {
        public delegate void ProcessingStarted();
        public delegate void ProcessingInProgress(int percentage);
        public delegate void ProcessingFinished();

        public event ProcessingStarted DataProcessingStarted;
        public event ProcessingInProgress DataProcessingInProgress;
        public event ProcessingFinished DataProcessingFinished;

        public async void ProcessSomeData(string text)
        {
            //notify process started
            DataProcessingStarted();

            await Task.Run(() =>
            {
                int progress = 0;
                Console.WriteLine("Processing data...");
                while (progress<100)
                {
                    //notify progress
                    DataProcessingInProgress(progress);
                    Thread.Sleep(300);
                    progress += 12;
                }

                DataProcessingInProgress(100);
            });

            //notify finish
            if (DataProcessingFinished != null)
            {
                DataProcessingFinished();
            }
        }
    }
}