namespace Hot.Application.Common.Models
{
    public class AccountDetailedModel : Account
    {
        public string ProfileName { get; set; } = string.Empty;
        public Address? Address { get; set; }
        public List<Access>? Accesses { get; set; }
        public List<AccessWeb>? AccessWebs { get; set; }
    }

    public class AccountDetailedModelProfile : AutoMapper.Profile
    {
        public AccountDetailedModelProfile()
        { 
            CreateMap<Account,AccountDetailedModel>(); 
        }
    }
}
