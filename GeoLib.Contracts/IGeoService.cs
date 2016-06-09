using System.Collections.Generic;
using System.ServiceModel;

namespace GeoLib.Contracts
{
    [ServiceContract]
    public interface IGeoService
    {
        [OperationContract]
        GeoData GetGeoData(string p_zipCode);

        [OperationContract]
        IEnumerable<GeoData> GetDataInRange(string p_zipCode, int p_range);
    }
}