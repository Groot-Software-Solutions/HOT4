using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface ISMSService
    {
        Task<bool> AddSMS(SmsToDo sms);
        Task<bool> UpdateSMS(SmsToDo sms);
        Task<List<EmailModel>> SmsBulkSend(string messageText);
        Task<List<SMSModel>> SMSInbox();
        Task<List<SMSModel>> SMSOutbox();
        Task<List<SMSModel>> GetSMSByAccountSMSDate(long accountId, DateTime smsDate, int pageNo, int pageSize);
        Task<SMSModel?> GetSMSById(long smsId);
        Task<List<SMSModel>> SMSSearch(SMSSearchModel smsSearch);
        Task<SMSModel?> DuplicateRecharge(DuplicateRechargeSrchModel smsDuplicateSearch);
        Task<bool> Resend(string mobile, string rechargeMobile);
    }
}
