using AutoMapper;
using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;
using Hot.Nyaradzo.Application.Common.Models;
using Hot.Nyaradzo.Domain.Entities; 

namespace Hot.Nyaradzo.Application.Common.Mapping;

    public class NyaradzoResultModelProfile : Profile
    {
        public NyaradzoResultModelProfile()
        {
            CreateMap<NyaradzoResult, NyaradzoResultModel>().ReverseMap();
        }
    }

    public class AccountSummaryProfile : Profile
    {
        public AccountSummaryProfile()
        {
            CreateMap<NyaradzoAccountSummary, AccountSummary>().ReverseMap();
        }
    }

    public class PaymentRequestProfile : Profile
    {
        public PaymentRequestProfile()
        {
            CreateMap<NyaradzoPaymentRequest, PaymentRequest>().ReverseMap();
        }
    }

    public class ReversalResultProfile : Profile
    {
        public ReversalResultProfile()
        {
            CreateMap<NyaradzoReversalResult, ReversalResult>().ReverseMap();
        }
    }

