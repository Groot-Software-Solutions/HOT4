using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ISMSRepository
    {
        Task<long> AddSMS(Sms sms);
        Task UpdateSMS(Sms sms);
        Task SendBulkSms(string messageText);
        Task<int> SaveBulkSms(string messageText);
        Task EmailAggregators(string sub, string messageText);
        Task EmailCorporates(string sub, string messageText);
        Task<List<SMSModel>> SMSInbox();
        Task<List<SMSModel>> SMSOutbox();
        Task<List<SMSModel>> GetSMSByAccountSMSDate(long accountId, DateTime smsDate);
        Task<List<SMSModel>> GetSMSBySMSId(long smsId);
        Task<SMSModel?> DuplicateRecharge(DuplicateRechargeSrchModel smsDuplicateSearch);
        Task Resend(string mobile, string rechargeMobile);
    }
}
