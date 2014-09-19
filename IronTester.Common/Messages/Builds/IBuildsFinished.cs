using System;
using NServiceBus;

namespace IronTester.Common.Messages.Builds
{
    public interface IBuildsFinished : IMessage
    {
        Guid RequestId { get; set; }
        bool BuildsSuccessful { get; set; }
        string BuildsFailReason { get; set; }
        string PathToBuildsArtifacts { get; set; }
    }
}
