using System;
using IronTester.Common.Messages.CancelRequest;
using IronTester.Common.Messages.Validation;
using NServiceBus;

namespace IronTester.Validator
{
    public class ValidationMessagesHandler : IHandleMessages<IPleaseValidate>, IHandleMessages<IPleaseCancel>
    {
        public IBus Bus { get; set; }

        public void Handle(IPleaseValidate message)
        {
            var willHandleRequest = !ValidationWorker.Requests.ContainsKey(message.RequestId);

            if (willHandleRequest)
            {
                ValidationWorker.Requests.TryAdd(message.RequestId,
                    new RequestModel()
                    {
                        TestsRequested = message.TestsRequested,
                        WasProcessed = false,
                        IsValid = false,
                        ValidationFailReason = null
                    });
            }

            Bus.Publish<IValidationRequestConfirmation>(x =>
                                                                           {
                                                                               x.RequestId = message.RequestId;
                                                                               x.WillValidate = willHandleRequest;
                                                                               x.DenialReson = willHandleRequest ? null : "Request already exists within validator";
                                                                           });
        }
        public void Handle(IPleaseCancel message)
        {
            if (!ValidationWorker.Requests.ContainsKey(message.RequestId)) return;
            RequestModel removedItem;
            ValidationWorker.Requests.TryRemove(message.RequestId, out removedItem);
        }
    }
}
