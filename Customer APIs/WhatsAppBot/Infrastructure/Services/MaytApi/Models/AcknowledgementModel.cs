#pragma warning disable IDE1006
using Infrastructure.Services.MaytApi.Models;

namespace Infrastructure.Services.MaytApi.Models
{
    public class AcknowledgementModel
    {
        public string type { get; set; }
        public string product_id { get; set; }
        public AcknowledgementData[] data { get; set; }
    }

}
#pragma warning restore IDE1006