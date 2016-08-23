using System;

namespace CommonUtilsLibrary
{
    public class CommonUtils
    {
        public static void PrintGreeting()
        {
            Console.WriteLine("Hello human!");
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