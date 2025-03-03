
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.Telecel;
using Hot.Domain.Entities;
using Hot.Domain.Enums;

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers.Airtime
{
    public class TelecelRechargeHandler : BaseRechargeHandler, IRechargeHandler
    {
        public TelecelRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
        {
        }
        public RechargeType Rechargetype { get; set; } = RechargeType.Airtime;
        public int BrandId { get; set; } = (int)Brands.Juice;
        public Currency Currency { get; set; } = Currency.ZWG;
        public async Task<OneOf<RechargeResult, RechargeServiceNotFoundException>> ProcessAsync(Recharge recharge, RechargePrepaid rechargePrepaid)
        {
            await SetRecharge(recharge, rechargePrepaid);
            await ApplyDiscountRules();
            var service = GetServiceFromProvider<ITelecelRechargeAPIService>();
            if (service is null) return await UpdateAndReturnServiceNotFound("Telecel Service");
            var response = await service.Recharge(recharge.Mobile, recharge.Amount, rechargePrepaid.Reference, Currency);
            SetResultFromResponse(response);
            await UpdateRechargeStatusAsync();
            var result = await GetRechargeResultFromResponseAsync(response);
            return result;
        }

        private async Task<RechargeResult> GetRechargeResultFromResponseAsync(TelecelRechargeResult response)
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

        private void SetResultFromResponse(TelecelRechargeResult response)
        {
            Recharge.StateId = (byte)(response.Successful ? 2 : 3);
            Recharge.RechargeDate = DateTime.Now;
            RechargePrepaid.ReturnCode = response.ReturnCode;
            RechargePrepaid.InitialBalance = response.InitialBalance;
            RechargePrepaid.FinalBalance = response.FinalBalance;
            RechargePrepaid.InitialWallet = response.InitialWallet;
            RechargePrepaid.FinalWallet = response.FinalWallet;
            RechargePrepaid.Narrative = $"{response.Narrative} - {response.RawResponseData}";
            RechargePrepaid.SMS = response.RawResponseData;
        }
    }

    public class TelecelUSDRechargeHandler : TelecelRechargeHandler, IRechargeHandler
    {
        public TelecelUSDRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
        {
            Currency = Currency.USD;
        }
        public new int BrandId { get; set; } = (int)Brands.TelecelUSD;

    }
}
