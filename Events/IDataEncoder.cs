using System.Security.Cryptography.X509Certificates;

namespace Events
{
    interface IDataEncoder
    {
        void EncodeData(DataFile data);
    }
}