using AutoMapper;

namespace Hot.Application.Actions.PaymentActions;
public record ListPaymentForAccountQuery(long AccountId) : IRequest<OneOf<List<PaymentDetailedModel>, AppException>>;
public class ListPaymentForAccountQueryHandler
    : IRequestHandler<ListPaymentForAccountQuery, OneOf<List<PaymentDetailedModel>, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<ListPaymentForAccountQueryHandler> logger;
    private readonly IMapper mapper;

    public ListPaymentForAccountQueryHandler(IDbContext context, ILogger<ListPaymentForAccountQueryHandler> logger, IMapper mapper)
    {
        _context = context;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<OneOf<List<PaymentDetailedModel>, AppException>> Handle(ListPaymentForAccountQuery request, CancellationToken cancellationToken)
    {
        List<PaymentSource> paymentSources = new();
        List<PaymentType> paymentTypes = new();
        List<Payment> payments = new();

        var paymentSourcesResponse = await _context.PaymentSources.ListAsync();
        if (paymentSourcesResponse.ResultOrNull() == null) return paymentSourcesResponse.AsT1.LogAndReturnError("Payment Sources Error", logger);
        paymentSources = paymentSourcesResponse.AsT0;

        var paymentTypesResponse = await _context.PaymentTypes.ListAsync();
        if (paymentSourcesResponse.ResultOrNull() == null) return paymentTypesResponse.AsT1.LogAndReturnError("Payment Types Error", logger);
        paymentTypes = paymentTypesResponse.AsT0;

        var paymentsResponse = await _context.Payments.ListAsync((int)request.AccountId);
        if (paymentsResponse.ResultOrNull() == null) return paymentsResponse.AsT1.LogAndReturnError("Payments Listing Error", logger);
        payments = paymentsResponse.AsT0;

        var list = payments.Select(p =>
       {
           var payment = mapper.Map<PaymentDetailedModel>(p);
           payment.PaymentSource = payment.GetPaymentSource(paymentSources);
           payment.PaymentType = payment.GetPaymentType(paymentTypes);
           return payment;

       }).ToList();

        return list;
    }
}


