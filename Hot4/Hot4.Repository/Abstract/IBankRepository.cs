using Hot4.Core.DataViewModels;
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IBankRepository
    {
        Task AddBankTrx(TblBankTrx bankTrx);

        Task UpdateBankTrx(TblBankTrx bankTrx);
        Task<TblBankTrxBatch> AddBankTrxBatch(TblBankTrxBatch tblBankTrxBatch);


        Task UpdatePaymentIDBankTrx(long paymentId, long bankTrxId);

        Task UpdateStateDBankTrx(byte bankTrxStateIDId, long bankTrxId);

        Task UpdateIdentifierDBankTrx(string identifier, long bankTrxId);

        Task<List<BankTransactionModel>> ListBankTransactions(long bankTrxBatchId);

        Task<List<BankBatchModel>> ListBankTransactionBatches(long bankId);



        Task<List<TblBank>> ListBanks();

        Task<TblBankTrxBatch?> GetBatch(byte bankID, string BatchReference);
    }
}
