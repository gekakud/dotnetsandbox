using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Net;
namespace Reflection
{
    internal class Program
    {
        private const string FileToLoad = @"C:\Users\media\Desktop\dotnettutors\CommonUtilsLibrary\bin\Debug\CommonUtilsLibrary.dll";
        private const string SaveInPath = @"C:\Users\media\Desktop\dotnettutors\CommonUtilsLibrary\bin\Debug\";
        private const string FileName = "NewlyCreated.dll";
        private const string Link = "http://54.191.5.165:8080/get_file";
        private static void Main()
        {
            //RunExample();
            var byteArrayData = DownloadFromWeb(Link);
            if (byteArrayData == null || byteArrayData.Length==0)
            {
                Console.WriteLine("failed to get file");
                Console.ReadKey();
                return;
            }
            
            //var someBytes = File.ReadAllBytes(FileToLoad);

            ConvertBytesAndExecute(SaveInPath + FileName, byteArrayData);
        }

        private static void ConvertBytesAndExecute(string path, byte[] someBytes)
        {
            File.WriteAllBytes(path, someBytes);

            var newAssembly = Assembly.LoadFile(path);
            var r = CreateAndInvoke(newAssembly, "CommonUtilsLibrary.CommonApi", null, "GetMathComputations", new object[] { 45 });
            Console.WriteLine(r);
            Console.ReadKey();
        }

        private static byte[] DownloadFromWeb(string deepLink)
        {
            using (WebClient client = new WebClient())
            {
                byte[] arr;
                client.Headers["User-Agent"] =
                "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                "(compatible; MSIE 6.0; Windows NT 5.1; " +
                ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                // Download
                try
                {
                    arr = client.DownloadData(deepLink);
                    Console.WriteLine("--- WebClient result ---");
                    Console.WriteLine("size: " + arr.Length + "KB");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }

                return arr;            
            }
        }

        private static void RunExample()
        {
            try
            {
                var assembly =
                    Assembly.LoadFile(FileToLoad);

                IList<Type> listOfTypes = assembly.GetExportedTypes().ToList();

                //static function
                var commonUtilsClass = assembly.GetType("CommonUtilsLibrary.CommonUtils");
                var m = commonUtilsClass.GetMethod("PrintGreeting");
                m.Invoke(null, null);

                //non-static with params
                var commonApiClass = assembly.GetType("CommonUtilsLibrary.CommonApi");
                var method = commonApiClass.GetMethod("GetMathComputations");
                var commonApiClassInstance = Activator.CreateInstance(commonApiClass);
                var result = method.Invoke(commonApiClassInstance, new object[] { 45 });
                Console.WriteLine(result);

                //
                var r = CreateAndInvoke(assembly, "CommonUtilsLibrary.CommonApi", null, "GetMathComputations", new object[] { 45 });
                Console.WriteLine(r);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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