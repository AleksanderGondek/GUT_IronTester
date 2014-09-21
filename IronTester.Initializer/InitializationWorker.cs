using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IronTester.Common.Messages.Initialization;
using IronTester.Common.Metadata;
using NServiceBus;

namespace IronTester.Initializer
{
    public class InitializationWorker : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }
        public readonly object BusLock;
        public static ConcurrentDictionary<Guid, RequestModel> Requests;

        private static Timer _timer;
        private static TimerCallback _timerCallback;

        public InitializationWorker()
        {
            Requests = new ConcurrentDictionary<Guid, RequestModel>();
            _timerCallback = InitializeRequest;
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

        public void InitializeRequest(Object stateInfo)
        {
            // Initialize
            Parallel.ForEach(Requests, request =>
            {
                //Pretend to do some initialization
                for (decimal i = 0; i <= 50; i += 10)
                {
                    lock (BusLock)
                    {
                        var i1 = i;
                        Bus.Publish<IInitializeStatus>(m =>
                        {
                            m.RequestId = request.Key;
                            m.Progress = i1;
                        });
                    }
                    //Wait 3 seconds
                    Thread.Sleep(3000);
                }

                // Check if source code path is correct
                var isRequestValid = ValidationData.ValidInitializationPaths.Contains(request.Value.SourceCodeLocation);

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
                            Bus.Publish<IInitializeStatus>(m =>
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
                    Bus.Publish<IInitializeFinished>(m =>
                    {
                        m.RequestId = request.Key;
                        m.InitializationSuccessful = request.Value.IsValid;
                        m.InitializationFailReason = request.Value.ValidationFailReason;
                        m.PathToInitializedFiles = GetRandomValidInitializedFilesPath();
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

        public static string GetRandomValidInitializedFilesPath()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var index = random.Next(ValidationData.ValidInitializedFilesPaths.Count);
            return ValidationData.ValidInitializedFilesPaths.ElementAt(index);
        }
    }
}
