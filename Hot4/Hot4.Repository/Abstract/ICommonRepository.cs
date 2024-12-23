namespace Hot4.Repository.Abstract
{
    public interface ICommonRepository
    {
        public Task<float> GetPrePaidStockBalance(int brandId);
    }
}
