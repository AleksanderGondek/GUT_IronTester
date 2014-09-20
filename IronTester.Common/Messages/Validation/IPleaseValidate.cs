using System;
using System.Collections.Generic;
using NServiceBus;

namespace IronTester.Common.Messages.Validation
{
    public interface IPleaseValidate : IEvent
    {
        Guid RequestId { get; set; }
        ICollection<string> TestsRequested { get; set; }
    }
}
