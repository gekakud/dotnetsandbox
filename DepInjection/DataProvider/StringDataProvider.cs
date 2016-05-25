using System;
using ConsoleApplication1.DataProvider;

namespace ConsoleApplication1
{
    public class StringDataProvider : DataProviderCommon, IData
    {
        public object GetData()
        {
            //Open file get data
            //convert to object
            //send request answer

            Console.WriteLine("got data from " + GetDataProviderType(this));

            return null;
        }

        public bool SaveData()
        {
            return true;
        }


        public void Convert()
        {
            ///Convert data
        }
    }
}