using System;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Dependency injection (Simple way)
            var programExcuter = new RetreiveData(new StringDataProvider());

            var diffrentExe = new RetreiveData(new DataBaseDataProvider());

            Console.ReadKey();
        }
    }

    public class RetreiveData
    {
        private readonly IData _data;

        public RetreiveData(IData p_data)
        {
            _data = p_data;
            _data.SaveData();
            _data.GetData();     
        }
    }
}