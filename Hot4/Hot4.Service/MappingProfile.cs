
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

        }
    }
}
