using AutoMapper;
using Hot4.Core.Helper;
using Hot4.Core.Settings;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using Microsoft.Extensions.Options;

namespace Hot4.Service.Concrete
{
    public class SMSService : ISMSService
    {
        private ISMSRepository _smsRepository;
        private readonly IMapper Mapper;
        private TemplateSettings _templateSettings { get; }
        public SMSService(ISMSRepository smsRepository, IMapper mapper, IOptions<TemplateSettings> templateSettings)
        {
            _smsRepository = smsRepository;
            Mapper = mapper;
            _templateSettings = templateSettings.Value;
        }
        public async Task<bool> AddSMS(SmsRecord sms)
        {
            var model = Mapper.Map<Sms>(sms);
            return await _smsRepository.AddSMS(model);
        }

        public async Task<SMSModel?> DuplicateRecharge(DuplicateRechargeSrchModel smsDuplicateSearch)
        {
            var record = await _smsRepository.DuplicateRecharge(smsDuplicateSearch);
            return Mapper.Map<SMSModel>(record);
        }

        public async Task<List<SMSModel>> GetSMSByAccountSMSDate(long accountId, DateTime smsDate, int pageNo, int pageSize)
        {
            var records = await _smsRepository.GetSMSByAccountSMSDate(accountId, smsDate, pageNo, pageSize);
            return Mapper.Map<List<SMSModel>>(records);
        }

        public async Task<SMSModel?> GetSMSById(long smsId)
        {
            var record = await GetEntityById(smsId);
            return Mapper.Map<SMSModel>(record);
        }

        public async Task<bool> Resend(string mobile, string rechargeMobile)
        {
            return await _smsRepository.Resend(mobile, rechargeMobile);
        }

        public async Task<List<EmailModel>> SmsBulkSend(string messageText)
        {
            var records = await _smsRepository.SmsBulkSend(messageText);
            var result = new List<EmailModel>();
            if (records != null && records.Any())
            {
                var emailLists = records.Where(d => Helper.CheckValidEmail(d.AccessCode)).ToList();
                //  string subject = "HOT Recharge Notification " + DateTime.Now.ToString("yyyy-MM-dd");
                string subject = _templateSettings.RechargeNotificationSubject + " " + DateTime.Now.ToString("yyyy-MM-dd");
                foreach (var emailData in emailLists)
                {
                    string emailAddress = emailData.AccessCode;
                    string accountName = emailData.Account.AccountName;
                    //  string htmlBody = $"Dear {accountName}<br><br>{messageText}<br><br>Best regards <br>the HOT Recharge Team";
                    //  string htmlBody= string.Format(_templateSettings.RechargeNotificationBody, accountName, messageText);
                    string htmlBody = _templateSettings.RechargeNotificationBody.Replace("accountName", accountName).Replace("messageText", messageText);
                    // Email.SendEmail(emailAddress, subject, htmlBody);
                }
                return Mapper.Map<List<EmailModel>>(emailLists);
            }
            return result;

        }

        public async Task<List<SMSModel>> SMSInbox()
        {
            var records = await _smsRepository.SMSInbox();
            return Mapper.Map<List<SMSModel>>(records);
        }

        public async Task<List<SMSModel>> SMSOutbox()
        {
            var records = await _smsRepository.SMSOutbox();
            return Mapper.Map<List<SMSModel>>(records);
        }

        public async Task<List<SMSModel>> SMSSearch(SMSSearchModel smsSearch)
        {
            var records = await _smsRepository.SMSSearch(smsSearch);
            return Mapper.Map<List<SMSModel>>(records);
        }

        public async Task<bool> UpdateSMS(SmsRecord sms)
        {
            var record = await GetEntityById(sms.Smsid);
            if (record != null)
            {
                Mapper.Map(sms, record);
                return await _smsRepository.UpdateSMS(record);
            }
            return false;
        }

        private async Task<Sms?> GetEntityById(long Smsid)
        {
            return await _smsRepository.GetSMSById(Smsid);
        }
    }
}
