namespace BuilderPattern.Builders
{

    //each sandwich in our store implements part or all from SandwichBuilder abstract class
    //it holds sandwich object being created after applying logic of ingridients
    public abstract class SandwichBuilder
    {
        protected Sandwich sandwich;

        public Sandwich GetSandwich()
        {
            return sandwich;
        }

        public void CreateNewSandwich()
        {
             sandwich = new Sandwich();
        }

        public abstract void PrepareBread();
        public abstract void ApplyMeatAndCheese();
        public abstract void ApplyVegetables();
        public abstract void AddCondiments();
    }
}
