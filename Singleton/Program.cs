using System;

namespace MySingleton
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //here we try to create two instances of MySingletonClass class, but as we expect
            //the MySingletonClass class handle only one. s and s1 are same objects!!!

            var s = MySingletonClass.Instance;
            var s1 = MySingletonClass.Instance;
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

    public class MySingletonClass
    {
        //static is a thread-safe @JohnSkeet
        private static readonly MySingletonClass instance = new MySingletonClass();

        private MySingletonClass()
        {
            //do some init stuff here
            Console.WriteLine("In singleton ctor");
        }

        public static MySingletonClass Instance
        {
            get { return instance; }
        }
    }
}