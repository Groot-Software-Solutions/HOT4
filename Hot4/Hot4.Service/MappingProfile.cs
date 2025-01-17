
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
            CreateMap<Access, AccountAccessModel>()
            .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => src.Deleted ?? false)).ReverseMap();
            // Access Web 
            CreateMap<AccessWeb, AccessWebModel>().ReverseMap();
            // Address
            CreateMap<Address, AddressModel>().ReverseMap();
            // bank 
            CreateMap<Banks, BankModel>().ReverseMap();
            // Channel
            CreateMap<Channels, ChannelModel>().ReverseMap();
            // Bundel
            CreateMap<Bundle, BundleModel>().ReverseMap();
            // Configs
            CreateMap<Configs, ConfigModel>().ReverseMap();


        }
    }
}
