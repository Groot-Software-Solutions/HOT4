namespace Sage.Domain.Entities
{
    public class BankFeedAccountGroup
    {
        public int ID { get; set; } = 0;
        public int BankFeedProviderId { get; set; } = 0;
        public int BankFeedProviderTypeId { get; set; } = 0;
        public string Description { get; set; } = "";
        public string Identifier { get; set; } = "";
        public bool RequiresAdditionalAuthentication { get; set; } = true;
        public int LastRefreshStatusId { get; set; } = 0;
    }




}
