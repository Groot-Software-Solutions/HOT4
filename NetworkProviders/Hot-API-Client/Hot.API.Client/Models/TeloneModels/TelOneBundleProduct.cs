using System.Text.Json.Serialization;

namespace Hot.API.Client.Models
{
    public class TelOneBundleProduct
    {
        [JsonPropertyName("ProductId")]
        public int ProductIdField { get; set; }
        [JsonPropertyName("Name")]
        public string NameField { get; set; }
        [JsonPropertyName("Description")]
        public string DescriptionField { get; set; }
        [JsonPropertyName("Price")]
        public decimal PriceField { get; set; }

    }
      
}
