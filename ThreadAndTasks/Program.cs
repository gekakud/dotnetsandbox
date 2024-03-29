﻿using System;

namespace ThreadAndTaskTutorial
{ 
    public class Program
    {
        private const int NumOfJobs = 20;

        static void Main()
        {
            StartJobs(NumOfJobs);

            DeadlockExample de = new DeadlockExample();

            de.SimulateDeadlock();
        }

        public static void StartJobs(int p_numOfJobs)
        {
            ThreadsExecutor jeExecutor = new ThreadsExecutor(p_numOfJobs);
            jeExecutor.StartJobs();

            Console.WriteLine("Enter to proceed");
            Console.ReadKey();
            TasksExecutor teExecutor = new TasksExecutor(p_numOfJobs);
            teExecutor.StartJobs();

            teExecutor.ShowResults();

            Console.ReadKey();
        }
        
    }
}
