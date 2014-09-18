using System;
using NServiceBus;

namespace IronTester.Common.Messages.Builds
{
    public interface IBuildsStatus : IMessage
    {
        Guid RequestId { get; set; }
        Decimal Progress { get; set; }
    }
}
