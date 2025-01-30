using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZesaAPI;

namespace Hot4.Service.Abstract
{
    public interface INetworkClient<T>
    {
        T GetNetwork();
        void Close();
        void Abort();
    }
}