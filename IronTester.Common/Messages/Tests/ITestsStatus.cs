using System;
using NServiceBus;

namespace IronTester.Common.Messages.Tests
{
    public interface ITestsStatus : IMessage
    {
        Guid RequestId { get; set; }
        Decimal Progress { get; set; }
    }
}
