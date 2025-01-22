using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class BankTrxService : IBankTrxService
    {
        private readonly IBankTrxRepository _bankTrxRepository;
        private readonly IMapper Mapper;
        public BankTrxService(IBankTrxRepository bankTrxRepository, IMapper mapper)
        {
            _bankTrxRepository = bankTrxRepository;
            Mapper = mapper;
        }
        public async Task<BankTransactionModel?> GetTrxById(long bankTransactionId)
        {
            var record = await GetEntityById(bankTransactionId);
            return Mapper.Map<BankTransactionModel?>(record);
        }
        public async Task<List<BankTransactionModel>> GetTrxByBatchId(long bankTransactionBatchId, bool isPending)
        {
            var records = await _bankTrxRepository.GetTrxByBatchId(bankTransactionBatchId, isPending);
            return Mapper.Map<List<BankTransactionModel>>(records);
        }
        public async Task<List<BankTransactionModel>> GetPendingTrxByType(byte bankTransactionTypeId)
        {
            var records = await _bankTrxRepository.GetPendingTrxByType(bankTransactionTypeId);
            return Mapper.Map<List<BankTransactionModel>>(records);
        }
        public async Task<List<BankTransactionModel>> GetAllTrxByType(byte bankTransactionTypeId)
        {
            var records = await _bankTrxRepository.GetAllTrxByType(bankTransactionTypeId);
            return Mapper.Map<List<BankTransactionModel>>(records);
        }
        public async Task<BankTransactionModel?> GetTrxByRef(string bankRef)
        {
            var record = await _bankTrxRepository.GetTrxByRef(bankRef);
            return Mapper.Map<BankTransactionModel?>(record);
        }
        public async Task<long?> GetDuplicateTrx(BankTransactionSearchModel bankTransactionSearch)
        {
            return await _bankTrxRepository.GetDuplicateTrx(bankTransactionSearch);
        }
        public async Task<List<BankTransactionModel>> GetTrxByPaymentId(string paymentId)
        {
            var records = await _bankTrxRepository.GetTrxByPaymentId(paymentId);
            return Mapper.Map<List<BankTransactionModel>>(records);

        }
        public Task<int?> GetEcoCashPendingTrxCount(EcoCashSearchModel ecoCashSearch)
        {
            return _bankTrxRepository.GetEcoCashPendingTrxCount(ecoCashSearch);
        }
        public async Task<bool> AddBankTrx(BankTrxRecord bankTransaction)
        {
            if (bankTransaction != null)
            {
                var model = Mapper.Map<BankTrx>(bankTransaction);
                return await _bankTrxRepository.AddBankTrx(model);
            }
            return false;
        }

        public async Task<bool> UpdateBankTrx(BankTrxRecord bankTransaction)
        {
            var record = await GetEntityById(bankTransaction.BankTrxId);
            if (record != null)
            {
                Mapper.Map(bankTransaction, record);
                return await _bankTrxRepository.UpdateBankTrx(record);
            }
            return false;
        }
        private async Task<BankTrx?> GetEntityById (long bankTrxId)
        {
            return await _bankTrxRepository.GetTrxById(bankTrxId);
        }

    }
}
