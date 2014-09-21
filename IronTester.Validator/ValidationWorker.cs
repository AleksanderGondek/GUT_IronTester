using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IronTester.Common.Messages.Validation;
using IronTester.Common.Metadata;
using NServiceBus;

namespace IronTester.Validator
{
    public class ValidationWorker : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }
        public readonly object BusLock;
        public static ConcurrentDictionary<Guid, RequestModel> Requests;

        private static Timer _timer;
        private static TimerCallback _timerCallback;

        public ValidationWorker()
        {
            Requests = new ConcurrentDictionary<Guid, RequestModel>();
            _timerCallback = ValidateRequest;
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

        public void ValidateRequest(Object stateInfo)
        {
            // Validation
            Parallel.ForEach(Requests, request =>
                                       {
                                           //Pretend to do some validation
                                           for (decimal i = 0; i <= 50; i+=10)
                                           {
                                               lock (BusLock)
                                               {
                                                   var i1 = i;
                                                   Bus.Publish<IValidationStatus>(m =>
                                                   {
                                                       m.RequestId = request.Key;
                                                       m.Progress = i1;
                                                   });
                                               }
                                               //Wait 3 seconds
                                               Thread.Sleep(3000);
                                           }

                                           // Do actual validation
                                           var isRequestValid =
                                               request.Value.TestsRequested.All(
                                                   x => ValidationData.ValidTests.Contains(x));

                                           request.Value.IsValid = isRequestValid;
                                           request.Value.ValidationFailReason = isRequestValid ? null : "Invalid tests requested";

                                           if (isRequestValid)
                                           {
                                               // Pretend to do some more work
                                               for (decimal i = 60; i <= 100; i += 10)
                                               {
                                                   lock (BusLock)
                                                   {
                                                       var i1 = i;
                                                       Bus.Publish<IValidationStatus>(m =>
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
                                               Bus.Publish<IValidationFinished>(m =>
                                               {
                                                   m.RequestId = request.Key;
                                                   m.ValidationSuccessful = request.Value.IsValid;
                                                   m.ValidationFailReason = request.Value.ValidationFailReason;
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
