using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxRepository
    {
        Task<BankTransactionModel> GetTranscationById(long bankTransactionId);
        Task<List<BankTransactionModel>> GetTranscationByBatch(long bankTransactionBatchId, bool isPending);
        Task<List<BankTransactionModel>> GetPendingTranscationByType(byte bankTransactionTypeId);
        Task<List<BankTransactionModel>> GetAllTranscationByType(byte bankTransactionTypeId); // pending stateId =0
        Task<List<BankTransactionModel>> GetTranscationByRef(string bankRef);

        Task<long?> GetDuplicateTranscation(BankTransactionSearchModel bankTransactionSearch);
        Task<List<BankTransactionModel>> GetTranscationByPaymentId(string paymentId);
        Task<int?> GetEcoCashPendingTranscationCount(EcoCashSearchModel ecoCashSearch);
        Task<long?> AddBankTransaction(BankTrx bankTransaction);
        Task UpdateBankTransaction(BankTrx bankTransaction);
        Task UpdateBankTransactionPaymentId(long paymentId, long bankTransactionId);
        Task UpdateBankTransactionState(byte stateId, long bankTransactionId);
        Task UpdateBankTransactionIdentifier(string identifier, long bankTransactionId);
    }
}
