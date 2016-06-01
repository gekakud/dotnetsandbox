using BuilderPattern.Builders;

namespace BuilderPattern
{
    public class SandwichMaker
    {
        private readonly SandwichBuilder builder;

        public SandwichMaker(SandwichBuilder builder)
        {
            //we set our builder to a specific one
            //that builder knows exactly which ingridients to use
            //for creating desired sandwich
            this.builder = builder;
        }

        public void BuildSandwich()
        {
            builder.CreateNewSandwich();
            builder.PrepareBread();
            builder.ApplyMeatAndCheese();
            builder.ApplyVegetables();
            builder.AddCondiments();
        }

        public Sandwich GetSandwhich()
        {
            return builder.GetSandwich();
        }
    }
}
