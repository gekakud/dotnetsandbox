using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadAndTaskTutorial
{
    internal class TasksExecutor:IExecutor
    {
        private int NumOfJobs { get; set; }
        readonly List<Task<int>> _tasksList = new List<Task<int>>();
        private readonly List<int> _resultList = new List<int>();

        public TasksExecutor(int p_num)
        {
            if (p_num <= 0)
                throw new ArgumentOutOfRangeException();

            NumOfJobs = p_num;

            for (var i = 0; i < NumOfJobs; i++)
            {
                _tasksList.Add(new Task<int>(JobToExecute));
            }
        }

        public void StartJobs()
        {
            foreach (var t in _tasksList)
            {
                t.Start();
                _resultList.Add(t.Result);
            }

            Task.WaitAll(_tasksList.ToArray() as Task<int>[]);
        }

        public void ShowResults()
        {
            //
            foreach (var result in _resultList)
            {
                Console.WriteLine(result.ToString());
            }
        }

        private int JobToExecute()
        {
            // job to do
            Console.WriteLine("task id is: {0}", Task.CurrentId);
            Thread.Sleep(300);
            if (Task.CurrentId != null) return Task.CurrentId.Value;

            return -1;
        }
    }
}
