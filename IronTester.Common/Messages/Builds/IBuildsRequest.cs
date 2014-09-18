using System;
using NServiceBus;

namespace IronTester.Common.Messages.Builds
{
    public interface IBuildsRequest : IMessage
    {
        Guid RequestId { get; set; }
        string SourceCodeLocation { get; set; }
    }
}
