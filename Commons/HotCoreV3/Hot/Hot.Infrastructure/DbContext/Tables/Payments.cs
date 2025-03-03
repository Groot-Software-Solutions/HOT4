using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using OneOf;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class Payments : Table<Payment>, IPayments
    {
        public Payments(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.GetSuffix = "_Select";
            base.AddSuffix = "_Save";
            base.AddParameters = "@PaymentId,@AccountId,@PaymentTypeId,@PaymentSourceId,@Amount,@PaymentDate,@Reference,@LastUser";
            base.UpdateSuffix = AddSuffix;
            base.UpdateParameters = AddParameters;

        }

        public async Task<OneOf<List<Payment>, HotDbException>> ListAsync(int AccountId)
        {
            string query = $"{StoredProcedurePrefix}{typeof(Payment).Name}{ListSuffix} @AccountId";
            var result = await dbHelper.Query<Payment>(query,new { AccountId });
            return result;
        }

        public async Task<OneOf<List<Payment>, HotDbException>> ListRecentAsync(int AccountId)
        {
            string query = $"{StoredProcedurePrefix}{typeof(Payment).Name}{ListSuffix}Recent @AccountId";
            var result = await dbHelper.Query<Payment>(query, new { AccountId });
            return result;
        }

        public async Task<OneOf<List<Payment>, HotDbException>> SearchOldAsync(string Filter)
        {
            string query = $"{StoredProcedurePrefix}{typeof(Payment).Name}{ListSuffix}SearchOld @Filter";
            var result = await dbHelper.Query<Payment>(query, new { Filter });
            return result; throw new NotImplementedException();
        }
    }
}
