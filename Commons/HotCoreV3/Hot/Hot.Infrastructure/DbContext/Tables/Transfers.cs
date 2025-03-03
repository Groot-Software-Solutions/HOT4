using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class Transfers : Table<Transfer>, ITransfers
    {
        public Transfers(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.AddSuffix = "_Save";
            base.UpdateSuffix = AddSuffix;

        }

        public OneOf<StockTradeInResult, HotDbException> StockTradeIn(long accountId, decimal amount, decimal rate)
        {
            return StockTradeInAsync(accountId, amount, rate).Result;
        }

        public async Task<OneOf<StockTradeInResult, HotDbException>> StockTradeInAsync(long accountId, decimal amount, decimal rate)
        {
            return await dbHelper.QuerySingle<StockTradeInResult>($"xStockTrade_Trade @accountId, @amount, @rate", new { accountId, amount, rate });
        }

        public OneOf<decimal, HotDbException> StockTradeInBalance(long accountId)
        {
            return StockTradeInBalanceAsync(accountId).Result;
        }

        public async Task<OneOf<decimal, HotDbException>> StockTradeInBalanceAsync(long accountId)
        {
            return await dbHelper.QuerySingle<decimal>($"xStockTrade_Balance @accountId", new { accountId });
        }
    }
}
