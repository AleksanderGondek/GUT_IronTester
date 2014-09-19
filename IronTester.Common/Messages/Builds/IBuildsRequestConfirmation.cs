using System;
using NServiceBus;

namespace IronTester.Common.Messages.Builds
{
    public interface IBuildsRequestConfirmation : IMessage
    {
        Guid RequestId { get; set; }
        bool WillBuild { get; set; }
        string DenialReason { get; set; }
    }
}
