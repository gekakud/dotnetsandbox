using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Dog
    {
        public static implicit operator Cat(Dog p_dog)
        {
            return new Cat();
        }
    }

    class Cat
    {
        
 

    }

    class Program
    {
        static void Main(string[] args)
        {

            Cat a = new Dog();


        }
    }
}
