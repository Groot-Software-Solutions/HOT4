namespace Hot.Ecocash.Domain.Entities
{
    public partial class PaymentAmountResponse //: ResponseItem
    { 
        public ChargingInformationResponse charginginformation { get; set; } = new ChargingInformationResponse();
        public ChargingMetaDataResponse chargeMetaData { get; set; } = new ChargingMetaDataResponse();

        public decimal totalAmountCharged { get; set; }
    }
}
