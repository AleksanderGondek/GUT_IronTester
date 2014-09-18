using System;
using NServiceBus;

namespace IronTester.Common.Commands
{
    public class PleaseCancelTests : ICommand
    {
        public Guid RequestId { get; set; }
        public bool ShouldCleanUp { get; set; }
    }
}
