using System;
using NServiceBus;

namespace IronTester.Common.Messages.Tests
{
    public interface ITestsFinished : IMessage
    {
        Guid RequestId { get; set; }
        bool TestsSuccessful { get; set; }
        string TestsFailReason { get; set; }
        string TestsArtifactsLocation { get; set; }
    }
}
