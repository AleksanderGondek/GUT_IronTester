using System;
using System.Collections.Generic;
using System.ComponentModel;
using IronTester.Common.Messages.Validation;
using NServiceBus;
using NServiceBus.AutomaticSubscriptions;
using NServiceBus.Features;

namespace IronTester.Validator
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, UsingTransport<Msmq>, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Serialization.Json();
            Configure.Features.AutoSubscribe(f => f.CustomAutoSubscriptionStrategy<IronAutoSub>());

            Configure.With()
                .Log4Net();
        }
    }

    public class IronAutoSub : IAutoSubscriptionStrategy
    {
        public IEnumerable<Type> GetEventsToSubscribe()
        {
            return new BindingList<Type> { typeof(IPleaseValidate) };
        }
    }
}
