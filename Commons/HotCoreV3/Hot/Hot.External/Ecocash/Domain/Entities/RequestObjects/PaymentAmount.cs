using Hot.Ecocash.Domain.Enums;

namespace Hot.Ecocash.Domain.Entities
{
    public class PaymentAmount
    {
        public ChargingInformation charginginformation { get; set; } = new ChargingInformation();
        public ChargingMetaData chargeMetaData { get; set; } = new ChargingMetaData();

        public PaymentAmount(decimal amount, string desc)
        {
            charginginformation.amount = amount;
            charginginformation.description = desc;
        }
        public PaymentAmount(decimal amount, string desc, string onBeHalfOf, Currencies currency = Currencies.ZiG) : this(amount, desc)
        {
            chargeMetaData.onBeHalfOf = onBeHalfOf;
            charginginformation.currency = (Enum.GetName(typeof(Currencies), currency)??"").Replace("_","-");
        }
    }
}
