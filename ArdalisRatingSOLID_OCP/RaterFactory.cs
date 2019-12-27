namespace ArdalisRating_OpenClosed
{
    public static class RaterFactory
    {
        public static Rater CreateRater(Policy policy,RatingEngine engine)
        {
            switch (policy.Type)
            {
                case PolicyType.Auto:
                    return new AutoPolicyRater(engine,engine.ConsoleLogger);

                case PolicyType.Land:
                    return new LandPolicyRater(engine, engine.ConsoleLogger);

                case PolicyType.Life:
                    return new LifePolicyRater(engine, engine.ConsoleLogger);


            }

            return null;
        }
    }
}