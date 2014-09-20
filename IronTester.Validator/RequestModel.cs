using System.Collections.Generic;

namespace IronTester.Validator
{
    public class RequestModel
    {
        public bool WasProcessed { get; set; }
        public ICollection<string> TestsRequested { get; set; }
        public bool IsValid { get; set; }
        public string ValidationFailReason { get; set; }
    }
}
