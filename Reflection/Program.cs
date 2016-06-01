using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflection
{
    internal class Program
    {
        private static void Main()
        {
            var assembly =
                Assembly.LoadFile(
                    @"C:\Users\EvgenyK\Desktop\dotnettutors\CommonUtilsLibrary\bin\Debug\CommonUtilsLibrary.dll");
            var myType = assembly.GetType("CommonUtilsLibrary.CommonUtils");

            IList<Type> listOfTypes = assembly.GetExportedTypes().ToList();
            var m = myType.GetMethod("PrintGreeting");
            m.Invoke(null, null);

            var instance = Activator.CreateInstance(myType);

            Pizza p = new PizzaBuilder().AddCheese().AddOlives().SetPizzaSize();


            Console.ReadKey();
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

        public PizzaBuilder AddCheese()
        {
            _pizzaInProduction.Cheese = true;
            //WTF?
            _pizzaInProduction.Cheese = false;
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

        public static implicit operator Pizza(PizzaBuilder p_builder)
        {
            return p_builder.Build();
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