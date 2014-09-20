using System;
using IronTester.Common.Messages.Saga;
using NServiceBus;

namespace IronTester.ManualMessagesPusher
{
    public class SafetyConsumer : IHandleMessages<ITestingRequestSagaStateHasChanged>, IHandleMessages<IProcessUpdate>, IHandleMessages<IProcessFailed>
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
    }
}
