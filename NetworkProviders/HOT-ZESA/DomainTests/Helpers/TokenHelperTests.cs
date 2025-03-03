using BillPayments.Domain.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DomainTests.Helpers
{
    public class TokenHelperTests
    {
        //private readonly string data = "42478434957254290413|3562.3|3562.3 kWh @ 0.116 $/kWh: : :|POWERT3EMDB4234687|41323|6198|15";
        private readonly string data = "42478434957254290413|3562.3||POWERT3EMDB4234687|41323|6198|0.15#53667968935921134723|655.5||00001230723210738154|17846973|2677046|0.15#10543351119402930789|655.5||00001230723210738154|17846973|2677046|0.15";
        [Fact]
        public void GetZesaReferenceTests()
        {

            var result = TokenHelper.GetZESAReference(data);
            result.Should().Be("POWERT3EMDB4234687");
        }
        [Fact]
        public void GetTokenTests()
        {

            var result = TokenHelper.GetToken(data);
            result.Should().Be("42478434957254290413");

        }
        [Fact]
        public void GetUnitsTests()
        {
            var result = TokenHelper.GetUnits(data);
            result.Should().Be((decimal)3562.3);

        }
        [Fact]
        public void GetTaxAmountTests()
        {
            var result = TokenHelper.GetTaxAmount(data);
            result.Should().Be((decimal)61.98);
        }
        [Fact]
        public void GetNetAmountTests()
        {
            var result = TokenHelper.GetNetAmount(data);
            result.Should().Be((decimal)413.23);
        }
        [Fact]
        public void GetLevyAmountTests()
        {
            var data = "RE Levy (6%)|12345678|600|00|0.06";
            var result = TokenHelper.GetLevy(data);
            result.Should().Be((decimal)6.00);
        }
        [Fact]
        public void GetAreasAmountTests()
        {
            var data = "Debt Recovery |A00039115|9000|0|18000";
            var result = TokenHelper.GetArrears(data);
            result.Should().Be((decimal)90.00);

            data = "Fraud Charge|POWERT3EMDB3222699|500|0|81755#General Debt|POWERT3EMDB3222699|500|0|101155";
            result = TokenHelper.GetArrears(data);
            result.Should().Be((decimal)10.00);
        }
        [Fact]
        public void IsDoubleDebtTests()
        {
            var data = "Fraud Charge|POWERT3EMDB3222699|500|0|81755#General Debt|POWERT3EMDB3222699|500|0|101155";
            var result = TokenHelper.IsDoubleDebt(data);
            result.Should().Be(true);
            data = "Debt Recovery |A00039115|9000|0|18000";
            result = TokenHelper.IsDoubleDebt(data);
            result.Should().Be(false);

        }
        [Fact]
        public void IsDoubleTokenTests()
        {
            // var data = "03039926611110380309|283.1|283.1 kWh @ 0.1 $/kWh: : :|POWERT3EMDB803197|2831|0|0%#08987731577319780478|1596.0|283.1 kWh @ 0.1 $/kWh: : :|null|0|0|0%";
            var result = TokenHelper.IsDoubleToken(data);
            result.Should().Be(true);
            var data2 = "42478434957254290413|3562.3|3562.3 kWh @ 0.116 $/kWh: : :|POWERT3EMDB4234687|41323|6198|15";
            var result2 = TokenHelper.IsDoubleToken(data2);
            result2.Should().Be(false);

        }
        [Fact]
        public void GetTokensFromDoubleTokenTests()
        {
            var data = "42478434957254290413|3562.3||POWERT3EMDB4234687|41323|6198|0.15#53667968935921134723|655.5||00001230723210738154|17846973|2677046|0.15#10543351119402930789|655.5||00001230723210738154|17846973|2677046|0.15";
            var result = TokenHelper.GetToken(data);
            result.Should().Be("42478434957254290413");
            var data2 = data.Split("#")[1];
            var result2 = TokenHelper.GetToken(data2);
            result2.Should().Be("53667968935921134723");
            var data3 = data.Split("#")[2];
            var result3 = TokenHelper.GetToken(data3);
            result3.Should().Be("10543351119402930789");

        }
        [Fact]
        public void IsValidDataTests()
        {
            var data = "42478434957254290413|3562.3|3562.3 kWh @ 0.116 $/kWh: : :|POWERT3EMDB4234687|41323|6198|15";
            var result = TokenHelper.IsValidInput(data);
            result.Should().Be(true);
            result = TokenHelper.IsValidInput(data, 2);
            result.Should().Be(false);
            result = TokenHelper.IsValidInput(data, 2, Numeric: false);
            result.Should().Be(true);
            result = TokenHelper.IsValidInput("");
            result.Should().Be(false);
            result = TokenHelper.IsValidInput(null);
            result.Should().Be(false);

        }
        [Fact]
        public void FormattedTokenTests()
        {
            var data = "42478434957254290413";
            var result = TokenHelper.FormattedToken(data);
            result.Should().Be("4247 8434 9572 5429 0413");
            result = TokenHelper.GetFormattedToken(data);
            result.Should().Be("4247 8434 9572 5429 0413");


        }


        [Fact]
        public void GetTokensFromResponseTests()
        {
            var data = "42478434957254290413|3562.3||POWERT3EMDB4234687|41323|6198|0.15#53667968935921134723|655.5||00001230723210738154|17846973|2677046|0.15#10543351119402930789|655.5||00001230723210738154|17846973|2677046|0.15";
            var tokens = PurchaseTokenHelper.GetTokens(new BillPayments.Domain.Models.PurchaseTokenResponse() { Token = data });
            tokens.Count.Should().Be(3);

        }
    }
}
