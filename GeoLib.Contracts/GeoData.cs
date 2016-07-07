using System.Runtime.Serialization;

namespace GeoLib.Contracts
{
    [DataContract]
    public class GeoData
    {
        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public int Status { get; set; }
    }
}