namespace StrategyPattern
{
    public class AmericanExpressStrategy : ValidationStrategy
    {
        public override bool IsCardValid(CreditCard p_card)
        {
            if (p_card.Number.ToString().StartsWith("5566"))
                return true;

            return false;
        }
    }
}