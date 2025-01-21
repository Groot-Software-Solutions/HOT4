using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.ViewModel
{
    public class PinRecord
    {
        public long PinId { get; set; }
        public long PinBatchId { get; set; }
        public byte PinStateId { get; set; }
        public byte BrandId { get; set; }
        public string Pin { get; set; }
        public string PinRef { get; set; }
        [Column(TypeName = "money")]
        public decimal PinValue { get; set; }
        public DateTime PinExpirty { get; set; }
    }
}
