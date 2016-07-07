using System;
using System.Collections.Generic;
using System.ServiceModel;
using GeoLib.Contracts;

namespace GeoLib.Proxies
{
    public class GeoClient : ClientBase<IGeoService>,IGeoService
    {
        public GeoData GetGeoData(string p_zipCode)
        {
            return Channel.GetGeoData(p_zipCode);
        }

        public IEnumerable<GeoData> GetDataInRange(string p_zipCode, int p_range)
        {
            return Channel.GetDataInRange(p_zipCode, p_range);
        }

        public GeoData Ping()
        {
            return Channel.Ping();
        }
    }
}