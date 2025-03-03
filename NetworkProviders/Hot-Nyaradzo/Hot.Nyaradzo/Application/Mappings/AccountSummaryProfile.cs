using AutoMapper;
using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;
using Hot.Domain.Enums;
using Hot.Nyaradzo.Domain.Entities;

namespace Hot.Nyaradzo.Application.Mappings;
public class AccountSummaryProfile : Profile
{
    //public AccountSummaryProfile()
    //{
    //    CreateMap<AccountSummary, NyaradzoAccountSummary>()
    //       .ForMember(n => n.currency, 
    //        mp => mp.MapFrom(ny => (Enum.GetNames(typeof(Currency)).Contains(ny.currencyCode) 
    //        ? Enum.Parse<Currency>(ny.currencyCode) 
    //        : Currency.ZWG) 
    //       )).ReverseMap();
    //}
 

}
