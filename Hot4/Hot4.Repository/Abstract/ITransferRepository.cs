using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ITransferRepository
    {
        Task<List<Transfer>> GetTransferByPaymentId(long paymentId);
        Task<Transfer?> GetTransferById(long transferId);
        Task<List<Transfer>> ListTransfer(int pageNo, int pageSize);
        Task<bool> AddTransfer(Transfer transfer);
        Task<bool> UpdateTransfer(Transfer transfer);
        Task<bool> DeleteTransfer(Transfer transfer);
        Task<decimal> GetStockTradeBalByAccountId(long accountId);
        Task<StockTradeModel> GetStockTrade(StockTradeSearchModel stockTradeSearch);
    }
}
