using AutoMapper;

namespace Hot.Application.Actions.PaymentActions
{
    public record SearchPaymentsQuery(string Filter): IRequest<OneOf<List<PaymentDetailedModel>, AppException>>;
    public class SearchPaymentsQueryHandler : IRequestHandler<SearchPaymentsQuery, OneOf<List<PaymentDetailedModel>, AppException>>
    {
        private readonly IDbContext context;
        private readonly ILogger<SearchPaymentsQueryHandler> logger;
        private readonly IMapper mapper;

        public SearchPaymentsQueryHandler(IDbContext context, ILogger<SearchPaymentsQueryHandler> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<OneOf<List<PaymentDetailedModel>, AppException>> Handle(SearchPaymentsQuery request, CancellationToken cancellationToken)
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
