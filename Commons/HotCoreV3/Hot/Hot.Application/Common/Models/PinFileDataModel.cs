namespace Hot.Application.Common.Models
{
    public class PinFileDataModel
    {
        public string BatchNumber { get; set; }
        public string SourceReference { get; set; }
        public int PinBatchType { get; set; } = 0;  
        public List<Domain.Entities.Pin> Pins { get; set; } = new();
        public int Quantity { get; set; }
    }
}
