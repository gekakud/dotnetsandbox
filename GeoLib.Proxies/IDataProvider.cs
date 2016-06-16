using System;
using System.Collections.Generic;
using GeoLib.Contracts;

namespace GeoLib.Proxies
{
    public interface IDataProvider
    {
        GeoData GetGeoData(string p_zipCode);
        IEnumerable<GeoData> GetDataInRange(string p_zipCode, int p_range);
    }
}