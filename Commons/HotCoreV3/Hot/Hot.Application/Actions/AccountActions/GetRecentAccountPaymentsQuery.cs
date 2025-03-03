namespace Hot.Application.Actions.AccountActions;
public record GetRecentAccountPaymentsQuery(int AccountId) : IRequest<OneOf<List<Payment>, AppException>>;
        public class GetRecentAccountPaymentsQueryHandler : IRequestHandler<GetRecentAccountPaymentsQuery, OneOf<List<Payment>, AppException>>
        {
            private IDbContext context;
            private readonly ILogger<GetRecentAccountPaymentsQueryHandler> logger;

            public GetRecentAccountPaymentsQueryHandler(IDbContext context, ILogger<GetRecentAccountPaymentsQueryHandler> logger)
            {
                this.context = context;
                this.logger = logger;
            }

            public async Task<OneOf<List<Payment>, AppException>> Handle(GetRecentAccountPaymentsQuery request, CancellationToken cancellationToken)
            {
                // Get
                var response = await context.Payments.ListRecentAsync(request.AccountId);
                if (response.IsT0)
                {
                    return response.AsT0;
                }
                return response.AsT1.LogAndReturnError(logger);
            }
        }
