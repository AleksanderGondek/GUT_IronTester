using System;
using IronTester.Common.Commands;
using NServiceBus;

namespace IronTester.Server
{
    public class TestHandler : IHandleMessages<PleaseDoTests>
    {
        public IBus Bus { get; set; }

        public void Handle(PleaseDoTests command)
        {
            Console.Out.WriteLine("IronTesterServer has received {0} command.", command.GetType().Name);
            Console.Out.WriteLine("Command RequestId: {0} ", command.RequestId);
            Console.Out.WriteLine("Command SourceCodeLocation: {0} ", command.SourceCodeLocation);
            Console.Out.WriteLine("Command TestsRequested: {0} ", command.TestsRequested);
        }
    }
}
