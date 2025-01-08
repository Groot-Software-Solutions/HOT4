using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ISMSRepository
    {
        Task<long> AddSMS(Sms sms);
        Task UpdateSMS(Sms sms);
        Task<List<EmailModel>> SmsBulkSend(string messageText);
        Task<List<SMSModel>> SMSInbox();
        Task<List<SMSModel>> SMSOutbox();
        Task<List<SMSModel>> GetSMSByAccountSMSDate(long accountId, DateTime smsDate, int pageNo, int pageSize);
        Task<List<SMSModel>> GetSMSBySMSId(long smsId);
        Task<List<SMSModel>> SMSSearch(SMSSearchModel smsSearch);
        Task<SMSModel?> DuplicateRecharge(DuplicateRechargeSrchModel smsDuplicateSearch);
        Task Resend(string mobile, string rechargeMobile);
    }
}
