namespace Hot.Application.Actions.PaymentActions;
public record CreatePaymentCommand(Payment Payment) : IRequest<OneOf<Payment, AppException>>;

        public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, OneOf<Payment, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<CreatePaymentCommandHandler> logger;

            public CreatePaymentCommandHandler(IDbContext context, ILogger<CreatePaymentCommandHandler> logger)
            {
                _context = context;
                this.logger = logger;
            }
      
            public async Task<OneOf<Payment, AppException>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
            {
                var payment = request.Payment;
                payment.PaymentDate = DateTime.Now;
                var result =await _context.Payments.AddAsync(payment); 
                if (result.ResultOrNull() == -1) return result.AsT1.LogAndReturnError("Error Adding Payment", logger);
                payment.PaymentId = result.AsT0; 
                return payment; 
            }
        }     

