using System;
using System.Collections.Generic;
using IronTester.Common.Commands;
using NServiceBus;

namespace IronTester.ManualMessagesPusher
{
    public class ManualMessagesPusher : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }
        
        public void Start()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Manual Message Pusher ready.");
            Console.WriteLine("To exit, Ctrl + C");
            Console.ResetColor();

            while (Console.ReadLine() != null)
            {
                var message = new PleaseDoTests
                              {
                                  SourceCodeLocation = @"/home/hell",
                                  TestsRequested = new List<string>() {"Test Uno", "Test Duo"}
                              };
                
                Bus.Send("IronTester.Server", message);

                Console.WriteLine("==========================================================================");
                Console.WriteLine("Send a new PlaceOrder message with id: {0}", message.RequestId.ToString("D"));
            }
        }

        public void Stop()
        {
            //Nothing
        }
    }
}
