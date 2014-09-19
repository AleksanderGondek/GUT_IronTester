using System;
using System.Collections.Generic;
using NServiceBus;

namespace IronTester.Common.Commands
{
    public class PleaseDoTests : ICommand
    {
        public Guid RequestId { get; set; }
        public string SourceCodeLocation { get; set; }
        public ICollection<string> TestsRequested { get; set; }

        public PleaseDoTests()
        {
            RequestId = Guid.NewGuid();
        }
    }
}
