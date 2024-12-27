using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class NetworkBalanceRepository : INetworkBalanceRepository
    {
        public ICommonRepository _commonRepository;
        public HotDbContext _context;
        public NetworkBalanceRepository(ICommonRepository commonRepository, HotDbContext context)
        {
            _commonRepository = commonRepository;
            _context = context;
        }
        public async Task<List<NetworkBalanceModel>> ListNetworkBalance(int BrandId)
        {
            var balanceNetOne = await _commonRepository.GetPrePaidStockBalance((int)Brands.EasyCall);

            var balanceTelecel = await _commonRepository.GetPrePaidStockBalance((int)Brands.Juice);

            var balanceZesa = (float)await (from r in _context.Recharge
                                            where r.BrandId == (int)Brands.ZETDC && r.StateId == (int)SmsState.Success
                                            join rp in _context.RechargePrepaid on r.RechargeId equals rp.RechargeId
                                            select rp.InitialBalance)
                                    .LastOrDefaultAsync();

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
