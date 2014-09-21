using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Routing;
using IronTester.Common.Messages.Saga;
using log4net.Appender;
using log4net.Core;
using NServiceBus;
using NServiceBus.AutomaticSubscriptions;

namespace IronTester.Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IBus Bus { get; set; }

        protected void Application_Start()
        {
            InitializeNServiceBus();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        static void InitializeNServiceBus()
        {
            Configure.ScaleOut(s => s.UseSingleBrokerQueue());
            Configure.Serialization.Json();
            Configure.Features.AutoSubscribe(f => f.CustomAutoSubscriptionStrategy<IronAutoSubFive>());

            Bus = Configure.With()
                .DefineEndpointName("IronTester.Website")
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

    public class IronAutoSubFive : IAutoSubscriptionStrategy
    {
        public IEnumerable<Type> GetEventsToSubscribe()
        {
            return new BindingList<Type> { typeof(ITestingRequestSagaStateHasChanged), typeof(IProcessUpdate), typeof(IProcessFailed) };
        }
    }
}
