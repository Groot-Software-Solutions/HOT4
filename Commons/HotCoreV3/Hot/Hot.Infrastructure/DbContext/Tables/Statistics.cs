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
    public class Statistics : Table<StatResult>, IStatistics
    {
        public Statistics(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
        }

        public async Task<OneOf<List<StatResult>, HotDbException>> GetRechargeTrafficLastDayAsync()
        {
            var Query = $"{StoredProcedurePrefix}Statistics_Get_Recharge_Traffic_Day";
            var result  = await dbHelper.Query<StatResult>(Query);
            return result;
        }


        public async Task<OneOf<List<StatResult>, HotDbException>> GetRechargeTrafficAsync()
        {
            string query = $"{StoredProcedurePrefix}Statistics_Get_Recharge_Traffic ";
            var result = await dbHelper.Query<StatResult>(query);
            return result;

        }

        public async Task<OneOf<List<StatResult>, HotDbException>> GetSmsTrafficAsync()
        {
            string query = $"{StoredProcedurePrefix}Statistics_Get_Sms_Traffic ";
            var result = await dbHelper.Query<StatResult>(query);
            return result;
        }
    }
}
