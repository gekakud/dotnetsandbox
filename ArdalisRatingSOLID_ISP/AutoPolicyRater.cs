using System;

namespace ArdalisRating_OpenClosed
{
    public class AutoPolicyRater:Rater
    {
        public AutoPolicyRater(IRaterContext con, RatingEngine engine) : base(con, engine)
        {
        }

        public override void Rate(Policy policy)
        {
            Logger.Log("Rating AUTO policy...");
            Logger.Log("Validating policy.");

            if (String.IsNullOrEmpty(policy.Make))
            {
                Logger.Log("Auto policy must specify Make");
                return;
            }
            if (policy.Make == "BMW")
            {
                if (policy.Deductible < 500)
                {
                    Engine.Rating = 1000m;
                }
                Engine.Rating = 900m;
            }
        }
    }
}