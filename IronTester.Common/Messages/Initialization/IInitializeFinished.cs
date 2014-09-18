using System;
using NServiceBus;

namespace IronTester.Common.Messages.Initialization
{
    public interface IInitializeFinished : IMessage
    {
        Guid RequestId { get; set; }
        bool InitializationSuccessful { get; set; }
        string InitializationFailReason { get; set; }
        string PathToInitializedFiles { get; set; }
    }
}
