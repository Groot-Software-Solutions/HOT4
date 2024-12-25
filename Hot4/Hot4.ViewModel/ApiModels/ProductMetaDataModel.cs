namespace Hot4.ViewModel.ApiModels
{
    public class ProductMetaDataModel
    {
        public int ProductMetaId { get; set; }
        public byte ProductId { get; set; }
        public byte ProductMetaDataTypeId { get; set; }
        public string Data { get; set; }
        public int BrandMetaId { get; set; }
    }
}
