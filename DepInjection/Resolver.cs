using System;
using DepInjection.DataProvider;

namespace DepInjection
{
    public class Resolver
    {
        //all business logic is here

        public IData ResolveProvider()
        {
            if (new Random().Next(2) == 1)
            {
                return new StringDataProvider();
            }

            return new DataBaseDataProvider();
        }
    }
}