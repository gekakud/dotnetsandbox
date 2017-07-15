using System;

namespace TPLDataflow
{
    public enum TypeOfClientRequest
    {
        Get,//0
        Set,
        Update,
        Find
    }

    public class ClientRequest
    {
        public TypeOfClientRequest ActionType { get; set; }
        public string Payload { get; set; }
        public Guid RequestId { get; set; }
    }
}