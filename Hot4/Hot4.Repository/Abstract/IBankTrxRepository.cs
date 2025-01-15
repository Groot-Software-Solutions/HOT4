using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxRepository
    {
        Task<BankTrx?> GetTrxById(long bankTransactionId);
        Task<List<BankTrx>> GetTrxByBatchId(long bankTransactionBatchId, bool isPending);
        Task<List<BankTrx>> GetPendingTrxByType(byte bankTransactionTypeId);
        Task<List<BankTrx>> GetAllTrxByType(byte bankTransactionTypeId); // pending stateId =0
        Task<BankTrx?> GetTrxByRef(string bankRef);
        Task<long?> GetDuplicateTrx(BankTransactionSearchModel bankTransactionSearch);
        Task<List<BankTrx>> GetTrxByPaymentId(string paymentId);
        Task<int?> GetEcoCashPendingTrxCount(EcoCashSearchModel ecoCashSearch);
        Task<long?> AddBankTrx(BankTrx bankTransaction);
        Task UpdateBankTrx(BankTrx bankTransaction);

    }
}
