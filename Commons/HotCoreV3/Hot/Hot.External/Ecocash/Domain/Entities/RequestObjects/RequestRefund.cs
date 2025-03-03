using Hot.Ecocash.Application.Common;
using Hot.Ecocash.Domain.Entities;

namespace Hot.Ecocash.Domain.Entities
{
    public class RequestRefund : RequestItem
    {
        public string originalEcocashReference { get; set; }
        public string tranType { get; set; } = "REF";
        public string superMerchantName { get; set; } = "CommShop";
        public string merchantName { get; set; } = "HotRecharge";

        public RequestRefund(string originalEcocashReference, ServiceOptions options) : base(options)
        {
            this.originalEcocashReference = originalEcocashReference;
        }
    }
}


