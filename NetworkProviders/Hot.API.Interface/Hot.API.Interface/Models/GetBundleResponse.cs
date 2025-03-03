using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface.Models
{
    public class GetBundleResponse
    {

        public int ReplyCode;
        public List<BundleProduct> Bundles;
       public  string AgentReference;
    
    }
}
