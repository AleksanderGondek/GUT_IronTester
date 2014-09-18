using System;
using NServiceBus;

namespace IronTester.Common.Commands
{
    public class PleaseFail : ICommand
    {
        public Guid RequestId { get; set; }
        public string FailType { get; set; }
    }
}
