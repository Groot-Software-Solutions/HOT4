using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using OneOf;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class WebRequests : Table<WebRequest>, IWebRequests
    {
        public WebRequests(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.AddSuffix = "_Save";
            base.AddParameters = "@HotTypeID,@AccessID,@StateID,@IsRequest,@AgentReference,@ReturnCode,@Reply,@ChannelID,@RechargeID,@WalletBalance,@Cost,@Discount,@Amount";
            base.UpdateSuffix = AddSuffix;
            base.UpdateParameters = AddParameters;
            base.GetSuffix = "_Select";
        }

        public async Task<OneOf<WebRequest, HotDbException>> GetAsync(string AgentReference, int AccessID)
        {
            string query = $"{GetSPPrefix()}{GetSuffix} @AgentReference,@AccessID";
            var result = await dbHelper.QuerySingle<WebRequest>(query, new { AgentReference, AccessID });
            return result;
        }

        OneOf<WebRequest, HotDbException> IWebRequests.Get(string AgentReference, int AccessID) => GetAsync(AgentReference, AccessID).Result;

    }
}
