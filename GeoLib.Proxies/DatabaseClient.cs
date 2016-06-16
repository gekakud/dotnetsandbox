using System.Collections.Generic;
using GeoLib.Contracts;

namespace GeoLib.Proxies
{
    public class DatabaseClient : IDataProvider
    {
        public GeoData GetGeoData(string p_zipCode)
        {
            return new GeoData();
        }

        public IEnumerable<GeoData> GetDataInRange(string p_zipCode, int p_range)
        {
            return new List<GeoData>();
        }
    }
}