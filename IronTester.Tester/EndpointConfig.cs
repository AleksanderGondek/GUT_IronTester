using System;
using System.Collections.Generic;
using System.ComponentModel;
using IronTester.Common.Messages.CancelRequest;
using IronTester.Common.Messages.Tests;
using NServiceBus;
using NServiceBus.AutomaticSubscriptions;

namespace IronTester.Tester
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, UsingTransport<Msmq>, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Serialization.Json();
            Configure.Features.AutoSubscribe(f => f.CustomAutoSubscriptionStrategy<IronAutoSubFour>());
            Configure.With()
                .Log4Net();
        }
    }

    public class IronAutoSubFour : IAutoSubscriptionStrategy
    {
        public IEnumerable<Type> GetEventsToSubscribe()
        {
            return new BindingList<Type> { typeof(IPleaseTest), typeof(IPleaseCancel) };
        }
    }
}
