using System;
using Newtonsoft.Json;

namespace AutoMapperEx
{
    internal class Presenter
    {
        public static void ShowOnUi(CustomerViewItem p_customer)
        {
            Console.WriteLine("Mapped Object:");
            Console.WriteLine(JsonConvert.SerializeObject(p_customer, Formatting.Indented));
        }
    }
}