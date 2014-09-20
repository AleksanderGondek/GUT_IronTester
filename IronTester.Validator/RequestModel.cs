using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
