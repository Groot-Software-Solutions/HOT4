
using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Service
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<StockData, StockDataModel>().ReverseMap();
            CreateMap<SelfTopUp, SelfTopUpModel>()
                .ForMember(dst => dst.SelfTopUpStateName, opt => opt.MapFrom(src => src.SelfTopUpState.SelfTopUpStateName))
                .ForMember(dst => dst.AccessCode, opt => opt.MapFrom(src => src.Access.AccessCode))
                .ForMember(dst => dst.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ReverseMap();
            CreateMap<SelfTopUp, SelfTopUpRecord>().ReverseMap();
            CreateMap<SelfTopUpState, SelfTopUpStateModel>().ReverseMap();
            CreateMap<Log, LogModel>().ReverseMap();
            CreateMap<Networks, NetworkModel>().ReverseMap();
            CreateMap<Limit, LimitModel>().ReverseMap();
            CreateMap<Recharge, RechargeModel>().ReverseMap();
            CreateMap<Recharge, RechargeDetailModel>()
                .ForMember(dst => dst.BrandName, opt => opt.MapFrom(src => src.BrandId != 0 ? src.Brand.BrandName : null))
                .ForMember(dst => dst.BrandSuffix, opt => opt.MapFrom(src => src.BrandId != 0 ? src.Brand.BrandSuffix : null))
                .ForMember(dst => dst.NetworkId, opt => opt.MapFrom(src => src.BrandId != 0 ? src.Brand.NetworkId : 0))
                .ForMember(dst => dst.Network, opt => opt.MapFrom(src => src.BrandId != 0 ? src.Brand.Network.Network : null))
                .ForMember(dst => dst.NetworkPrefix, opt => opt.MapFrom(src => src.BrandId != 0 ? src.Brand.Network.Prefix : null))
                .ForMember(dst => dst.State, opt => opt.MapFrom(src => src.StateId != 0 ? src.State.State : null))
                .ReverseMap();
            CreateMap<Account, AccountModel>().ReverseMap();
            CreateMap<RechargePrepaid, RechargePrepaidModel>().ReverseMap();
            CreateMap<PinBatchTypes, PinBatchTypeModel>().ReverseMap();
            CreateMap<PinBatches, PinBatchModel>()
                .ForMember(dst => dst.PinBatchType, opt => opt.MapFrom(src => src.PinBatchTypeId != 0 ? src.PinBatchType.PinBatchType : null))
                .ReverseMap();

            CreateMap<PinBatches, PinBatchRecord>().ReverseMap();
            CreateMap<Pins, PinRecord>().ReverseMap();
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

            CreateMap<BankTrxBatch, BankTrxBatchRecord>().ReverseMap();
            CreateMap<BankTrxBatch, BankBatchModel>()
                            .ForMember(dst => dst.BankName, opt => opt.MapFrom(src => src.BankId != 0 ? src.Bank.Bank : null))
                            .ReverseMap();

            CreateMap<BankTrx, BankTrxRecord>().ReverseMap();
            CreateMap<BankTrx, BankTransactionModel>()
                            .ForMember(dst => dst.BankTrxType, opt => opt.MapFrom(src => src.BankTrxTypeId != 0 ? src.BankTrxType.BankTrxType : null))
                            .ForMember(dst => dst.BankTrxState, opt => opt.MapFrom(src => src.BankTrxStateId != 0 ? src.BankTrxState.BankTrxState : null))
                            .ReverseMap();

            CreateMap<BankTrxStates, BankTransactionStateModel>().ReverseMap();
            CreateMap<BankTrxTypes, BankTransactionTypeModel>().ReverseMap();
            CreateMap<BankvPayment, BankvPaymentModel>().ReverseMap();
            CreateMap<Brand, BrandRecord>().ReverseMap();
            CreateMap<BrandModel, Brand>().ReverseMap();
            CreateMap<Smpp, SMPPModel>().ReverseMap();
            CreateMap<Sms, SmsRecord>().ReverseMap();
            CreateMap<Access, EmailModel>()
                .ForMember(dst => dst.AccountName, memberOptions => memberOptions.MapFrom(src => src.Account.AccountName))
                .ForMember(dst => dst.Email, memberOptions => memberOptions.MapFrom(src => src.AccessCode))
                .ReverseMap();
            CreateMap<Sms, SMSModel>()
                .ForMember(dst => dst.Priority, opt => opt.MapFrom(src => src.Priority.Priority))
                .ForMember(dst => dst.State, opt => opt.MapFrom(src => src.State.State))
                .ReverseMap();
            CreateMap<States, StateModel>().ReverseMap();
            CreateMap<Subscriber, SubscriberModel>()
                .ForMember(dst => dst.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ReverseMap();
            CreateMap<Subscriber, SubscriberRecord>().ReverseMap();
            CreateMap<Template, TemplateModel>().ReverseMap();
            CreateMap<Transfer, TransferModel>()
                .ForMember(dst => dst.ChannelName, opt => opt.MapFrom(src => src.Channel.Channel))
                 .ReverseMap();
            CreateMap<Transfer, TransferRecord>().ReverseMap();
            CreateMap<WalletType, WalletTypeModel>().ReverseMap();
            CreateMap<WebRequests, WebRequestModel>().ReverseMap();
            CreateMap<Access, AccessModel>().ReverseMap();
            CreateMap<Access, AccountAccessModel>()
            .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => src.Deleted ?? false)).ReverseMap();
            CreateMap<AccessWeb, AccessWebModel>().ReverseMap();
            CreateMap<Address, AddressModel>().ReverseMap();
            CreateMap<Banks, BankModel>().ReverseMap();
            CreateMap<Channels, ChannelModel>().ReverseMap();
            CreateMap<Bundle, BundleModel>()
    .ForMember(dst => dst.Network, opt => opt.MapFrom(src => src.Brand.Network.Network))
    .ForMember(dst => dst.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
    .ReverseMap();
            CreateMap<Bundle, BundleRecord>().ReverseMap();
            CreateMap<Configs, ConfigModel>().ReverseMap();
            CreateMap<HotTypes, HotTypeModel>()
                .ForMember(dst => dst.TypeCode, opt => opt.MapFrom(src => src.HotTypeCodes.Select(d => d.TypeCode).FirstOrDefault()))
                .ForMember(dst => dst.HotTypeCodeId, opt => opt.MapFrom(src => src.HotTypeCodes.Select(d => d.HotTypeCodeId).FirstOrDefault()))
                .ReverseMap();
            CreateMap<HotTypes, HotTypeRecord>().ReverseMap();
            CreateMap<Payment, PaymentModel>()
                .ForMember(dst => dst.PaymentSource, opt => opt.MapFrom(src => src.PaymentSource.PaymentSource))
                .ForMember(dst => dst.PaymentType, opt => opt.MapFrom(src => src.PaymentType.PaymentType)).ReverseMap();
            CreateMap<ProductField, ProductFieldModel>().ReverseMap();
            CreateMap<ProductMetaData, ProductMetaDataModel>()
    .ForMember(dst => dst.BrandMetaId, opt => opt.MapFrom(src => src.ProductMetaId)).ReverseMap();
            CreateMap<ProductMetaDataType, ProductMetaDataTypeModel>().ReverseMap();
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<ProfileDiscount, ProfileDiscountModel>()
    .ForMember(dst => dst.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
    .ForMember(dst => dst.BrandSuffix, opt => opt.MapFrom(src => src.Brand.BrandSuffix))
    .ForMember(dst => dst.NetworkId, opt => opt.MapFrom(src => src.Brand.NetworkId))
    .ForMember(dst => dst.Network, opt => opt.MapFrom(src => src.Brand.Network))
    .ForMember(dst => dst.NetworkPrefix, opt => opt.MapFrom(src => src.Brand.Network.Prefix)).ReverseMap();
            CreateMap<Profile, ProfileModel>().ReverseMap();
            CreateMap<ReservationLog, ReservationLogModel>().ReverseMap();

        }
    }
}
