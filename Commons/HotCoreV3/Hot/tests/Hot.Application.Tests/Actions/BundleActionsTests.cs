using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Application.Actions.BundleActions;

namespace Hot.Application.Tests.Actions
{
    public class BundleActionsTests
    {

        [Fact]
        public async Task SearchBundles_ShouldReturnList_Test()
        {
            //Arrange
            var logger = LoggerFaker.GetLogger<GetAllBudlesHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomBundlesList(10);
            var request = new GetAllBundles();
            var sut = new GetAllBudlesHandler(_dbcontext, logger);



            //Act
            var result = await sut.Handle(request, new CancellationToken());



            //Assert
            result.IsT0.Should().Be(true);
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(10);
            await _dbcontext.Received().Bundles.ListAsync();

        }

    }
}
