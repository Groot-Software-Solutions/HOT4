namespace Hot4.Core.DataViewModels
{
    public class AccountRechargePinModel
    {
        public long PinID { get; set; }
        public long PinBatchID { get; set; }
        public byte PinStateID { get; set; }
        public int ProductID { get; set; }
        public string Pin { get; set; }
        public string PinRef { get; set; }
        public decimal PinValue { get; set; }
        public DateTime PinExpiry { get; set; }
    }
}
