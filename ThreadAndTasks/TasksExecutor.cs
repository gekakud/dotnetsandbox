using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadAndTaskTutorial
{
    class TasksExecutor:IExecutor
    {
        private int NumOfJobs { get; set; }
        List<Task> tasksList = new List<Task>();
        private List<int> resultList = new List<int>();

        public TasksExecutor(int num)
        {
            if (num <= 0)
                throw new ArgumentOutOfRangeException();

            NumOfJobs = num;

            for (int i = 0; i < NumOfJobs; i++)
            {
                tasksList.Add(new Task<int>(JobToExecute));
                //resultList.Add(Task.Factory.StartNew());
            }
        }

        public void StartJobs()
        {
            foreach (var t in tasksList)
            {
                t.Start();
            }
        }

        private int JobToExecute()
        {
            // job to do
            Console.WriteLine("task id is: {0}", Task.CurrentId);
            return 5;
        }

    }
}
