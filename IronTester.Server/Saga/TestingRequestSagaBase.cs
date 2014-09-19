using System;
using IronTester.Common.Commands;
using IronTester.Common.Messages.Builds;
using IronTester.Common.Messages.Initialization;
using IronTester.Common.Messages.Saga;
using IronTester.Common.Messages.Tests;
using IronTester.Common.Messages.Validation;
using IronTester.Common.Metadata;
using NServiceBus;
using NServiceBus.Saga;

namespace IronTester.Server.Saga
{
    public partial class TestingRequestSaga : Saga<TestingRequestData>, IAmStartedByMessages<PleaseDoTests>, IHandleMessages<PleaseCancelTests>, IHandleMessages<PleaseRestart>
    {
        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<PleaseDoTests>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<PleaseCancelTests>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<PleaseRestart>(m => m.RequestId).ToSaga(s => s.RequestId);

            ConfigureMapping<IValidationRequestConfirmation>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<IValidationStatus>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<IValidationFinished>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<IInitializeRequestConfirmation>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<IInitializeStatus>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<IInitializeFinished>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<IBuildsRequestConfirmation>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<IBuildsStatus>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<IBuildsFinished>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<ITestsRequestConfirmation>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<ITestsStatus>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<ITestsFinished>(m => m.RequestId).ToSaga(s => s.RequestId);
        }

        public void Handle(PleaseDoTests command)
        {
            Data.RequestId = command.RequestId;        
            Data.SourceCodeLocation = command.SourceCodeLocation;
            Data.TestsRequested = command.TestsRequested;
            Data.CurrentState = Convert.ToInt32(TestingRequestSagaStates.TestingRequested);

            // Saga changed state notification
            NotifyOfSagaStateChange((TestingRequestSagaStates)Data.CurrentState, null);

            // Request validation
            Bus.Publish(Bus.CreateInstance<IPleaseValidate>(
                x =>
                {
                    x.RequestId = Data.RequestId;
                    x.TestsRequested = Data.TestsRequested;
                }));
        }


        public void Handle(PleaseCancelTests message)
        {
            if (!StopAllOperations()) return;

            Data.CurrentState = Convert.ToInt32(TestingRequestSagaStates.Cancelled);
        }

        public void Handle(PleaseRestart message)
        {
            if (!StopAllOperations()) return;

            TestingRequestData.WipeClean(Data);
            Data.CurrentState = Convert.ToInt32(TestingRequestSagaStates.TestingRequested);
        }

        private bool StopAllOperations()
        {
            // TODO: Logic responisble for stoping Validation/Initialization/etc
            return true;
        }

        private void NotifyOfSagaStateChange(TestingRequestSagaStates state, string errorText)
        {
            if (TestingRequestSagaStates.Failed == state)
            {
                Bus.Publish(Bus.CreateInstance<IProcessFailed>(
                    x =>
                    {
                        x.RequestId = Data.RequestId;
                        x.CurrentSagaState = Data.CurrentState;
                        x.FailureReasons = errorText;
                    }));                
            }
            else
            {
                Bus.Publish(Bus.CreateInstance<ITestingRequestSagaStateHasChanged>(
                    x =>
                    {
                        x.RequestId = Data.RequestId;
                        x.CurrentSagaState = Data.CurrentState;
                    }));
            }
        }

        private void NotifyOfSagaStateChangeProgress(TestingRequestSagaStates state, decimal progress)
        {
            Bus.Publish(Bus.CreateInstance<IProcessUpdate>(
                x =>
                {
                    x.RequestId = Data.RequestId;
                    x.CurrentSagaState = Data.CurrentState;
                    x.CurrentProgress = progress;
                }));            
        }
    }
}
