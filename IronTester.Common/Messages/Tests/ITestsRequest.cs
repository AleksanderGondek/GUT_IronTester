using System;
using System.Collections.Generic;
using NServiceBus;

namespace IronTester.Common.Messages.Tests
{
    public interface ITestsRequest : IMessage
    {
        Guid RequestId { get; set; }
        string FilesToTestLocation { get; set; }
        ICollection<string> TestsRequested { get; set; }
    }
}
