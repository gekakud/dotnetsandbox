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
            try
            {
                var assembly =
                    Assembly.LoadFile(
                        @"C:\Users\EvgenyK\Desktop\dotnettutors\CommonUtilsLibrary\bin\Debug\CommonUtilsLibrary.dll");

                IList<Type> listOfTypes = assembly.GetExportedTypes().ToList();

                var commonUtilsClass = assembly.GetType("CommonUtilsLibrary.CommonUtils");
                var m = commonUtilsClass.GetMethod("PrintGreeting");
                m.Invoke(null, null);


                var commonApiClass = assembly.GetType("CommonUtilsLibrary.CommonApi");
                var method = commonApiClass.GetMethod("GetMathComputations");
                var commonApiClassInstance = Activator.CreateInstance(commonApiClass);
                var result = method.Invoke(commonApiClassInstance, new object[] {45});
                Console.WriteLine(result);

                var r = CreateAndInvoke(assembly, "CommonUtilsLibrary.CommonApi", null, method.Name, new object[] {45});
                Console.WriteLine(r);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        public static object CreateAndInvoke(Assembly assembly, string p_typeName, object[] p_constructorArgs,
            string p_methodName, object[] p_methodArgs)
        {
            var type = assembly.GetType(p_typeName);
            var method = type.GetMethod(p_methodName);
            object instance;

            if (p_constructorArgs == null)
            {
                instance = Activator.CreateInstance(type);
            }
            else
            {
                instance = Activator.CreateInstance(type, p_constructorArgs);
            }

            return method.Invoke(instance, p_methodArgs);
        }
    }
}