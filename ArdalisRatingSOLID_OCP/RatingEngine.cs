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

        //there were too many responsibilities for RatingEngine: logging, persistence and encoding format
        //so we move this responsibilities to a separate classes
        public ConsoleLogger ConsoleLogger = new ConsoleLogger();
        PolicySource PolicySource = new PolicySource();
        PolicySerializer PolicySerializer = new PolicySerializer();


        //now rate method is open for extension of different types of policies
        //and closed for modifications - we do not need to change this method in order to add support
        //for new policy types
        public void Rate()
        {
            ConsoleLogger.Log("Starting rate.");
            ConsoleLogger.Log("Loading policy.");

            // load policy - open file policy.json
            string policyJson = PolicySource.ReadFromJsonFile("policy.json");

            var policy = PolicySerializer.GetPolicyFromJsonString(policyJson);

            //imagine we want add another rater for another policy type
            //we should move all logic to separate classes with abstraction of common methods and props
            //also it will make the logic of rating more testable and easy to extent
            var rater = RaterFactory.CreateRater(policy, this);
            rater?.Rate(policy);

            ConsoleLogger.Log("Rating completed.");
        }
    }
}