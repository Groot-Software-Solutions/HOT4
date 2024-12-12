
using Hot4.Core.DataViewModels;
using Hot4.Core.Enums;
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ISMSRepository
    {
        Task<long> AddSMS(TblSms sms);
        Task UpdateSMS(TblSms sms);
        Task ResendWithTransaction(TblSms smsRequest);

        Task<List<AccountSmsModel>> ListSMSInViewsForAccount(long accountId, DateTime dateTime);

        Task<List<AccountSmsModel>> ListSMSOutViewsForAccount(long smsId);

        Task<List<AccountSmsModel>> ListSMSSearchViewsForAccount(DateTime startdate, DateTime enddate, string mobile, string messageText, byte smppId, long stateId);

        Task<List<VwSm>> GetPendingSMSWithTransaction();

        Task Reply(TblSms sms, List<TblTemplate> templates);

        Task ReplyWithTransaction(TblSms sms, List<TblTemplate> templates);

        Task ReplyCustomer(string mobile, TblSms sms, List<TblTemplate> templates);

        Task ClearSMSPassword(TblSms sms, bool hadValidPassword, HotTypes hotTypeSMS);

        Task<TblSms?> Duplicate(TblSms sms);

        Task<List<AccountSmsModel>> RefreshListSMSOutViewsForAccount(long accountId, DateTime dateTime);

        Task<int> SendEmails(string message, string subject, string emailtype);
    }
}
