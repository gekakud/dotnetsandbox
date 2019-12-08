using System;

namespace CommonUtilsLibrary
{
    public class CommonUtils
    {
        public CommonUtils()
        {
            PrivateInteger = 50;
        }

        private int PrivateInteger { get; set; }

        public void PrintGreeting()
        {
            Console.WriteLine("Hello, I have {0} bucks", PrivateInteger);
        }
    }

    public class CommonDataStructures
    {
        public struct SomeDummyDataStruct
        {
            private int id;
            private int phone;
            private string name;
        }
    }

    public class CommonApi
    {
        public string GetSomeText()
        {
            return "hi";
        }

        public double GetMathComputations(int p_x)
        {
            return Math.Cos(p_x);
        }
    }
}