using System;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dependency injection (Simple way)
            Run programExcuter = new Run(new StringDataProvider());

            Run diffrentExe = new Run(new DataBaseDataProvider());

            Console.ReadKey();
        }
    }

    public class Run
    {
        private readonly IData _data;

        public Run(IData p_data)
        {
            _data = p_data;
            _data.GetData();
            _data.SaveData();
        }
    } 
}
