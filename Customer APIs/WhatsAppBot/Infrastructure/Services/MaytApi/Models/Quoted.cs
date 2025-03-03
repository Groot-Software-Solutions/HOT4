namespace Infrastructure.Services.MaytApi.Models
{ 
    public class Quoted
    {
        public string id { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public User user { get; set; }
        public int timestamp { get; set; }
    }

}
