using Hot.Application.Actions.ProfileActions;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;

namespace Hot.Application.Tests.Actions
{
    public class ProfileActionsTests
    {
        [Fact]
        public async Task GetProfiles_ShouldReturnList_Test()
        {
            //Arrange
            var logger = LoggerFaker.GetLogger<GetProfilesQueryHander>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomProfilesList(10);
            var request = new GetProfilesQuery();
            var sut = new GetProfilesQueryHander(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(10);
            await _dbcontext.Received().Profiles.ListAsync();
        }
    }
}
