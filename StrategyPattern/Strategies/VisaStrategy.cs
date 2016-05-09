namespace StrategyPattern
{
    public class VisaStrategy : ValidationStrategy
    {
        public override bool IsCardValid(CreditCard p_card)
        {
            if (p_card.Number.ToString().StartsWith("4580"))
                return true;

            return false;
        }
    }
}