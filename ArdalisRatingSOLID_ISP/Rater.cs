namespace ArdalisRating_OpenClosed
{
    public abstract class Rater
    {
        public ILogger Logger;
        public RatingEngine Engine;

        public Rater(IRaterContext con, RatingEngine engine)
        {
            Engine = engine;
            Logger = con.Logger;
        }

        public abstract void Rate(Policy policy);
    }
}