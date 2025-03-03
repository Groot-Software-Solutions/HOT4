using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Models
{
    public class GetBundleResponse
    {

        public int ReplyCode { get; set; }
        public List<BundleProduct> Bundles { get; set; }
        public  string AgentReference { get; set; }

    }
}
