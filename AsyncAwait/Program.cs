using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    public class Program
    {
        static void Main(string[] args)
        {
            Example();

            Console.ReadKey();
        }

        static async void Example()
        {
            Executer e = new Executer();

            bool res = await e.StartAsyncTask();

            Console.WriteLine("HAHA");
        }
    }

    public class Executer
    {
        public async Task<bool> StartAsyncTask()
        {
            try
            {
                await Task.Run(() => { Console.WriteLine("First await action"); })
                        .ContinueWith(prevTask =>
                        {
                            Thread.Sleep(3000);
                            Console.WriteLine("Second await action with prev task status:" + prevTask.Status);
                        });
            }
            catch (Exception e)
            {

                Console.WriteLine("GOT exceptio:" + e.Message);
            }

            return true;
        }

        public async Task<bool> StartAsyncTaskWithException()
        {
            try
            {
                await Task.Run(() => { Console.WriteLine("First await action"); })
                    .ContinueWith(prevTask =>
                    {
                        throw new Exception("hhh");
                        Thread.Sleep(3000);
                        Console.WriteLine("Second await action with prev task status:" + prevTask.Status);
                    });
            }
            catch (Exception e)
            {

                Console.WriteLine("GOT exceptio:" + e.Message);
            }

            return true;
        }
    }
}
