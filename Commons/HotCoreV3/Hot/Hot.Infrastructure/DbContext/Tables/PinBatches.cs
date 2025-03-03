using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Infrastructure.DbContext.Tables
{
   public  class PinBatches : Table<PinBatch>, IPinBatches
    {
        public PinBatches(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.AddSuffix = "_Insert"; 
            base.AddParameters = "@PinBatch, @PinBatchTypeId";
            base.UpdateSuffix = AddSuffix;
            base.UpdateParameters = AddParameters;
        }


        public override async Task<OneOf<int, HotDbException>> AddAsync(PinBatch Item)
        {
            string query = $"{StoredProcedurePrefix}{typeof(Pin).Name}{AddSuffix} {AddParameters}";
            var param = new
            { 
                Item.PinBatchTypeId, 
                PinBatch = Item.Name, 
            }; 
            var result = await dbHelper.QuerySingle<int>(query, param);
            return result;
        }

        public OneOf<List<PinBatch>, HotDbException> List(int PinBatchTypeId)
        {
            return ListAsync(PinBatchTypeId).Result;
        }

        public async Task<OneOf<List<PinBatch>, HotDbException>> ListAsync(int PinBatchTypeId)
        {
            string query = $"{StoredProcedurePrefix}{typeof(PinBatch).Name}{ListSuffix} @PinBatchTypeID";
            var result = await dbHelper.Query<PinBatch>(query, new { PinBatchTypeId });
            return result;
        }
    }
}
