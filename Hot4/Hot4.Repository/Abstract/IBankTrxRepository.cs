using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxRepository
    {
        Task<BankTransactionModel> GetTranscation_by_Id(long bankTransactionId);
        Task<List<BankTransactionModel>> GetTranscation_by_Batch(long bankTransactionBatchId, bool isPending);
        Task<List<BankTransactionModel>> GetPendingTranscation_by_Type(byte bankTransactionTypeId);
        Task<List<BankTransactionModel>> GetAllTranscation_by_Type(byte bankTransactionTypeId); // pending stateId =0
        Task<List<BankTransactionModel>> GetTranscation_by_Ref(string bankRef);

        Task<long?> GetDuplicateTranscation(BankTransactionSearchModel bankTransactionSearch);
        Task<List<BankTransactionModel>> GetTranscation_by_PaymentId(string paymentId);
        Task<int?> GetEcoCashPendingTranscationCount(EcoCashSearchModel ecoCashSearch);
        Task<long?> AddBankTransaction(BankTrx bankTransaction);
        Task UpdateBankTransaction(BankTrx bankTransaction);
        Task UpdateBankTransaction_PaymentId(long paymentId, long bankTransactionId);
        Task UpdateBankTransaction_State(byte stateId, long bankTransactionId);
        Task UpdateBankTransaction_Identifier(string identifier, long bankTransactionId);
    }
}
