using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            //like in Strategy pattern, we want to build specific object(sandwich)
            //there are two types of sandwiches our store has
            //SandwichMaker class knows apply the order for right sandwich building
            //but it is not responsible for details or any logic - which meat or bread to put, etc.
            var sandwichMaker = new SandwichMaker(new MySandwichBuilder());
            sandwichMaker.BuildSandwich();
            var sandwich1 = sandwichMaker.GetSandwhich();

            sandwich1.Display();

            var sandwichMaker2 = new SandwichMaker(new ClubSandwichBuilder());
            sandwichMaker2.BuildSandwich();
            var sandwich2 = sandwichMaker2.GetSandwhich();

            sandwich2.Display();
            Console.ReadKey();
        }
    }
}
