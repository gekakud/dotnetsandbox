using System;

namespace MySingleton
{
    public class Program
    {
        private static void Main()
        {
            //here we try to create two instances of MySingletonClass class, but as we expect
            //the MySingletonClass class handle only one. s and s1 are same objects!!!

            var s = MySingletonClass.Instance;
            var s1 = MySingletonClass.Instance;
            if (s == s1)
            {
                Console.WriteLine("same {0} class",s.GetType().Assembly.GetName().Name);
            }
            else
            {
                Console.WriteLine("OK");
            }

            //why this is lazy?
            //because we don't even think about instanciating, but just calling the method
            MySingletonLazyClass.SayHello();

            MySingletonLazyClass.SayHello();
            MySingletonLazyClass.SayHello();

            Console.ReadKey();
        }
    }

    public class MySingletonClass
    {
        //static is a thread-safe @JohnSkeet
        //readonly property instantiates only once while calling the constructor!
        private static readonly MySingletonClass instance = new MySingletonClass();

        private MySingletonClass()
        {
            //happen once!
            //do some init stuff here
            Console.WriteLine("In singleton ctor");
        }

        public static MySingletonClass Instance
        {
            get { return instance; }
        }
    }

    public class MySingletonLazyClass
    {
        private static readonly MySingletonLazyClass instance = new MySingletonLazyClass();

        //empty static constructor - forces laziness
        //garantees that the constructor will be called before first use
        //use this ctor for lazyness only
        static MySingletonLazyClass()
        {
            Console.WriteLine("In static lazy ctor");
        }

        private MySingletonLazyClass()
        {
            //happen once!
            //do some init stuff here
            Console.WriteLine("In MySingletonLazyClass ctor");
        }

        //first time we call MySingletonLazyClass.SayHello the static ctor garantee instance created
        public static void SayHello()
        {
            Console.WriteLine("Hello");
        }

        public static MySingletonLazyClass Instance
        {
            get { return instance; }
        }
    }
}