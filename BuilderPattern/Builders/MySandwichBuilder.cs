using System.Collections.Generic;

namespace BuilderPattern.Builders
{
    public class MySandwichBuilder : SandwichBuilder
    {
        //logic of preparation and ingridients for this sandwich type

        public override void AddCondiments()
        {
            sandwich.HasMayo = false;
            sandwich.HasMustard = true;
        }

        public override void ApplyVegetables()
        {
            sandwich.Vegetables = new List<string> { "Tomato", "Onion" };
        }

        public override void ApplyMeatAndCheese()
        {
            sandwich.CheeseType = CheeseType.Cheddar;
            sandwich.MeatType = MeatType.Turkey;
        }

        public override void PrepareBread()
        {
            sandwich.BreadType = BreadType.White;
            sandwich.IsToasted = true;
        }
    }
}
