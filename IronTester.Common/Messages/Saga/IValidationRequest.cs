using System;
using System.Collections.Generic;
using NServiceBus;

namespace IronTester.Common.Messages.Saga
{
    public interface IValidationRequest : IMessage
    {
        Guid RequestId { get; set; }
        string SourceCodeLocation { get; set; }
        ICollection<string> TestsRequested { get; set; }
    }
}
