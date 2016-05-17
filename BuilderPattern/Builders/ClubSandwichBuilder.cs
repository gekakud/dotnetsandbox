using System.Collections.Generic;
using BuilderPattern.Builders;

namespace BuilderPattern
{
    public class ClubSandwichBuilder : SandwichBuilder
    {
        //logic of preparation and ingridients for this sandwich type

        public override void AddCondiments()
        {
            sandwich.HasMayo = true;
            sandwich.HasMustard = true;
        }

        public override void ApplyVegetables()
        {
            sandwich.Vegetables = new List<string> {"Tomato", "Onion", "Lettuce"};
        }

        public override void ApplyMeatAndCheese()
        {
            sandwich.CheeseType = CheeseType.Swiss;
            sandwich.MeatType = MeatType.Turkey;
        }

        public override void PrepareBread()
        {
            sandwich.BreadType = BreadType.White;
            sandwich.IsToasted = true;
        }
    }
}