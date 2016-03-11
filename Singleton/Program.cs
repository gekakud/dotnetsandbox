using System;

namespace Singleton
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //here we try to create two instances of Singleton class, but as we expect
            //the Singleton class handle only one. s and s1 are same objects!!!

            var s = Singleton.Instance;
            var s1 = Singleton.Instance;
            if (s == s1)
            {
                Console.WriteLine("same shit");
            }
            else
            {
                Console.WriteLine("OK");
            }

            Console.ReadKey();
        }
    }

    internal class Singleton
    {
        //static is a thread-safe @JohnSkeet
        private static readonly Singleton instance = new Singleton();

        private Singleton()
        {
            //do some init stuff here
            Console.WriteLine("In singleton ctor");
        }

        public static Singleton Instance
        {
            get { return instance; }
        }
    }
}