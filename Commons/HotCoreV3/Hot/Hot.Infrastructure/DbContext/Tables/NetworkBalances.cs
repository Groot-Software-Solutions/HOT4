using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Application.Common.Models;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class NetworkBalances : Table<NetworkBalance>, INetworkBalances
    {
        public NetworkBalances(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.ListSuffix = "_List";

        }

        public async Task<OneOf<NetworkBalance, HotDbException>> GetByIdAsync(int BrandId)
        {
            return await dbHelper.QuerySingle<NetworkBalance>($"{GetSPPrefix()}_Get @BrandId", new { BrandId });
        }
    }
}
