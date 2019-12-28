namespace ArdalisRating_OpenClosed
{
    class UnknownPolicyRater:Rater
    {
        public UnknownPolicyRater(IRaterContext con, RatingEngine engine) : base(con, engine)
        {
        }

        public override void Rate(Policy policy)
        {
            Logger.Log("Unknown policy: rate is undefined");
        }
    }
}
