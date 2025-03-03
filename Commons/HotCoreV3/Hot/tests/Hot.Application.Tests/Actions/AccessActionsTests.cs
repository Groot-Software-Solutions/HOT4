using Hot.Application.Actions.AccessActions;
using Hot.Domain.Entities;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;

namespace Hot.Application.Tests.Actions
{
    public class AccessActionsTests
    {

        [Fact]
        public async Task ResetPin_ShouldReturnBoolean_Test()
        {

            //Arrange
            var logger = LoggerFaker.GetLogger<AccessResetPinCommandHandler>();
            var CurrentPin = "0000";
            var access = AccessDbFacker.GetSingle();
            access.AccessPassword = CurrentPin;
            var _dbcontext = DbContextFaker.Mock()
               .Gives().Access(access)
               .Gives().AccessPasswordChange(true);
            var request = new AccessResetPinCommand(12345);
            var sut = new AccessResetPinCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            await _dbcontext.Received().Accesss.PasswordChangeAsync(Arg.Is<Access>(a => a.AccessPassword != CurrentPin));
            result.AsT0.Should().NotBe(CurrentPin);
        }

        [Fact] 
        public async Task DisableAccess_ShouldReturnBoolean_Test()
        {

            //Arrange
            var access = AccessDbFacker.GetSingle();
            var logger = LoggerFaker.GetLogger<DeleteAccessCommandHandler>();
            var _dbcontext = DbContextFaker.Mock()
               .Gives().Access(access)
               .Gives().AccessRemove(true);
            var request = new DeleteAccessCommand(123456);
            var sut = new DeleteAccessCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert          
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(true);
            await _dbcontext.Received().Accesss.RemoveAsync(Arg.Is<int>(a => a == 123456));
        }
        [Fact]
        public async Task CreateAccess_ShouldReturnAccess_Test()
        {
            //Arrange 
            var access = AccessDbFacker.GetSingle();
            int accessID = (int)access.AccessId;
            var account = AccountDbFacker.GetSingle();
            var logger = LoggerFaker.GetLogger<AddAccessCommandHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().Access(access)
                .Gives().Account(account)
                .Gives().AccessAdd(access);
            var request = new AddAccessCommand("Norlin", 123652, "1235", 2, "Test");
            var sut = new AddAccessCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            await _dbcontext.Received().Accesss.AddAsync(Arg.Is<Access>(a => a.AccountId == 123652));
            await _dbcontext.Received().Accesss.AddAsync(Arg.Is<Access>(a => a.AccessPassword == "1235"));
            await _dbcontext.Received().Accesss.AddAsync(Arg.Is<Access>(a => a.ChannelID == 2));
        }
        [Fact]
        public async Task EnableAccess_ShouldReturnBoolean_Test()
        {
            //Arrange
            var access = AccessDbFacker.GetSingle();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().Access(access)
                .Gives().AccessUnDelete(true);
            var logger = LoggerFaker.GetLogger<EnableAccessCommandHandler>();
            var request = new EnableAccessCommand(123456);
            var sut = new EnableAccessCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(true);
            await _dbcontext.Received().Accesss.GetAsync(Arg.Any<int>());
            await _dbcontext.Received().Accesss.UnDeleteAsync(Arg.Is<long>(a => a == 123456));
        }
        [Fact]
        public async Task UpdateAccess_ShouldReturnAccess_Test()
        {
            //Arrange
            var accessWeb = AccessDbFacker.GetAccessWebSingle();
            var access = AccessDbFacker.GetSingle();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().Access(access)
                .Gives().AccessUpdate(true)
                .Gives().GetAccessWeb(accessWeb)
                .Gives().AccessWebUpdate(true);
            var logger = LoggerFaker.GetLogger<UpdateAccessCommandHandler>();
            var request = new UpdateAccessCommand(12345, "0774526348", 2, "Norlin");
            var sut = new UpdateAccessCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            //result.AsT0.Should().Be(access);
            result.IsT0.Should().BeTrue();
            result.AsT0.AccessCode.Should().Be(request.AccessCode);
            result.AsT0.ChannelID.Should().Be((byte)request.ChannelID);
            await _dbcontext.Accesss.UpdateAsync(Arg.Is<Access>(a => a.AccessCode == request.AccessCode));
            await _dbcontext.Accesss.UpdateAsync(Arg.Is<Access>(a => a.AccessId == request.AccessId));
        }

    }
}
