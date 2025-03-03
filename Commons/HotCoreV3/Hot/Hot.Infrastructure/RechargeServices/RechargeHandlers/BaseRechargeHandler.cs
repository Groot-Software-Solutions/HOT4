using Hot.Application.Common.Extensions;
using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Hot.Domain.Enums;
using Hot.Infrastructure.FactoryServices;
using Microsoft.Extensions.DependencyInjection;

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers;
public abstract class BaseRechargeHandler
{
    private readonly IDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    internal Recharge Recharge { get; set; } = new();
    internal RechargePrepaid RechargePrepaid { get; set; } = new();
    internal Account Account { get; set; } = new();
    internal Access Access { get; set; } = new();
    internal Brand Brand { get; set; } = new();

    protected BaseRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider)
    {
        _dbContext = dbcontext;
        _serviceProvider = serviceProvider;
    }

    internal virtual string GetReference()
    {
        if (string.IsNullOrEmpty(RechargePrepaid.Reference)) return Recharge.RechargeId.ToString();
        return RechargePrepaid.Reference;
    }

    internal async Task ApplyDiscountRules()
    {
        if (IsCorporateOrAggregator()) return; // Ignore all discount rule for Aggregators and Corporates
        await Task.Run(() => { ProcessDiscountRules(Recharge); });
    }

    private bool IsCorporateOrAggregator()
    {
        return Account.ProfileID > 9 && Account.ProfileID < 60;
    }

    private static void ProcessDiscountRules(Recharge recharge)
    {
        var factory = new DiscountRulesFactory();
        var rules = factory.GetRules();
        rules.ForEach(r => r.Apply(recharge));
    }

    internal T? GetServiceFromProvider<T>()
    {
        return _serviceProvider.GetService<T>();
    }

    internal async Task SetRecharge(Recharge recharge, RechargePrepaid rechargePrepaid)
    {
        Recharge = recharge;
        RechargePrepaid = rechargePrepaid;
        RechargePrepaid.Reference = GetReference();
        await UpdateAccountDataAsync();
        await LoadBrandAsync();

    }

    internal async Task<bool> UpdateRechargeStatusAsync()
    {
        var updateRechargeResponse = await _dbContext.Recharges.UpdateAsync(Recharge);
        if (updateRechargeResponse.IsT1) return false;
        var updateRechargePrepaidResponse = await _dbContext.RechargePrepaids.UpdateAsync(RechargePrepaid);
        if (updateRechargePrepaidResponse.IsT1) return false;
        return true;
    }

    internal async Task<RechargeServiceNotFoundException> UpdateAndReturnServiceNotFound(string Name)
    {
        Recharge.StateId = 3;
        Recharge.RechargeDate = DateTime.Now;
        RechargePrepaid.Narrative = "Recharge Service not found in services";
        RechargePrepaid.ReturnCode = "-1";
        await UpdateRechargeStatusAsync();
        return new RechargeServiceNotFoundException(Name, "Service not found in services");
    }

    internal async Task<decimal?> GetWalletBalanceAsync()
    {
        var result = Account.WalletBalance((WalletTypes)Brand.WalletTypeId);
        if (Recharge.StateId != (int)States.Failure)
        {
            await UpdateAccountDataAsync();
            result = Account.WalletBalance((WalletTypes)Brand.WalletTypeId);
        }
        return result;
    }

    internal async Task<bool> UpdateAccountDataAsync()
    {
        var accessResponse = await _dbContext.Accesss.GetAsync((int)Recharge.AccessId);
        if (accessResponse.IsT1) return false;
        Access = accessResponse.AsT0;
        var accountResponse = await _dbContext.Accounts.GetAsync((int)accessResponse.AsT0.AccountId);
        if (accountResponse.IsT1) return false;
        Account = accountResponse.AsT0;
        return true;
    }

    internal async Task<bool> LoadBrandAsync()
    {
        var brandResponse = await _dbContext.Brands.GetAsync(Recharge.BrandId);
        if (brandResponse.IsT1) return false;
        Brand = brandResponse.AsT0;
        return true;
    }


}
