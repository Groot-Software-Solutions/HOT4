using Hot.Ecocash.Application.Common;
using Hot.Ecocash.Application.Common.Interfaces;
using Hot.Ecocash.Domain.Enums;
using Microsoft.Extensions.Configuration;
using System.Runtime;

namespace Hot.Ecocash.Infrastructure.Services
{
    public class EcocashServiceFactory : IEcocashServiceFactory
    {
        readonly IEcocashService zesaService;
        readonly IEcocashService mainService;
        readonly IEcocashService apiUserService;
        readonly IEcocashService fcaService;
        private readonly IConfiguration configuration;
        public EcocashServiceFactory(IConfiguration configuration, IEcocashService mainservice, IEcocashService zesaservice, IEcocashService apiservice, IEcocashService fcaservice)
        {
            this.configuration = configuration;
            mainService = mainservice;
            zesaService = zesaservice;
            apiUserService = apiservice;
            fcaService = fcaservice;

            mainService.SetOptions(GetSettings("MainEcoCash"), "MainEcoCash");
            zesaService.SetOptions(GetSettings("ZesaEcoCash"), "ZESAEcoCash");
            apiUserService.SetOptions(GetSettings("APIUsersEcoCash"), "APIUsersEcoCash");
            fcaService.SetOptions(GetSettings("FCAEcoCash"), "FCAEcoCash");
        }

        public IEcocashService GetService(EcocashAccounts account)
        {
            return account switch
            {
                EcocashAccounts.APIUserAccount => apiUserService,
                EcocashAccounts.ZESAAccount => zesaService,
                EcocashAccounts.FCAAccount => fcaService,
                _ => mainService,
            };
        }
        public EcocashAccounts GetEcocashAccountByMerchant(string merchantCode)
        {
            if (merchantCode == GetSettings("APIEcoCash").MechantCode) return EcocashAccounts.APIUserAccount;
            if (merchantCode == GetSettings("FCAEcoCash").MechantCode) return EcocashAccounts.FCAAccount;
            if (merchantCode == GetSettings("ZesaEcoCash").MechantCode) return EcocashAccounts.ZESAAccount;
            return EcocashAccounts.MainAccount;

        }
        private ServiceOptions GetSettings(string name)
        {
            var accounts = configuration.GetSection("EcocashAccounts");
            ServiceOptions result = new(accounts.GetSection(name));
            return result;
        }

    }
}
