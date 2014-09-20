using System;
using System.Collections.Generic;
using System.ComponentModel;
using IronTester.Common.Messages.Builds;
using NServiceBus;
using NServiceBus.AutomaticSubscriptions;

namespace IronTester.Builder
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, UsingTransport<Msmq>, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Serialization.Json();
            Configure.Features.AutoSubscribe(f => f.CustomAutoSubscriptionStrategy<IronAutoSubThree>());

            Configure.With()
                .Log4Net();
        }
    }

    public class IronAutoSubThree : IAutoSubscriptionStrategy
    {
        public IEnumerable<Type> GetEventsToSubscribe()
        {
            return new BindingList<Type> { typeof(IPleaseBuild) };
        }
    }
}
