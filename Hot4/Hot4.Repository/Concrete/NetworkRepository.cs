using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class NetworkRepository : RepositoryBase<Networks>, INetworkRepository
    {
        private readonly ICommonRepository _commonRepository;
        public NetworkRepository(HotDbContext context, ICommonRepository commonRepository) : base(context)
        {
            _commonRepository = commonRepository;
        }

        public async Task<bool> AddNetwork(Networks networks)
        {
            await Create(networks);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateNetwork(Networks networks)
        {
            Update(networks);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteNetwork(Networks networks)
        {
            Delete(networks);
            await SaveChanges();
            return true;
        }
        public async Task<Networks?> GetNetworkById(byte networkId)
        {
            return await GetById(networkId);
        }
        public async Task<List<Networks>> GetNetworkIdentityByMobile(string mobile)
        {
            return await GetByCondition(d => mobile.Substring(1, 2) == d.Prefix || mobile.Substring(1, 4) == d.Prefix)
                .ToListAsync();
        }
        public async Task<List<NetworkBalanceModel>> GetBrandNetworkBalance()
        {
            var balanceNetOne = await _commonRepository.GetPrePaidStockBalance((int)Brands.EasyCall);

            var balanceTelecel = await _commonRepository.GetPrePaidStockBalance((int)Brands.Juice);

            var balanceZesa = await _commonRepository.GetPrePaidStockBalance((int)Brands.Juice);

            return new List<NetworkBalanceModel>
                                  {
                                    new NetworkBalanceModel
                                       {
                                         BrandId = (int)Brands.EasyCall,
                                         Balance = balanceNetOne,
                                         Name =NetworkName.NetOne.ToString() // "Netone"
                                    },
                                  new NetworkBalanceModel
                                         {
                                            BrandId = (int)Brands.Juice,
                                      Balance = balanceTelecel,
                                          Name =NetworkName.Telecel.ToString() // "Telecel"
                                  },
                                    new NetworkBalanceModel
                                             {
                                          BrandId = (int)Brands.ZETDC,
                                  Balance = balanceZesa,
                                  Name =NetworkName.ZESA.ToString() // "ZESA"
                                    }
            };

        }
    }
}
