using StrategyPattern;

namespace UnitTestsNUnit
{
    public class TestObjectInstance
    {
        public CreditCard c;
        public TestObjectInstance()
        {
            c = new CreditCard(CardType.Visa)
            {
                Cvv = 123,
                Name = "Bugagaga",
                Number = 4580445544333
            };
        }
    }
}