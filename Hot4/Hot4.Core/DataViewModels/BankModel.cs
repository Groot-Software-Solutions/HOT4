namespace Hot4.Core.DataViewModels
{
    public class BankModel
    {
        public byte BankID { get; set; }

        public string Bank { get; set; }

        public int? SageBankID { get; set; }
        public bool IsActive { get; set; }
    }
}
