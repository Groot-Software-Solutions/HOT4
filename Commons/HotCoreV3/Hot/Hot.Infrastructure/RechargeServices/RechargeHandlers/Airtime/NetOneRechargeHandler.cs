using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.NetOne;
using Hot.Domain.Entities;
using Hot.Domain.Enums;

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers.Airtime
{
    public class NetOneRechargeHandler : BaseRechargeHandler, IRechargeHandler
    {
        public NetOneRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider)
            : base(dbcontext, serviceProvider)
        {
        }

        public int BrandId { get; set; } = (int)Brands.EasyCall;
        public RechargeType Rechargetype { get; set; } = RechargeType.Airtime;
        public Currency Currency { get; set; } = Currency.ZWG;
        public async Task<OneOf<RechargeResult, RechargeServiceNotFoundException>> ProcessAsync(Recharge recharge, RechargePrepaid rechargePrepaid)
        {
            await SetRecharge(recharge, rechargePrepaid);
            await ApplyDiscountRules();
            var service = GetServiceFromProvider<INetOneRechargeAPIService>();
            if (service is null) return await UpdateAndReturnServiceNotFound("Netone Airtime Service");
            var response = await service.Recharge(Recharge.Mobile, Recharge.Amount, (int)recharge.RechargeId, Currency);
            SetResultFromResponse(response);
            await UpdateRechargeStatusAsync();
            var result = await GetRechargeResultFromResponseAsync(response);
            return result;
        }

        internal override string GetReference()
        {
            return $"{Recharge.RechargeId}";
        }

        private async Task<RechargeResult> GetRechargeResultFromResponseAsync(NetOneRechargeResult response)
        {
            return new RechargeResult()
            {
                Recharge = Recharge,
                RechargePrepaid = RechargePrepaid,
                ErrorData = response.Successful ? null : response.RawResponseData,
                Message = response.TransactionResult ?? "",
                Successful = response.Successful,
                WalletBalance = await GetWalletBalanceAsync(),
            };
        }

        private void SetResultFromResponse(NetOneRechargeResult response)
        {
            Recharge.StateId = (byte)(response.Successful ? 2 : 3);
            Recharge.RechargeDate = DateTime.Now;
            RechargePrepaid.Narrative = response.RawResponseData ?? "";
            RechargePrepaid.ReturnCode = response.Successful ? "1" : "0";
            RechargePrepaid.InitialBalance = response.InitialBalance;
            RechargePrepaid.FinalBalance = response.FinalBalance;
            RechargePrepaid.InitialWallet = response.InitialWallet;
            RechargePrepaid.FinalWallet = response.FinalWallet;
            RechargePrepaid.Narrative = response.Narrative;
            RechargePrepaid.ReturnCode = response.ReturnCode;
            RechargePrepaid.Data = response.RechargeId.ToString();

        }
    }

    public class NetOneUSDRechargeHandler : NetOneRechargeHandler, IRechargeHandler
    {
        public NetOneUSDRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider)
            : base(dbcontext, serviceProvider)
        {
            base.BrandId = (int)Brands.NetoneUSD;
            base.Currency = Currency.USD;
        }

        public new int BrandId { get; set; } = (int)Brands.NetoneUSD; 
        public new Currency Currency { get; set; } = Currency.USD;
    }

}