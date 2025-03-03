namespace Hot.Ecocash.Domain.Entities
{
    public class ChargingMetaDataResponse: ChargingMetaData
    {
        public int id { get; set; } = 0;
        public decimal version { get; set; } = 0m;
        private string _serviceId { get; set; } = ""; 
        public string purchaseCategoryCode { get; set; } = "Airtime";

        public string serviceId
        {
            get
            {
                return _serviceId;
            } 
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "";
                if (value.Contains("null"))
                    value = "";
                _serviceId = value;
            }
        }
    }
}