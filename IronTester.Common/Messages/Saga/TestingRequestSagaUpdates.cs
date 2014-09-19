using System;

namespace IronTester.Common.Messages.Saga
{
    public interface ITestingRequestSagaStateHasChanged
    {
        Guid RequestId { get; set; }
        int CurrentSagaState { get; set; }
    }

    public interface IProcessUpdate : ITestingRequestSagaStateHasChanged
    {
        decimal CurrentProgress { get; set; }
    }

    public interface IProcessFailed : ITestingRequestSagaStateHasChanged
    {
        string FailureReasons { get; set; }
    }
}
