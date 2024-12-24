namespace Hot4.ViewModel.ApiModels
{
    public class PaymentSourceModel
    {
        public int PaymentSourceId { get; set; }
        public string PaymentSource { get; set; }
        public int WalletTypeId { get; set; }
        public string PaymentSourceText { get; set; }
    }
}
