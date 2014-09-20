using System.Collections.Generic;

namespace IronTester.Common.Metadata
{
    public static class ValidationData
    {
        public static readonly List<string> ValidTests = new List<string> { "Unit", "Integration", "Stress", "Regression" }; 
        public static readonly List<string> ValidInitializationPaths = new List<string> { @"C:\Something\", @"D:\SomethingElse\", @"E:\SomethingTotallyElse\", @"Z:\YouShouldLookHereYouKnow\" }; 
        public static readonly List<string> ValidInitializedFilesPaths = new List<string> { @"F:\InterestingPath\", @"G:\AnotherInterestingPath\", @"H:\SuperficialyInterestingPath\", @"I:\FBISurviellenceVan\" };
        public static readonly List<string> ValidBuildsArtifactsPaths = new List<string> {  @"J:\BuildArtifact1\", @"K:\BuildArtifact2\", @"L:\YouW8M8\", @"M:\KnockKnockJoke\" }; 
    }
}
