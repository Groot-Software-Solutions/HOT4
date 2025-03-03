namespace Hot.Application.Common.Models
{
    public  class PaymentDetailedModel: Payment
    {
        public string? PaymentSource { get; set; }
        public string? PaymentType { get; set; }
    }

    public class PaymentDetailModelProfile : AutoMapper.Profile
    {
        public PaymentDetailModelProfile()
        {
            CreateMap<Payment,PaymentDetailedModel>();
        }
    }
}
