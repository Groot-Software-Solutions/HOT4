using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using OneOf;
using System.Reflection;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class Recharges : Table<Recharge>, IRecharges
    {
        public Recharges(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.AddSuffix = "_Save";
            base.AddParameters = "@RechargeID, @StateID ,@AccessID ,@Amount,@Discount,@Mobile,@BrandID ,@SMSID";
            base.UpdateParameters = AddParameters; 
            base.UpdateSuffix = AddSuffix;
            base.GetSuffix = "_Select";
            base.SearchSuffix = "_Find_By_Mobile";
            base.SearchParameter = "Mobile";

        }


        public override async Task<OneOf<int, HotDbException>> AddAsync(Recharge item)
        {
            string query = $"{GetSPPrefix()}{AddSuffix} {AddParameters}";
            var result = await dbHelper.ExecuteScalar<int,object>(query, 
                new { item.RechargeId,item.StateId,item.AccessId,item.Amount,item.Discount,item.Mobile,item.BrandId,SMSID=0});
            return result;
        }
        public override async Task<OneOf<bool, HotDbException>> UpdateAsync(Recharge item)
        {
            string query = $"{GetSPPrefix()}{UpdateSuffix} {UpdateParameters}"; 
            var result = await dbHelper.Execute(query, 
                new { item.RechargeId, item.StateId, item.AccessId, item.Amount, item.Discount, item.Mobile, item.BrandId, SMSID = 0 });
            return result;
        }
        public OneOf<List<Recharge>, HotDbException> PendingAfricom()
        {
          return PendingAfricomAsync().Result;
        }
        public async Task<OneOf<List<Recharge>, HotDbException>> PendingAfricomAsync()
        {
            return await dbHelper.Query<Recharge>($"{GetSPPrefix()}_Pending_Africom");
        }
        public OneOf<List<Recharge>, HotDbException> Pending(List<int> BrandIds)
        {
            return PendingAsync(BrandIds).Result;
        }
        public async Task<OneOf<List<Recharge>, HotDbException>> PendingAsync(List<int> BrandIds)
        { 
            var param = new Dictionary<string, object>() { { $"@BrandId", string.Join(",", BrandIds) } };
            return await dbHelper.Query<Recharge>($"{GetSPPrefix()}_PendingByBrandIds",param);
        }
        public OneOf<List<Recharge>, HotDbException> PendingEconet()
        {
            return PendingEconetAsync().Result;
        }
        public async Task<OneOf<List<Recharge>, HotDbException>> PendingEconetAsync()
        {
            return await dbHelper.Query<Recharge>($"{GetSPPrefix()}_Pending_Econet");
        }
        public OneOf<List<Recharge>, HotDbException> PendingNetOne()
        {
            return PendingNetOneAsync().Result;
        }

        public async Task<OneOf<List<Recharge>, HotDbException>> PendingNetOneAsync()
        {
            return await dbHelper.Query<Recharge>($"{GetSPPrefix()}_Pending_Netone");
        }

        public OneOf<List<Recharge>, HotDbException> PendingOther()
        {
            return PendingOtherAsync().Result;
        }

        public async Task<OneOf<List<Recharge>, HotDbException>> PendingOtherAsync()
        {
            return await dbHelper.Query<Recharge>($"{GetSPPrefix()}_Pending_Other");
        }

        public OneOf<List<Recharge>, HotDbException> SelectAggregator(long AccountId, DateTime StartDate, DateTime EndDate)
        {
            return SelectAggregatorAsync(AccountId, StartDate, EndDate).Result;
        }

        public async Task<OneOf<List<Recharge>, HotDbException>> SelectAggregatorAsync(long AccountId, DateTime StartDate, DateTime EndDate)
        {
            var  Query = $"{GetSPPrefix()}_AggregatorSelect @Accountid,@startdate,@endate";
            return await dbHelper.Query<Recharge>(Query, new { AccountId, StartDate, EndDate });
        }

        public OneOf<Recharge, HotDbException> WebDuplicate(long AccessId, string Mobile, decimal Amount)
        {
            return WebDuplicateAsync(AccessId, Mobile, Amount).Result;
        }

        public async Task<OneOf<Recharge, HotDbException>> WebDuplicateAsync(long AccessId, string Mobile, decimal Amount)
        {
            var Query = $"{GetSPPrefix()}_Web_Duplicate @AccessId,@Mobile,@Amount";
            return await dbHelper.QuerySingle<Recharge>(Query, new { AccessId, Mobile, Amount });
        }

        public async Task<OneOf<List<Recharge>, HotDbException>> FindByAccountAsync(long AccountId, string ?Mobile)
        {
            var Query = $"{GetSPPrefix()}_Find @AccountId,@Mobile";
            return await dbHelper.Query<Recharge>(Query, new { AccountId, Mobile });
        }

        public async Task<OneOf<List<RechargeResultModel>, HotDbException>> FindByMobileAsync(string? Mobile)
        {
            var Query = $"{GetSPPrefix()}_Find_By_Mobile @Mobile";
            return await dbHelper.Query<RechargeResultModel>(Query, new {  Mobile });
        }

        public async Task<OneOf<List<RechargeResultModel>, HotDbException>> FindOldAsync(string? Filter)
        {
            var Query = $"{GetSPPrefix()}_FindOld @Filter";
            return await dbHelper.Query<RechargeResultModel>(Query, new { Filter });
        }
    }

   
}
