using System;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Dependency injection (Simple way)
            var _resolver = new Resolver();

            var d1 = new RetreiveData(_resolver.ResolveProvider());
            var d2 = new RetreiveData(_resolver.ResolveProvider());

            Console.ReadKey();
        }
    }

    public class Resolver
    {
        //all business logic is here

        public IData ResolveProvider()
        {
            if (new Random().Next(2) == 1)
            {
                return new StringDataProvider();
            }

            return new DataBaseDataProvider();
        }
    }

    public class RetreiveData
    {
        public RetreiveData(IData p_data)
        {
            var data = p_data;
            data.SaveData();
            data.GetData();
        }
    }
}