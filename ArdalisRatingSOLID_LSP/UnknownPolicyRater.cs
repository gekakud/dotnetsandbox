using System;
using System.Collections.Generic;
using System.Text;

namespace ArdalisRating_OpenClosed
{
    class UnknownPolicyRater:Rater
    {
        public UnknownPolicyRater(RatingEngine _ratingEngine, ConsoleLogger logger) : base(_ratingEngine, logger)
        {
        }

        public override void Rate(Policy policy)
        {
            ConsoleLogger.Log("Unknown policy: rate is undefined");
        }
    }
}
