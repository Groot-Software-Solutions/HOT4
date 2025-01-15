
using Hot4.DataModel.Models;
using Hot4.ViewModel;
using Profile = AutoMapper.Profile;


namespace Hot4.Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Access , AccessModel>().ReverseMap();
            CreateMap<Access, AccountAccessModel>().ReverseMap();
            // Access Web 
            CreateMap<AccessWeb, AccessWebModel>().ReverseMap();
            // Address
            CreateMap<Address, AddressModel>().ReverseMap();
            // bank 
            CreateMap<Banks, BankModel>().ReverseMap();
            // Channel
            CreateMap<Channels, ChannelModel>().ReverseMap();

        }
    }
}
