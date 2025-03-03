using Hot.Application.Common.Extensions;

namespace Hot.Application.Tests.Extensions;

public class IDNumberExtensionsTests
{
    [Theory]
    [InlineData("00-000000Z00", true)]
    [InlineData("00-0000000Z00", true)]
    [InlineData("00 000000 Z 00", true)]
    [InlineData("00-000000 Z00", true)]
    [InlineData(" 00-000000 Z00 ", true)]
    [InlineData("00-000000 Z 200", false)]
    public void IDNumber_ValidatesSuccessfully(string data, bool expected)
    {
        var result = data.IsValidIDNumber();
        result.Should().Be(expected);
    }

}
