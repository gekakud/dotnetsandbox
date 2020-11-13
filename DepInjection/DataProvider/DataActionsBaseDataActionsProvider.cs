using System;

namespace DepInjection.DataProvider
{
    public class DataActionsBaseDataActionsProvider : DataProviderCommon, IDataActions
    {
        public object GetData()
        {
            //Get data from Data base
            Console.WriteLine("got data from " + GetDataProviderType(this));

            return null;
        }

        public bool SaveData(string p_data)
        {
            return true;
        }
    }
}