using System;
using NServiceBus;

namespace IronTester.Common.Commands
{
    public class PleaseThrowException : ICommand
    {
        public Guid RequestId { get; set; }
        public Exception Exception { get; set; }
    }
}
