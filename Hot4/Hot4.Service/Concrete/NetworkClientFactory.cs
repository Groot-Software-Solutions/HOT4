using Hot4.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Concrete
{
    public class NetworkClientFactory
    {
        public static INetworkClient<T> Create<T>(string endpointAddress, int timeout = 30000)
        {
            var binding = new BasicHttpBinding
            {
                SendTimeout = TimeSpan.FromSeconds(timeout / 1000) // Convert milliseconds to seconds
            };

            // Create the channel factory for the service endpoint
            var channelFactory = new ChannelFactory<T>(binding, new EndpointAddress(endpointAddress));

            // Wrap the factory in a wrapper class and return it
            var wrapper = new ChannelFactoryWrapper<T>(channelFactory);
            return wrapper;
        }
    }
}
