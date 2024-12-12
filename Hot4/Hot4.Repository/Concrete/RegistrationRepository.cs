using Hot4.Core.Enums;
using Hot4.Core.Helper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using System.Text.RegularExpressions;

namespace Hot4.Repository.Concrete
{
    public class RegistrationRepository : IRegistrationRepository
    {

        private readonly IAccountRepository _accountRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly ISMSRepository _smsRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IAccessRepository _accessRepository;
        private readonly IHotTypeRepository _hotTypeRepository;
        public RegistrationRepository(IAccountRepository accountRepository, ITemplateRepository templateRepository, ISMSRepository smsRepository, IConfigRepository configRepository, IAccessRepository accessRepository, IHotTypeRepository hotTypeRepository)
        {
            _accountRepository = accountRepository;
            _templateRepository = templateRepository;
            _smsRepository = smsRepository;
            _configRepository = configRepository;
            _accessRepository = accessRepository;
            _hotTypeRepository = hotTypeRepository;
        }

        public async Task Register(TblSms sms)
        {
            if (await RegistrationDuplicate(sms))
            {
                return;
            }
            if (!await RegistrationCorrectlyFormatted(sms))
            {
                return;
            }

            var account = await ParseRegistration(sms);
            var access = BuildRegistrationAccess(sms);

            access = await _accountRepository.AddWithAccessWithTransaction(account, access);

            var template = await _templateRepository.GetTemplate((int)Templates.SuccessfulRegistration);
            if (template == null)
            {
                return;
            }
            if (access != null)
            {
                template.TemplateText = template.TemplateText.Replace("%PASSWORD%", access.AccessPassword);
            }

            List<TblTemplate> templates = new List<TblTemplate>
            {
                template,
                await _templateRepository.GetTemplate((int)Templates.HelpEcocash),
                await _templateRepository.GetTemplate((int)Templates.HelpBank),
                await _templateRepository.GetTemplate((int)Templates.HelpRecharge),
            };

            await _smsRepository.ReplyWithTransaction(sms, templates);
        }

        public Task<Access?> RegisterSelfTopUpUser(TblSms sms)
        {
            throw new NotImplementedException();
        }

        private async Task<TblAccount> CreateNewSelfTopUpAccount(TblSms sms)
        {
            var config = await _configRepository.GetConfig();
            return new TblAccount
            {
                AccountName = $"SelfTopUp-{sms.Mobile}",
                Email = string.Empty,
                NationalId = sms.Mobile,
                InsertDate = DateTime.Now,
                ReferredBy = sms.Mobile
                //  ProfileId = config.p.ProfileId_NewSelfTopUpDealer,
            };
        }

        private async Task<TblAccount> ParseRegistration(TblSms sms)
        {
            var config = await _configRepository.GetConfig();
            var hotType = await _hotTypeRepository.GetHotType((int)HotTypes.Registration);

            ParseMessageFields(sms, hotType, out string firstname, out string surname, out string nationalID, out string email);

            var account = new TblAccount
            {
                AccountName = $"{surname}, {firstname}",
                NationalId = nationalID,
                Email = email,
                ReferredBy = sms.Mobile,
                InsertDate = DateTime.Now,
                ProfileId = config.ProfileIdNewSmsdealer,
            };

            if (sms.Smstext.ToUpper().StartsWith("BA"))
            {
                account.ProfileId = (int)Profiles.EconetBA;
                account.ReferredBy = "Econet-BA";
            }

            return account;
        }

        private static void ParseMessageFields
            (TblSms sms,
            TblHotType hotType,
            out string firstname,
            out string surname,
            out string idNumber,
            out string email)
        {
            var fields = Regex.Matches(sms.Smstext,/* hotType.RegexString*/"", RegexOptions.IgnoreCase).ToList();
            firstname = fields.Select(f => f.Groups["Firstname"].Value).ToList()[0];
            surname = fields.Select(f => f.Groups["Surname"].Value).ToList()[0];
            idNumber = fields.Select(f => f.Groups["IDNumber"].Value).ToList()[0];
            email = fields.Select(f => f.Groups["Email"].Value).ToList()[0];
        }

        private Access BuildRegistrationAccess(TblSms sms)
        {
            return new Access
            {
                AccessCode = sms.Mobile,
                AccessPassword = Helper.CreateRandomSMSAccessCode(),
                ChannelId = (byte)Channels.SMS,
            };
        }

        private async Task<bool> RegistrationCorrectlyFormatted(TblSms sms)
        {
            var hotType = await _hotTypeRepository.GetHotType((int)HotTypes.Registration);
            if (!Regex.Match(sms.Smstext,/* hotType.RegexString*/"", RegexOptions.IgnoreCase).Success)
            {
                var template = await _templateRepository.GetTemplate((int)Templates.HelpRegistration);
                if (template != null)
                {
                    await _smsRepository.ReplyWithTransaction(sms, new List<TblTemplate> { template });


                    return false;
                }
            }
            return true;
        }

        private async Task<bool> RegistrationDuplicate(TblSms sms)
        {
            var access = await _accessRepository.GetByAccessCode(sms.Mobile);

            if (access != null)
            {
                var account = await _accountRepository.GetAccount(access.AccountId);

                if (account != null && account.ProfileId == (int)Profiles.SelfTopUp)
                {
                    //TODO - GS - Reply Templates?
                    //await RegisterSelfTopUpUserAsync(sms)
                }
                else
                {
                    var template = await _templateRepository.GetTemplate((int)Templates.FailedRegistrationDuplicate);

                    if (template != null)
                    {
                        template.TemplateText = template.TemplateText.Replace("%NAME%", account?.AccountName);

                        await _smsRepository.ReplyWithTransaction(sms, new List<TblTemplate> { template });
                    }
                }

                return true;
            }

            return false;
        }
    }
}
