using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class BankTrxBatchService : IBankTrxBatchService
    {
        private IBankTrxBatchRepository _bankTrxBatchRepository;
        private IMapper Mapper { get; }
        public BankTrxBatchService(IBankTrxBatchRepository bankTrxBatchRepository, IMapper mapper)
        {
            _bankTrxBatchRepository = bankTrxBatchRepository;
            this.Mapper = mapper;
        }

        public async Task<long> AddBatch(BankTrxBatchToDo bankTrxBatch)
        {
            var res = Mapper.Map<BankTrxBatchToDo, BankTrxBatch>(bankTrxBatch);
            return await _bankTrxBatchRepository.AddBatch(res);
        }
        public async Task UpdateBatch(BankTrxBatchToDo bankTrxBatch)
        {
            var res = await _bankTrxBatchRepository.GetBatchById(bankTrxBatch.BankTrxBatchId);
            if (res != null)
            {
                var model = Mapper.Map<BankTrxBatchToDo, BankTrxBatch>(bankTrxBatch);
                await _bankTrxBatchRepository.UpdateBatch(model);
            }
            else
            {
                throw new Exception("Batch not found");
            }
        }
        public async Task<BankBatchModel?> GetBatchById(long batchId)
        {
            var res = await _bankTrxBatchRepository.GetBatchById(batchId);
            if (res != null)
            {
                return Mapper.Map<BankTrxBatch, BankBatchModel>(res);
            }
            return null;
        }
        public async Task DeleteBatch(long batchId)
        {
            var res = await _bankTrxBatchRepository.GetBatchById(batchId);
            if (res != null)
            {
                await _bankTrxBatchRepository.DeleteBatch(res);
            }
            else
            {
                throw new Exception("Batch not found");
            }
        }

        public async Task<List<BankBatchModel>> GetBatchByBankId(byte bankId)
        {
            var res = await _bankTrxBatchRepository.GetBatchByBankId(bankId);
            return Mapper.Map<List<BankTrxBatch>, List<BankBatchModel>>(res);
        }

        public async Task<BankBatchModel?> GetCurrentBatch(byte bankId, string batchReference, string lastUser)
        {
            var res = await _bankTrxBatchRepository.GetCurrentBatch(bankId, batchReference, lastUser);
            return Mapper.Map<BankTrxBatch, BankBatchModel>(res);
        }

        public async Task<long?> GetCurrentBatchByBankIdAndRefId(byte bankId, string batchRef = null)
        {
            return await _bankTrxBatchRepository.GetCurrentBatchByBankIdAndRefId(bankId, batchRef);
        }


    }
}
