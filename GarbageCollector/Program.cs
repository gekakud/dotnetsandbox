using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarbageCollector
{
    class Program
    {
        //The finalizer method is called when your object is garbage collected and you have no guarantee when this will happen(you can force it, but it will hurt performance).

        //The Dispose method on the other hand is meant to be called by the code that created your class so that you can clean up
        //and release any resources you have acquired(unmanaged data, database connections, file handles, etc) the moment the code is done with your object.

        //The standard practice is to implement IDisposable and Dispose so that you can use your object in a using statment. Such as using(var foo = new MyObject()) { }.
        //And in your finalizer, you call Dispose, just in case the calling code forgot to dispose of you.
        
        static void Main(string[] args)
        {
        }
    }
}
