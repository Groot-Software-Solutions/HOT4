using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using Hot.Domain.Enums;

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers.Vouchers
{
    public class PinRechargeHandler : BaseRechargeHandler, IRechargeHandler
    {
        public PinRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
        {
        }
        public RechargeType Rechargetype { get; set; } = RechargeType.Pin;
        public int BrandId { get; set; } = 0;

        public async Task<OneOf<RechargeResult, RechargeServiceNotFoundException>> ProcessAsync(Recharge recharge, RechargePrepaid rechargePrepaid)
        {
            await SetRecharge(recharge, rechargePrepaid);
            await ApplyDiscountRules();
            var service = GetServiceFromProvider<IDbContext>();
            if (service is null) return await UpdateAndReturnServiceNotFound("Pin Service");
            var response = await service.Pins.RechargeAsync(recharge.Amount, recharge.BrandId, recharge.RechargeId);
            SetResultFromResponse(response);
            await UpdateRechargeStatusAsync();
            var result = await GetRechargeResultFromResponseAsync(response);
            return result;
        }

        private async Task<RechargeResult> GetRechargeResultFromResponseAsync(OneOf<List<Pin>, HotDbException> response)
        {

            return new RechargeResult()
            {
                Recharge = Recharge,
                RechargePrepaid = RechargePrepaid,
                ErrorData = IsTransactionSuccessful(response) ? null : GetPinError(response),
                Message = IsTransactionSuccessful(response) ? SuccessfulMessage(response) : GetPinError(response),
                Successful = IsTransactionSuccessful(response),
                Data = IsTransactionSuccessful(response) ? response.AsT0 : null,
                WalletBalance = await GetWalletBalanceAsync(),
            };
        }

        private void SetResultFromResponse(OneOf<List<Pin>, HotDbException> response)
        {
            Recharge.StateId = (byte)(IsTransactionSuccessful(response) ? 2 : 3);
            Recharge.RechargeDate = DateTime.Now;
            RechargePrepaid.Narrative = IsTransactionSuccessful(response)
                ? SuccessfulMessage(response)
                : GetPinError(response);
            RechargePrepaid.ReturnCode = IsTransactionSuccessful(response) ? "1" : "0";

        }

        private static string SuccessfulMessage(OneOf<List<Pin>, HotDbException> response)
        {
            return $"{response.AsT0.Count} Pins Successfully retrieved";
        }

        private static string GetPinError(OneOf<List<Pin>, HotDbException> response)
        {
            if (response.IsT1) return response.AsT1.Message;
            if (response.AsT0.Count == 0) return "No Pins available to pins in stock";
            return "Error Getting the Pins";
        }

        private static bool IsTransactionSuccessful(OneOf<List<Pin>, HotDbException> response)
        {
            if (response.IsT1) return false;
            if (response.AsT0.Count == 0) return false;
            return true;
        }
    }
}
