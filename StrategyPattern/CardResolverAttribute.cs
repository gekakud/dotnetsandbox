using System;

namespace StrategyPattern
{
    public class CardResolverAttribute : Attribute
    {
        public readonly Type Strategy;

        public CardResolverAttribute(Type p_strategy)
        {
            Strategy = p_strategy;
        }
    }
}