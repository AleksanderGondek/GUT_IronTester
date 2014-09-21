using System;
using System.Globalization;
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
        }
    }
}