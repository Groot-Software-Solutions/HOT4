using AutoMapper;
using Hot.Application.Actions.PinBatchActions;
using Hot.Application.Common.Models;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Tests.Common.Fakers.Services;

namespace Hot.Application.Tests.Actions
{
    public class PinBatchActionsTests
    {
        [Fact]
        public async Task GetPinBatches_ShouldReturnList_Test()
        {
            //Arrange
            var _mapper = new MapperConfiguration( opt => opt.AddProfile(new PinBatchDetailedModelProfile()) ).CreateMapper();
            var _dbcontext = DbContextFaker.Mock()
               .Gives().RandomPinBatchesList(10)
               .Gives().RandomPinBatcheTypesList();
            var logger = LoggerFaker.GetLogger<GetPinBatchesQueryHandler>();
            var request = new GetPinBatchesQuery(2);
            var sut = new GetPinBatchesQueryHandler(_dbcontext, logger, _mapper);

            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();  
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Count.Should().Be(10);
            await _dbcontext.Received().PinBatches.ListAsync(Arg.Is<int>(a => a == request.PinBatchTypeId));
        }
        [Fact]
        public async Task GetPinBatchesTypes_ShouldReturnList_Test()
        {
            //Arrange
            var logger = LoggerFaker.GetLogger<GetPinBatchTypesQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomPinBatcheTypesList(10);
            var request = new GetPinBatchTypesQuery();
            var sut = new GetPinBatchTypesQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Count.Should().Be(10);
            await _dbcontext.Received().PinBatchTypes.ListAsync();
        }
      
    }
}
