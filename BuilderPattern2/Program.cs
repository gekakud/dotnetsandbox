namespace BuilderPattern2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Pizza p = new PizzaBuilder().AddCheese().AddOlives().SetPizzaSize();
        }
    }

    public class PizzaBuilder
    {
        private readonly Pizza _pizzaInProduction = new Pizza();

        public Pizza Build()
        {
            return new Pizza
            {
                Cheese = _pizzaInProduction.Cheese,
                XLsize = _pizzaInProduction.XLsize,
                ExtraCheese = _pizzaInProduction.ExtraCheese,
                Olives = _pizzaInProduction.Olives,
                Onions = _pizzaInProduction.Onions
            };
        }

        public static implicit operator Pizza(PizzaBuilder p_builder)
        {
            return p_builder.Build();
        }

        public PizzaBuilder AddCheese()
        {
            _pizzaInProduction.Cheese = true;
            return this;
        }

        public PizzaBuilder AddOnion()
        {
            _pizzaInProduction.Onions = true;
            return this;
        }

        public PizzaBuilder AddOlives()
        {
            _pizzaInProduction.Olives = true;
            return this;
        }

        public PizzaBuilder SetPizzaSize()
        {
            _pizzaInProduction.XLsize = true;
            return this;
        }

        public PizzaBuilder SetDoubleCheese()
        {
            _pizzaInProduction.ExtraCheese = true;
            return this;
        }
    }

    public class Pizza
    {
        private double _cost;

        public Pizza()
        {
            _cost = 0;
        }

        public bool Onions { get; set; }
        public bool Olives { get; set; }
        public bool Cheese { get; set; }
        public bool XLsize { get; set; }
        public bool ExtraCheese { get; set; }

        public double GetPizzaCost()
        {
            return _cost;
        }

        public void SetDiscount(double p_discount)
        {
            _cost = _cost*p_discount;
        }
    }
}