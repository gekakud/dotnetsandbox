using System;
using System.Collections.Generic;
using GeoLib.Contracts;
using GeoLib.DataProvider;

namespace GeoLib.Services
{
    public class GeoManager : IGeoService
    {
        public DataRepository DataRepoTest;

        public GeoManager()
        {
        }

        public GeoManager(DataRepository p_repository)
        {
            DataRepoTest = p_repository;
        }

        public GeoData GetGeoData(string p_zipCode)
        {
            var provider = DataRepoTest ?? new DataRepository();
            return new GeoData
            {
                City = provider.GetCityByZip(p_zipCode),
                Country = provider.GetCountryByZip(p_zipCode),
                ZipCode = p_zipCode
            };
        }

        public IEnumerable<GeoData> GetDataInRange(string p_zipCode, int p_range)
        {
            throw new NotImplementedException();
        }
    }
}