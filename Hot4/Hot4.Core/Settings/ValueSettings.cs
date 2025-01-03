namespace Hot4.Core.Settings
{
    public class ValueSettings
    {
        public int RechargeIdByBrandIds { get; set; }
        public string SmsBulkSendExcludeAccessCode { get; set; }
        public string SmsBulkSendGreaterThenMobileNo { get; set; }
        public string SmsBulkSmsSendExcludeAccessCode { get; set; }
        public string SmsBulkSmsSendGreaterThenMobileNo { get; set; }
        public int SMSInboxQueueSize { get; set; }
        public int SMSOutboxQueueSize { get; set; }
        public int StockTradeRateNullValue { get; set; }
        public string StockTradePaymentByRef { get; set; }
        public string StockTradeSuccessResponse { get; set; }
        public string StockTradeInvalidAmountResponse { get; set; }
        public string StockTradeInvalidPaymentResponse { get; set; }
    }
}
