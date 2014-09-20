using System.Collections.Generic;

namespace IronTester.Common.Metadata
{
    public static class ValidTestRequests
    {
        public static readonly List<string> ValidTests = new List<string> { "Unit", "Integration", "Stress", "Regression" }; 
    }
}
