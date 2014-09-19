using System;
using NServiceBus;

namespace IronTester.Common.Messages.Initialization
{
    public interface IPleaseInitialize : IMessage
    {
        Guid RequestId { get; set; }
        string SourceCodeLocation { get; set; }
    }
}
