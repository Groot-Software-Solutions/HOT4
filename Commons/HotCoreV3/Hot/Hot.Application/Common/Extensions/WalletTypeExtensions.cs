using Hot.Domain.Entities;

namespace Hot.Application.Common.Extensions;

public static class WalletTypeExtensions
{
    public static Currency GetCurrency(this WalletTypes wallet)
    {
        return wallet switch
        {
            WalletTypes.USD => Currency.USD,
            WalletTypes.USDUtility => Currency.USD,
            _ => Currency.ZWG
        };
    }

    public static string GetWalletName(this WalletTypes wallet)
    {
        return wallet switch
        {
            WalletTypes.ZWG => "ZWG",
            WalletTypes.USD => "USD",
            WalletTypes.ZESA => "Utility ZWG",
            WalletTypes.USDUtility => "Utility USD",
            _ => "Unknown"
        };
    }
}