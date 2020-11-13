using System;

namespace DepInjection.DataProvider
{
    public class RandomResolver
    {
        //all business logic is here

        public IDataActions ResolveProvider()
        {
            if (new Random().Next(2) == 1)
            {
                return new StringDataActionsProvider();
            }

            return new DataActionsBaseDataActionsProvider();
        }
    }
}