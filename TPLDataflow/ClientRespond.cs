using System;

namespace TPLDataflow
{
    public class ClientRespond
    {
        public string Data { get; set; }
        public Guid RequestId { get; set; }
        public bool IsError { get; set; }
    }
}