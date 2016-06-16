using System;
using GeoLib.Proxies;

namespace GeoLib.ClientApp
{
    internal class DataModel
    {
        public delegate void StatusChangedEventHandler(object p_source, EventArgs p_args);
        
        public event StatusChangedEventHandler StatusChanged;

        private static DataModel s_dataModelInstance;

        public IDataProvider AnotherDummyDataProvider;
        public IDataProvider GeoServiceDataProvider;

        private DataModel()
        {
            //open several clients for each type of data source
            //1. you can get data from service
            //2. or you just fetch it from DB or any other source
            GeoServiceDataProvider = new GeoClient();

            //AnotherDummyDataProvider = new DatabaseClient();
        }

        public static DataModel Instance
        {
            get { return s_dataModelInstance ?? (s_dataModelInstance = new DataModel()); }
        }
    }
}