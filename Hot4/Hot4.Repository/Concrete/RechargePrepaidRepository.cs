//using Hot4.Core.DataViewModels;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;

namespace Hot4.Repository.Concrete
{
    public class RechargePrepaidRepository : RepositoryBase<RechargePrepaid>, IRechargePrepaidRepository
    {
        public RechargePrepaidRepository(HotDbContext context) : base(context) { }
        public async Task AddRechargePrepaid(RechargePrepaid rechargeprepaid)
        {
            await Create(rechargeprepaid);
            await SaveChanges();
        }
        public async Task UpdateRechargePrepaid(RechargePrepaid rechargePrepaid)
        {
            Update(rechargePrepaid);
            await SaveChanges();
        }
        public async Task<RechargePrepaidModel?> GetRechargePrepaidById(long rechargeId)
        {
            var result = await GetById(rechargeId);
            if (result != null)
            {
                return new RechargePrepaidModel
                {
                    Data = result.Data,
                    DebitCredit = result.DebitCredit,
                    FinalBalance = result.FinalBalance,
                    FinalWallet = result.FinalWallet,
                    InitialBalance = result.InitialBalance,
                    InitialWallet = result.InitialWallet,
                    Narrative = result.Narrative,
                    RechargeId = result.RechargeId,
                    Reference = result.Reference,
                    ReturnCode = result.ReturnCode,
                    SMS = result.Sms,
                    Window = result.Window
                };
            }
            return null;
        }

    }
}
