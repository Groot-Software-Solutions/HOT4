
using Hot4.Core.Enums;
using Hot4.Core.Helper;
using Hot4.Core.Settings;
using Hot4.DataModel.Data;

using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hot4.Repository.Concrete
{
    public class SMSRepository : RepositoryBase<Sms>, ISMSRepository
    {
        private ValueSettings _valueSettings { get; }

        public SMSRepository(HotDbContext context, IOptions<ValueSettings> valueSettings) : base(context)
        {
            _valueSettings = valueSettings.Value;

        }

        public async Task<bool> AddSMS(Sms sms)
        {
            await Create(sms);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateSMS(Sms sms)
        {
            Update(sms);
            await SaveChanges();
            return true;
        }
        public async Task<List<Access>?> SmsBulkSend(string messageText)
        {
            var accountIds = await _context.Payment.Where(d => d.PaymentDate > DateTime.Now.AddDays(-90)
                              && d.PaymentTypeId != (int)PaymentMethodType.Writeoff)
                              .Select(d => d.AccountId).ToListAsync();
            //The below is commented as it needs to be discussed first
            /*
            var mobileList = await _context.Access
              .Where(d => d.ChannelId == (int)ChannelName.Sms && d.Deleted == false
              && EF.Constant(accountIds).Contains(d.AccountId)
              && !string.Join(",", _valueSettings.SmsBulkSendExcludeAccessCode)
              .Contains(d.AccessCode.Substring(d.AccessCode.Length - 7))
              && (!Regex.IsMatch(d.AccessCode, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*") &&
              Convert.ToInt64(d.AccessCode) <= _valueSettings.SmsBulkSendGreaterThenMobileNo))
                .Select(a => a.AccessCode)
              .Distinct().ToListAsync();

            var smsRecords = mobileList.Select(mobile => new Sms
            {
                SmppId = null,
                StateId = 0,
                PriorityId = 0,
                Direction = false,
                Mobile = mobile,
                Smstext = messageText,
                Smsdate = DateTime.Now,
                SmsidIn = null
            }).ToList();

            await _context.Sms.AddRangeAsync(smsRecords);
            await _context.SaveChangesAsync();
            */

            return await _context.Access.Include(d => d.Account)
                    .Where(d => d.ChannelId == (int)ChannelName.Web
                    && EF.Constant(accountIds).Contains(d.AccountId)
                    ).ToListAsync();

            // return emailLists.Where(d => Helper.CheckValidEmail(d.AccessCode)).ToList();

        }
        public async Task<List<Sms>> SMSInbox()
        {
            int queueSize = _valueSettings.SMSInboxQueueSize;

            var smsDetail = await _context.Sms.Include(d => d.Priority)
                .Where(d => d.Direction == true && d.StateId == (int)SmsState.Pending)
                .OrderByDescending(d => d.PriorityId)
                .ThenBy(d => d.Smsdate)
                .Take(queueSize)
                .ToListAsync();
            if (smsDetail != null && smsDetail.Any())
            {
                var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    foreach (var sms in smsDetail)
                    {
                        sms.StateId = (int)SmsState.Busy;
                    }
                    _context.Sms.UpdateRange(smsDetail);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return await _context.Sms.Include(d => d.State).Include(d => d.Priority)
                     .Where(d => smsDetail.Select(m => m.Smsid).Contains(d.Smsid)).ToListAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return new List<Sms>();
        }
        public async Task<List<Sms>> SMSOutbox()
        {
            int queueSize = _valueSettings.SMSOutboxQueueSize;

            var smsDetail = await _context.Sms.Include(d => d.State).Include(d => d.Priority)
                .Where(d => d.Direction == false && d.StateId == (int)SmsState.Pending && d.Smsdate < DateTime.Now)
                .OrderByDescending(d => d.PriorityId)
                .ThenBy(d => d.Smsdate)
                .Take(queueSize)
                .ToListAsync();

            if (smsDetail != null && smsDetail.Any())
            {
                var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    foreach (var sms in smsDetail)
                    {
                        sms.StateId = (int)SmsState.Busy;
                    }
                    _context.Sms.UpdateRange(smsDetail);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return await _context.Sms.Include(d => d.State).Include(d => d.Priority)
                        .Where(d => smsDetail.Select(m => m.Smsid).Contains(d.Smsid)).ToListAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                return new List<Sms>();
            }
        }
        public async Task<List<Sms>> GetSMSByAccountSMSDate(long accountId, DateTime smsDate, int pageNo, int pageSize)
        {
            var startDate = smsDate.Date;
            var endDate = smsDate.Date.AddDays(1).AddTicks(-1);

            var result = from sms in _context.Sms.Include(d => d.State).Include(d => d.Priority)
                          .Where(d => d.Direction == true && d.Smsdate >= startDate && smsDate <= endDate)
                         join acss in _context.Access on sms.Mobile equals acss.AccessCode
                         where acss.AccountId == accountId
                         orderby sms.Smsdate descending
                         select sms;

            return await PaginationFilter.GetPagedData(result, pageNo, pageSize).Queryable.ToListAsync();


        }
        public async Task<Sms?> GetSMSById(long smsId)
        {
            return await _context.Sms.Include(d => d.State).Include(d => d.Priority)
                .FirstOrDefaultAsync(d => d.Smsid == smsId);

        }
        public async Task<List<Sms>> SMSSearch(SMSSearchModel smsSearch)
        {
            var states = new List<int>();

            if (smsSearch.StateId == -1)
            {
                states = await _context.State.Select(d => Convert.ToInt32(d.StateId)).ToListAsync();
            }
            else
            {
                states.Add(smsSearch.StateId);
            }

            if (smsSearch.SmppId == -1)
            {
                return await PaginationFilter.GetPagedData(GetByCondition(d =>
                           d.Smsdate >= smsSearch.StartDate && d.Smsdate <= smsSearch.EndDate
                        && EF.Constant(states).Contains(d.StateId)
                        && (string.IsNullOrEmpty(smsSearch.Mobile) || d.Mobile.Contains(smsSearch.Mobile))
                        && (string.IsNullOrEmpty(smsSearch.MessageText) || d.Smstext.Contains(smsSearch.MessageText)))
                    .Include(d => d.State).Include(d => d.Priority)
                    .OrderBy(d => d.Smsdate), smsSearch.PageNo, smsSearch.PageSize)
                    .Queryable.ToListAsync();

            }
            else
            {
                return await PaginationFilter.GetPagedData(GetByCondition(d =>
                  d.Smsdate >= smsSearch.StartDate && d.Smsdate <= smsSearch.EndDate
                  && d.SmppId == smsSearch.SmppId
                  && EF.Constant(states).Contains(d.StateId)
                  && d.Mobile.Contains(smsSearch.Mobile)
                  && d.Smstext.Contains(smsSearch.MessageText)
                ).Include(d => d.State).Include(d => d.Priority)
                .OrderBy(d => d.Smsdate), smsSearch.PageNo, smsSearch.PageSize)
                    .Queryable.ToListAsync();

            }
        }
        public async Task<Sms?> DuplicateRecharge(DuplicateRechargeSrchModel smsDuplicateSearch)
        {
            return await _context.Sms.FirstOrDefaultAsync(d => d.Smsid != smsDuplicateSearch.SmsId
                             && d.Smstext == smsDuplicateSearch.SmsText
                             && d.Mobile == smsDuplicateSearch.Mobile
                             && d.Smsdate > DateTime.Now.AddMinutes(-5));

        }
        public async Task<bool> Resend(string mobile, string rechargeMobile)
        {
            var smsRecord = await (from sms in _context.Sms
                                   where sms.Mobile == mobile
                                  && sms.Smstext.Contains(rechargeMobile)
                                  && sms.Direction == true
                                   join rch in _context.SmsRecharge on sms.Smsid equals rch.SmsId
                                   orderby sms.Smsdate descending
                                   select sms
                                 ).FirstOrDefaultAsync();

            if (smsRecord != null)
            {
                smsRecord.StateId = (int)SmsState.Pending;
                _context.Update(smsRecord);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
