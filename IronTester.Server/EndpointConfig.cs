using System;
using NServiceBus;

namespace IronTester.Server
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, AsA_Publisher, UsingTransport<Msmq>, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Serialization.Json();

            Configure.With()
                .Log4Net();
        }
    }

    public class ServerEyeCandy : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("The IronTesterServer endpoint is now running and ready to accept messages.");
            Console.ResetColor();
        }

        public void Stop()
        {
            Console.Out.WriteLine("I am sorry Dave, I am afraid I cannot allow you to do that...");
        }
    }
}
