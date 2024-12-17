using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class createviewaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW [dbo].[vwBalances]
AS
SELECT        a.AccountID, SUM(a.amountAirtime) AS Balance, SUM(a.amountZesa) AS ZESABalance, SUM(a.AmountUSD) AS USDBalance, SUM(a.AmountUSDUtility) AS USDUtilityBalance, SUM(a.amountAirtime) / (1 - d.Discount / 100) 
                         AS SaleValue
FROM            (SELECT        AccountID, SUM(CASE WHEN NOT PaymentTypeID IN (17, 16, 18, 19) THEN Amount ELSE 0 END) AS amountAirtime, SUM(CASE WHEN PaymentTypeID IN (16, 18) THEN Amount ELSE 0 END) AS amountZesa, 
                                                    SUM(CASE WHEN PaymentTypeID = 17 THEN Amount ELSE 0 END) AS AmountUSD, SUM(CASE WHEN PaymentTypeID = 19 THEN Amount ELSE 0 END) AS AmountUSDUtility
                          FROM            (SELECT        AccountID, Amount, PaymentTypeID
                                                    FROM            dbo.tblPayment AS nolock) AS a_2
                          GROUP BY AccountID
                          UNION
                          SELECT        a_1.AccountID, - SUM((CASE WHEN b_1.WalletTypeId = 1 THEN amount ELSE 0 END) * ((100 - r.Discount) / 100)) AS AmountAirtime, - SUM((CASE WHEN b_1.WalletTypeId = 2 THEN amount ELSE 0 END) 
                                                   * ((100 - r.Discount) / 100)) AS AmountZesa, - SUM((CASE WHEN b_1.WalletTypeId = 3 THEN amount ELSE 0 END) * ((100 - r.Discount) / 100)) AS AmountUSD, 
                                                   - SUM((CASE WHEN b_1.WalletTypeId = 4 THEN amount ELSE 0 END) * ((100 - r.Discount) / 100)) AS AmountUSDUtility
                          FROM            (SELECT        AccessID, Amount, Discount, StateID, BrandID
                                                    FROM            dbo.tblRecharge AS nolock) AS r INNER JOIN
                                                       (SELECT        AccessID, AccountID
                                                         FROM            dbo.tblAccess AS nolock) AS a_1 ON a_1.AccessID = r.AccessID INNER JOIN
                                                       (SELECT        BrandID, WalletTypeId
                                                         FROM            dbo.tblBrand) AS b_1 ON b_1.BrandID = r.BrandID
                          WHERE        (r.StateID IN (1, 2, 4))
                          GROUP BY a_1.AccountID) AS a INNER JOIN
                             (SELECT        y.AccountID, z.discount AS Discount
                               FROM            dbo.tblAccount AS y INNER JOIN
                                                             (SELECT        ProfileID, AVG(Discount) AS discount
                                                               FROM            dbo.tblProfileDiscount
                                                               GROUP BY ProfileID) AS z ON z.ProfileID = y.ProfileID) AS d ON a.AccountID = d.AccountID
GROUP BY a.AccountID, d.Discount
GO");

            migrationBuilder.Sql(@"CREATE VIEW [dbo].[vwAccount]
AS
SELECT        a.AccountID, p.ProfileID, p.ProfileName, a.AccountName, a.NationalID, a.Email, a.ReferredBy, ISNULL(b.Balance, 0) AS Balance, ISNULL(b.SaleValue, 0) AS SaleValue, ISNULL(b.ZESABalance, 0) AS ZESABalance, 
                         ISNULL(b.USDBalance, 0) AS USDBalance, ISNULL(b.USDUtilityBalance, 0) AS USDUtilityBalance
FROM            dbo.tblAccount AS a WITH (nolock) INNER JOIN
                         dbo.tblProfile AS p WITH (nolock) ON a.ProfileID = p.ProfileID LEFT OUTER JOIN
                         dbo.vwBalances AS b WITH (nolock) ON b.AccountID = a.AccountID
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP view if exists [dbo].[vwBalances]");
            migrationBuilder.Sql("DROP view if exists [dbo].[vwAccount]");
        }
    }
}
