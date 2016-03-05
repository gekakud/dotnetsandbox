using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadAndTaskTutorial
{
    class ThreadsExecutor:IExecutor
    {
        private int NumOfJobs { get; set; }
        List<Thread> _jobList = new List<Thread>();

        private delegate void NotifierDelegate(int id);

        private event NotifierDelegate myNotifier;
        
        /// <summary>
        /// Comments test
        /// </summary>
        /// <param name="num"></param>

        public ThreadsExecutor(int num)
        {
            if (num <= 0)
                throw new ArgumentOutOfRangeException();

            myNotifier = new NotifierDelegate(OnJobCompleted);

            NumOfJobs = num;
            for (int i = 0; i < NumOfJobs; i++)
            {
                _jobList.Add(new Thread((JobToExecute)));
            }
        }

        private void OnJobCompleted(int id)
        {
            NumOfJobs--;
            Console.WriteLine("Thread {0} has finished. Threads still alive: {1}", id, NumOfJobs);
        }

        public void StartJobs()
        {
            foreach (var job in _jobList)
            {
                job.Start();
            }
        }

        private void JobToExecute()
        {
            double result = 0;
            for (int i = 0; i < 100000; i++)
            {
                result += (double)i * (Math.PI / Math.Sqrt(26784) / 0.074);
            }
            myNotifier(Thread.CurrentThread.ManagedThreadId);
        }
    }
}
