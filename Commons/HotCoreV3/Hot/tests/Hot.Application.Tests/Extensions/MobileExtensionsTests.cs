using Hot.Application.Common.Extensions;

namespace Hot.Application.Tests.Extensions;
public class MobileExtensionsTests
    {

        [Theory]
        [InlineData("0772000000","0772000000")]
        [InlineData("772000000", "0772000000")]
        [InlineData("263772000000", "0772000000")]
        [InlineData("+263772000000", "0772000000")]
        public void ToMobileNumber_ShouldReturnMobileTest(string number, string expected)
        {
            var result = number.ToMobile();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("0772000000", "263772000000")]
        [InlineData("772000000", "263772000000")]
        [InlineData("263772000000", "263772000000")]
        [InlineData("+263772000000", "263772000000")]
        public void ToMSISDN_ShouldReturnMSISDNTest(string number, string expected)
        {
            var result = number.ToMSISDN();
            result.Should().Be(expected);
        }

    }
