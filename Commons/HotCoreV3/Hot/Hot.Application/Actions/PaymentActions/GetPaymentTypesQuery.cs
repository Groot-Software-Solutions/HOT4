namespace Hot.Application.Actions.PaymentActions;
public record GetPaymentTypesQuery : IRequest<OneOf<List<PaymentType>, AppException>>;
        public class GetPaymentTypesQueryHandler : IRequestHandler<GetPaymentTypesQuery, OneOf<List<PaymentType>, AppException>>
        {
            private IDbContext context;
            private readonly ILogger<GetPaymentTypesQueryHandler> logger;

            public GetPaymentTypesQueryHandler(IDbContext context, ILogger<GetPaymentTypesQueryHandler> logger)
            { 
                this.context = context;
                this.logger = logger;
            }

            public async Task<OneOf<List<PaymentType>, AppException>> Handle(GetPaymentTypesQuery request, CancellationToken cancellationToken)
            {
                var response = await context.PaymentTypes.ListAsync();
                if (response.IsT0)
                {
                    return response.AsT0;
                }
                return response.AsT1.LogAndReturnError(logger);
            }
        }

