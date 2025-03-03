namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface ISMSs : IDbContextTable<SMS>
        , IDbCanAdd<SMS>
        , IDbCanAddInTransaction<SMS>
        , IDbCanGetById<SMS>
        , IDbCanList<SMS>
        , IDbCanRemoveById<SMS>
        , IDbCanSearch<SMS>
        , IDbCanUpdate<SMS>
        , IDbCanUpdateInTransaction<SMS>
    {
        public Task<OneOf<SMS, HotDbException>> DuplicateAsync(SMS Sms);
        public OneOf<SMS, HotDbException> Duplicate(SMS Sms);

        public Task<OneOf<List<SMS>, HotDbException>> InboxAsync();
        public OneOf<List<SMS>, HotDbException> Inbox();

        public Task<OneOf<List<SMS>, HotDbException>> OutboxAsync();
        public OneOf<List<SMS>, HotDbException> Outbox();

        public Task<OneOf<bool, HotDbException>> ResendAsync(string Mobile, string RechargeMobile);
        public OneOf<bool, HotDbException> Resend(string Mobile, string RechargeMobile);

        public Task<OneOf<List<SMS>, HotDbException>> SearchByMobileAsync(string Mobile);
        public OneOf<List<SMS>, HotDbException> SearchByMobile(string Mobile);

        public Task<OneOf<List<SMS>, HotDbException>> SearchByDatesAsync(DateTime StartDate, DateTime EndDate);
        public OneOf<List<SMS>, HotDbException> SearchByDates(DateTime StartDate, DateTime EndDate);

        public Task<OneOf<List<SMS>, HotDbException>> SearchByFilterAsync(string Filter);
        public OneOf<List<SMS>, HotDbException> SearchByFilter(string Filter);





    }
}
