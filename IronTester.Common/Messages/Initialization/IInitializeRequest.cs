using System;
using NServiceBus;

namespace IronTester.Common.Messages.Initialization
{
    public interface IInitializeRequest : IMessage
    {
        Guid RequestId { get; set; }
        string SourceCodeLocation { get; set; }
    }
}
