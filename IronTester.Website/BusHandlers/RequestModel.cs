using System;

namespace IronTester.Website.BusHandlers
{
    public class RequestModel
    {
        public Guid RequestId { get; set; }
        public string CurrentSagaState { get; set; }
        public decimal CurrentProgress { get; set; }
        public string FailureReasons { get; set; }
    }
}