using System;
using IronTester.Common.Messages.Tests;
using IronTester.Common.Metadata;
using NServiceBus;
using NServiceBus.Saga;

namespace IronTester.Server.Saga
{
    public partial class TestingRequestSaga : IHandleMessages<ITestsRequestConfirmation>, IHandleMessages<ITestsStatus>, IHandleMessages<ITestsFinished>,
        IHandleTimeouts<PossibleTestingLoop>
    {
        public void Handle(ITestsRequestConfirmation message)
        {
            if (!Convert.ToInt32(TestingRequestSagaStates.BuildsFinished).Equals(Data.CurrentState)) return;

            Data.CurrentState = message.WillTest ? Convert.ToInt32(TestingRequestSagaStates.TestsStarted) : Convert.ToInt32(TestingRequestSagaStates.Failed);
            Data.TestsDenialReason = message.DenialReason;

            NotifyOfSagaStateChange((TestingRequestSagaStates)Data.CurrentState, Data.TestsDenialReason);
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

            NotifyOfSagaStateChangeProgress((TestingRequestSagaStates)Data.CurrentState, Data.TestingProgress);
            RequestTimeout(TimeSpan.FromMinutes(30), new PossibleTestingLoop());
        }

        public void Handle(ITestsFinished message)
        {
            if (!Convert.ToInt32(TestingRequestSagaStates.TestsInProgress).Equals(Data.CurrentState)) return;

            Data.TestsSuccessful = message.TestsSuccessful;
            Data.TestsFailReason = message.TestsFailReason;
            Data.TestsArtifactsLocation = message.TestsArtifactsLocation;
            Data.CurrentState = message.TestsSuccessful ? Convert.ToInt32(TestingRequestSagaStates.Finished) : Convert.ToInt32(TestingRequestSagaStates.Failed);

            NotifyOfSagaStateChange((TestingRequestSagaStates)Data.CurrentState, Data.BuildsFailReason);
        }

        public void Timeout(PossibleTestingLoop state)
        {
            if (Convert.ToInt32(TestingRequestSagaStates.TestsInProgress).Equals(Data.CurrentState))
            {   //Tests are propably looped - fail saga
                //Cancel testing
                StopAllOperations();
                Data.CurrentState = Convert.ToInt32(TestingRequestSagaStates.Cancelled);
                NotifyOfSagaStateChange((TestingRequestSagaStates)Data.CurrentState, null);
                MarkAsComplete();   
            }
        }
    }
}
