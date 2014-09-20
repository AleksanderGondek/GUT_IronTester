using System;
using System.Collections.Generic;
using System.ComponentModel;
using IronTester.Common.Messages.Builds;
using IronTester.Common.Messages.Initialization;
using IronTester.Common.Messages.Tests;
using IronTester.Common.Messages.Validation;
using NServiceBus;
using NServiceBus.AutomaticSubscriptions;

namespace IronTester.Server
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, UsingTransport<Msmq>, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Serialization.Json();
            Configure.Features.AutoSubscribe(f => f.CustomAutoSubscriptionStrategy<IronAutoSubZero>());

            Configure.With()
                .Log4Net();
        }
    }

    public class IronAutoSubZero : IAutoSubscriptionStrategy
    {
        public IEnumerable<Type> GetEventsToSubscribe()
        {
            return new BindingList<Type> { typeof(IValidationRequestConfirmation)
                                            , typeof(IValidationStatus)
                                            , typeof(IValidationFinished)
                                            , typeof(IInitializeStatus)
                                            , typeof(IInitializeRequestConfirmation)
                                            , typeof(IInitializeFinished)
                                            , typeof(IBuildsStatus)
                                            , typeof(IBuildsRequestConfirmation)
                                            , typeof(IBuildsFinished)
                                            , typeof(ITestsStatus)
                                            , typeof(ITestsRequestConfirmation)
                                            , typeof(ITestsFinished)};
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
