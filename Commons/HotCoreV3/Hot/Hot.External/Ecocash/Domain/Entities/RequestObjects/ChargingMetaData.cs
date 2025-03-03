using Hot.Ecocash.Domain.Enums;
using System;

namespace Hot.Ecocash.Domain.Entities
{
    public class ChargingMetaData
    {
        private PaymentChannel _channel = PaymentChannel.SMS;
        public string channel
        {
            get
            {
                return Enum.GetName(typeof(PaymentChannel), _channel);
            }

            set
            {
                Enum.TryParse(value, out _channel); 
            }
        }

        public string onBeHalfOf { get; set; } = "CommShop";
    }
}
