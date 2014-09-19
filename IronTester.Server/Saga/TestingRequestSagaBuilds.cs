using System;
using IronTester.Common.Messages.Builds;
using IronTester.Common.Metadata;
using NServiceBus;

namespace IronTester.Server.Saga
{
    public partial class TestingRequestSaga : IHandleMessages<IBuildsRequestConfirmation>, IHandleMessages<IBuildsStatus>, IHandleMessages<IBuildsFinished>
    {
        public void Handle(IBuildsRequestConfirmation message)
        {
            if (Convert.ToInt32(TestingRequestSagaStates.InitializationFinished).Equals(Data.CurrentState)) return;

            Data.CurrentState = message.WillBuild ? Convert.ToInt32(TestingRequestSagaStates.BuildsStarted) : Convert.ToInt32(TestingRequestSagaStates.Failed);
            Data.BuildsDenialReason = message.DenialReason;
        }

        public void Handle(IBuildsStatus message)
        {
            if (!Convert.ToInt32(TestingRequestSagaStates.BuildsStarted).Equals(Data.CurrentState)
                && !Convert.ToInt32(TestingRequestSagaStates.BuildsInProgress).Equals(Data.CurrentState))
            {
                return;
            }

            Data.CurrentState = Convert.ToInt32(TestingRequestSagaStates.BuildsInProgress);
            Data.BuildsProgress = message.Progress;
        }

        public void Handle(IBuildsFinished message)
        {
            if (Convert.ToInt32(TestingRequestSagaStates.BuildsInProgress).Equals(Data.CurrentState)) return;

            Data.BuildsSuccessful = message.BuildsSuccessful;
            Data.BuildsFailReason = message.BuildsFailReason;
            Data.PathToBuildsArtifacts = message.PathToBuildsArtifacts;
            Data.CurrentState = message.BuildsSuccessful ? Convert.ToInt32(TestingRequestSagaStates.BuildsFinished) : Convert.ToInt32(TestingRequestSagaStates.Failed);

            // TODO: Saga changed state notification

            if (Convert.ToInt32(TestingRequestSagaStates.Failed).Equals(Data.CurrentState)) return;
            // TODO: Request Tests
        }
    }
}
