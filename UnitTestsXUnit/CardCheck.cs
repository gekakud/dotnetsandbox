using FluentAssertions;
using Xunit;
using StrategyPattern;

namespace UnitTestsXUnit
{
    public class CardCheck
    {
        [Fact]
        void CheckCardTypeIsCorrect()
        {
            var c = new CreditCard(CardType.Visa)
            {
                Cvv = 234,
                Name = "Moshe",
                Number = 458001124422
            };

            c.IsValid().Should().BeTrue();
        }
    }
}
