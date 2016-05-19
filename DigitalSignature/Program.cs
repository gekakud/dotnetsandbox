using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignature
{
    class Program
    {
        static void Main(string[] args)
        {
           var r = new TestSignature();
            r.Encrypt();

            Console.ReadKey();
        }
    }

    public class TestSignature
    {
        public void Encrypt()
        {
            DigitalSignatureProvider.DigitalSignature dgs = new DigitalSignatureProvider.DigitalSignature();

            Console.WriteLine(dgs.SignDocument("path is here", "type of algo").ToString());
            
        }
    }

}
