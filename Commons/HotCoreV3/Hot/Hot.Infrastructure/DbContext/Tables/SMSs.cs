using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using Dapper;
using OneOf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using OneOf.Types;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class SMSs : Table<SMS>, ISMSs
    {

        public SMSs(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
        }

        public override async Task<OneOf<int, HotDbException>> AddAsync(SMS Item) => await SaveSMSAsync(Item);

        public override async Task<OneOf<int, HotDbException>> AddAsync(SMS Item, IDbConnection connection, IDbTransaction transaction)
            => await SaveSMSAsync(Item, connection, transaction);

        public override async Task<OneOf<bool, HotDbException>> UpdateAsync(SMS Item)
        {
            var result = await SaveSMSAsync(Item);
            if (result.IsT1) return result.AsT1;
            return true;
        }
        private async Task<OneOf<int, HotDbException>> SaveSMSAsync(SMS Item, IDbConnection connection, IDbTransaction transaction)
        {
            var query = $"xSMS_Save @SMSID, @SmppID, @StateID, @PriorityID, @Direction, @Mobile, @SMSText, @SMSID_In";
            var parameters = new
            {
                Item.SMSID,
                Item.SmppID,
                Item.State?.StateID,
                Item.Priority?.PriorityId,
                Item.Direction,
                Item.Mobile,
                Item.SMSText,
                Item.SMSID_In
            };
            return await dbHelper.ExecuteScalar(query, parameters, connection, transaction);
        }

        private async Task<OneOf<int, HotDbException>> SaveSMSAsync(SMS Item)
        {
            var query = $"xSMS_Save @SMSID, @SmppID, @StateID, @PriorityID, @Direction, @Mobile, @SMSText, @SMSID_In";
            var parameters = new
            {
                Item.SMSID,
                Item.SmppID,
                Item.State?.StateID,
                Item.Priority?.PriorityId,
                Item.Direction,
                Item.Mobile,
                Item.SMSText,
                Item.SMSID_In
            };
            return await dbHelper.ExecuteScalar(query, parameters);
        }

        public async Task<OneOf<SMS, HotDbException>> DuplicateAsync(SMS Sms)
        {
            return await dbHelper.QuerySingle<SMS>($"xDuplicateRecharge @SMSID, @SMSText, @Mobile", Sms);
        }

        public async Task<OneOf<List<SMS>, HotDbException>> InboxAsync()
        {
            var response = await dbHelper.Query<SMSDto>($"{GetSPPrefix()}_Inbox");
            if (response.IsT0) return response.AsT0.Select(s => (SMS)s).ToList();
            return response.AsT1;
        }

        public async Task<OneOf<List<SMS>, HotDbException>> OutboxAsync()
        {
            var response = await dbHelper.Query<SMSDto>($"{GetSPPrefix()}_Outbox");
            if (response.IsT0) return response.AsT0.Select(s => (SMS)s).ToList();
            return response.AsT1;
        }

        public async Task<OneOf<bool, HotDbException>> ResendAsync(string Mobile, string RechargeMobile)
        {
            return await dbHelper.Execute($"xResend @Mobile, @RechargeMobile", new { Mobile, RechargeMobile });
        }


        public OneOf<SMS, HotDbException> Duplicate(SMS Sms)
            => DuplicateAsync(Sms).Result;

        public OneOf<List<SMS>, HotDbException> Inbox()
            => InboxAsync().Result;

        public OneOf<List<SMS>, HotDbException> Outbox()
            => OutboxAsync().Result;

        public OneOf<bool, HotDbException> Resend(string Mobile, string RechargeMobile)
            => ResendAsync(Mobile, RechargeMobile).Result;

        public async Task<OneOf<List<SMS>, HotDbException>> SearchByMobileAsync(string Mobile)
        {
            return await dbHelper.Query<SMS>($"{GetSPPrefix()}_Search_ByMobile @Mobile", new { Mobile });
        }

        public OneOf<List<SMS>, HotDbException> SearchByMobile(string Mobile)
        {
            return SearchByMobileAsync(Mobile).Result;
        }

        public async Task<OneOf<List<SMS>, HotDbException>> SearchByDatesAsync(DateTime StartDate, DateTime EndDate)
        {
            return await dbHelper.Query<SMS>($"{GetSPPrefix()}_Search_ByDates @StartDate, @EndDate", new { StartDate, EndDate });
        }

        public OneOf<List<SMS>, HotDbException> SearchByDates(DateTime StartDate, DateTime EndDate)
        {
            return SearchByDatesAsync(StartDate, EndDate).Result;
        }

        public async Task<OneOf<List<SMS>, HotDbException>> SearchByFilterAsync(string Filter)
        {
            return await dbHelper.Query<SMS>($"{GetSPPrefix()}_Search_ByFilter @MessageText", new { MessageText = Filter });
        }

        public OneOf<List<SMS>, HotDbException> SearchByFilter(string Filter)
        {
            return SearchByFilterAsync(Filter).Result;
        }
    }

    internal class SMSDto
    {
        public long SMSID { get; set; }

        public byte? SmppID { get; set; }

        public byte StateID { get; set; }

        public string State { get; set; }

        public byte PriorityID { get; set; }

        public string Priority { get; set; }

        public bool Direction { get; set; }

        public string Mobile { get; set; }

        public string SMSText { get; set; }

        public DateTime SMSDate { get; set; }

        public DateTime? InsertDate { get; set; }

        public long? SMSID_In { get; set; }


        public static implicit operator SMS(SMSDto d) =>
            new()
            {
                Direction = d.Direction,
                InsertDate = d.InsertDate ?? DateTime.Now,
                SMSID_In = d.SMSID_In,
                Mobile = d.Mobile,
                SMSText = d.SMSText,
                SMSDate = d.SMSDate,
                Priority = new() { PriorityId = d.PriorityID, Name = d.Priority },
                SMSID = d.SMSID,
                SmppID = d.SmppID,
                State = new() { StateID = d.StateID, Name = d.State },
            };
    }
}
