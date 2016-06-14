using System.Collections.Generic;
using GeoLib.Contracts;
using GeoLib.Proxies;

namespace GeoLib.ClientApp
{
    internal class DataModel
    {
        private static DataModel s_dataModelInstance;
        public GeoClient GeoServiceDataProvider;
        public List<GeoData> AnotherDummyDataProvider; 

        private DataModel()
        {
            GeoServiceDataProvider = new GeoClient();
            AnotherDummyDataProvider = new List<GeoData>();
        }

        public static DataModel Instance
        {
            get { return s_dataModelInstance ?? (s_dataModelInstance = new DataModel()); }
        }
    }
}