using System;

namespace ArdalisRating_OpenClosed
{
    public class AutoPolicyRater:Rater
    {
        public AutoPolicyRater(RatingEngine _ratingEngine, ConsoleLogger logger) : base(_ratingEngine, logger)
        {
        }

        public override void Rate(Policy policy)
        {
            ConsoleLogger.Log("Rating AUTO policy...");
            ConsoleLogger.Log("Validating policy.");

            if (String.IsNullOrEmpty(policy.Make))
            {
                ConsoleLogger.Log("Auto policy must specify Make");
                return;
            }
            if (policy.Make == "BMW")
            {
                if (policy.Deductible < 500)
                {
                    _engine.Rating = 1000m;
                }
                _engine.Rating = 900m;
            }
        }
    }
}