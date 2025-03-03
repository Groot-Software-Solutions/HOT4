#pragma warning disable IDE1006
using Infrastructure.Services.MaytApi.Enums;

namespace Infrastructure.Services.MaytApi.Models
{
    public class AcknowledgementData
    {
        public string ackType { get; set; }
        public AckCode ackCode { get; set; }
        public string chatId { get; set; }
        public string msgId { get; set; }
        public int time { get; set; }
    }

}
#pragma warning restore IDE1006