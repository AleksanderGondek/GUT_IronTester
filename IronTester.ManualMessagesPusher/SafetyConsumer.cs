using System;
using IronTester.Common.Messages.Builds;
using IronTester.Common.Messages.Initialization;
using IronTester.Common.Messages.Saga;
using IronTester.Common.Messages.Tests;
using IronTester.Common.Messages.Validation;
using NServiceBus;

namespace IronTester.ManualMessagesPusher
{
    public class SafetyConsumer : IHandleMessages<ITestingRequestSagaStateHasChanged>, IHandleMessages<IProcessUpdate>, IHandleMessages<IProcessFailed>,
        IHandleMessages<IPleaseBuild>, IHandleMessages<IPleaseTest>
    {
        public void Handle(ITestingRequestSagaStateHasChanged message)
        {
            Console.WriteLine("BOOP");
        }

        public void Handle(IProcessUpdate message)
        {
            Console.WriteLine("BOOP");
        }

        public void Handle(IProcessFailed message)
        {
            Console.WriteLine("BOOP");
        }

        public void Handle(IPleaseValidate message)
        {
            Console.WriteLine("BOOP");
        }

        public void Handle(IPleaseInitialize message)
        {
            Console.WriteLine("BOOP");
        }

        public void Handle(IPleaseBuild message)
        {
            Console.WriteLine("BOOP");
        }

        public void Handle(IPleaseTest message)
        {
            Console.WriteLine("BOOP");
        }
    }
}
