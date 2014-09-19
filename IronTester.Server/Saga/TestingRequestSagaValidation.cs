using System;
using IronTester.Common.Messages.Initialization;
using IronTester.Common.Messages.Validation;
using IronTester.Common.Metadata;
using NServiceBus;

namespace IronTester.Server.Saga
{
    public partial class TestingRequestSaga :   IHandleMessages<IValidationRequestConfirmation>, IHandleMessages<IValidationStatus>, IHandleMessages<IValidationFinished>
    {
        public void Handle(IValidationRequestConfirmation message)
        {
            if (!Convert.ToInt32(TestingRequestSagaStates.TestingRequested).Equals(Data.CurrentState)) return;

            Data.CurrentState = message.WillValidate ? Convert.ToInt32(TestingRequestSagaStates.ValidationStarted) : Convert.ToInt32(TestingRequestSagaStates.Failed);
            Data.ValidationDenialReason = message.DenialReson;

            NotifyOfSagaStateChange((TestingRequestSagaStates)Data.CurrentState, Data.ValidationDenialReason);
        }

        public void Handle(IValidationStatus message)
        {
            if (!Convert.ToInt32(TestingRequestSagaStates.ValidationStarted).Equals(Data.CurrentState)
                && !Convert.ToInt32(TestingRequestSagaStates.ValidationInProgress).Equals(Data.CurrentState))
            {
                return;
            }

            Data.CurrentState = Convert.ToInt32(TestingRequestSagaStates.ValidationInProgress);
            Data.ValidationProgress = message.Progress;

            NotifyOfSagaStateChangeProgress((TestingRequestSagaStates)Data.CurrentState, Data.ValidationProgress);
        }

        public void Handle(IValidationFinished message)
        {
            if (!Convert.ToInt32(TestingRequestSagaStates.ValidationInProgress).Equals(Data.CurrentState)) return;

            Data.ValidationSuccessful = message.ValidationSuccessful;
            Data.ValidationFailReason = message.ValidationFailReason;
            Data.CurrentState = message.ValidationSuccessful ? Convert.ToInt32(TestingRequestSagaStates.ValidationFinished) : Convert.ToInt32(TestingRequestSagaStates.Failed);

            NotifyOfSagaStateChange((TestingRequestSagaStates)Data.CurrentState, Data.ValidationFailReason);

            if (!Convert.ToInt32(TestingRequestSagaStates.Failed).Equals(Data.CurrentState)) return;

            Bus.Publish(Bus.CreateInstance<IPleaseInitialize>(
                x =>
                {
                    x.RequestId = Data.RequestId;
                    x.SourceCodeLocation = Data.SourceCodeLocation;
                }));
        }
    }
}
