using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArdalisRating_OpenClosed
{
    public class ConsoleLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class PolicySource
    {
        public string ReadFromJsonFile(string path)
        {
            return File.ReadAllText(path);
        }
    }

    public class PolicySerializer
    {
        public Policy GetPolicyFromJsonString(string policyJson)
        {
            return JsonConvert.DeserializeObject<Policy>(policyJson,
                new StringEnumConverter());
        }
    }

    /// <summary>
    /// The RatingEngine reads the policy application details from a file and produces a numeric 
    /// rating value based on the details.
    /// </summary>
    public class RatingEngine
    {
        public decimal Rating { get; set; }

        public ConsoleLogger ConsoleLogger = new ConsoleLogger();
        PolicySource PolicySource = new PolicySource();
        PolicySerializer PolicySerializer = new PolicySerializer();

        public void Rate()
        {
            ConsoleLogger.Log("Starting rate.");
            ConsoleLogger.Log("Loading policy.");

            // load policy - open file policy.json
            string policyJson = PolicySource.ReadFromJsonFile("policy.json");

            var policy = PolicySerializer.GetPolicyFromJsonString(policyJson);

            var rater = RaterFactory.CreateRater(policy, this);

            // LSP - detecting LSP violations in code:
            // type checking with IS or AS in polymorphic code
            // Null check
            // NotImplementedExceptions

            //rater?.Rate(policy);
            //we can use C# feature to check for Nulls or ensure we do not return null from Factory
            //Null object pattern is also an option
            rater.Rate(policy);

            ConsoleLogger.Log("Rating completed.");
        }
    }
}