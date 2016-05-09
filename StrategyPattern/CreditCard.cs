using System;

namespace StrategyPattern
{
    public class CreditCard
    {
        public long Number { get; set; }
        public int Cvv { get; set; }
        public string Name { get; set; }

        private ValidationStrategy strategy;

        public CreditCard(CardType p_validationStrategy)
        {
            //TODO: check null
            var attr = p_validationStrategy.GetAttributeOfType<CardResolverAttribute>();

            var r = Activator.CreateInstance(attr.Strategy);
            this.strategy = r as ValidationStrategy;
        }

        public bool IsValid()
        {
            return this.strategy.IsCardValid(this);
        }      
    }
}