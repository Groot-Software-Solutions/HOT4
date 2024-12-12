namespace Hot4.Core.DataViewModels
{
    public class AttributeModel
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string? Text { get; set; }
        public decimal? Numeric { get; set; }
        public DateTime? Date { get; set; }
    }
}
