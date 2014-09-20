namespace IronTester.Common.Metadata
{
    public enum TestingRequestSagaStates
    {
        Failed = -1,
        Cancelled = 0,
        TestingRequested = 1,
        ValidationStarted = 2,
        ValidationInProgress = 3,
        ValidationFinished = 4,
        InitializationStarted = 5,
        InitializationInProgress = 6,
        InitializationFinished = 7,
        BuildsStarted = 8,
        BuildsInProgress = 9,
        BuildsFinished = 10,
        TestsStarted = 11,
        TestsInProgress = 12,
        Finished = 13
    }
}
