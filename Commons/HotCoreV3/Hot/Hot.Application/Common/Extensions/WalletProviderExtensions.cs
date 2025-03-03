namespace Hot.Application.Common.Extensions;

public static class WalletProviderExtensions
{
    public static int GetBankTrxTypeId(this WalletProvider walletProvider)
    {
        return walletProvider switch
        {
            WalletProvider.Ecocash => 17,
            WalletProvider.OneMoney => 19,
            _ => 0
        };
    }

    public static int GetBankId(this WalletProvider walletProvider, Currency currency)
    {
        return walletProvider switch
        {
            WalletProvider.Ecocash => 6,
            WalletProvider.OneMoney => currency switch { Currency.USD => 16, _ => 8 },
            _ => 0
        };
    }

    public static WalletProvider? GetWalletProvider(this string Mobile)
    {
        if (Mobile.StartsWith("071")) return WalletProvider.OneMoney;
        if (Mobile.StartsWith("073")) return WalletProvider.Telecash;
        if (Mobile.StartsWith("078")) return WalletProvider.Ecocash;
        if (Mobile.StartsWith("077")) return WalletProvider.Ecocash;
        return null;
    }

    public static int GetBankTrxTypeId(this string BillerMobile)
    {
        var walletProvider = BillerMobile.GetWalletProvider() ?? throw new InvalidWalletProviderNumberException(BillerMobile, "WalletProvider");
        return walletProvider.GetBankTrxTypeId();
    }
    public static int GetBankId(this string BillerMobile, Currency currency)
    {
        var walletProvider = BillerMobile.GetWalletProvider() ?? throw new InvalidWalletProviderNumberException(BillerMobile, "WalletProvider");
        return walletProvider.GetBankId(currency);
    }
}
