using System;

namespace GenericTypes
{
    internal class Program
    {
        private static void Main()
        {
            var s2 = new SomeClass<AnotherTypeClass>(new AnotherTypeClass());
            var s3 = new SomeClass<SomeTypeClass>(new SomeTypeClass());

            s2.PrintMyType();
            s3.PrintMyType();

            Console.ReadKey();
        }
    }
}
