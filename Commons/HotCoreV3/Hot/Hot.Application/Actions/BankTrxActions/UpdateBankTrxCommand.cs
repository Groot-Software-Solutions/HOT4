namespace Hot.Application.Actions.BankTrxActions;
public record UpdateBankTrxCommand(BankTrx BankTrx) : IRequest<OneOf<bool, AppException>>;

        public class UpdateBankTrxCommandHandler : IRequestHandler<UpdateBankTrxCommand, OneOf<bool, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<UpdateBankTrxCommandHandler> logger;

            public UpdateBankTrxCommandHandler(IDbContext context,ILogger<UpdateBankTrxCommandHandler> logger)
            {
                _context = context;
                this.logger = logger;
            }
           
            public async Task<OneOf<bool, AppException>> Handle(UpdateBankTrxCommand request, CancellationToken cancellationToken)
            {
                var bankTrx = request.BankTrx;

                var result =  await _context.BankTrxs.UpdateAsync(bankTrx);
                if (result.ResultOrNull() == false) return result.AsT1.LogAndReturnError("BankTrx error", logger);
                var banktrxresult = result.AsT0;
                return banktrxresult;

            }
        }

