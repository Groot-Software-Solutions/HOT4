namespace Hot.Application.Actions.RechargeActions
{
    public class ProcessRechargeBase
    {

        public static Recharge CreateRecharge(long AccessId, decimal Amount, string TargetMobile, int brandId, decimal discount)
        {
            return new Recharge()
            {
                AccessId = AccessId,
                Amount = Amount,
                BrandId = (byte)brandId,
                Discount = discount,
                RechargeDate = DateTime.Now,
                Mobile = TargetMobile,
                StateId = (byte)States.Busy
            };
        }

        public static RechargePrepaid CreateRechargePrepaid(Recharge recharge)
        {
            return new RechargePrepaid()
            {
                RechargeId = recharge.RechargeId,
                DebitCredit = false,
                ReturnCode = "-1",
                Narrative = "Pending",
                Reference = GetReference(recharge)
            };
        }

        public static decimal GetCost(decimal Amount, decimal discount)
        {
            if (discount == 0) return Amount;
            return Amount - (discount / 100 * Amount);
        }

        public static string GetReference(Recharge recharge)
        {
            return $"{recharge.BrandId}+{recharge.Mobile}+{DateTime.Now:MM-dd-yyyy hh:mm:ss.fff}";
        }

        public static decimal GetWalletBalance(Brand brand, Account account)
        {
            return account.WalletBalance((WalletTypes)brand.WalletTypeId);
        }

        public static async Task<OneOf<int, AppException>> GetBrandId(string Mobile, IDbContext _context, ILogger<object> logger)
        {
            int networkId = await GetNetwork(Mobile, _context, logger);
            var suffix = Mobile.Last().ToString();
            string brandSuffix = suffix.IsNumeric() ? "" : suffix;
            var brandResult = await _context.Brands.IndentifyAsync(networkId, brandSuffix);
            if (brandResult.IsT1) return brandResult.AsT1.ReturnDbException(logger);
            int brandId = brandResult.AsT0;
            return brandId;
        }

        public static async Task<int> GetNetwork(string Mobile, IDbContext _context, ILogger<object> logger)
        {
            var networkResult = await _context.Networks.IndentifyAsync(Mobile);
            if (networkResult.IsT1) throw CommandHelper.ReturnDbException(networkResult.AsT1, logger);
            int networkId = networkResult.AsT0;
            return networkId;
        }


        public static OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>
            ReturnRechargeSetupError(OneOf<Tuple<Recharge, RechargePrepaid,Account>, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, AppException> response)
        {
            if (response.IsT1) return response.AsT1;
            if (response.IsT2) return response.AsT2;
            if (response.IsT3) return response.AsT3;
            if (response.IsT4) return response.AsT4;
            return new AppException("Object is not a error but is T0", response.AsT0);
        }

        public static async Task<OneOf<Tuple<Recharge, RechargePrepaid, Account>, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, AppException>>
            SetupRecharge(long accessId, int brandId, decimal amount, string targetMobile, IDbContext dbContext, ILogger logger)
        {

            var accessResult = await dbContext.Accesss.GetAsync((int)accessId);
            if (accessResult.IsT1) return new AccountNotFoundException($"AccessId: {accessId}", accessResult.AsT1);
            var access = accessResult.AsT0;

            var accountResponse = await dbContext.Accounts.GetAsync((int)access.AccountId);
            if (accountResponse.IsT1) return new AccountNotFoundException($"AccountId: {access.AccountId}", accessResult.AsT1);
            var account = accountResponse.AsT0;

            var brandResponse = await dbContext.Brands.GetAsync(brandId);
            if (brandResponse.IsT1) return new NotAllowedToSellBrandException(brandId, account.AccountID, account.AccountName, null, brandResponse.AsT1, dbContext);
            var brand = brandResponse.AsT0;
            if (!string.IsNullOrEmpty(brand.BrandSuffix)) targetMobile = targetMobile.Replace(brand.BrandSuffix, "");

            var discountResult = await dbContext.ProfileDiscounts.DiscountAsync(accountResponse.AsT0.ProfileID, brandId);
            if (discountResult.IsT1) return new NotAllowedToSellBrandException(brandId, account.AccountID, account.AccountName, brand.BrandName, discountResult.AsT1, dbContext);
            var discount = discountResult.AsT0.Discount;

            decimal cost = GetCost(amount, discount);
            decimal walletBalance = GetWalletBalance(brand, account);
            decimal saleValue = GetSaleValue(discount, walletBalance);
            if (cost > walletBalance) return new InsufficientFundsException(walletBalance, cost, saleValue, targetMobile, dbContext);

            Recharge recharge = CreateRecharge(accessId, amount, targetMobile, brandId, discount);
            var rechargeResult = await dbContext.Recharges.AddAsync(recharge);
            if (rechargeResult.IsT1) return rechargeResult.AsT1.LogAndReturnError(logger);
            recharge.RechargeId = rechargeResult.AsT0;

            RechargePrepaid rechargePrepaid = CreateRechargePrepaid(recharge);
            var rechargePrepaidResult = await dbContext.RechargePrepaids.AddAsync(rechargePrepaid);
            if (rechargePrepaidResult.IsT1) return rechargePrepaidResult.AsT1.LogAndReturnError(logger);
            rechargePrepaid.RechargeId = recharge.RechargeId;

            return new Tuple<Recharge, RechargePrepaid, Account>(recharge, rechargePrepaid, account);
        }

        public static decimal GetSaleValue(decimal discount, decimal walletBalance)
        {
            if (discount == 0) return walletBalance;
            return walletBalance + (walletBalance * (discount / 100));
        }
    }
}