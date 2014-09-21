using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IronTester.Common.Commands;
using IronTester.Common.Metadata;
using log4net.Appender;
using log4net.Core;
using NServiceBus;

namespace IronTester.RequestProducer
{
    class Program
    {
        public static IBus Bus { get; set; }
        public static Random Random { get; private set; }

        private static TimerCallback _timerCallback;
        private static Timer _timer;

        static void Main()
        {
            InitializeNServiceBus();
            Random = new Random(DateTime.Now.Millisecond);
            _timerCallback = SendThirtyRequests;

            string readLine;
            do
            {
                PrintOutLegened();

                readLine = Console.ReadLine();
                if (readLine != null) readLine = readLine.ToLower();

                switch (readLine)
                {
                    case "a":
                        Console.WriteLine("Automatic mode: on. Starting producing requests...");
                        AutomaticLoopMode();
                        break;
                    case "m":
                        if (_timer != null)
                        {
                            _timer.Dispose();
                            Console.WriteLine("Automatic mode: off.");
                        }

                        Console.WriteLine("Manual mode: on. Press anything to send 30 reuqests...");
                        Console.ReadLine();
                        
                        SendThirtyRequests(null);
                        break;
                    case "x":
                        if (_timer != null) _timer.Dispose();
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Wrong input. Try again...");
                        break;
                }
                
                Console.Clear();

            } while (readLine != null && readLine.ToLower() != "x");
        }

        static void AutomaticLoopMode()
        {
            _timer = new Timer(_timerCallback, null, 1000, 60000);
        }

        static void SendThirtyRequests(Object stateInfo)
        {
            for (var i = 0; i < 30; i++)
            {
                var guid = Guid.NewGuid();
                
                Bus.Send("IronTester.Server",
                    Bus.CreateInstance<PleaseDoTests>(y =>
                    {
                        y.RequestId = guid;
                        y.SourceCodeLocation = GetRandomSourcePath();
                        y.TestsRequested = new List<string> { GetRandomTest(), GetRandomTest(), GetRandomTest() };
                    }));
                
                Thread.Sleep(1000);
            }
        }

        public static string GetRandomTest()
        {
            var index = Random.Next(ValidationData.ValidTests.Count);
            return ValidationData.ValidTests.ElementAt(index);
        }

        public static string GetRandomSourcePath()
        {
            var index = Random.Next(ValidationData.ValidInitializationPaths.Count);
            return ValidationData.ValidInitializationPaths.ElementAt(index);
        }

        static void PrintOutLegened()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("The IronTester.RequestProducer endpoint is now running and ready to produce request.");
            Console.WriteLine("Please press appropriate letter to choose work mode ");
            Console.WriteLine(" [A] - for automatic mode. Producer will spit out 30 requests every 1 minute.");
            Console.WriteLine(" [M] - for manual mode. Producer will spit out 30 requests every time user presses a key.");
            Console.WriteLine(" [X] - for exit.");
            Console.ResetColor();
        }

        static void InitializeNServiceBus()
        {
            Configure.ScaleOut(s => s.UseSingleBrokerQueue());
            Configure.Serialization.Json();

            Bus = Configure.With()
                .DefaultBuilder()
                .Log4Net(new DebugAppender { Threshold = Level.Warn })
                .UseTransport<Msmq>()
                .PurgeOnStartup(false)
                .UnicastBus()
                .CreateBus()
                .Start(
                    () =>
                        Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
        }
    }
}
