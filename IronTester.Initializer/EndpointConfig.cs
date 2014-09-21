using System;
using System.Collections.Generic;
using System.ComponentModel;
using IronTester.Common.Messages.CancelRequest;
using IronTester.Common.Messages.Initialization;
using log4net.Appender;
using log4net.Core;
using NServiceBus;
using NServiceBus.AutomaticSubscriptions;

namespace IronTester.Initializer
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, UsingTransport<Msmq>, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Serialization.Json();
            Configure.Features.AutoSubscribe(f => f.CustomAutoSubscriptionStrategy<IronAutoSubTwo>());

            Configure.With()
                .Log4Net(new DebugAppender { Threshold = Level.Warn });
        }
    }

    public class IronAutoSubTwo : IAutoSubscriptionStrategy
    {
        public IEnumerable<Type> GetEventsToSubscribe()
        {
            return new BindingList<Type> { typeof(IPleaseInitialize), typeof(IPleaseCancel) };
        }
    }
    public class InitializerEyeCandy : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("The IronTester.Initializer endpoint is now running and ready to accept messages.");
            Console.Out.WriteLine("Console logging treshold level: Warn");
            Console.ResetColor();
        }

        public void Stop()
        {
            Console.Out.WriteLine("I am sorry Dave, I am afraid I cannot allow you to do that...");
        }
    }
}
