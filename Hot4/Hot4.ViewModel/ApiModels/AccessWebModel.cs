namespace Hot4.ViewModel.ApiModels
{
    public class AccessWebModel
    {
        public long AccessId { get; set; }
        public required string AccessName { get; set; }
        public required string WebBackground { get; set; }
        public bool SalesPassword { get; set; }
        public string? ResetToken { get; set; }
    }
}
