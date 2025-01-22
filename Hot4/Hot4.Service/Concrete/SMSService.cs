using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class SMSService : ISMSService
    {
        private readonly ISMSRepository _smsRepository;
        private readonly IMapper Mapper;
        public SMSService(ISMSRepository smsRepository, IMapper mapper)
        {
            _smsRepository = smsRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddSMS(SmsRecord sms)
        {
            if (sms != null)
            {
                var model = Mapper.Map<Sms>(sms);
                return await _smsRepository.AddSMS(model);
            }
            return false;           
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
            return Mapper.Map<List<EmailModel>>(records);
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
        private async Task<Sms?> GetEntityById (long smsid)
        {
            return await _smsRepository.GetSMSById(smsid);
        }
    }
}
