namespace Hot.Application.Actions.PaymentActions;
public record GetPaymentSourcesQuery : IRequest<OneOf<List<PaymentSource>, AppException>>;         
        public class GetPaymentSourcesQueryHandler : IRequestHandler<GetPaymentSourcesQuery, OneOf<List<PaymentSource>, AppException>>
        {
            private readonly IDbContext context;
            private readonly ILogger<GetPaymentSourcesQueryHandler> logger;

            public GetPaymentSourcesQueryHandler(IDbContext context, ILogger<GetPaymentSourcesQueryHandler> logger)
            {
                this.context = context;
                this.logger = logger;
            }

            public async Task<OneOf<List<PaymentSource>, AppException>> Handle(GetPaymentSourcesQuery request, CancellationToken cancellationToken)
            {
                var response = await context.PaymentSources.ListAsync();
                if (response.IsT0)
                {
                    return response.AsT0;
                }
                var error = response.AsT1;
                error.LogError(logger);
                return new AppException($"{error.GetType().Name} Error", error);
            }
        }

