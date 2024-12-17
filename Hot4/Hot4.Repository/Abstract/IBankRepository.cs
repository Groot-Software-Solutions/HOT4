using Hot4.Core.DataViewModels;
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IBankRepository
    {
        Task AddBankTrx(BankTrx bankTrx);

        Task UpdateBankTrx(BankTrx bankTrx);
        Task<BankTrxBatch> AddBankTrxBatch(BankTrxBatch tblBankTrxBatch);


        Task UpdatePaymentIDBankTrx(long paymentId, long bankTrxId);

        Task UpdateStateDBankTrx(byte bankTrxStateIDId, long bankTrxId);

        Task UpdateIdentifierDBankTrx(string identifier, long bankTrxId);

        Task<List<BankTransactionModel>> ListBankTransactions(long bankTrxBatchId);

        Task<List<BankBatchModel>> ListBankTransactionBatches(long bankId);



        Task<List<Banks>> ListBanks();

        Task<BankTrxBatch?> GetBatch(byte bankID, string BatchReference);
    }
}
