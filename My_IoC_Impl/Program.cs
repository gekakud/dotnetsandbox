using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_IoC_Impl
{
    class Program
    {
        static void Main(string[] args)
        {
            IoC.Register<ISomeService,SomeServiceImplementation>(new SomeServiceImplementation());
            Console.WriteLine("Trying to resolve type...");
            ISomeService someImpl = IoC.Resolve<ISomeService>();
            someImpl.PrintGreeting();
            Console.ReadKey();
        }
    }

    interface ISomeService
    {
        void PrintGreeting();
    }

    class SomeServiceImplementation:ISomeService
    {
        public SomeServiceImplementation()
        {
            Console.WriteLine("Hello from {0} constructor!", this.GetType());
        }

        public void PrintGreeting()
        {
            Console.WriteLine("Hello there!");
        }
    }
}
