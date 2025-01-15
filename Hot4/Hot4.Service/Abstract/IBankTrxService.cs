using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IBankTrxService
    {
        Task<BankTransactionModel?> GetTrxById(long bankTransactionId);
        Task<List<BankTransactionModel>> GetTrxByBatchId(long bankTransactionBatchId, bool isPending);
        Task<List<BankTransactionModel>> GetPendingTrxByType(byte bankTransactionTypeId);
        Task<List<BankTransactionModel>> GetAllTrxByType(byte bankTransactionTypeId); // pending stateId =0
        Task<BankTransactionModel?> GetTrxByRef(string bankRef);
        Task<long?> GetDuplicateTrx(BankTransactionSearchModel bankTransactionSearch);
        Task<List<BankTransactionModel>> GetTrxByPaymentId(string paymentId);
        Task<int?> GetEcoCashPendingTrxCount(EcoCashSearchModel ecoCashSearch);
        Task<long?> AddBankTrx(BankTrxToDo bankTransaction);
        Task UpdateBankTrx(BankTrxToDo bankTransaction);
        //Task UpdateBankTrxPaymentId(long paymentId, long bankTransactionId);
        //Task UpdateBankTrxState(byte stateId, long bankTransactionId);
        //Task UpdateBankTrxIdentifier(string identifier, long bankTransactionId);
    }
}
