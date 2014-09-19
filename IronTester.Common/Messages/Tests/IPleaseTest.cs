using System;
using NServiceBus;

namespace IronTester.Common.Messages.Tests
{
    public interface IPleaseTest : IMessage
    {
        Guid RequestId { get; set; }
        string SourceCodeLocation { get; set; }
    }
}
