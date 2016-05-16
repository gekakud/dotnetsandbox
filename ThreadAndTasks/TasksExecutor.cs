using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadAndTaskTutorial
{
    class TasksExecutor:IExecutor
    {
        private int NumOfJobs { get; set; }
        readonly List<Task<int>> tasksList = new List<Task<int>>();
        private List<int> _resultList = new List<int>();

        public TasksExecutor(int p_num)
        {
            if (p_num <= 0)
                throw new ArgumentOutOfRangeException();

            NumOfJobs = p_num;

            for (var i = 0; i < NumOfJobs; i++)
            {
                tasksList.Add(new Task<int>(JobToExecute));
            }
        }

        public void StartJobs()
        {
            foreach (var t in tasksList)
            {
                t.Start();
                _resultList.Add(t.Result);
            }
        }

        public void ShowResults()
        {
            foreach (var result in _resultList)
            {
                Console.WriteLine(result.ToString());
            }
        }

        private int JobToExecute()
        {
            // job to do
            Console.WriteLine("task id is: {0}", Task.CurrentId);
            Thread.Sleep(10);
            if (Task.CurrentId != null) return Task.CurrentId.Value;

            return -1;
        }
    }
}
