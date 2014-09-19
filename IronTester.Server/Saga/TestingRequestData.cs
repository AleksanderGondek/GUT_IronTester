using System;
using System.Collections.Generic;
using NServiceBus.Saga;

namespace IronTester.Server.Saga
{
    public class TestingRequestData : ContainSagaData
    {
        public Guid RequestId { get; set; }
        public string CurrentState { get; set; }

        // Request Related Fields
        public string SourceCodeLocation { get; set; }
        public ICollection<string> TestsRequested { get; set; }

        // Validation Related Fields
        public Decimal ValidationProgress { get; set; }
        public bool ValidationSuccessful { get; set; }
        public string ValidationFailReason { get; set; }

        // Initialization Related Fields
        public Decimal InitializationProgress { get; set; }
        public bool InitializationSuccessful { get; set; }
        public string InitializationFailReason { get; set; }
        public string PathToInitializedFiles { get; set; }

        // Builds Related Fields
        public Decimal BuildsProgress { get; set; }
        public bool BuildsSuccessful { get; set; }
        public string BuildsFailReason { get; set; }
        public string PathToBuildsArtifacts { get; set; }

        // Test Related Fields
        public Decimal TestingProgress { get; set; }
        public bool TestsSuccessful { get; set; }
        public string TestsFailReason { get; set; }
        public string TestsArtifactsLocation { get; set; }
    }
}
