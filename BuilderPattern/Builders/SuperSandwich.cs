using System;

namespace BuilderPattern.Builders
{
    class SuperSandwichBuilder : SandwichBuilder
    {
        public override void PrepareBread()
        {
            throw new NotImplementedException();
        }

        public override void ApplyMeatAndCheese()
        {
            throw new NotImplementedException();
        }

        public override void ApplyVegetables()
        {
            throw new NotImplementedException();
        }

        public override void AddCondiments()
        {
            throw new NotImplementedException();
        }
    }
}
