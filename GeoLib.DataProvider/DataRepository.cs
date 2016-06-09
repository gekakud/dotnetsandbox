namespace GeoLib.DataProvider
{
    public class DataRepository
    {
        public string GetCityByZip(string p_zipCode)
        {
            return "Karmiel";
        }

        public string GetCountryByZip(string p_zipCode)
        {
            return "Israel";
        }

        public string GetCoordinatesByZip(string p_zipCode)
        {
            return "Lat Long";
        }
    }
}