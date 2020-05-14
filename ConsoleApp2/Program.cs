using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //running on threadpool threads
            IProgress<string> progress = new Progress<string>(str =>
            {
                Console.WriteLine(
                    $"Progress handled by TID: {Thread.CurrentThread.ManagedThreadId}::progress message=> {str}");
                Console.Out.Flush();
            });

            var tasks = new Task[5];
            for (var i = 0; i < 5; i++)
            {
                var j = i;
                var t = Task.Run(async () =>
                {
                    await Task.Delay(500);
                    progress.Report($"started! ThreadID: {Thread.CurrentThread.ManagedThreadId}\tNum: {j + 1}");

                    await Task.Delay(500);
                    progress.Report($"running! ThreadID: {Thread.CurrentThread.ManagedThreadId}\tNum: {j + 1}");

                    await Task.Delay(500);
                    progress.Report($"finished! ThreadID: {Thread.CurrentThread.ManagedThreadId}\tNum: {j + 1}");
                });
                tasks[i] = t;
            }

            await Task.WhenAll(tasks);

            Console.ReadKey();
        }
    }
}
