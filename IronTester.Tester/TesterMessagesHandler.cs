using System;
using IronTester.Common.Messages.CancelRequest;
using IronTester.Common.Messages.Tests;
using NServiceBus;

namespace IronTester.Tester
{
    public class TesterMessagesHandler : IHandleMessages<IPleaseTest>, IHandleMessages<IPleaseCancel>
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
        public void Handle(IPleaseCancel message)
        {
            if (!TesterWorker.Requests.ContainsKey(message.RequestId)) return;
            RequestModel removedItem;
            TesterWorker.Requests.TryRemove(message.RequestId, out removedItem);
        }
    }
}
