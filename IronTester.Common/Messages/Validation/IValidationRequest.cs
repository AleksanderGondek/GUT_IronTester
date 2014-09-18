using System;
using System.Collections.Generic;
using NServiceBus;

namespace IronTester.Common.Messages.Validation
{
    public interface IValidationRequest : IMessage
    {
        Guid RequestId { get; set; }
        string SourceCodeLocation { get; set; }
        ICollection<string> TestsRequested { get; set; }
    }
}
