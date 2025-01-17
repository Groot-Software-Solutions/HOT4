
using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Service
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<BankTrxBatch, BankTrxBatchToDo>().ReverseMap();
            CreateMap<BankTrxBatch, BankBatchModel>()
                            .ForMember(dst => dst.BankName, opt => opt.MapFrom(src => src.BankId != 0 ? src.Bank.Bank : null))
                            .ReverseMap();

            CreateMap<BankTrx, BankTrxToDo>().ReverseMap();
            CreateMap<BankTrx, BankTransactionModel>()
                            .ForMember(dst => dst.BankTrxType, opt => opt.MapFrom(src => src.BankTrxTypeId != 0 ? src.BankTrxType.BankTrxType : null))
                            .ForMember(dst => dst.BankTrxState, opt => opt.MapFrom(src => src.BankTrxStateId != 0 ? src.BankTrxState.BankTrxState : null))
                            .ReverseMap();

            CreateMap<BankTrxStates, BankTransactionStateModel>().ReverseMap();
            CreateMap<BankTrxTypes, BankTransactionTypeModel>().ReverseMap();
            CreateMap<BankvPayment, BankvPaymentModel>().ReverseMap();
            CreateMap<Brand, BrandToDo>().ReverseMap();
            CreateMap<BrandModel, Brand>().ReverseMap();
            CreateMap<Smpp, SMPPModel>().ReverseMap();
            CreateMap<Sms, SmsToDo>().ReverseMap();
            CreateMap<Access, EmailModel>()
                .ForMember(dst => dst.AccountName, memberOptions => memberOptions.MapFrom(src => src.Account.AccountName))
                .ForMember(dst => dst.Email, memberOptions => memberOptions.MapFrom(src => src.AccessCode))
                .ReverseMap();
            CreateMap<Sms, SMSModel>().ReverseMap();
            CreateMap<States, StateModel>().ReverseMap();
            CreateMap<Subscriber, SubscriberModel>().ReverseMap();
            CreateMap<Subscriber, SubscriberToDo>().ReverseMap();
            CreateMap<Template, TemplateModel>().ReverseMap();
            CreateMap<Transfer, TransferModel>().ReverseMap();
            CreateMap<Transfer, TransferToDo>().ReverseMap();
            CreateMap<WalletType, WalletTypeModel>().ReverseMap();
            CreateMap<WebRequests, WebRequestModel>().ReverseMap();
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
