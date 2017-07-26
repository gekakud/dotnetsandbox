using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReflectionSimple
{
    internal class Program
    {
        private static void Main()
        {
            Directory.SetCurrentDirectory(@"..\..\..\");
            var path1 = Directory.GetCurrentDirectory();
            var path2 = @"CommonUtilsLibrary\bin\Debug\CommonUtilsLibrary.dll";
            var path3 = Path.Combine(path1, path2);
            var assembly =
                Assembly.LoadFile(path3);
            var myType = assembly.GetType("CommonUtilsLibrary.CommonUtils");

            IList<Type> listOfTypes = assembly.GetExportedTypes().ToList();
            var m = myType.GetMethod("PrintGreeting");
            m.Invoke(null, null);

            var instance = Activator.CreateInstance(myType);

            Console.ReadKey();
        }
    }
}