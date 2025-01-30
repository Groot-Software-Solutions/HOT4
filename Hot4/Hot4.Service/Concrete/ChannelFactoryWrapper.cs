using Hot4.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ZesaAPI;

namespace Hot4.Service.Concrete
{
    public class ChannelFactoryWrapper<T> : INetworkClient<T>
    {
        private readonly ChannelFactory<T> _factory;

        public ChannelFactoryWrapper(ChannelFactory<T> factory)
        {
            _factory = factory;
        }
        public void Abort()
        {
            _factory.Abort();
        }

        public void Close()
        {
            _factory.Close();
        }

        public T GetNetwork()
        {
            return _factory.CreateChannel();
        }

    }
}
