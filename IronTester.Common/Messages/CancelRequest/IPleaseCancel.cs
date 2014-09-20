using System;
using NServiceBus;

namespace IronTester.Common.Messages.CancelRequest
{
    public interface IPleaseCancel : IEvent
    {
        Guid RequestId { get; set; }
    }
}
