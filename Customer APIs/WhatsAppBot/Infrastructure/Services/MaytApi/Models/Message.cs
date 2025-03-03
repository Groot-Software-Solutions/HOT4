#pragma warning disable IDE1006
namespace Infrastructure.Services.MaytApi.Models
{ 
    public class Message
    {
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public bool fromMe { get; set; }
        public string _serialized { get; set; }
    }
}
#pragma warning restore IDE1006