namespace Hot.Application.Actions.ReportsActions
{

    public record GetPaymentsQuery(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? AccountId, int? PaymentTypeId, int? BankId) : IRequest<OneOf<List<PaymentResult>, AppException>>;
    public class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, OneOf<List<PaymentResult>, AppException>>
    {
        private readonly IDbContext context;
        private readonly ILogger<GetPaymentsQueryHandler> logger;

        public GetPaymentsQueryHandler(IDbContext context, ILogger<GetPaymentsQueryHandler> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<OneOf<List<PaymentResult>, AppException>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            var response = await context.Report.GetPaymentsAsync(request.StartDate, request.EndDate, request.ReportTypeId, request.AccountId, request.PaymentTypeId, request.BankId);
            if (response.IsT1) return response.AsT1.LogAndReturnError(logger);
            var paymentsList = response.AsT0;

            return paymentsList;
        }

    }
}
