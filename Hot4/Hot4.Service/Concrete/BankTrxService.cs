using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class BankTrxService : IBankTrxService
    {
        private IBankTrxRepository _bankTrxRepository;
        public IMapper Mapper { get; }
        public BankTrxService(IBankTrxRepository bankTrxRepository, IMapper mapper)
        {
            _bankTrxRepository = bankTrxRepository;
            Mapper = mapper;
        }
        public async Task<BankTransactionModel?> GetTrxById(long bankTransactionId)
        {
            var res = await _bankTrxRepository.GetTrxById(bankTransactionId);
            return Mapper.Map<BankTransactionModel?>(res);
        }
        public async Task<List<BankTransactionModel>> GetTrxByBatchId(long bankTransactionBatchId, bool isPending)
        {
            var res = await _bankTrxRepository.GetTrxByBatchId(bankTransactionBatchId, isPending);
            return Mapper.Map<List<BankTransactionModel>>(res);
        }
        public async Task<List<BankTransactionModel>> GetPendingTrxByType(byte bankTransactionTypeId)
        {
            var res = await _bankTrxRepository.GetPendingTrxByType(bankTransactionTypeId);
            return Mapper.Map<List<BankTransactionModel>>(res);
        }
        public async Task<List<BankTransactionModel>> GetAllTrxByType(byte bankTransactionTypeId)
        {
            var res = await _bankTrxRepository.GetAllTrxByType(bankTransactionTypeId);
            return Mapper.Map<List<BankTransactionModel>>(res);
        }
        public async Task<BankTransactionModel?> GetTrxByRef(string bankRef)
        {
            var res = await _bankTrxRepository.GetTrxByRef(bankRef);
            return Mapper.Map<BankTransactionModel?>(res);
        }

        public async Task<long?> GetDuplicateTrx(BankTransactionSearchModel bankTransactionSearch)
        {
            return await _bankTrxRepository.GetDuplicateTrx(bankTransactionSearch);
        }
        public async Task<List<BankTransactionModel>> GetTrxByPaymentId(string paymentId)
        {
            var res = await _bankTrxRepository.GetTrxByPaymentId(paymentId);
            return Mapper.Map<List<BankTransactionModel>>(res);

        }
        public Task<int?> GetEcoCashPendingTrxCount(EcoCashSearchModel ecoCashSearch)
        {
            return _bankTrxRepository.GetEcoCashPendingTrxCount(ecoCashSearch);
        }
        public async Task<long?> AddBankTrx(BankTrxToDo bankTransaction)
        {
            var res = Mapper.Map<BankTrx>(bankTransaction);
            return await _bankTrxRepository.AddBankTrx(res);
        }

        public async Task UpdateBankTrx(BankTrxToDo bankTransaction)
        {
            var res = await GetTrxById(bankTransaction.BankTrxId);
            if (res != null)
            {
                var model = Mapper.Map<BankTrx>(bankTransaction);
                await _bankTrxRepository.UpdateBankTrx(model);
            }
            else
            {
                throw new Exception("Bank Transaction not found");
            }
        }

        //public async Task UpdateBankTrxIdentifier(string identifier, long bankTransactionId)
        //{
        //    var res = await GetTrxById(bankTransactionId);
        //    if (res != null)
        //    {
        //        res.Identifier = identifier;
        //        await UpdateBankTrx(Mapper.Map<BankTrxToDo>(res));
        //    }
        //    else
        //    {
        //        throw new Exception("Bank Transaction not found");
        //    }
        //}

        //public async Task UpdateBankTrxPaymentId(long paymentId, long bankTransactionId)
        //{
        //    var res = await GetTrxById(bankTransactionId);
        //    if (res != null)
        //    {
        //        res.PaymentId = paymentId;
        //        await UpdateBankTrx(Mapper.Map<BankTrxToDo>(res));
        //    }
        //    else
        //    {
        //        throw new Exception("Bank Transaction not found");
        //    }
        //}

        //public async Task UpdateBankTrxState(byte stateId, long bankTransactionId)
        //{
        //    var res = await GetTrxById(bankTransactionId);
        //    if (res != null)
        //    {
        //        res.BankTrxStateId = stateId;
        //        await UpdateBankTrx(Mapper.Map<BankTrxToDo>(res));
        //    }
        //    else
        //    {
        //        throw new Exception("Bank Transaction not found");
        //    }
        //}
    }
}
