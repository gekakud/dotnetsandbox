using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace SignalR
{
    class Program
    {
        static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8088";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR("/mainstream",new HubConfiguration());
        }
    }

    [HubName("CentralHub")]
    public class CentralHub : Hub
    {
        public void NotifyNewConnection(string name)
        {
            Console.WriteLine($"{name} joined chat");
            Clients.Others.messageFromServer("server",$"{name} joined chat");
        }

        public void SendToAll(string name, string message)
        {
            Console.WriteLine($"{name} sent message: {message}");
            Clients.All.messageFromServer(name, message);
        }
    }
}
