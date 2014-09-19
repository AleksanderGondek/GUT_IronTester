using System;
using NServiceBus;

namespace IronTester.Common.Messages.Builds
{
    public interface IPleaseBuild : IMessage
    {
        Guid RequestId { get; set; }
        string SourceCodeLocation { get; set; }
    }
}
