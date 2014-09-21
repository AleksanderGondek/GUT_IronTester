using System;
using System.Collections.Generic;

namespace IronTester.Website.Models
{
    public class RequestCreationModel
    {
        public Guid RequestId { get; set; }
        public string SourceCodeLocation { get; set; }
        public string TestsRequested { get; set; }
    }
}