using AutoMapper;
using Hot.Application.Actions.PaymentActions;
using Hot.Application.Common.Models;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Domain.Entities;
using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;

namespace Hot.Application.Tests.Actions
{
    public class PaymentActionsTests
    {
        private readonly IMapper mapper =
        Substitute.For<IMapper>();

        [Fact]
        public async Task CreatePayment_ShouldReturnPayment_Test()
        {
            //Arrange
            var payment = PaymentDbFacker.GetSingle();
            var logger = LoggerFaker.GetLogger<CreatePaymentCommandHandler>();
            var _dbcontext = DbContextFaker.Mock()
               .Gives().PaymentAdd(12);
            var request = new CreatePaymentCommand(payment);
            var sut = new CreatePaymentCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(payment);
            result.AsT0.PaymentId.Should().Be(12);
            await _dbcontext.Received().Payments.AddAsync(Arg.Is<Payment>(a => a == request.Payment));
        }
        [Fact]
        public async Task GetPaymentSources_ShouldReturnList_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomPaymentSourcesList(10);
            var logger = LoggerFaker.GetLogger<GetPaymentSourcesQueryHandler>();
            var request = new GetPaymentSourcesQuery();
            var sut = new GetPaymentSourcesQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Count.Should().Be(10);
            await _dbcontext.Received().PaymentSources.ListAsync();
        }
        [Fact]
        public async Task GetPaymentTypes()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomPaymentTypesList(12);
            var logger = LoggerFaker.GetLogger<GetPaymentTypesQueryHandler>();
            var request = new GetPaymentTypesQuery();
            var sut = new GetPaymentTypesQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Count.Should().Be(12);
            await _dbcontext.Received().PaymentTypes.ListAsync();
        }
        [Fact]
        public async Task ListPaymentForAccount_ShouldReturnList_Test()
        {
            //Arrange
            var paymentsources = PaymentSourceDbFacker.GetPaymentSourcesList(10);
            var paymentTypes = PaymentTypeDbFacker.GetList(10);
            var payments = PaymentDbFacker.GetPaymentsList(10);
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomPaymentTypesList(12)
                .Gives().RandomPaymentSourcesList(10)
                .Gives().RandomPaymentsByAccountIdList(10);
            var _mapper = new MapperConfiguration(
                 opt => opt.AddProfile(new PaymentDetailModelProfile())
                 ).CreateMapper();
            var logger = LoggerFaker.GetLogger<ListPaymentForAccountQueryHandler>();
            var request = new ListPaymentForAccountQuery(1232456);
            var sut = new ListPaymentForAccountQueryHandler(_dbcontext, logger, _mapper);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Count.Should().Be(10);
            await _dbcontext.Received().Payments.ListAsync(Arg.Is<int>(a => a == request.AccountId));
        }



        [Fact]
        public async Task SearchPayments_ShouldReturnList_Test()
        {
            //Arrange
            var _mapper = new MapperConfiguration(
         opt => opt.AddProfile(new PaymentDetailModelProfile())
         ).CreateMapper();
            var logger = LoggerFaker.GetLogger<SearchPaymentsQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
             .Gives().PaymentsList(5);
            var request = new SearchPaymentsQuery("testfilter");
            var sut = new SearchPaymentsQueryHandler(_dbcontext, logger, _mapper);

            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(5);
            await _dbcontext.Received().Payments.ListAsync(Arg.Any<int>());
        }
        
        [Fact]
        public async Task SearchOldPayments_ShouldReturnList_Test()
        {
            //Arrange
            var _mapper = new MapperConfiguration(
         opt => opt.AddProfile(new PaymentDetailModelProfile())
         ).CreateMapper();
            var logger = LoggerFaker.GetLogger<SearchOldPaymentQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
             .Gives().PaymentsList(7);
            var request = new SearchOldPaymentQuery("test filter");
            var sut = new SearchOldPaymentQueryHandler(_dbcontext, logger, _mapper);

            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(7);
            await _dbcontext.Received().Payments.ListAsync(Arg.Any<int>());
        }


   
    }
}
