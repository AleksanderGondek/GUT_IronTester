using System;
using System.Collections.Generic;
using NServiceBus;

namespace IronTester.Common.Messages.Tests
{
    public interface ITestsRequestConfirmation : IMessage
    {
        Guid RequestId { get; set; }
        bool WillTest { get; set; }
        string DenialReason { get; set; }
    }
}
