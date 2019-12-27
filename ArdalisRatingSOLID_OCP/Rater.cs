namespace ArdalisRating_OpenClosed
{
    public abstract class Rater
    {
        public ConsoleLogger ConsoleLogger = new ConsoleLogger();
        public RatingEngine _engine;

        public Rater(RatingEngine _ratingEngine, ConsoleLogger logger)
        {
            _engine = _ratingEngine;
            ConsoleLogger = logger;
        }

        public abstract void Rate(Policy policy);
    }
}