using System;
using System.Globalization;
using System.Linq;
using IronTester.Common.Messages.Saga;
using IronTester.Common.Metadata;
using Microsoft.AspNet.SignalR;
using NServiceBus;

namespace IronTester.Website.BusHandlers
{
    public class MessagesHandler : IHandleMessages<ITestingRequestSagaStateHasChanged>, IHandleMessages<IProcessUpdate>, IHandleMessages<IProcessFailed>
    {
        public void Handle(ITestingRequestSagaStateHasChanged message)
        {
            var requestModel = new RequestModel()
                               {
                                   RequestId = message.RequestId,
                                   CurrentSagaState = Enum.GetName(typeof (TestingRequestSagaStates), message.CurrentSagaState),
                                   CurrentProgress = 0.0m,
                                   FailureReasons = null
                               };

            AllRequestsHub.Requests.AddOrUpdate(requestModel.RequestId, requestModel, (key, oldValue) => requestModel);

            var context = GlobalHost.ConnectionManager.GetHubContext<AllRequestsHub>();
            context.Clients.All.requestStateChanged(requestModel.RequestId.ToString("D"), 
                requestModel.CurrentSagaState, 
                requestModel.CurrentProgress.ToString(CultureInfo.InvariantCulture),
                requestModel.FailureReasons);

            SendStats();
        }

        public void Handle(IProcessUpdate message)
        {
            var requestModel = new RequestModel()
            {
                RequestId = message.RequestId,
                CurrentSagaState = Enum.GetName(typeof(TestingRequestSagaStates), message.CurrentSagaState),
                CurrentProgress = message.CurrentProgress,
                FailureReasons = null
            };

            AllRequestsHub.Requests.AddOrUpdate(requestModel.RequestId, requestModel, (key, oldValue) => requestModel);

            var context = GlobalHost.ConnectionManager.GetHubContext<AllRequestsHub>();
            context.Clients.All.requestStateChanged(requestModel.RequestId.ToString("D"),
                requestModel.CurrentSagaState,
                requestModel.CurrentProgress.ToString(CultureInfo.InvariantCulture),
                requestModel.FailureReasons);

            SendStats();
        }

        public void Handle(IProcessFailed message)
        {
            var requestModel = new RequestModel()
            {
                RequestId = message.RequestId,
                CurrentSagaState = Enum.GetName(typeof(TestingRequestSagaStates), message.CurrentSagaState),
                FailureReasons = message.FailureReasons
            };

            AllRequestsHub.Requests.AddOrUpdate(requestModel.RequestId, requestModel, (key, oldValue) => requestModel);

            var context = GlobalHost.ConnectionManager.GetHubContext<AllRequestsHub>();
            context.Clients.All.requestStateChanged(requestModel.RequestId.ToString("D"),
                requestModel.CurrentSagaState,
                requestModel.CurrentProgress.ToString(CultureInfo.InvariantCulture),
                requestModel.FailureReasons);
            
            SendStats();
        }

        public void SendStats()
        {
            var totalNumberOfRequests = AllRequestsHub.Requests.Count;
            var failedRequests = AllRequestsHub.Requests.Count(x => x.Value.CurrentSagaState == "Failed");
            var cancelledRequests = AllRequestsHub.Requests.Count(x => x.Value.CurrentSagaState == "Cancelled");
            var validatedRequests = AllRequestsHub.Requests.Count(x => x.Value.CurrentSagaState.ToLower().Contains("validation"));
            var initializedRequests = AllRequestsHub.Requests.Count(x => x.Value.CurrentSagaState.ToLower().Contains("initialization"));
            var buildRequests = AllRequestsHub.Requests.Count(x => x.Value.CurrentSagaState.ToLower().Contains("build"));
            var testedRequests = AllRequestsHub.Requests.Count(x => x.Value.CurrentSagaState.ToLower().Contains("test") || x.Value.CurrentSagaState.ToLower().Contains("finished"));
            var finishedRequests = AllRequestsHub.Requests.Count(x => x.Value.CurrentSagaState.ToLower().Contains("finished"));
            var timestamp = DateTime.Now.ToString("yyyy-M-dd HH:mm:ss");

            var context = GlobalHost.ConnectionManager.GetHubContext<AllRequestsHub>();

            //JS is a werid world... 
            context.Clients.All.consumeStats(timestamp,
                totalNumberOfRequests,
                failedRequests, cancelledRequests,
                validatedRequests,
                initializedRequests,
                buildRequests,
                testedRequests,
                finishedRequests);
        }
    }
}