using IronTester.Common.Commands;
using IronTester.Common.Messages.Builds;
using IronTester.Common.Messages.Initialization;
using IronTester.Common.Messages.Tests;
using IronTester.Common.Messages.Validation;
using NServiceBus;
using NServiceBus.Saga;

namespace IronTester.Server.Saga
{
    public class TestingRequestSaga : Saga<TestingRequestData>, 
                                        IAmStartedByMessages<PleaseDoTests>, 
                                        IHandleMessages<IValidationRequestConfirmation>,
                                        IHandleMessages<IValidationStatus>, 
                                        IHandleMessages<IValidationFinished>,
                                        IHandleMessages<IInitializeRequestConfirmation>,
                                        IHandleMessages<IInitializeStatus>,
                                        IHandleMessages<IInitializeFinished>,
                                        IHandleMessages<IBuildsRequestConfirmation>,
                                        IHandleMessages<IBuildsStatus>,
                                        IHandleMessages<IBuildsFinished>,
                                        IHandleMessages<ITestsRequestConfirmation>,
                                        IHandleMessages<ITestsStatus>,
                                        IHandleMessages<ITestsFinished>
    {

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<PleaseDoTests>(m => m.RequestId).ToSaga(s => s.RequestId);
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
            Data.CurrentState = "Testing Requested";

            // TODO: Saga changed state notification
            // TODO: Request validation
        }

        public void Handle(IValidationRequestConfirmation message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.CurrentState = message.WillValidate ? "Validation Started" : "Failed";
            Data.ValidationDenialReason = message.DenialReson;
        }

        public void Handle(IValidationStatus message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.CurrentState = "Validation in progress";
            Data.ValidationProgress = message.Progress;
        }

        public void Handle(IValidationFinished message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.ValidationSuccessful = message.ValidationSuccessful;
            Data.ValidationFailReason = message.ValidationFailReason;
            Data.CurrentState = message.ValidationSuccessful ? "Validation Finished" : "Failed";

            // TODO: Saga changed state notification

            if ("Failed".Equals(Data.CurrentState)) return;
            // TODO: Request Initialization
        }

        public void Handle(IInitializeRequestConfirmation message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.CurrentState = message.WillInitialize ? "Initialization Started" : "Failed";
            Data.InitializationDenialReason = message.DenialReason;
        }

        public void Handle(IInitializeStatus message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.CurrentState = "Initialization in progress";
            Data.InitializationProgress = message.Progress;
        }

        public void Handle(IInitializeFinished message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.InitializationSuccessful = message.InitializationSuccessful;
            Data.InitializationFailReason = message.InitializationFailReason;
            Data.PathToInitializedFiles = message.PathToInitializedFiles;
            Data.CurrentState = message.InitializationSuccessful ? "Initialization Finished" : "Failed";

            // TODO: Saga changed state notification

            if ("Failed".Equals(Data.CurrentState)) return;
            // TODO: Request Builds
        }


        public void Handle(IBuildsRequestConfirmation message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.CurrentState = message.WillBuild ? "Builds Started" : "Failed";
            Data.BuildsDenialReason = message.DenialReason;
        }

        public void Handle(IBuildsStatus message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.CurrentState = "Builds in progress";
            Data.BuildsProgress = message.Progress;
        }

        public void Handle(IBuildsFinished message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.BuildsSuccessful = message.BuildsSuccessful;
            Data.BuildsFailReason = message.BuildsFailReason;
            Data.PathToBuildsArtifacts = message.PathToBuildsArtifacts;
            Data.CurrentState = message.BuildsSuccessful ? "Builds Finished" : "Failed";

            // TODO: Saga changed state notification

            if ("Failed".Equals(Data.CurrentState)) return;
            // TODO: Request Tests
        }

        public void Handle(ITestsRequestConfirmation message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.CurrentState = message.WillTest ? "Tests Started" : "Failed";
            Data.TestsDenialReason = message.DenialReason;
        }

        public void Handle(ITestsStatus message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.CurrentState = "Tests in progress";
            Data.TestingProgress = message.Progress;
        }

        public void Handle(ITestsFinished message)
        {
            if ("Failed".Equals(Data.CurrentState)) return;

            Data.TestsSuccessful = message.TestsSuccessful;
            Data.TestsFailReason = message.TestsFailReason;
            Data.TestsArtifactsLocation = message.TestsArtifactsLocation;
            Data.CurrentState = message.TestsSuccessful ? "Finished" : "Failed";

            // TODO: Saga changed state notification

            if ("Failed".Equals(Data.CurrentState)) return;
            //TODO: Saga finished notification
        }
    }
}
