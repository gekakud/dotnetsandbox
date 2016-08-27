using System;

namespace ThreadAndTaskTutorial
{ 
    public class Program
    {
        private const int NumOfJobs = 10;

        static void Main()
        {
            StartJobs(NumOfJobs);
        }

        public static void StartJobs(int p_numOfJobs)
        {
            ThreadsExecutor jeExecutor = new ThreadsExecutor(p_numOfJobs);
            jeExecutor.StartJobs();

            TasksExecutor teExecutor = new TasksExecutor(p_numOfJobs);
            teExecutor.StartJobs();

            teExecutor.ShowResults();
            Console.ReadKey();
        }
        
    }
}
