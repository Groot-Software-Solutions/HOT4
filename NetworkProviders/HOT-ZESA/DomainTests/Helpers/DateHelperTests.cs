using BillPayments.Domain.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;


namespace DomainTests.Helpers
{
    public class DateHelperTests
    {

        [Fact]
        public void BillPaymentFormatTests()
        {
            // Redundant check but here just in case method is changed
            var data = DateTime.Now;
            var result = DateHelper.BillPaymentFormat(data);
            result.Should().Be(data.ToString("MMddyyHHmmss"));   
        }

        [Fact]
        public void ParseBillPaymentFormatTests()
        {
            var data = "120920155022";
            var result = DateHelper.ParseBillPaymentFormat(data);
            result.Should()
                .HaveDay(9)
                .And.HaveMonth(12)
                .And.HaveYear(2020)
                .And.HaveHour(15)
                .And.HaveMinute(50)
                .And.HaveSecond(22);

        }
    }
}
