using System;
using IronTester.Common.Messages.Initialization;
using IronTester.Common.Metadata;
using NServiceBus;

namespace IronTester.Server.Saga
{
    public partial class TestingRequestSaga : IHandleMessages<IInitializeRequestConfirmation>, IHandleMessages<IInitializeStatus>, IHandleMessages<IInitializeFinished>
    {
        public void Handle(IInitializeRequestConfirmation message)
        {
            if (Convert.ToInt32(TestingRequestSagaStates.ValidationFinished).Equals(Data.CurrentState)) return;

            Data.CurrentState = message.WillInitialize ? Convert.ToInt32(TestingRequestSagaStates.InitializationStarted) : Convert.ToInt32(TestingRequestSagaStates.Failed);
            Data.InitializationDenialReason = message.DenialReason;
        }

        public void Handle(IInitializeStatus message)
        {
            if (!Convert.ToInt32(TestingRequestSagaStates.InitializationStarted).Equals(Data.CurrentState)
                && !Convert.ToInt32(TestingRequestSagaStates.InitializationInProgress).Equals(Data.CurrentState))
            {
                return;
            }

            Data.CurrentState = Convert.ToInt32(TestingRequestSagaStates.InitializationInProgress);
            Data.InitializationProgress = message.Progress;
        }

        public void Handle(IInitializeFinished message)
        {
            if (Convert.ToInt32(TestingRequestSagaStates.InitializationInProgress).Equals(Data.CurrentState)) return;

            Data.InitializationSuccessful = message.InitializationSuccessful;
            Data.InitializationFailReason = message.InitializationFailReason;
            Data.PathToInitializedFiles = message.PathToInitializedFiles;
            Data.CurrentState = message.InitializationSuccessful ? Convert.ToInt32(TestingRequestSagaStates.InitializationFinished) : Convert.ToInt32(TestingRequestSagaStates.Failed);

            // TODO: Saga changed state notification

            if (Convert.ToInt32(TestingRequestSagaStates.Failed).Equals(Data.CurrentState)) return;
            // TODO: Request Builds
        }
    }
}
