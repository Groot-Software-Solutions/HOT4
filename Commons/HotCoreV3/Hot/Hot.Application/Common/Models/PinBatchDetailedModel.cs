namespace Hot.Application.Common.Models
{
    public class PinBatchDetailedModel: PinBatch
    {
        public string PinBatchType { get; set; } = string.Empty;
    }
    public class PinBatchDetailedModelProfile : AutoMapper.Profile
    {
        public PinBatchDetailedModelProfile()
        {
            CreateMap<PinBatch, PinBatchDetailedModel>();
        }
    }
}
