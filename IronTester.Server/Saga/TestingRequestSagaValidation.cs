using System;
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
        }

        public void Handle(IValidationFinished message)
        {
            if (Convert.ToInt32(TestingRequestSagaStates.ValidationInProgress).Equals(Data.CurrentState)) return;

            Data.ValidationSuccessful = message.ValidationSuccessful;
            Data.ValidationFailReason = message.ValidationFailReason;
            Data.CurrentState = message.ValidationSuccessful ? Convert.ToInt32(TestingRequestSagaStates.ValidationFinished) : Convert.ToInt32(TestingRequestSagaStates.Failed);

            // TODO: Saga changed state notification

            if (Convert.ToInt32(TestingRequestSagaStates.Failed).Equals(Data.CurrentState)) return;
            // TODO: Request Initialization
        }
    }
}
