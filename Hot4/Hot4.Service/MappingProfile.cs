
using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Service
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<BankTrxBatch, BankTrxBatchToDo>().ReverseMap();
            // CreateMap<BankTrxBatch, BankBatchModel>().ReverseMap();
            CreateMap<BankTrxBatch, BankBatchModel>().ForMember(dst => dst.BankName, opt => opt.MapFrom(src => src.BankId != 0 ? src.Bank.Bank : null)).ReverseMap();
        }
    }
}
