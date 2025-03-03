
using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using OneOf;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class Limits : Table<Limit>, ILimits
    {
        public Limits(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.AddSuffix = "s_Save";
            base.IdPrefix = "Limit";
            base.UpdateSuffix = base.AddSuffix;

        }

        public async Task<OneOf<LimitModel, HotDbException>> GetCurrentLimitsAsync(int NetworkId, long AccountId)
        {
            string query = $"{GetSPPrefix()}s_Select @NetworkId, @AccountId";
            var result = await dbHelper.QuerySingle<LimitModel>(query, new { NetworkId, AccountId });
            return result;
        }
    }
}
