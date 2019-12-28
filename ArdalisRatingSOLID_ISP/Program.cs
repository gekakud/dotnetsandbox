using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArdalisRating_OpenClosed
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ardalis Insurance Rating System Starting...");

            // ISP:
            // clients should not be forced to depend on methods they do not use
            // we must prefer small interfaces to large
            // ISP helps with SRP and LSP

            var concreteRaterContext = new DefaultJsonPolicyContext(new ConsoleLogger(), new JsonPolicyReader());
            var engine = new RatingEngine(concreteRaterContext);
            engine.Rate();

            if (engine.Rating > 0)
            {
                Console.WriteLine($"Rating: {engine.Rating}");
            }
            else
            {
                Console.WriteLine("No rating produced.");
            }

        }
    }

    //this for example violates ISP, because there are not implemented methods!
    //must be splitted into more "smaller" interfaces for JSON and XML
    public class JsonPolicyReader : IPolicyReader
    {
        public string ReadFromJsonFile(string path)
        {
            return File.ReadAllText(path);
        }

        public string ReadFromXmlFile(string path)
        {
            throw new NotImplementedException();
        }

        public Policy GetPolicyFromJsonString(string policyJson)
        {
            return JsonConvert.DeserializeObject<Policy>(policyJson,
                new StringEnumConverter());
        }

        public Policy GetPolicyFromXmlString(string policyJson)
        {
            throw new NotImplementedException();
        }
    }

    public interface IPolicyReader
    {
        string ReadFromJsonFile(string path);
        string ReadFromXmlFile(string path);

        Policy GetPolicyFromJsonString(string policyJson);
        Policy GetPolicyFromXmlString(string policyJson);
    }

    //concrete implementation for JSON and console logger
    //we also can implement XML based, only by adding its implementation
    public class DefaultJsonPolicyContext : IRaterContext
    {
        public ILogger Logger { get; set; }
        public IPolicyReader Reader { get; set; }

        public DefaultJsonPolicyContext(ILogger loggerImpl, IPolicyReader readerImpl)
        {
            Logger = loggerImpl;
            Reader = readerImpl;
        }

        public Policy GetPolicy()
        {
            Logger.Log("getting policy");
            // load policy - open file policy.json
            string policyJson = Reader.ReadFromJsonFile("policy.json");

            var policy = Reader.GetPolicyFromJsonString(policyJson);

            return policy;
        }
    }

    public interface IRaterContext
    {
        ILogger Logger { get; set; }
        IPolicyReader Reader { get; set; }
        Policy GetPolicy();
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public interface ILogger
    {
        void Log(string message);
    }
}
