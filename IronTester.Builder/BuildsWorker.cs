using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IronTester.Common.Messages.Builds;
using IronTester.Common.Metadata;
using NServiceBus;

namespace IronTester.Builder
{
    public class BuildsWorker : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }
        public readonly object BusLock;
        public static ConcurrentDictionary<Guid, RequestModel> Requests;

        private static Timer _timer;
        private static TimerCallback _timerCallbackValidate;

        public BuildsWorker()
        {
            Requests = new ConcurrentDictionary<Guid, RequestModel>();
            _timerCallbackValidate = BuildRequest;
            BusLock = new object();
        }

        public void Start()
        {
            // Run ValidateRequest every 60 seconds, start after 1 second
            _timer = new Timer(_timerCallbackValidate, null, 1000, 60000);
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        public void BuildRequest(Object stateInfo)
        {
            // Building
            Parallel.ForEach(Requests, request =>
            {
                //Pretend to do some Building
                for (decimal i = 0; i <= 50; i += 10)
                {
                    lock (BusLock)
                    {
                        var i1 = i;
                        Bus.Publish<IBuildsStatus>(m =>
                        {
                            m.RequestId = request.Key;
                            m.Progress = i1;
                        });
                    }
                    //Wait 3 seconds
                    Thread.Sleep(3000);
                }

                // Check if source code path is correct
                var isRequestValid = ValidationData.ValidInitializedFilesPaths.Contains(request.Value.SourceCodeLocation);

                request.Value.IsValid = isRequestValid;
                request.Value.ValidationFailReason = isRequestValid ? null : "Invalid source code path";

                if (isRequestValid)
                {
                    // Pretend to do some more work
                    for (decimal i = 60; i <= 100; i += 10)
                    {
                        lock (BusLock)
                        {
                            var i1 = i;
                            Bus.Publish<IBuildsStatus>(m =>
                            {
                                m.RequestId = request.Key;
                                m.Progress = i1;
                            });
                        }
                        //Wait 3 seconds
                        Thread.Sleep(3000);
                    }
                }

                //Send Finish Notification
                lock (BusLock)
                {
                    Bus.Publish<IBuildsFinished>(m =>
                    {
                        m.RequestId = request.Key;
                        m.BuildsSuccessful = request.Value.IsValid;
                        m.BuildsFailReason = request.Value.ValidationFailReason;
                        m.PathToBuildsArtifacts = GetRandomBuildsArtifactsPath();
                    });
                }

                request.Value.WasProcessed = true;
            });

            //Throw out processed requests
            var requestsToBeRemoved = Requests.Where(x => x.Value.WasProcessed).Select(y => y.Key).ToList();
            foreach (var request in requestsToBeRemoved)
            {
                RequestModel removedRequest;
                Requests.TryRemove(request, out removedRequest);
            }
        }

        public static string GetRandomBuildsArtifactsPath()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var index = random.Next(ValidationData.ValidBuildsArtifactsPaths.Count);
            return ValidationData.ValidBuildsArtifactsPaths.ElementAt(index);
        }
    }
}
