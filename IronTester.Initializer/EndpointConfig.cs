using System;
using System.Collections.Generic;
using System.ComponentModel;
using IronTester.Common.Messages.CancelRequest;
using IronTester.Common.Messages.Initialization;
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
                .Log4Net();
        }
    }

    public class IronAutoSubTwo : IAutoSubscriptionStrategy
    {
        public IEnumerable<Type> GetEventsToSubscribe()
        {
            return new BindingList<Type> { typeof(IPleaseInitialize), typeof(IPleaseCancel) };
        }
    }
}
