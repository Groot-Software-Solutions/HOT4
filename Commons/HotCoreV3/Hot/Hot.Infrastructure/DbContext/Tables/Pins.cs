using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class Pins : Table<Pin>, IPins
    {
        public Pins(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.AddSuffix = "_Insert";
            base.AddParameters = "@PinBatchID,@PinStateID,@BrandID,@Pin,@PinRef,@PinValue,@PinExpiry";
            base.UpdateSuffix = AddSuffix;
            base.UpdateParameters = AddParameters;
        }

        public override async Task<OneOf<int, HotDbException>> AddAsync(Pin Item)
        {
            string query = $"{StoredProcedurePrefix}{typeof(Pin).Name}{AddSuffix} {AddParameters}";
            var param = new
            {
                Item.BrandId,
                Item.PinBatchId,
                Item.PinExpiry,
                Pin = Item.PinNumber,
                Item.PinRef,
                Item.PinStateId,
                Item.PinValue
            };

            var result = await dbHelper.QuerySingle<int>(query, param);
            return result;
        }

        public async Task<OneOf<List<Pin>, HotDbException>> PinsInBatchAsync(int PinBatchId)
        {
            string query = $"{StoredProcedurePrefix}{typeof(Pin).Name}_Loaded @PinBatchId";
            var result = await dbHelper.Query<Pin>(query, new { PinBatchId });
            return result;
        }

        public OneOf<List<Pin>, HotDbException> PinsInBatch(int PinBatchId)
        {
            return PinsInBatchAsync(PinBatchId).Result;
        }

        public OneOf<List<PinStockModel>, HotDbException> PromoStock()
        {
            return PromoStockAsync().Result;
        }

        public async Task<OneOf<List<PinStockModel>, HotDbException>> PromoStockAsync()
        {
            string query = $"{StoredProcedurePrefix}{typeof(Pin).Name}_Stock_Promo";
            var result = await dbHelper.Query<PinStockModel>(query);
            return result;
        }

        public OneOf<List<PinStockModel>, HotDbException> Stock()
        {
            return StockAsync().Result;
        }

        public async Task<OneOf<List<PinStockModel>, HotDbException>> StockAsync()
        {
            string query = $"{StoredProcedurePrefix}{typeof(Pin).Name}_Stock";
            var result = await dbHelper.Query<PinStockModel>(query);
            return result;
        }

        public OneOf<List<PinStockModel>, HotDbException> StockLoadedInBatch(int PinBatchId)
        {
            return StockLoadedInBatchAsync(PinBatchId).Result;
        }

        public async Task<OneOf<List<PinStockModel>, HotDbException>> StockLoadedInBatchAsync(int PinBatchId)
        {
            string query = $"{StoredProcedurePrefix}{typeof(Pin).Name}_Loaded @PinBatchId";
            var result = await dbHelper.Query<PinStockModel>(query, new { PinBatchId });
            return result;
        }
         
        public async Task<OneOf<List<Pin>, HotDbException>> PromoRechargeAsync(string AccessCode, int BrandId, decimal PinValue, int Quantity, string Mobile)
        {
            string query = $"{StoredProcedurePrefix}{typeof(Pin).Name}_Recharge_Promo @AccessCode,@Quantity,@PinValue,@BrandId, @Mobile";
            var result = await dbHelper.Query<Pin>(query, new { AccessCode, BrandId, PinValue, Quantity, Mobile });
            return result;
        }

        public OneOf<List<Pin>, HotDbException> PromoRecharge(string AccessCode, int BrandId, decimal PinValue, int Quantity, string Mobile)
        {
            return PromoRechargeAsync(AccessCode, BrandId, PinValue, Quantity, Mobile).Result;
        }

        public async Task<OneOf<bool, HotDbException>> PromoHasPurchasedAsync(long AccountId)
        {
            string query = $"{StoredProcedurePrefix}{typeof(Pin).Name}_Has_Redeemed_Promo @AccountId";
            var result = await dbHelper.QuerySingle<bool>(query, new { AccountId });
            return result;
        }

        public OneOf<bool, HotDbException> PromoHasPurchased(long AccountId)
        {
            return PromoHasPurchasedAsync(AccountId).Result;
        }

        public OneOf<List<Pin>, HotDbException> Recharge(decimal Amount, int BrandId, long RechargeId)
        {
            return RechargeAsync(Amount,BrandId,RechargeId).Result;
        }

        public async Task<OneOf<List<Pin>, HotDbException>> RechargeAsync(decimal Amount, int BrandId, long RechargeId)
        {
            string query = $"{StoredProcedurePrefix}{typeof(Pin).Name}_Recharge @Amount,@BrandId,@RechargeId ";
            var result = await dbHelper.Query<Pin>(query, new { Amount,BrandId,RechargeId });
            return result;
        }
    }
}
