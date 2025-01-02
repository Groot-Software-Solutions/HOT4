using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ITransferRepository
    {
        Task<List<TransferModel>> GetTransferByPaymentId(long paymentId);
        Task<List<TransferModel>> ListTransfer(int pageNo, int pageSize);
        Task AddTransfer(Transfer transfer);
        Task UpdateTransfer(Transfer transfer);
        Task DeleteTransfer(Transfer transfer);
        Task<decimal> GetStockTradeBalance(long accountId);
        Task<StockTradeModel> GetStockTrade(StockTradeSearchModel stockTradeSearch);
    }
}
