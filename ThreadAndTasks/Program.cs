using System;

namespace ThreadAndTaskTutorial
{ 
    class Program
    {
        private const int NumOfJobs = 10;

        static void Main()
        {
            ThreadsExecutor jeExecutor = new ThreadsExecutor(NumOfJobs);
            jeExecutor.StartJobs();

            TasksExecutor teExecutor = new TasksExecutor(NumOfJobs);
            teExecutor.StartJobs();

            Console.ReadKey();
        }

        
    }
}
