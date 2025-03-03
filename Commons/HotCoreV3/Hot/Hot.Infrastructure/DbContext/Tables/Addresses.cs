using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using OneOf;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class Addresses : Table<Address>, IAddresses
    {
        public Addresses(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x"; 
            base.AddSuffix = "_Save";
            base.AddParameters = "@AccountID,@Address1,@Address2,@City,@ContactName,@ContactNumber,@VatNumber,@Latitude,@Longitude,@SageID,@InvoiceFreq,@SageIDUsd,@Confirmed";
            base.UpdateSuffix = AddSuffix;
            base.UpdateParameters = AddParameters;

            base.GetSuffix = "_Select";
            base.IdPrefix = "Account";

        }

        public OneOf<List<Address>, HotDbException> SearchWithSageId(string sageId)
        {
           return SearchWithSageIdAsync(sageId).Result;
        }

        public async Task<OneOf<List<Address>,HotDbException>> SearchWithSageIdAsync(string sageId)
        {
            return await dbHelper.Query<Address>($"{GetSPPrefix()}_SearchWithSageid  @sageId", new { sageId });
        }
    }
}
