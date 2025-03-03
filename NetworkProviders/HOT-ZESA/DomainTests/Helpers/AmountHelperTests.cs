using BillPayments.Domain.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DomainTests.Helpers
{
    public class AmountHelperTests
    {


        [Fact]
        public void ActualAmountTests()
        {
            var data = "41323";
            var result = AmountHelper.ActualAmount(data);
            result.Should().Be((decimal)413.23);

            var data1 = 41323;
            var result1 = AmountHelper.ActualAmount(data1);
            result1.Should().Be((decimal)413.23);

        }

        [Fact]
        public void ActualAmountNullTests()
        {
            var result = AmountHelper.ActualAmount(null);
            result.Should().Be((int)0);
        }

        [Fact]
        public void BillPaymentAmountTests()
        {
            var Amount = 100;
            var result = AmountHelper.BillPaymentAmount(Amount);
            result.Should().Be((int)10000);
        }

    }
}
