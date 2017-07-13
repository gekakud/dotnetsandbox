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

            var ggg = new Data<IDataExtentionSimple>();
            ggg.Value.GetDataTypeName();

            Console.ReadKey();
        }
    }

    internal class Data<T> where T: IDataExtentionSimple
    {
        public Type DataType
        {
            get { return typeof(T); }
        }

        public T Value { get; set; }
    }

    internal interface IDataExtentionSimple
    {
        string GetDataTypeName();
    }

    internal interface IDataExtention<in A, out B>
    {
        string GetDataTypeName();
        void GetGenericParamType(A p_param);
        B ReturnSomeGenericType();
    }
}
