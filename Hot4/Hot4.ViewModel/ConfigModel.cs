namespace Hot4.ViewModel
{
    public class ConfigModel
    {
        public byte ConfigId { get; set; }
        public int ProfileIdNewSmsdealer { get; set; }
        public int ProfileIdNewWebDealer { get; set; }
        public decimal MinRecharge { get; set; }
        public decimal MaxRecharge { get; set; }
        public bool PrepaidEnabled { get; set; }
        public decimal MinTransfer { get; set; }
    }
}
