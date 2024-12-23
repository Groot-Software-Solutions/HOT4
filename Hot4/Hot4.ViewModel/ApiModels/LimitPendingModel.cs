namespace Hot4.ViewModel.ApiModels
{
    public class LimitPendingModel
    {
        public float LimitRemaining { get; set; }
        public float RemainingLimit { get; set; }
        public float RemainingDailyLimit { get; set; }
        public float RemainingMonthlyLimit { get; set; }
        public float MonthlyLimit { get; set; }
        public float SalesMonthly { get; set; }
        public int LimitTypeId { get; set; }
        public int NetworkId { get; set; }
        public float DailyLimit { get; set; }
        public float SalesToday { get; set; }

    }
}
