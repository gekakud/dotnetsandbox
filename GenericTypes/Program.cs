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

    internal class SomeClass<T> 
    {
        private T someArg;

        public SomeClass(T someArg)
        {
            this.someArg = someArg;
        }

        public void PrintMyType()
        {
            Console.WriteLine("Got {0} type", this.someArg.GetType());
        }
    }

    internal class SomeTypeClass
    {
        private int DummyData { get; set; }
    }
}
