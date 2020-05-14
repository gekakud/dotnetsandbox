using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Check c = new Check();
            c.check();

            for (int i = 0; i < 2; i++,SleepHere())
            {
                var u = 6;
            }

            var t = 6;
        }

        static void SleepHere()
        {
            var y = 7;
        }
    }

    internal class Check
    {
        public void check(CancellationToken token = default)
        {
            var t = 6;
        }
    }
}
