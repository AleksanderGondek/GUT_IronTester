using System;
using IronTester.Common.Commands;
using IronTester.Common.Messages.Validation;
using NServiceBus;
using NServiceBus.Saga;

namespace IronTester.Server.Saga
{
    public class TestingRequestSaga : Saga<TestingRequestData>, 
                                        IAmStartedByMessages<PleaseDoTests>, 
                                        IHandleMessages<IValidationRequestConfirmation>,
                                        IHandleMessages<IValidationStatus>, 
                                        IHandleMessages<IValidationFinished>
    {
        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<PleaseDoTests>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<IValidationRequestConfirmation>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<IValidationStatus>(m => m.RequestId).ToSaga(s => s.RequestId);
            ConfigureMapping<IValidationFinished>(m => m.RequestId).ToSaga(s => s.RequestId);
        }

        public void Handle(PleaseDoTests command)
        {
            Console.Out.WriteLine("IronTesterServer has received {0} command.", command.GetType().Name);
            Console.Out.WriteLine("Command RequestId: {0} ", command.RequestId);
            Console.Out.WriteLine("Command SourceCodeLocation: {0} ", command.SourceCodeLocation);
            Console.Out.WriteLine("Command TestsRequested: {0} ", command.TestsRequested);

            Data.RequestId = command.RequestId;
            Data.SourceCodeLocation = command.SourceCodeLocation;
            Data.TestsRequested = command.TestsRequested;

            Data.CurrentState = "New";
            Console.Out.WriteLine("TestingRequestSaga ({0}) state changed to: {1}", Data.RequestId.ToString("D"), Data.CurrentState);

            //Start validation
            
        }

        public void Handle(IValidationRequestConfirmation message)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(IValidationStatus message)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(IValidationFinished message)
        {
            throw new System.NotImplementedException();
        }
    }
}
