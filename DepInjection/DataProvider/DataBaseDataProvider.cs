using System;

namespace DepInjection.DataProvider
{
    public class DataBaseDataProvider : DataProviderCommon, IData
    {
        public object GetData()
        {
            //Get data from Data base
            Console.WriteLine("got data from " + GetDataProviderType(this));

            return null;
        }

        public bool SaveData()
        {
            return true;
        }
    }
}