namespace StrategyPattern
{
    public abstract class ValidationStrategy
    {
        public abstract bool IsCardValid(CreditCard p_card);
    }
}