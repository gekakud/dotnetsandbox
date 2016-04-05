using System;

namespace GenericTypes
{
    internal class Program
    {
        private static void Main()
        {
            var s1 = new SomeClass<string>("hello");
            var s2 = new SomeClass<int>(10);
            var s3 = new SomeClass<SomeTypeClass>(new SomeTypeClass());

            s1.PrintMyType();
            s2.PrintMyType();
            s3.PrintMyType();

            Console.ReadKey();
        }
    }
}
