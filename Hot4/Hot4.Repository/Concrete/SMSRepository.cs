
using Hot4.Core.Enums;
using Hot4.Core.Helper;
using Hot4.DataModel.Data;

using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class SMSRepository : RepositoryBase<Sms>, ISMSRepository
    {
        public SMSRepository(HotDbContext context) : base(context) { }
        public async Task<long> AddSMS(Sms sms)
        {
            await Create(sms);
            await SaveChanges();
            return sms.StateId;
        }
        public async Task UpdateSMS(Sms sms)
        {
            await Update(sms);
            await SaveChanges();
        }

        public async Task SendBulkSms(string messageText)
        {
            var accountIds = await _context.Payment.Where(d => d.PaymentDate > DateTime.Now.AddDays(-90)
                              && d.PaymentTypeId != (int)PaymentMethodType.Writeoff)
                              .Select(d => d.AccountId).ToListAsync();

            var mobileList = await _context.Access
              .Where(d => d.ChannelId == (int)ChannelName.Sms && d.Deleted == false
              && EF.Constant(accountIds).Contains(d.AccountId)
              && !new[] { "2200958", "2205072", "2250126", "2256859", "2275909", "2206206" }
                       .Contains(d.AccessCode.Substring(d.AccessCode.Length - 7))
              && string.Compare(d.AccessCode, "0799999999") > 0)
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

            string subject = "HOT Recharge Notification " + DateTime.Now.ToString("yyyy-MM-dd");

            var emailList = await _context.Access.Include(d => d.Account)
                .Where(d => d.ChannelId == (int)ChannelName.Web && EF.Constant(accountIds).Contains(d.AccountId))
                .Select(d => new { d.AccessCode, d.Account.AccountName }).ToListAsync();

            foreach (var emailData in emailList)
            {
                string emailAddress = emailData.AccessCode;
                string accountName = emailData.AccountName;
                string htmlBody = $"Dear {accountName}<br><br>{messageText}<br><br>Best regards <br>the HOT Recharge Team";

                Email.SendEmail(emailAddress, subject, htmlBody);
            }
        }

        public async Task<int> SaveBulkSms(string messageText)
        {

            var recentPaymentAccountIds = await _context.Payment
                .Where(d => d.PaymentDate > DateTime.Now.AddDays(-90))
                .Select(d => d.AccountId)
                .Distinct().ToListAsync();

            #region code for another datbase
            #endregion

            // var recentAccountIds = recentPayments.Concat(recentPaymentsHot4Arch).Distinct();

            var tempMobileNumbers = await _context.Access
                .Where(d => d.ChannelId == (int)ChannelName.Sms && d.Deleted == false
                            && recentPaymentAccountIds.Contains(d.AccountId)
                            && !new[] { "2200958", "2205072", "2250126", "2256859", "2275909", "2206206", "4666874" }
                                .Contains(d.AccessCode.Substring(d.AccessCode.Length - 7))
                            && d.AccessCode.CompareTo("0799999999") <= 0)
                .Select(a => a.AccessCode)
                .Distinct().ToListAsync();

            var smsRecords = tempMobileNumbers.Select(mobile => new Sms
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

            return tempMobileNumbers.Count;
        }
        public async Task EmailAggregators(string sub, string messageText)
        {
            var accountIds = await _context.Payment.Where(d => d.PaymentDate > DateTime.Now.AddDays(-180))
                                                         .Select(d => d.AccountId).ToListAsync();

            var emailAccessAccount = await _context.Access.Include(d => d.Account)
                                    .Where(d => d.ChannelId == (int)ChannelName.Web
                                    && d.Deleted == false
                                    && Helper.CheckValidEmail(d.AccessCode) == true
                                    && d.Account.ProfileId > (int)Profiles.BLANK && d.Account.ProfileId <= (int)Profiles.BRAND_AMB_CX_50_300
                                    && EF.Constant(accountIds).Contains(d.AccountId)
                                    ).Select(d => new { d.AccessCode, d.Account.AccountName }).ToListAsync();

            int x = 0;
            foreach (var email in emailAccessAccount)
            {
                x++;
                string html = $"Dear {email.AccountName}<br><br>{messageText}";
                Email.SendEmail(email.AccessCode, sub, html);
            }

            x++;
            string summaryHtml = $"Dear HOT Recharge<br><br>{messageText}";
            string finalSub = $"{sub} :send to {x} Aggregators";
            Email.SendEmail("register@hot.co.zw", finalSub, summaryHtml, new string[] { "kurt@hot.co.zw", "rob@hot.co.zw", "gina@hot.co.zw" });
        }
        public async Task EmailCorporates(string sub, string messageText)
        {
            int x = 0;
            sub = sub + " " + DateTime.Now.ToString();

            var accountIds = await _context.Payment.Where(d => d.PaymentDate > DateTime.Now.AddDays(-180))
                                                        .Select(d => d.AccountId).ToListAsync();

            var emailData = await _context.Access.Include(d => d.Account)
                .Where(d => d.ChannelId == (int)ChannelName.Web && d.Deleted == false
                   && (d.Account.ProfileId < (int)Profiles.EASYLINK_MTS
                   || (d.Account.ProfileId >= (int)Profiles.BRAND_AMB_CX_50_300 && d.Account.ProfileId <= (int)Profiles.NEVER_ACTIVE))
                   && Helper.CheckValidEmail(d.AccessCode) == true
                   && EF.Constant(accountIds).Contains(d.AccountId))
                .OrderBy(d => d.Account.ProfileId)
                .Select(d => new { d.AccessCode, d.Account.AccountName }).ToListAsync();

            foreach (var email in emailData)
            {
                x += 1;
                string html = $"Dear {email.AccountName}<br><br>{messageText}";

                Email.SendEmail(email.AccessCode, sub, html);
            }

            string finalHtml = "Dear HOT Recharge<br><br>" + messageText;
            sub = sub + " :sent to " + x.ToString() + " Corporates";
            Email.SendEmail("register@hot.co.zw", sub, finalHtml, new string[] { "kurt@hot.co.zw; rob@hot.co.zw; gina@hot.co.zw" });
        }
        public async Task<List<SMSModel>> SMSInbox()
        {
            int queueSize = 500;

            var smsDetail = await _context.Sms
                .Where(d => d.Direction == true && d.StateId == (int)SmsState.Pending)
                .Include(d => d.State).Include(d => d.Priority)
                .OrderByDescending(d => d.PriorityId)
                .ThenBy(d => d.Smsdate)
                .Take(queueSize)
                //.Select(s => s.Smsid)
                .ToListAsync();

            //foreach (var smsID in smsIDs)
            //{
            //    var sms = tblSMS.FirstOrDefault(s => s.SMSID == smsID);
            //    if (sms != null)
            //    {
            //        sms.StateID = 1;
            //    }
            //}
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

                return smsDetail.Select(d => new SMSModel
                {
                    Direction = d.Direction,
                    InsertDate = d.InsertDate,
                    Mobile = d.Mobile.Replace(" ", ""),
                    Priority = d.Priority.Priority,
                    PriorityId = d.PriorityId,
                    SmppId = d.SmppId,
                    SMSDate = d.Smsdate,
                    SMSId = d.Smsid,
                    SMSIDIn = d.SmsidIn,
                    SMSText = d.Smstext,
                    State = d.State.State,
                    StateId = d.StateId,
                }).OrderByDescending(d => d.PriorityId)
            .ThenBy(d => d.SMSDate)
            .ToList();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
        public async Task<List<SMSModel>> SMSOutbox()
        {
            int queueSize = 500;

            var smsDetail = await _context.Sms
                .Where(d => d.Direction == false && d.StateId == (int)SmsState.Pending && d.Smsdate < DateTime.Now)
                .Include(d => d.State).Include(d => d.Priority)
                .OrderByDescending(d => d.PriorityId)
                .ThenBy(d => d.Smsdate)
                .Take(queueSize)
                .ToListAsync();
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

                return smsDetail.Select(d => new SMSModel
                {
                    Direction = d.Direction,
                    InsertDate = d.InsertDate,
                    Mobile = d.Mobile.Replace(" ", ""),
                    Priority = d.Priority.Priority,
                    PriorityId = d.PriorityId,
                    SmppId = d.SmppId,
                    SMSDate = d.Smsdate,
                    SMSId = d.Smsid,
                    SMSIDIn = d.SmsidIn,
                    SMSText = d.Smstext,
                    State = d.State.State,
                    StateId = d.StateId,
                }).OrderByDescending(d => d.PriorityId)
                .ThenBy(d => d.SMSDate)
                .ToList();


            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
        public async Task<List<SMSModel>> GetSMSByAccountSMSDate(long accountId, DateTime smsDate)
        {
            var startDate = smsDate.Date;
            var endDate = smsDate.Date.AddDays(1).AddTicks(-1);

            return await (from sms in _context.Sms.Include(d => d.State).Include(d => d.Priority)
                         .Where(d => d.Direction == true && d.Smsdate >= startDate && smsDate <= endDate)
                          join acss in _context.Access on sms.Mobile equals acss.AccessCode
                          where acss.AccountId == accountId
                          select new SMSModel
                          {
                              Direction = sms.Direction,
                              InsertDate = sms.InsertDate,
                              Mobile = sms.Mobile.Replace(" ", ""),
                              Priority = sms.Priority.Priority,
                              PriorityId = sms.PriorityId,
                              SmppId = sms.SmppId,
                              SMSDate = sms.Smsdate,
                              SMSId = sms.Smsid,
                              SMSIDIn = sms.SmsidIn,
                              SMSText = sms.Smstext,
                              State = sms.State.State,
                              StateId = sms.StateId,
                          })
                .OrderByDescending(d => d.SMSDate)
                .ToListAsync();
        }
        public async Task<List<SMSModel>> GetSMSBySMSId(long smsId)
        {
            return await GetByCondition(d => d.Smsid == smsId)
                .Include(d => d.State).Include(d => d.Priority)
                .Select(d => new SMSModel
                {
                    Direction = d.Direction,
                    InsertDate = d.InsertDate,
                    Mobile = d.Mobile.Replace(" ", ""),
                    Priority = d.Priority.Priority,
                    PriorityId = d.PriorityId,
                    SmppId = d.SmppId,
                    SMSDate = d.Smsdate,
                    SMSId = d.Smsid,
                    SMSIDIn = d.SmsidIn,
                    SMSText = d.Smstext,
                    State = d.State.State,
                    StateId = d.StateId,
                })
                .OrderByDescending(d => d.SMSDate)
                .ToListAsync();
        }
        public async Task<List<SMSModel>> SMSSearch(SMSSearchModel smsSearch)
        {
            var states = new List<byte>();

            if (smsSearch.StateId == -1)
            {
                states = await _context.State.Select(d => d.StateId).ToListAsync();
            }
            else
            {
                states.Add(smsSearch.StateId);
            }

            if (smsSearch.SmppId == -1)
            {
                return await GetByCondition(d => d.Smsdate >= smsSearch.StartDate
                        && d.Smsdate <= smsSearch.EndDate && states.Contains(d.StateId)
                        && (string.IsNullOrEmpty(smsSearch.Mobile) || d.Mobile.Contains(smsSearch.Mobile))
                        && (string.IsNullOrEmpty(smsSearch.MessageText) || d.Smstext.Contains(smsSearch.MessageText)))
                    .Include(d => d.State).Include(d => d.Priority)
                                .Select(d => new SMSModel
                                {
                                    Direction = d.Direction,
                                    InsertDate = d.InsertDate,
                                    Mobile = d.Mobile.Replace(" ", ""),
                                    Priority = d.Priority.Priority,
                                    PriorityId = d.PriorityId,
                                    SmppId = d.SmppId,
                                    SMSDate = d.Smsdate,
                                    SMSId = d.Smsid,
                                    SMSIDIn = d.SmsidIn,
                                    SMSText = d.Smstext,
                                    State = d.State.State,
                                    StateId = d.StateId,
                                }).OrderBy(d => d.SMSDate).ToListAsync();

            }

            else
            {

                return await _context.Sms.Include(d => d.State).Include(d => d.Priority)
                    .Where(d => d.Smsdate >= smsSearch.StartDate && d.Smsdate <= smsSearch.EndDate)
                    .Where(d => d.SmppId == smsSearch.SmppId)
                    .Where(d => states.Contains(d.StateId))
                    .Where(d => d.Mobile.Contains(smsSearch.Mobile))
                    .Where(d => d.Smstext.Contains(smsSearch.MessageText))
                    .Select(d => new SMSModel
                    {
                        Direction = d.Direction,
                        InsertDate = d.InsertDate,
                        Mobile = d.Mobile.Replace(" ", ""),
                        Priority = d.Priority.Priority,
                        PriorityId = d.PriorityId,
                        SmppId = d.SmppId,
                        SMSDate = d.Smsdate,
                        SMSId = d.Smsid,
                        SMSIDIn = d.SmsidIn,
                        SMSText = d.Smstext,
                        State = d.State.State,
                        StateId = d.StateId,
                    })
                    .OrderBy(sms => sms.SMSDate)
                    .Take(200)
                    .ToListAsync();
            }
        }
        public async Task<SMSModel?> DuplicateRecharge(DuplicateRechargeSrchModel smsDuplicateSearch)
        {
            var result = await _context.Sms.FirstOrDefaultAsync(d => d.Smsid != smsDuplicateSearch.SmsId
                             && d.Smstext == smsDuplicateSearch.SmsText
                             && d.Mobile == smsDuplicateSearch.Mobile
                             && d.Smsdate > DateTime.Now.AddMinutes(-5));

            if (result != null)
            {
                return new SMSModel
                {
                    Direction = result.Direction,
                    InsertDate = result.InsertDate,
                    Mobile = result.Mobile.Replace(" ", ""),
                    Priority = result.Priority.Priority,
                    PriorityId = result.PriorityId,
                    SmppId = result.SmppId,
                    SMSDate = result.Smsdate,
                    SMSId = result.Smsid,
                    SMSIDIn = result.SmsidIn,
                    SMSText = result.Smstext,
                    State = result.State.State,
                    StateId = result.StateId,
                };
            }
            else
            {
                return null;
            }

        }
        public async Task Resend(string mobile, string rechargeMobile)
        {
            var smsId = await (from sms in _context.Sms
                               where sms.Mobile == mobile
                              && sms.Smstext.Contains(rechargeMobile)
                              && sms.Direction == true
                               join rch in _context.SmsRecharge on sms.Smsid equals rch.SmsId
                               select new { sms.Smsid, sms.Smsdate }
                                 ).OrderByDescending(d => d.Smsdate).Select(d => d.Smsid).FirstOrDefaultAsync();

            if (smsId > 0)
            {
                var smsToUpdate = await _context.Sms.FirstOrDefaultAsync(d => d.Smsid == smsId);
                if (smsToUpdate != null)
                {

                    smsToUpdate.StateId = (int)SmsState.Pending;
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
