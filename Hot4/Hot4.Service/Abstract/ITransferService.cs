using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface ITransferService
    {
        Task<List<TransferModel>> GetTransferByPaymentId(long paymentId);
        Task<TransferModel> GetTransferById(long transferId);
        Task<List<TransferModel>> ListTransfer(int pageNo, int pageSize);
        Task<bool> AddTransfer(TransferToDo transfer);
        Task<bool> UpdateTransfer(TransferToDo transfer);
        Task<bool> DeleteTransfer(long transferId);
        Task<decimal> GetStockTradeBalByAccountId(long accountId);
        Task<StockTradeModel> GetStockTrade(StockTradeSearchModel stockTradeSearch);
    }
}
