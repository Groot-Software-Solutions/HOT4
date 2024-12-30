//using Hot4.Core.DataViewModels;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class RechargePrepaidRepository : RepositoryBase<RechargePrepaid>, IRechargePrepaidRepository
    {
        public RechargePrepaidRepository(HotDbContext context) : base(context) { }
        public async Task InsertRechargePrepaid(RechargePrepaid rechargeprepaid)
        {
            await Create(rechargeprepaid);
            await SaveChanges();
        }

        public async Task UpdateRechargePrepaid(RechargePrepaid rechargePrepaid)
        {
            await Update(rechargePrepaid);
            await SaveChanges();
        }
        public async Task<RechargePrepaid?> GetRechargePrepaid(long rechargeId)
        {
            return await GetById(rechargeId);
        }
        public async Task<RechargePrepaid?> GetRechargeReversal(long rechargeId)
        {
            return await GetByCondition(d => d.Reference == rechargeId.ToString()).FirstOrDefaultAsync();
        }



        //public async Task<AccountRechargePrepaidModel> SelectRechargePrepaid(long rechargeId)
        //{
        //    var result = await GetById(rechargeId);


        //    if (result != null)
        //    {
        //        var recharge = new AccountRechargePrepaidModel()
        //        {
        //            Data = result.Data,
        //            DebitCredit = result.DebitCredit ? "Debit" : "Credit",
        //            FinalBalance = result.FinalBalance,
        //            InitialBalance = result.InitialBalance,
        //            Reference = result.Reference,
        //            SMS = result.Sms,
        //            Narrative = result.Narrative,
        //            RechargeID = result.RechargeId,
        //            FinalWallet = result.FinalWallet,
        //            InitialWallet = result.InitialWallet,
        //            ReturnCode = result.ReturnCode,
        //            Window = result.Window

        //        };

        //        return recharge;
        //    }

        //    return ReturnEmpty();
        //}
        //private AccountRechargePrepaidModel ReturnEmpty()
        //{
        //    return new AccountRechargePrepaidModel()
        //    {
        //        Data = string.Empty,
        //        DebitCredit = string.Empty,
        //        FinalBalance = 0,
        //        InitialBalance = 0,
        //        Reference = string.Empty,
        //        SMS = string.Empty,
        //        Narrative = string.Empty,
        //        RechargeID = 0,
        //        FinalWallet = 0,
        //        InitialWallet = 0,
        //        ReturnCode = string.Empty,
        //        Window = DateTime.Now
        //    };
        //}


    }
}
