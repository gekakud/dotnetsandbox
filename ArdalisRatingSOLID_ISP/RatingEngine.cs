using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArdalisRating_OpenClosed
{
    /// <summary>
    /// The RatingEngine reads the policy application details from a file and produces a numeric 
    /// rating value based on the details.
    /// </summary>
    public class RatingEngine
    {
        public decimal Rating { get; set; }

        // ISP - imagine all this moved to a IRaterContext as project grow
        private ILogger Logger;
        private IRaterContext RaterContext;
//        PolicySource PolicySource = new PolicySource();
//        PolicySerializer PolicySerializer = new PolicySerializer();

        public RatingEngine(IRaterContext context)
        {
            RaterContext = context;
            Logger = context.Logger;
        }
        
        public void Rate()
        {
            Logger.Log("Starting rate.");
            Logger.Log("Loading policy.");

            var rater = RaterFactory.CreateRater(RaterContext, this);

            rater.Rate(RaterContext.GetPolicy());

            Logger.Log("Rating completed.");
        }
    }
}