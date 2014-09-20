using IronTester.Common.Messages.Initialization;
using NServiceBus;

namespace IronTester.Initializer
{
    public class InitializationMessagesHandler : IHandleMessages<IPleaseInitialize>
    {
        public IBus Bus { get; set; }

        public void Handle(IPleaseInitialize message)
        {
            var willHandleRequest = !InitializationWorker.Requests.ContainsKey(message.RequestId);

            if (willHandleRequest)
            {
                InitializationWorker.Requests.TryAdd(message.RequestId,
                    new RequestModel()
                    {
                        SourceCodeLocation = message.SourceCodeLocation,
                        WasProcessed = false,
                        IsValid = false,
                        ValidationFailReason = null
                    });
            }

            Bus.Publish<IInitializeRequestConfirmation>(x =>
            {
                x.RequestId = message.RequestId;
                x.WillInitialize = willHandleRequest;
                x.DenialReason = willHandleRequest ? null : "Request already exists within initializator";
            });
        }
    }
}
