using System;
using NServiceBus;

namespace IronTester.Common.Messages.Builds
{
    public interface IBuildsFinished : IMessage
    {
        Guid RequestId { get; set; }
        bool BuildSuccessful { get; set; }
        string BuildFailReason { get; set; }
        string PathToBuildArtifacts { get; set; }
    }
}
