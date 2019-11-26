using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadAndTaskTutorial
{
    internal class ThreadsExecutor : IExecutor
    {
        private readonly List<Thread> _jobList = new List<Thread>();

        /// <summary>
        ///     Comments test
        /// </summary>
        /// <param name="num"></param>
        public ThreadsExecutor(int num)
        {
            if (num <= 0)
                throw new ArgumentOutOfRangeException();

            MyNotifier = OnJobCompleted;

            NumOfJobs = num;
            for (var i = 0; i < NumOfJobs; i++)
            {
                _jobList.Add(new Thread(JobToExecute));
            }
        }

        private int NumOfJobs { get; set; }

        public void StartJobs()
        {
            _jobList.ForEach(j=>j.Start());
        }

        private event NotifierDelegate MyNotifier;

        private void OnJobCompleted(int id)
        {
            NumOfJobs--;
            Console.WriteLine("Thread {0} has finished. Threads still alive: {1}", id, NumOfJobs);
        }

        private void JobToExecute()
        {
            double result = 0;
            for (var i = 0; i < 10000000; i++)
            {
                result += i*(Math.PI/Math.Sqrt(26784)/0.00074);
            }
            MyNotifier(Thread.CurrentThread.ManagedThreadId);
        }

        private delegate void NotifierDelegate(int id);
    }
}