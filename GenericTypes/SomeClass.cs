using System;

namespace GenericTypes
{
    public class SomeClass<T> 
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
}