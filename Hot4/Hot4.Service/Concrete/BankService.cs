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
            var result = await _bankRepository.ListBanks();
            var res =  _mapper.Map< List<BankModel>>(result);
            return res;

        }
        public async Task<bool> AddBank(BankModel bankModel)
        {
            var payload = _mapper.Map<Banks>(bankModel);
            await _bankRepository.AddBank(payload);
            return true;

        }

        public async Task<bool> UpdateBank(BankModel bankModel)
        {
            var payload = _mapper.Map<Banks>(bankModel);
            await _bankRepository.UpdateBank(payload);
            return true;
        }

        public async Task<bool> DeleteBank(BankModel bankModel)
        {
            var payload = _mapper.Map<Banks>(bankModel);
           await _bankRepository.DeleteBank(payload);
            return true;
        }


    }
}
