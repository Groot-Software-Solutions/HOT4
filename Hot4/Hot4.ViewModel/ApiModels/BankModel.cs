namespace Hot4.ViewModel.ApiModels
{
    public class BankModel
    {
        public byte BankId { get; set; }
        public required string Bank { get; set; }
        public int? SageBankId { get; set; }
    }
}
