using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities; 

namespace Hot.Infrastructure.DbContext.Tables
{
    public class SelfTopUps : Table<SelfTopUp>, ISelfTopUps
    {

        public SelfTopUps(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.IdPrefix = "SelfTopUp";
            base.AddSuffix = "_Save";
            base.AddParameters = "@SelfTopUpID,@AccessID,@BankTrxID,@RechargeID,@BrandId,@ProductCode,@NotificationNumber ,@BillerNumber,@StateId,@Amount,@TargetNumber,@Currency ";
            base.UpdateSuffix = AddSuffix;
            base.UpdateParameters = AddParameters;  

        }
        public async Task<OneOf<List<SelfTopUp>, HotDbException>> ListPendingRechargeAsync()
        {
            string query = $"{StoredProcedurePrefix}{typeof(SelfTopUp).Name}_PendingRecharge";
            var result = await dbHelper.Query<SelfTopUp>(query);
            return result;
        }

        public OneOf<List<SelfTopUp>, HotDbException> ListPendingRecharge()
        {
            return ListPendingRechargeAsync().Result;
        }
        public async Task<OneOf<SelfTopUp, HotDbException>> GetByBankTrxIdAsync(long BankTrxId)
        {
            string query = $"{StoredProcedurePrefix}{typeof(SelfTopUp).Name}_Get_By_BankTrxId @BankTrxId";
            var parameters = new Dictionary<string, object>() { { $"@BankTrxId", BankTrxId } };
            var result = await dbHelper.QuerySingle<SelfTopUp>(query, parameters);
            return result;
        }
        public OneOf<SelfTopUp,HotDbException> GetByBankTrxId(long BankTrxId)
        {
            return GetByBankTrxIdAsync(BankTrxId).Result;
        }

        
    }
}
