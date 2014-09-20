using System.Collections.Generic;

namespace IronTester.Common.Metadata
{
    public static class ValidationData
    {
        public static readonly List<string> ValidTests = new List<string> { "Unit", "Integration", "Stress", "Regression" }; 
        public static readonly List<string> ValidInitializationPaths = new List<string> { @"C:\Something\", @"D:\SomethingElse\", @"E:\SomethingTotallyElse\", @"Z:\YouShouldLookHereYouKnow\"}; 
    }
}
