using System;

namespace GenericTypes
{
    public class SomeClass<T> where T : ICache
    {
        private T someArg;

        public SomeClass(T someArg)
        {
            this.someArg = someArg;
        }

        public void PrintMyType()
        {
            someArg.Name();
        }
    }
}