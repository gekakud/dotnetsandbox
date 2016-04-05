using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ
{
    class Program
    {
        // LINQ is a library full of method extensions for IEnumerable
        static void Main(string[] args)
        {
            //IEnumerable has only one function GetEnumerator();
            //Returns an enumerator that iterates through the collection.
            IEnumerable<string> cities = new[] {"London", "TLV", "Los Angeles", "Paris", "Kiev","Lillihamer"};

            //Lets create method extension for IEnumerable in order to find all strings in collection
            //that start with "L".
            //...so, we can retreive data as we do in SQL

            Console.WriteLine("Find all cities that start with L:");
            IEnumerable<string> basicQuery = cities.StringThatStartWith("L");
            foreach (var city in basicQuery)
            {
                Console.WriteLine(city);
            }

            //LINQ is just a bunch of extension methods for IEnumerable sitting in System.Linq library!!!
            //These methods could be splitted into Filtering, Grouping, Partitioning, Aggregation.... types
            //for instance, the Where method belong to Filtering
            //lets use Where to filter all cities start with "L" and print them in revese order

            Console.WriteLine("\nWith LINQ. Do the same and print in reverse order:");
            var linqQuery = cities.Where(p_x => p_x.StartsWith("L"));
            linqQuery = linqQuery.Reverse();

            foreach (var city in linqQuery)
            {
                Console.WriteLine(city);
            }

            //Now we will try to create a generic filter
            //we will pass a function(or delegate) to a LINQ as a parameter
            //we pass the function in order LINQ can figure out what we are trying to do

            Console.WriteLine("\nWith LINQ. Generic type, passing delegate:");

            var linqGenericQuery = cities.Filter(delegate(string p_item)
            {
                return p_item.StartsWith("L");
            });
            foreach (var city in linqGenericQuery)
            {
                Console.WriteLine(city);
            }

            Console.ReadKey();
        }
    }
}
