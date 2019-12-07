using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Interoperability
{
    // P/Invoke is a technology that allows you to access structs, callbacks,
    // and functions in unmanaged libraries from your managed code
    class Program
    {
        // ********* Calling unmanaged(native) function in managed code ***********

        // Import user32.dll (containing the function we need) and define
        // the method corresponding to the native function.
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]

        //It defines a managed method that has the exact same signature as the unmanaged one
        // extern - tells the runtime this is an external method, and that when you invoke it,
        //the runtime should find it in the DLL specified in DllImport attribute
        private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

        // ********* END Calling unmanaged(native) function in managed code ***********

        //*********************************************************************************

        // ********* Invoking managed code from unmanaged code ****************

        // Define a delegate that corresponds to the unmanaged function.
        private delegate bool EnumWC(IntPtr hwnd, IntPtr lParam);

        // Import user32.dll (containing the function we need) and define
        // the method corresponding to the native function.
        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWC lpEnumFunc, IntPtr lParam);

        // Define the implementation of the delegate; here, we simply output the window handle.
        private static bool OutputWindow(IntPtr hwnd, IntPtr lParam)
        {
            Console.WriteLine(hwnd.ToInt64());
            return true;
        }
        // ********* END Invoking managed code from unmanaged code ****************

        public static void Main(string[] args)
        {
            // 1. Calling unmanaged(native) function in managed code
            // Invoke the function as a regular managed method.
            MessageBox(IntPtr.Zero, "Hello from native code", "Attention!", 0);


            // 2.Invoking managed code from unmanaged code
            // OutputWindow is a function defined in managed code which will
            // be called from unmanaged(native) code -- callbacks from native code into managed code
            EnumWindows(OutputWindow, IntPtr.Zero);

            Console.ReadKey();
        }
    }
}
