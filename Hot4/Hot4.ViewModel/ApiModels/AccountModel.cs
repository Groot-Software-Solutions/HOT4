namespace Hot4.ViewModel.ApiModels
{
    public class AccountModel
    {
        public long AccountId { get; set; }
        public int ProfileId { get; set; }
        public required string AccountName { get; set; }
        public required string NationalId { get; set; }
        public required string Email { get; set; }
        public required string ReferredBy { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
