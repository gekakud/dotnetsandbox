namespace ArdalisRating_OpenClosed
{
    public class LandPolicyRater : Rater
    {
        public LandPolicyRater(RatingEngine _ratingEngine, ConsoleLogger logger) : base(_ratingEngine, logger)
        {
        }

        public override void Rate(Policy policy)
        {
            ConsoleLogger.Log("Rating LAND policy...");
            ConsoleLogger.Log("Validating policy.");
            if (policy.BondAmount == 0 || policy.Valuation == 0)
            {
                ConsoleLogger.Log("Land policy must specify Bond Amount and Valuation.");
                return;
            }

            if (policy.BondAmount < 0.8m * policy.Valuation)
            {
                ConsoleLogger.Log("Insufficient bond amount.");
                return;
            }

            _engine.Rating = policy.BondAmount * 0.05m;
        }
    }
}