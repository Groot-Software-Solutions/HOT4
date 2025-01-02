using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ICommonRepository
    {
        Task<float> GetPrePaidStockBalance(int brandId);
        Task<decimal> GetBalance(long accountId);
        Task<decimal> GetSaleValue(long accountId);
        Task<List<ViewBalanceModel>> GetViewBalanceList(List<long> accountIds);
        Task<List<ViewAccountModel>> GetViewAccountList(List<long> accountIds);
        Task<decimal> GetUSDBalance(long accountId);

    }
}
