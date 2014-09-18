using System;
using NServiceBus;

namespace IronTester.Common.Messages.Initialization
{
    public interface IInitializeStatus : IMessage
    {
        Guid RequestId { get; set; }
        Decimal Progress { get; set; }
    }
}
