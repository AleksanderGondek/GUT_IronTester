using System;
using System.Collections.Generic;
using NServiceBus.Saga;

namespace IronTester.Server.Saga
{
    public class TestingRequestData : ContainSagaData
    {
        public Guid RequestId { get; set; }
        public int CurrentState { get; set; }

        // Request Related Fields
        public string SourceCodeLocation { get; set; }
        public ICollection<string> TestsRequested { get; set; }

        // Validation Related Fields
        public string ValidationDenialReason { get; set; }
        public Decimal ValidationProgress { get; set; }
        public bool ValidationSuccessful { get; set; }
        public string ValidationFailReason { get; set; }

        // Initialization Related Fields
        public string InitializationDenialReason { get; set; }
        public Decimal InitializationProgress { get; set; }
        public bool InitializationSuccessful { get; set; }
        public string InitializationFailReason { get; set; }
        public string PathToInitializedFiles { get; set; }

        // Builds Related Fields
        public string BuildsDenialReason { get; set; }
        public Decimal BuildsProgress { get; set; }
        public bool BuildsSuccessful { get; set; }
        public string BuildsFailReason { get; set; }
        public string PathToBuildsArtifacts { get; set; }

        // Test Related Fields
        public string TestsDenialReason { get; set; }
        public Decimal TestingProgress { get; set; }
        public bool TestsSuccessful { get; set; }
        public string TestsFailReason { get; set; }
        public string TestsArtifactsLocation { get; set; }
        
        public static void WipeClean(TestingRequestData data)
        {
            data.CurrentState = Convert.ToInt32(Common.Metadata.TestingRequestSagaStates.TestingRequested);

            data.ValidationDenialReason = null;
            data.ValidationFailReason = null;
            data.ValidationProgress = 0.0m;
            data.ValidationSuccessful = false;

            data.InitializationDenialReason = null;
            data.InitializationFailReason = null;
            data.InitializationProgress = 0.0m;
            data.InitializationSuccessful = false;
            data.PathToInitializedFiles = null;

            data.BuildsDenialReason = null;
            data.BuildsFailReason = null;
            data.BuildsProgress = 0.0m;
            data.BuildsSuccessful = false;
            data.PathToBuildsArtifacts = null;

            data.TestsDenialReason = null;
            data.TestsFailReason = null;
            data.TestingProgress = 0.0m;
            data.TestsSuccessful = false;
            data.TestsArtifactsLocation = null;
        }
    }
}
