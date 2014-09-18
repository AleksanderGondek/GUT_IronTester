using System;
using NServiceBus;

namespace IronTester.Common.Commands
{
    public class PleaseRestart : ICommand
    {
        public Guid RequestId { get; set; }
        public string RestartReason { get; set; }
    }
}
