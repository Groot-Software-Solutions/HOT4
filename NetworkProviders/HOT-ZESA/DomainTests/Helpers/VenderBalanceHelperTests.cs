using BillPayments.Domain.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DomainTests.Helpers
{
    public class VenderBalanceHelperTests
    {
        [Fact]
        public void GetBalanceAmountTests()
        {
            var data = "Muk Plc|0119257147506|ZWL|361260";
            var result = VendorBalanceHelper.GetBalanceAmount(data);
            result.Should().Be((decimal)3612.60);

        }
    }
}
