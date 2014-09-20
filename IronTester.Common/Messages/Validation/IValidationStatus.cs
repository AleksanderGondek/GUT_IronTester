using System;
using NServiceBus;

namespace IronTester.Common.Messages.Validation
{
    public interface IValidationStatus : IEvent
    {
        Guid RequestId { get; set; }
        Decimal Progress { get; set; }
    }
}
