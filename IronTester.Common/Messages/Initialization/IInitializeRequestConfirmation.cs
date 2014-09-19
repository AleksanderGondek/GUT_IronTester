using System;
using NServiceBus;

namespace IronTester.Common.Messages.Initialization
{
    public interface IInitializeRequestConfirmation : IMessage
    {
        Guid RequestId { get; set; }
        bool WillInitialize { get; set; }
        string DenialReason { get; set; }
    }
}
