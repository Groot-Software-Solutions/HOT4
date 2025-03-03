using Hot.Econet.API.Installers;
using Hot.Econet.Prepaid.Enums;
using Hot.Econet.Prepaid.Models;
using Hot.Econet.Prepaid.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hot.Econet.API.EndpointDefinitions;

public class EconetPrepaidEndpointDefinition : IEndpointDefinition
{

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/account-balance-zwl", ZWLBalance);
        app.MapGet("/account-balance-usd", USDBalance);
        app.MapPost("/recharge-airtime-zwl", ZWLRecharge);
        app.MapPost("/recharge-airtime-usd", USDRecharge);
        app.MapPost("/recharge-data-zwl", ZWLRechargeData);
        app.MapPost("/recharge-data-usd", USDRechargeData);
    }

    public void DefineServices(IServiceCollection services)
    {

    }

    private string URL = "https://emr-econet-zw.patternmatched.com:7002/xmlrpc";
    private string ZWLUsername = "CommShopXMLUser";
    private string ZWLPassword = "CommShopXMLUserPMT52";
    private string USDUsername = "CommXmlUSD";
    private string USDPassword = "CommXmlUSDPMT20";

    [Produces(typeof(AccountBalanceResponse))]
    internal AccountBalanceResponse ZWLBalance()
    {
        //var service = new ApiClient(ZWLUsername, ZWLPassword, URL);
        var result = ApiClient.AccountBalanceV2(URL, ZWLUsername, ZWLPassword);
        return result;
    }

    [Produces(typeof(AccountBalanceResponse))]
    internal AccountBalanceResponse USDBalance()
    {
        var service = new ApiClient(USDUsername, USDPassword, URL);
        var result = service.AccountBalance();
        return result;
    }

    [Produces(typeof(LoadAirtimeResponse))]
    internal IResult ZWLRecharge(string TargetMobile, decimal Amount, string Reference)
    {
        var service = new ApiClient(ZWLUsername, ZWLPassword, URL);
        var result = service.LoadAirtime(TargetMobile, (int)(Amount * 100), Reference, CurrencyCode.ZWG);
        return Results.Ok(result);
    }

    [Produces(typeof(LoadAirtimeResponse))]
    internal IResult USDRecharge(string TargetMobile, decimal Amount, string Reference)
    {
        var service = new ApiClient(USDUsername, USDPassword, URL);
        var result = service.LoadAirtime(TargetMobile, (int)(Amount * 100), Reference, CurrencyCode.USD);
        return Results.Ok(result);
    }

    [Produces(typeof(LoadDataResponse))]
    internal IResult ZWLRechargeData(string TargetMobile, decimal Amount, string ProductCode, string Reference)
    {
        var service = new ApiClient(ZWLUsername, ZWLPassword, URL);
        var result = service.LoadDataBundle(TargetMobile, (int)(Amount * 100), ProductCode, Reference, CurrencyCode.ZWG);
        return Results.Ok(result);
    }

    [Produces(typeof(LoadDataResponse))]
    internal IResult USDRechargeData(string TargetMobile, decimal Amount, string ProductCode, string Reference)
    {
        var service = new ApiClient(USDUsername, USDPassword, URL);
        var result = service.LoadDataBundle(TargetMobile, (int)(Amount * 100), ProductCode, Reference, CurrencyCode.USD);
        return Results.Ok(result);
    }

}
