
using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Service
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<PinBatchTypes, PinBatchTypeModel>().ReverseMap();
            CreateMap<PinBatches, PinBatchModel>()
                .ForMember(dst => dst.PinBatchType, opt => opt.MapFrom(src => src.PinBatchTypeId != 0 ? src.PinBatchType.PinBatchType : null))
                .ReverseMap();

            CreateMap<PinBatches, PinBatchToDo>().ReverseMap();
            CreateMap<Pins, PinToDo>().ReverseMap();
            CreateMap<Pins, PinDetailModel>()
                .ForMember(dst => dst.PinBatch, opt => opt.MapFrom(src => src.PinBatchId != 0 ? src.PinBatch.PinBatch : null))
                .ForMember(dst => dst.BatchDate, opt => opt.MapFrom(src => src.PinBatch.BatchDate))
                .ForMember(dst => dst.PinState, opt => opt.MapFrom(src => src.PinStateId != 0 ? src.PinState.PinState : null))
                 .ForMember(dst => dst.BrandName, opt => opt.MapFrom(src => src.BrandId != 0 ? src.Brand.BrandName : null))
                  .ForMember(dst => dst.BrandSuffix, opt => opt.MapFrom(src => src.BrandId != 0 ? src.Brand.BrandSuffix : null))
                  .ForMember(dst => dst.PinBatchTypeId, opt => opt.MapFrom(src => src.PinBatch.PinBatchTypeId))
                .ForMember(dst => dst.PinBatchType, opt => opt.MapFrom(src => src.PinBatch.PinBatchTypeId != 0 ? src.PinBatch.PinBatchType.PinBatchType : null))
                .ForMember(dst => dst.NetworkId, opt => opt.MapFrom(src => src.BrandId != 0 ? src.Brand.NetworkId : 0))
                  .ForMember(dst => dst.Network, opt => opt.MapFrom(src => src.BrandId != 0 ? src.Brand.Network.Network : null))
                  .ForMember(dst => dst.Prefix, opt => opt.MapFrom(src => src.BrandId != 0 ? src.Brand.Network.Prefix : null))
                .ForMember(dst => dst.PinNumber, opt => opt.MapFrom(src => src.Pin))
                .ReverseMap();

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
            CreateMap<Access, AccessModel>().ReverseMap();
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
