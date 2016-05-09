namespace StrategyPattern
{
    public enum CardType
    {
        [CardResolver(typeof(VisaStrategy))]
        Visa,
        [CardResolver(typeof(MasterCardStrategy))]
        MasterCard,
        [CardResolver(typeof(AmericanExpressStrategy))]
        AmericanExpress
    }
}