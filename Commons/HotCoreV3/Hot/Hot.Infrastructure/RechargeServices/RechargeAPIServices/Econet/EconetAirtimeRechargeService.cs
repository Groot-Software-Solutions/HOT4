using EconetAPI;
using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Econet;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Hot.Infrastructure.RechargeServices.Econet;
public class EconetAirtimeRechargeService : IEconetRechargeAPIService
{
    private readonly EconetSoapClient _client;

    public EconetAirtimeRechargeService(IConfiguration configuration)
    {
        var EconetConfigSection = configuration.GetSection("RechargeServices:Econet");
        var RemoteUrl = EconetConfigSection.GetValue("EconetAPIUrl", "http://10.10.31.60:8017/econet.asmx");
        _client = new EconetSoapClient(EconetSoapClient.EndpointConfiguration.EconetSoap, RemoteUrl);
    }

    public async Task<EconetRechargeResult> Debit(string Mobile, decimal Amount, string Reference)
    {
        var result = await _client.DebitMobileAsync(Mobile, Amount, Reference);

        return new EconetRechargeResult()
        {
            Successful = result.responseCode == "000",
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = result.narrative,
            InitialBalance = (decimal)result.initialBalance,
            FinalBalance = (decimal)result.finalBalance,
            ResponseCode = result.responseCode,
        };
    }

    public async Task<EconetBalanceResult> QueryEndUserBalance(string Mobile)
    {
        var Reference = Guid.NewGuid().ToString();
        var result = await _client.EndUserBalanceAsync(Mobile, Reference);

        return new EconetBalanceResult()
        {
            Successful = result.responseCode == "000",
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = result.narrative,
            Balance = (decimal)result.currentBalance,
            ResponseCode = result.responseCode,
        };
    }

    public async Task<EconetRechargeResult> Recharge(string Mobile, decimal Amount, string Reference)
    {
        var result = await _client.RechargeMobileAsync(Mobile, Amount, Reference); 
        ParseNarative(result, out decimal initialWallet, out decimal finalWallet);
        return new EconetRechargeResult()
        {
            Successful = result.responseCode == "000",
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = result.narrative.Split("#")[0],
            InitialBalance = (decimal)result.initialBalance,
            FinalBalance = (decimal)result.finalBalance,
            ResponseCode = result.responseCode,
            InitialWallet = initialWallet,
            FinalWallet = finalWallet,
        };
    }

    private static string ParseNarative(CreditResponse result, out decimal initialWallet, out decimal finalWallet)
    {
        var narrative = result.narrative;
        initialWallet = 0;
        finalWallet = 0;
        if (result.narrative.Contains("#prepaid"))
        {
            var values = narrative.Split("#");
             narrative = values[0];
            try
            {
                values[1].TryParse(out initialWallet);
                values[2].TryParse(out finalWallet);
            }
            catch (Exception) { }
        }
        return narrative;
    }
}
