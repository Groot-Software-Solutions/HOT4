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
        private readonly IMapper Mapper;
        public BankTrxBatchService(IBankTrxBatchRepository bankTrxBatchRepository, IMapper mapper)
        {
            _bankTrxBatchRepository = bankTrxBatchRepository;
            this.Mapper = mapper;
        }

        public async Task<bool> AddBatch(BankTrxBatchToDo bankTrxBatch)
        {
            var model = Mapper.Map<BankTrxBatch>(bankTrxBatch);
            await _bankTrxBatchRepository.AddBatch(model);
            return true;
        }
        public async Task<bool> UpdateBatch(BankTrxBatchToDo bankTrxBatch)
        {
            var record = await _bankTrxBatchRepository.GetBatchById(bankTrxBatch.BankTrxBatchId);
            if (record != null)
            {
                Mapper.Map(bankTrxBatch, record);
                return await _bankTrxBatchRepository.UpdateBatch(record);
            }
            return false;
        }
        public async Task<bool> DeleteBatch(long batchId)
        {
            var record = await _bankTrxBatchRepository.GetBatchById(batchId);
            if (record != null)
            {
                return await _bankTrxBatchRepository.DeleteBatch(record);
            }
            return false;
        }
        public async Task<BankBatchModel?> GetBatchById(long batchId)
        {
            var record = await _bankTrxBatchRepository.GetBatchById(batchId);
            return Mapper.Map<BankBatchModel?>(record);
        }

        public async Task<List<BankBatchModel>> GetBatchByBankId(byte bankId)
        {
            var records = await _bankTrxBatchRepository.GetBatchByBankId(bankId);
            return Mapper.Map<List<BankBatchModel>>(records);
        }

        public async Task<BankBatchModel?> GetCurrentBatch(byte bankId, string batchReference, string lastUser)
        {
            var record = await _bankTrxBatchRepository.GetCurrentBatch(bankId, batchReference, lastUser);
            return Mapper.Map<BankBatchModel?>(record);
        }

        public async Task<long?> GetCurrentBatchByBankIdAndRefId(byte bankId, string batchRef = null)
        {
            return await _bankTrxBatchRepository.GetCurrentBatchByBankIdAndRefId(bankId, batchRef);
        }
    }
}
