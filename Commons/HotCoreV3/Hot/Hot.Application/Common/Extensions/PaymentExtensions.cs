namespace Hot.Application.Common.Extensions
{
    public static class PaymentExtensions
    {
        public static string GetPaymentSource(this Payment payment, List<PaymentSource> list)
        {
            var result = list.Where(p => p.PaymentSourceId == payment.PaymentSourceId).FirstOrDefault();
            if (result == null)
                return "Unknown";
            return result.PaymentSourceText;
        }
        public static string GetPaymentType(this Payment payment, List<PaymentType> list)
        {
            var result = list.Where(p => p.PaymentTypeId == payment.PaymentTypeId).FirstOrDefault();
            if (result == null)
                return "Unknown";
            return result.PaymentTypeText;
        }
    }
}
