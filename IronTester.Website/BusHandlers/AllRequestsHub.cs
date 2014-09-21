using System;
using System.Collections.Concurrent;
using System.Globalization;
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
    }
}