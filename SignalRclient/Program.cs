using System;
using Microsoft.AspNet.SignalR.Client;

namespace SignalRclient
{
    class Program
    {
        private static void Main()
        {
            var connection = new HubConnection("http://localhost:8088/mainstream");
            var hubProxy = connection.CreateHubProxy("CentralHub");

            Console.WriteLine("Enter your name");
            var name = Console.ReadLine();

            connection.Start().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");

                    hubProxy.On<string, string>("messageFromServer", (s1, s2) => {
                        Console.WriteLine(s1 + ": " + s2);
                    });

                    hubProxy.Invoke<string>("NotifyNewConnection", name);

                    while (true)
                    {
                        var message = Console.ReadLine();

                        if (string.IsNullOrEmpty(message))
                        {
                            break;
                        }

                        hubProxy.Invoke<string>("SendToAll", name, message).ContinueWith(task1 => {
                            if (task1.IsFaulted)
                            {
                                Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                            }
                            else
                            {
                                Console.WriteLine(task1.Result);
                            }
                        });
                    }
                }
            }).Wait();

            Console.Read();
            connection.Stop();
        }
    }
}
