namespace Hot.Application.Common.Extensions
{
    public static class PinBatchExtensions
    {
        public static string GetPinBatchType(this PinBatch pinbatch, List<PinBatchType> list)
        {
            var result = list.Where(p => p.PinBatchTypeId == pinbatch.PinBatchTypeId).FirstOrDefault();
            if (result == null)
                return "Unknown";
            return result.PinBatchTypeText;
        }
    }
}
