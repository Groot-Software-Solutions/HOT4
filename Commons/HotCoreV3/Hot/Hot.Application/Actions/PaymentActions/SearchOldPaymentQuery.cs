using AutoMapper;

namespace Hot.Application.Actions.PaymentActions
{
    public record SearchOldPaymentQuery(string Filter) : IRequest<OneOf<List<PaymentDetailedModel>, AppException>>;
    public class SearchOldPaymentQueryHandler : IRequestHandler<SearchOldPaymentQuery, OneOf<List<PaymentDetailedModel>, AppException>>
    {
        private readonly IDbContext context;
        private readonly ILogger<SearchOldPaymentQueryHandler> logger;
        private readonly IMapper mapper;

        public SearchOldPaymentQueryHandler(IDbContext context, ILogger<SearchOldPaymentQueryHandler> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<OneOf<List<PaymentDetailedModel>, AppException>> Handle(SearchOldPaymentQuery request, CancellationToken cancellationToken)
        {
            var response = await context.Payments.SearchAsync(request.Filter);
            if (response.IsT0)
            {
                return mapper.Map<List<PaymentDetailedModel>>(response.AsT0);
            }
            //var error = response.AsT1;
            //error.LogError(logger);
            //return new AppException($"{error.GetType().Name} Error", error);
            return response.AsT1.LogAndReturnError(logger);

        }
    }
}
