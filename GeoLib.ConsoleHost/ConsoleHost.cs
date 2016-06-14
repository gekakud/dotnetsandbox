using System;
using System.ServiceModel;
using GeoLib.Contracts;
using GeoLib.Services;

namespace GeoLib.ConsoleHost
{
    internal class ConsoleHost
    {
        private static void Main()
        {
            var host = new ServiceHost(typeof (GeoManager));

            const string address = "net.tcp://localhost:6789/GeoService";
            var bind = new NetTcpBinding();
            var contract = typeof (IGeoService);

            try
            {
                host.AddServiceEndpoint(contract, bind, address);
                host.Open();
            }
            catch (Exception)
            {
                host.Close();
                return;
            }

            Console.WriteLine("Console hosting service is running. Press [Enter] for exit.");
            Console.ReadKey();

            host.Close();
        }
    }
}