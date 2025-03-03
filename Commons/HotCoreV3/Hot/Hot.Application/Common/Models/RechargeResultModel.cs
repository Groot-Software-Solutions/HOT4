using Profile = AutoMapper.Profile;

namespace Hot.Application.Common.Models
{
    public class RechargeResultModel : Recharge

    {
        public string State { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string AccessCode { get; set; } = string.Empty;

    }
    public class RechargeResultModelModelProfile : Profile
    {
        public RechargeResultModelModelProfile()
        {

            CreateMap<RechargeResultModel,Recharge>().ReverseMap();

        }
    }
}
