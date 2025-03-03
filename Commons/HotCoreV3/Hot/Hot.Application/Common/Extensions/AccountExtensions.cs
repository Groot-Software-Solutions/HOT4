namespace Hot.Application.Common.Extensions
{
    public static class AccountExtensions
    {
        public static decimal WalletBalance(this Account account, WalletTypes wallet)
        {
            return wallet switch
            {
                WalletTypes.ZESA => account.ZesaBalance,
                WalletTypes.USD => account.USDBalance,
                WalletTypes.USDUtility => account.USDUtilityBalance,
                _ => account.Balance
            };
        }
    }
}
