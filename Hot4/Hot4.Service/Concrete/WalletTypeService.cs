using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Repository.Concrete;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class WalletTypeService : IWalletTypeService
    {
        private readonly IWalletTypeRepository _walletTypeRepository;
        private readonly IMapper Mapper;
        public WalletTypeService(IWalletTypeRepository walletTypeRepository, IMapper mapper)
        {
            _walletTypeRepository = walletTypeRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddWalletType(WalletTypeModel walletType)
        {
            if (walletType != null)
            {
                var model = Mapper.Map<WalletType>(walletType);
                return await _walletTypeRepository.AddWalletType(model);
            }
            return true;
            
        }
        public async Task<bool> DeleteWalletType(int walletTypeId)
        {
            var record = await GetEntityById(walletTypeId);
            if (record != null)
            {
                return await _walletTypeRepository.DeleteWalletType(record);
            }
            return false;
        }
        public async Task<WalletTypeModel?> GetWalletTypeById(int walletTypeId)
        {
            var record = await GetEntityById(walletTypeId);
            return Mapper.Map<WalletTypeModel>(record);
        }
        public async Task<List<WalletTypeModel>> ListWalletType()
        {
            var records = await _walletTypeRepository.ListWalletType();
            return Mapper.Map<List<WalletTypeModel>>(records);
        }
        public async Task<bool> UpdateWalletType(WalletTypeModel walletType)
        {
            var record = await GetEntityById(walletType.WalletTypeId);
            if (record != null)
            {
                var model = Mapper.Map(walletType, record);
                return await _walletTypeRepository.UpdateWalletType(record);
            }
            return false;
        }
        private async Task<WalletType?> GetEntityById(int WalletTypeId)
        {
            return await _walletTypeRepository.GetWalletTypeById(WalletTypeId);
        }
    }
}
