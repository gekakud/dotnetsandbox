using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflection
{
    internal class Program
    {
        private static void Main()
        {
            var assembly =
                Assembly.LoadFile(
                    @"C:\Users\gekak_000\Desktop\MyTutorial\CommonUtilsLibrary\bin\Debug\CommonUtilsLibrary.dll");
            var myType = assembly.GetType("CommonUtilsLibrary.CommonUtils");

            IList<Type> listOfTypes = assembly.GetExportedTypes().ToList();
            var m = myType.GetMethod("PrintGreeting");
            m.Invoke(null, null);

            var instance = Activator.CreateInstance(myType);

            Console.ReadKey();
        }
    }
}