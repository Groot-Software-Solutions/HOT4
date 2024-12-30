using Hot4.Core.Enums;
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ISMSRepository
    {
        Task<long> AddSMS(Sms sms);
        Task UpdateSMS(Sms sms);
        Task ResendWithTransaction(Sms smsRequest);

        // Task<List<AccountSmsModel>> ListSMSInViewsForAccount(long accountId, DateTime dateTime);

        //  Task<List<AccountSmsModel>> ListSMSOutViewsForAccount(long smsId);

        //  Task<List<AccountSmsModel>> ListSMSSearchViewsForAccount(DateTime startdate, DateTime enddate, string mobile, string messageText, byte smppId, long stateId);

        Task<List<VwSm>> GetPendingSMSWithTransaction();

        Task Reply(Sms sms, List<Template> templates);

        Task ReplyWithTransaction(Sms sms, List<Template> templates);

        Task ReplyCustomer(string mobile, Sms sms, List<Template> templates);

        Task ClearSMSPassword(Sms sms, bool hadValidPassword, HotTypeName hotTypeSMS);

        Task<Sms?> Duplicate(Sms sms);

        //   Task<List<AccountSmsModel>> RefreshListSMSOutViewsForAccount(long accountId, DateTime dateTime);

        Task<int> SendEmails(string message, string subject, string emailtype);
    }
}
