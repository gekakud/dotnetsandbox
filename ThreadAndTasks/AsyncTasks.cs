using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadAndTaskTutorial
{
    class AsyncTasks:IExecutor
    {
        private List<Task> asyncJobs = new List<Task>();
        private int numOfJobs;
        public AsyncTasks(int numOfJobs)
        {
            if (numOfJobs > 0)
            {
                this.numOfJobs = this.numOfJobs;
                InitJoblist();

                StartJobs();
            }


                
        }

        private void InitJoblist()
        {
            for (int i = 0; i < this.numOfJobs; i++)
            {
                asyncJobs.Add(new Task<string>(SlowDude));
            }
            Console.WriteLine("{0} tasks in list", asyncJobs.Count);
        }

        private string SlowDude()
        {
            Thread.Sleep(2000);
            const string response = "I'am done!";
            return response;
        }

        public async void StartJobs()
        {
            
        }
    }
}
