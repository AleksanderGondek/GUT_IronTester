using IronTester.Common.Messages.Tests;
using NServiceBus;

namespace IronTester.Tester
{
    public class TesterMessagesHandler : IHandleMessages<IPleaseTest>
    {
        public IBus Bus { get; set; }

        public void Handle(IPleaseTest message)
        {
            var willHandleRequest = !TesterWorker.Requests.ContainsKey(message.RequestId);

            if (willHandleRequest)
            {
                TesterWorker.Requests.TryAdd(message.RequestId,
                    new RequestModel()
                    {
                        SourceCodeLocation = message.SourceCodeLocation,
                        WasProcessed = false,
                        IsValid = false,
                        ValidationFailReason = null
                    });
            }

            Bus.Publish<ITestsRequestConfirmation>(x =>
            {
                x.RequestId = message.RequestId;
                x.WillTest = willHandleRequest;
                x.DenialReason = willHandleRequest ? null : "Request already exists within tester!";
            });
        }
    }
}
