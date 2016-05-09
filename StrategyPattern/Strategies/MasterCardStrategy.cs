namespace StrategyPattern
{
    public class MasterCardStrategy : ValidationStrategy
    {
        public override bool IsCardValid(CreditCard p_card)
        {
            if (p_card.Number.ToString().StartsWith("3140"))
                return true;

            return false;
        }
    }
}