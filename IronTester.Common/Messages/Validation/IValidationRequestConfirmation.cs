using System;
using NServiceBus;

namespace IronTester.Common.Messages.Validation
{
    public interface IValidationRequestConfirmation : IEvent
    {
        Guid RequestId { get; set; }
        bool WillValidate { get; set; }
        string DenialReson { get; set; }
    }
}
