using System;
using System.Collections.Generic;
using System.ComponentModel;
using IronTester.Common.Messages.Validation;
using NServiceBus;
using NServiceBus.AutomaticSubscriptions;

namespace IronTester.Validator
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, UsingTransport<Msmq>, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Serialization.Json();
            Configure.Features.AutoSubscribe(f => f.CustomAutoSubscriptionStrategy<IronAutoSubOne>());
            Configure.With()
                .Log4Net();
        }
    }

    public class IronAutoSubOne : IAutoSubscriptionStrategy
    {
        public IEnumerable<Type> GetEventsToSubscribe()
        {
            return new BindingList<Type> { typeof(IPleaseValidate) };
        }
    }
}
