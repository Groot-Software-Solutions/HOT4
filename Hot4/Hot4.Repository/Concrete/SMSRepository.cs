using Hot4.Core.DataViewModels;
using Hot4.Core.Enums;
using Hot4.Core.Helper;
using Hot4.DataModel.Data;

using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class SMSRepository : RepositoryBase<Sms>, ISMSRepository
    {
        public SMSRepository(HotDbContext context) : base(context) { }
        public async Task<long> AddSMS(Sms sms)
        {
            await Create(sms);
            //await CreateReturn(sms, "SMSID");
            await SaveChanges();
            return sms.StateId;
        }

        public async Task ClearSMSPassword(Sms sms, bool hadValidPassword, HotTypeState hotTypeSMS)
        {
            var hotType = await _context.HotType.FirstOrDefaultAsync(d => d.HotTypeId == (int)hotTypeSMS);


            //var fields = Regex.Matches(sms.Smstext, hotType.RegexString, RegexOptions.IgnoreCase).ToList();
            //var smsPassword = fields.Select(f => f.Groups["Pin"].Value).ToList()[0];

            //sms.SMSText = sms.SMSText.Remove(sms.SMSText.LastIndexOf(smsPassword, StringComparison.OrdinalIgnoreCase));
            //sms.SMSText = sms.SMSText + (hadValidPassword
            //    ? "[Valid PIN]"
            //    : "[Wrong PIN]");

            await Update(sms);
        }

        public async Task<Sms?> Duplicate(Sms sms)
        {
            return await GetByCondition(d =>
                       d.Smstext == sms.Smstext
                    && d.Mobile == sms.Mobile
                    && d.Smsid != sms.Smsid
                    && d.Smsdate > sms.Smsdate.AddMinutes(-5)).FirstOrDefaultAsync();
        }

        public async Task<List<VwSm>> GetPendingSMSWithTransaction()
        {
            var smsList = new List<VwSm>();

            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                smsList = await _context.VwSms
                    .Where(d => d.StateId == (byte)SmsStates.Pending
                        && d.Direction == true)
                    .OrderByDescending(d => d.PriorityId)
                    .ThenBy(d => d.Smsdate)
                    .Take(500)
                    .ToListAsync();
                if (smsList != null && smsList.Count > 0)
                {
                    smsList.ForEach(d => d.StateId = (byte)SmsStates.Busy);

                    _context.Sms.UpdateRange(smsList.Select(d =>
                        new Sms
                        {
                            Direction = d.Direction,
                            InsertDate = d.InsertDate,
                            Mobile = d.Mobile,
                            PriorityId = d.PriorityId,
                            SmppId = d.SmppId,

                            Smsdate = d.Smsdate,
                            Smsid = d.Smsid,
                            SmsidIn = d.SmsidIn,
                            Smstext = d.Smstext,
                            StateId = d.StateId,
                        }));

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                }
            }
            catch (Exception)
            {
                if (_context.Database.CurrentTransaction != null)
                {
                    await _context.Database.RollbackTransactionAsync();
                }

                throw;
            }

            return smsList ?? new List<VwSm>();
        }

        public async Task<List<AccountSmsModel>> ListSMSInViewsForAccount(long accountId, DateTime dateTime)
        {
            var res = new List<AccountSmsModel>();

            var access = await _context.Access.FirstOrDefaultAsync(d => d.AccountId == accountId);
            if (access != null)
            {
                res = await (from sms in _context.Sms
                             join state in _context.State
                             on sms.StateId equals state.StateId
                             join prios in _context.Priority
                             on sms.PriorityId equals prios.PriorityId
                             join smpp in _context.Smpp
                             on sms.SmppId equals smpp.SmppId
                             where sms.Direction == true
                             && sms.Mobile == access.AccessCode
                             && sms.Smsdate.Date == dateTime.Date

                             select new AccountSmsModel()
                             {
                                 DrectionText = sms.Direction ? "In" : "Out",
                                 SMSText = sms.Smstext,
                                 InsertDate = sms.InsertDate,
                                 Mobile = sms.Mobile,
                                 Priority = prios.Priority,
                                 SmppID = smpp.SmppId,
                                 SMSDate = sms.Smsdate,
                                 SMSID = sms.Smsid,
                                 State = state.State,
                                 SMSID_In = sms.SmsidIn
                             }).ToListAsync();
            }
            return res;
        }

        public async Task<List<AccountSmsModel>> ListSMSOutViewsForAccount(long smsId)
        {
            return await (from sms in _context.Sms
                          join state in _context.State
                          on sms.StateId equals state.StateId
                          join prios in _context.Priority
                          on sms.PriorityId equals prios.PriorityId
                          where sms.SmsidIn == smsId

                          select new AccountSmsModel()
                          {
                              DrectionText = sms.Direction ? "In" : "Out",
                              SMSText = sms.Smstext,
                              InsertDate = sms.InsertDate,
                              Mobile = sms.Mobile,
                              Priority = prios.Priority,
                              SMSDate = sms.Smsdate,
                              SMSID = sms.Smsid,
                              State = state.State,
                              SMSID_In = sms.SmsidIn
                          }).ToListAsync();
        }

        public async Task<List<AccountSmsModel>> ListSMSSearchViewsForAccount(DateTime startdate, DateTime enddate, string mobile, string messageText, byte smppId, long stateId)
        {
            return await (from sms in _context.Sms
                          join acss in _context.Access
                          on sms.Mobile equals acss.AccessCode
                          join state in _context.State
                          on sms.StateId equals state.StateId
                          join prios in _context.Priority
                          on sms.PriorityId equals prios.PriorityId
                          join smpp in _context.Smpp
                          on sms.SmppId equals smpp.SmppId
                          where sms.Smstext == messageText
                          && sms.Smsid == smppId
                          && sms.StateId == stateId

                          && sms.Mobile == mobile
                          && sms.Smsdate.Date >= startdate.Date
                          && sms.Smsdate.Date <= enddate.Date
                          select new AccountSmsModel()
                          {
                              DrectionText = sms.Direction ? "In" : "Out",
                              SMSText = sms.Smstext,
                              InsertDate = sms.InsertDate,
                              Mobile = sms.Mobile,
                              Priority = prios.Priority,
                              SmppID = smpp.SmppId,
                              SMSDate = sms.Smsdate,
                              SMSID = sms.Smsid,
                              State = state.State,
                              SMSID_In = sms.SmsidIn
                          }).ToListAsync();
        }

        public async Task<List<AccountSmsModel>> RefreshListSMSOutViewsForAccount(long accountId, DateTime dateTime)
        {
            var res = new List<AccountSmsModel>();
            var access = await _context.Access.FirstOrDefaultAsync(d => d.AccountId == accountId);
            if (access != null)
            {
                res = await (from sms in _context.Sms
                             join state in _context.State
                              on sms.StateId equals state.StateId
                             join prios in _context.Priority
                             on sms.PriorityId equals prios.PriorityId
                             where sms.Direction == false
                             && sms.Mobile == access.AccessCode
                             && sms.Smsdate.Date == dateTime.Date

                             select new AccountSmsModel()
                             {
                                 DrectionText = sms.Direction ? "In" : "Out",
                                 SMSText = sms.Smstext,
                                 InsertDate = sms.InsertDate,
                                 Mobile = sms.Mobile,
                                 Priority = prios.Priority,
                                 SMSDate = sms.Smsdate,
                                 SMSID = sms.Smsid,
                                 State = state.State,
                                 SMSID_In = sms.SmsidIn
                             }).ToListAsync();
            }
            return res;
        }

        public async Task Reply(Sms sms, List<Template> templates)
        {
            sms.StateId = (byte)SmsStates.Success;
            _context.Sms.Update(sms);


            foreach (var template in templates)
            {
                var reply = new Sms
                {
                    Direction = false,
                    Mobile = sms.Mobile,
                    SmsidIn = sms.Smsid,
                    Smstext = template.TemplateText,
                    PriorityId = (byte)PriorityType.Normal,
                    StateId = (byte)SmsStates.Pending,
                    InsertDate = DateTime.Now,
                    Smsdate = DateTime.Now,
                };

                await _context.AddAsync(reply);
            }

            await _context.SaveChangesAsync();
        }

        public async Task ReplyCustomer(string mobile, Sms sms, List<Template> templates)
        {
            foreach (var template in templates)
            {
                var reply = new Sms
                {
                    Direction = false,
                    Mobile = mobile,
                    SmsidIn = sms.Smsid,
                    Smstext = template.TemplateText,
                    PriorityId = (byte)PriorityType.Normal,
                    StateId = (byte)SmsStates.Pending,
                    InsertDate = DateTime.Now,
                    Smsdate = DateTime.Now,
                };
                await Create(reply);

            }
            await SaveChanges();
        }

        public async Task ReplyWithTransaction(Sms sms, List<Template> templates)
        {
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                await Reply(sms, templates);

                await transaction.CommitAsync();

            }
            catch (Exception)
            {
                if (_context.Database.CurrentTransaction != null)
                {
                    await _context.Database.RollbackTransactionAsync();
                }
                throw;
            }
        }

        public async Task ResendWithTransaction(Sms smsRequest)
        {
            string target = string.Empty; //By default, no target filter

            if (smsRequest.Smstext.Split(' ').Length > 1)
            {
                target = smsRequest.Smstext.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1];
            }

            var resendSMS = await (from sms in _context.Sms
                                   join smsrech in _context.SmsRecharge
                                       on sms.Smsid equals smsrech.SmsId
                                   where sms.Mobile == smsRequest.Mobile
                                        && EF.Constant(sms.Smstext).Contains(target)
                                        && sms.Direction == true
                                   select sms)
                                   .OrderByDescending(d => d.Smsdate)
                                   .Take(1)
                                   .FirstOrDefaultAsync();

            if (resendSMS != null)
            {
                try
                {
                    await using var transaction = await _context.Database.BeginTransactionAsync();
                    var updateSMSList = await GetByCondition(d => d.Smsid == resendSMS.Smsid).ToListAsync();
                    if (updateSMSList != null)
                    {
                        updateSMSList.ForEach(d => d.StateId = (byte)SmsStates.Pending);
                        _context.Sms.UpdateRange(updateSMSList); // BulkUpdate(updateSMSList);
                    }

                    smsRequest.StateId = (byte)SmsStates.Success;
                    _context.Sms.Update(smsRequest); // Update(smsRequest);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    if (_context.Database.CurrentTransaction != null)
                    {
                        await _context.Database.RollbackTransactionAsync();
                    }
                    throw;
                }
            }
        }



        public async Task<int> SendEmails(string message, string subject, string emailtype)
        {
            int emailsSent = 0;

            var recentAccounts = await _context.Access
                .Where(a => a.ChannelId == (byte)ChannelType.Web)
                .Join(_context.Account,
                a => a.AccountId,
                b => b.AccountId,
                (a, b) => new { a.AccessCode, b.ProfileId, b.AccountName, b.Email, a.AccountId })
                .Where(ab => (ab.ProfileId < 20 || ab.ProfileId > 30))
                .Select(ab => new { ab.Email, ab.AccountName })
                .ToListAsync();

            foreach (var account in recentAccounts)
            {
                string emailAdd = account.Email;
                string accountName = account.AccountName;
                string htmlBody = $"Dear {accountName}<br><br>{message}";

                Email.SendEmail(emailAdd, subject, htmlBody);
                emailsSent++;
            }

            string finalSubject = $"{subject} :sent to {emailsSent + 1} " + emailtype;
            string finalBody = $"Dear HOT Recharge<br><br>{message}";

            Email.SendEmail("register@hot.co.zw", finalSubject, finalBody, "kurt@hot.co.zw");

            return emailsSent;
        }

        public async Task UpdateSMS(Sms sms)
        {
            await Update(sms);
            await SaveChanges();

        }


    }
}
