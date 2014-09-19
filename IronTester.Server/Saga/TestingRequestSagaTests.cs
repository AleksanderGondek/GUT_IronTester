using System;
using IronTester.Common.Messages.Tests;
using IronTester.Common.Metadata;
using NServiceBus;

namespace IronTester.Server.Saga
{
    public partial class TestingRequestSaga : IHandleMessages<ITestsRequestConfirmation>, IHandleMessages<ITestsStatus>, IHandleMessages<ITestsFinished>
    {
        public void Handle(ITestsRequestConfirmation message)
        {
            if (Convert.ToInt32(TestingRequestSagaStates.BuildsFinished).Equals(Data.CurrentState)) return;

            Data.CurrentState = message.WillTest ? Convert.ToInt32(TestingRequestSagaStates.TestsStarted) : Convert.ToInt32(TestingRequestSagaStates.Failed);
            Data.TestsDenialReason = message.DenialReason;
        }

        public void Handle(ITestsStatus message)
        {
            if (!Convert.ToInt32(TestingRequestSagaStates.TestsStarted).Equals(Data.CurrentState)
                && !Convert.ToInt32(TestingRequestSagaStates.TestsInProgress).Equals(Data.CurrentState))
            {
                return;
            }

            Data.CurrentState = Convert.ToInt32(TestingRequestSagaStates.TestsInProgress);
            Data.TestingProgress = message.Progress;
        }

        public void Handle(ITestsFinished message)
        {
            if (Convert.ToInt32(TestingRequestSagaStates.TestsInProgress).Equals(Data.CurrentState)) return;

            Data.TestsSuccessful = message.TestsSuccessful;
            Data.TestsFailReason = message.TestsFailReason;
            Data.TestsArtifactsLocation = message.TestsArtifactsLocation;
            Data.CurrentState = message.TestsSuccessful ? Convert.ToInt32(TestingRequestSagaStates.Finished) : Convert.ToInt32(TestingRequestSagaStates.Failed);

            // TODO: Saga changed state notification

            if (Convert.ToInt32(TestingRequestSagaStates.Failed).Equals(Data.CurrentState)) return;
            //TODO: Saga finished notification
        }
    }
}
