using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using IronTester.Common.Commands;
using Microsoft.AspNet.SignalR;

namespace IronTester.Website.BusHandlers
{
    public class AllRequestsHub : Hub
    {
        private static ConcurrentDictionary<Guid, RequestModel> _requests; 
        public static ConcurrentDictionary<Guid, RequestModel> Requests
        {
            get { return _requests ?? (_requests = new ConcurrentDictionary<Guid, RequestModel>()); }
            set { _requests = value; }
        }

        public void getAllRequests()
        {
            foreach (var request in Requests.Values)
            {
                Clients.All.requestStateChanged(request.RequestId.ToString("D"),
                request.CurrentSagaState,
                request.CurrentProgress.ToString(CultureInfo.InvariantCulture),
                request.FailureReasons);
            }
        }

        public void getAllStats()
        {
            var totalNumberOfRequests = Requests.Count;
            var failedRequests = Requests.Count(x => x.Value.CurrentSagaState == "Failed");
            var cancelledRequests = Requests.Count(x => x.Value.CurrentSagaState == "Cancelled");
            var validatedRequests = Requests.Count(x => x.Value.CurrentSagaState.ToLower().Contains("validation"));
            var initializedRequests = Requests.Count(x => x.Value.CurrentSagaState.ToLower().Contains("initialization"));
            var buildRequests = Requests.Count(x => x.Value.CurrentSagaState.ToLower().Contains("build"));
            var testedRequests = Requests.Count(x => x.Value.CurrentSagaState.ToLower().Contains("test") || x.Value.CurrentSagaState.ToLower().Contains("finished"));
            var finishedRequests = Requests.Count(x => x.Value.CurrentSagaState.ToLower().Contains("finished"));
            var timestamp = DateTime.Now.ToString("u");
            //JS is a werid world... 
            Clients.All.consumeStats(timestamp,
                totalNumberOfRequests,
                failedRequests,cancelledRequests,
                validatedRequests,
                initializedRequests,
                buildRequests,
                testedRequests,
                finishedRequests);
        }

        public void cancel(string requestId)
        {
            MvcApplication.Bus.Send("IronTester.Server",
                MvcApplication.Bus.CreateInstance<PleaseCancelTests>(y =>
                {
                    y.RequestId = new Guid(requestId);
                }));
        }

        public void restart(string requestId)
        {
            MvcApplication.Bus.Send("IronTester.Server",
                MvcApplication.Bus.CreateInstance<PleaseRestart>(y =>
                {
                    y.RequestId = new Guid(requestId);
                }));
        }
    }
}