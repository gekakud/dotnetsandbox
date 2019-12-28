namespace ArdalisRating_OpenClosed
{
    public static class RaterFactory
    {
        public static Rater CreateRater(IRaterContext context, RatingEngine engine)
        {
            switch (context.GetPolicy().Type)
            {
                case PolicyType.Auto:
                    return new AutoPolicyRater(context, engine);

                case PolicyType.Land:
                    return new LandPolicyRater(context, engine);

                case PolicyType.Life:
                    return new LifePolicyRater(context, engine);

                default:
                    return new UnknownPolicyRater(context, engine);

            }
        }
    }
}