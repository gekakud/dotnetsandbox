using System;

namespace AutoMapperEx
{
    internal class DataProvider
    {
        public static Customer GetCustomer()
        {
            return new Customer
            {
                DateOfBirth = DateTime.Now,
                FirstName = "Geka",

                ID = 3244444,
                NumberOfOrders = 22,
                Address = "karmiel"
            };
        }
    }
}