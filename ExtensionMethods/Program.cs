using System;

namespace ExtensionMethods
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //For method extension the following is required:
            //1. new static class and static method
            //2. keyword "this extensionType" are passed to a static method 
            //3. a new extension must appear at a list of methods for extensionType objects
            var check = new MyExtCheck("Hello,I have four spaces here");

            Console.ReadKey();
        }
    }
}