namespace Hot.Ecocash.Domain.Entities
{
    public class ChargingInformationResponse : ChargingInformation 
    {
        public int id { get; set; } = 0;
        public decimal version { get; set; } = 0m;
    }
}
