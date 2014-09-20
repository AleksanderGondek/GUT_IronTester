using System;
using IronTester.Common.Messages.Builds;
using IronTester.Common.Messages.Tests;
using IronTester.Common.Metadata;
using NServiceBus;

namespace IronTester.Server.Saga
{
    public partial class TestingRequestSaga : IHandleMessages<IBuildsRequestConfirmation>, IHandleMessages<IBuildsStatus>, IHandleMessages<IBuildsFinished>
    {
        public void Handle(IBuildsRequestConfirmation message)
        {
            if (!Convert.ToInt32(TestingRequestSagaStates.InitializationFinished).Equals(Data.CurrentState)) return;

            Data.CurrentState = message.WillBuild ? Convert.ToInt32(TestingRequestSagaStates.BuildsStarted) : Convert.ToInt32(TestingRequestSagaStates.Failed);
            Data.BuildsDenialReason = message.DenialReason;

            NotifyOfSagaStateChange((TestingRequestSagaStates)Data.CurrentState, Data.BuildsDenialReason);
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

            NotifyOfSagaStateChangeProgress((TestingRequestSagaStates)Data.CurrentState, Data.BuildsProgress);
        }

        public void Handle(IBuildsFinished message)
        {
            if (!Convert.ToInt32(TestingRequestSagaStates.BuildsInProgress).Equals(Data.CurrentState)) return;

            Data.BuildsSuccessful = message.BuildsSuccessful;
            Data.BuildsFailReason = message.BuildsFailReason;
            Data.PathToBuildsArtifacts = message.PathToBuildsArtifacts;
            Data.CurrentState = message.BuildsSuccessful ? Convert.ToInt32(TestingRequestSagaStates.BuildsFinished) : Convert.ToInt32(TestingRequestSagaStates.Failed);

            NotifyOfSagaStateChange((TestingRequestSagaStates)Data.CurrentState, Data.BuildsFailReason);

            if (Convert.ToInt32(TestingRequestSagaStates.Failed).Equals(Data.CurrentState)) return;
            
            Bus.Publish(Bus.CreateInstance<IPleaseTest>(
                x =>
                {
                    x.RequestId = Data.RequestId;
                    x.SourceCodeLocation = Data.PathToBuildsArtifacts;
                }));
        }
    }
}
