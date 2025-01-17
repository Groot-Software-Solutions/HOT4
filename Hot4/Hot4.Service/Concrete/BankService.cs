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
        private readonly IMapper _mapper;

        public BankService(IBankRepository bankRepository , IMapper mapper)
        {
            _bankRepository = bankRepository;
            _mapper = mapper;
            
        }
        public async Task<List<BankModel>> ListBanks()
        {
            var records = await _bankRepository.ListBanks();
            var model =  _mapper.Map< List<BankModel>>(records);
            return model;

        }
        public async Task<bool> AddBank(BankModel bankModel)
        {
            if (bankModel != null)
            {
                var model = _mapper.Map<Banks>(bankModel);
              return  await _bankRepository.AddBank(model);
            }
            return false;
        }
        public async Task<bool> UpdateBank(BankModel bankModel)
        {
            var record =await _bankRepository.GetByBankId(bankModel.BankId);
            if (record != null)
            {
                _mapper.Map(bankModel, record);
               return await _bankRepository.UpdateBank(record);
            }
            return false;
            
        }
        public async Task<bool> DeleteBank(BankModel bankModel)
        {
            var record = await _bankRepository.GetByBankId(bankModel.BankId);
            if (record != null)
            {   
              return  await _bankRepository.DeleteBank(record);
            }
            return false;
            
        }

    }
}
