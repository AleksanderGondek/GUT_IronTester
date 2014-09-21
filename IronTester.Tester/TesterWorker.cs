using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IronTester.Common.Messages.Tests;
using IronTester.Common.Metadata;
using NServiceBus;

namespace IronTester.Tester
{
    public class TesterWorker : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }
        public readonly object BusLock;
        public static ConcurrentDictionary<Guid, RequestModel> Requests;

        private static Timer _timer;
        private static TimerCallback _timerCallback;

        public TesterWorker()
        {
            Requests = new ConcurrentDictionary<Guid, RequestModel>();
            _timerCallback = TestRequest;
            BusLock = new object();
        }

        public void Start()
        {
            // Run ValidateRequest every 60 seconds, start after 1 second
            _timer = new Timer(_timerCallback, null, 1000, 60000);
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        public void TestRequest(Object stateInfo)
        {
            // Tests
            Parallel.ForEach(Requests, request =>
            {
                //Pretend to do some testing
                for (decimal i = 0; i <= 50; i += 10)
                {
                    lock (BusLock)
                    {
                        var i1 = i;
                        Bus.Publish<ITestsStatus>(m =>
                        {
                            m.RequestId = request.Key;
                            m.Progress = i1;
                        });
                    }
                    //Wait 3 seconds
                    Thread.Sleep(3000);
                }

                // Check if build path is correct
                var isRequestValid = ValidationData.ValidBuildsArtifactsPaths.Contains(request.Value.SourceCodeLocation);

                request.Value.IsValid = isRequestValid;
                request.Value.ValidationFailReason = isRequestValid ? null : "Invalid build artifacts path";

                if (isRequestValid)
                {
                    // Pretend to do some more work
                    for (decimal i = 60; i <= 100; i += 10)
                    {
                        lock (BusLock)
                        {
                            var i1 = i;
                            Bus.Publish<ITestsStatus>(m =>
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
                    Bus.Publish<ITestsFinished>(m =>
                    {
                        m.RequestId = request.Key;
                        m.TestsSuccessful = request.Value.IsValid;
                        m.TestsFailReason = request.Value.ValidationFailReason;
                        m.TestsArtifactsLocation = @"X:\Some\Magical\Non\Exisiting\Dir\With\Tests\Results";
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
    }
}
