using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IMapper Mapper;

        public BankService(IBankRepository bankRepository , IMapper mapper)
        {
            _bankRepository = bankRepository;
            Mapper = mapper;
            
        }
        public async Task<List<BankModel>> ListBanks()
        {
            var records = await _bankRepository.ListBanks();
            return  Mapper.Map< List<BankModel>>(records);
        }
        public async Task<bool> AddBank(BankModel bankModel)
        {
            if (bankModel != null)
            {
              var model = Mapper.Map<Banks>(bankModel);
              return  await _bankRepository.AddBank(model);
            }
            return false;
        }
        public async Task<bool> UpdateBank(BankModel bankModel)
        {
            var record =await GetEntityById(bankModel.BankId);
            if (record != null)
            {
                Mapper.Map(bankModel, record);
               return await _bankRepository.UpdateBank(record);
            }
            return false;            
        }
        public async Task<bool> DeleteBank(byte BankId)
        {
            var record = await GetEntityById(BankId);
            if (record != null)
            {   
              return  await _bankRepository.DeleteBank(record);
            }
            return false;            
        }
        public async Task<BankModel> GetByBankId(byte BankId)
        {
            var record = await GetEntityById(BankId);
            return Mapper.Map<BankModel>(record);     
        }
        private async Task<Banks> GetEntityById (byte BankId)
        {
            return await _bankRepository.GetByBankId(BankId);
        }
    }
}
