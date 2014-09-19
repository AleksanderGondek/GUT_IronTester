using NServiceBus;

namespace IronTester.ManualMessagesPusher
{
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Client, UsingTransport<Msmq>, IWantCustomInitialization
    {
	    public void Init()
	    {
            Configure.Serialization.Json();

            Configure.With()
                .Log4Net();
	    }
    }
}
