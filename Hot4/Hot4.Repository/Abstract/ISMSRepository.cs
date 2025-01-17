using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ISMSRepository
    {
        Task<bool> AddSMS(Sms sms);
        Task<bool> UpdateSMS(Sms sms);
        // Task<List<EmailModel>> SmsBulkSend(string messageText);
        Task<List<Access>> SmsBulkSend(string messageText);
        //  Task<List<SMSModel>> SMSInbox();
        Task<List<Sms>> SMSInbox();
        //  Task<List<SMSModel>> SMSOutbox();
        Task<List<Sms>> SMSOutbox();
        //  Task<List<SMSModel>> GetSMSByAccountSMSDate(long accountId, DateTime smsDate, int pageNo, int pageSize);
        Task<List<Sms>> GetSMSByAccountSMSDate(long accountId, DateTime smsDate, int pageNo, int pageSize);
        // Task<List<SMSModel>> GetSMSBySMSId(long smsId);
        Task<Sms?> GetSMSById(long smsId);
        // Task<List<SMSModel>> SMSSearch(SMSSearchModel smsSearch);
        Task<List<Sms>> SMSSearch(SMSSearchModel smsSearch);
        // Task<SMSModel?> DuplicateRecharge(DuplicateRechargeSrchModel smsDuplicateSearch);
        Task<Sms?> DuplicateRecharge(DuplicateRechargeSrchModel smsDuplicateSearch);
        Task<bool> Resend(string mobile, string rechargeMobile);
    }
}
