using System.Collections.Generic;

namespace Hot.API.Client.Models
{
    public class QueryTeloneBundlesResponse : Response
    {
        public List<TelOneBundleProduct> Products { get; set; }

    }

}
