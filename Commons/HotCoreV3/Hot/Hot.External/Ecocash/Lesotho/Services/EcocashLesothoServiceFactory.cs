using Hot.Ecocash.Application.Common;
using Hot.Ecocash.Application.Common.Interfaces;
using Hot.Ecocash.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Hot.Ecocash.Lesotho.Services;

public class EcocashLesothoServiceFactory : IEcocashServiceFactory
{ 
    readonly IEcocashService mainService; 

    public EcocashLesothoServiceFactory(IConfiguration configuration , IEcocashService mainservice)
    {
        mainService = mainservice;  
        mainService.SetOptions(GetSettings("MainEcoCash", configuration), "MainEcoCash"); 
    }

    public IEcocashService GetService(EcocashAccounts account)
    {
        return account switch
        { 
            _ => mainService,
        };
    }

    private static ServiceOptions GetSettings(string name, IConfiguration configuration)
    {
        var accounts = configuration.GetSection("EcocashAccounts");
        ServiceOptions result = new(accounts.GetSection(name));
        return result;
    }

    public EcocashAccounts GetEcocashAccountByMerchant(string merchantCode)
    {
       return EcocashAccounts.MainAccount;
    }
}
