#pragma warning disable IDE1006
namespace Infrastructure.Services.MaytApi.Models

{
    public class MessageBody
    {
        public string type { get; set; }
        public User user { get; set; }
        public string reply { get; set; }
        public Message message { get; set; }
        public int phoneId { get; set; }
        public int phone_id { get; set; }
        public string receiver { get; set; }
        public string productId { get; set; }
        public int timestamp { get; set; }
        public string product_id { get; set; }
        public string conversation { get; set; }
        public string conversation_name { get; set; }
    }


}
#pragma warning restore IDE1006