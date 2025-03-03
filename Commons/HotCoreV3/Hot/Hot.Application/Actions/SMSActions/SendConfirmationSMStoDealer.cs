namespace Hot.Application.Actions.SMSActions;

public record SendConfirmationSMStoDealerCommand(decimal Balance, decimal SaleValue, decimal Amount,
    decimal Discount, string DealerMobile, string TargetMobile)
: IRequest<OneOf<bool, AppException>>;
public class SendConfirmationSMStoDealerCommandHandler : IRequestHandler<SendConfirmationSMStoDealerCommand, OneOf<bool, AppException>>
{
    private readonly IDbContext dbContext;
    private readonly ILogger<SendConfirmationSMStoDealerCommandHandler> logger;

    public SendConfirmationSMStoDealerCommandHandler(IDbContext dbContext, ILogger<SendConfirmationSMStoDealerCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<bool, AppException>> Handle(SendConfirmationSMStoDealerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            decimal balance = request.Balance;
            decimal saleValue = request.SaleValue;
            string cost = (request.Amount * (1 - (request.Discount / 100))).ToString("N4");
            var response = await dbContext.Templates.GetAsync((int)Templates.SuccessfulRecharge_Dealer_Header);
            if (response.IsT1) response.AsT1.LogAndReturnError(logger);
            Template reply =response.AsT0;

            reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.TargetMobile);
            reply.TemplateText = reply.TemplateText.Replace("%AMOUNT%", request.Amount.ToString("N2"));
            reply.TemplateText = reply.TemplateText.Replace("%DISCOUNT%", request.Discount.ToString("N2"));
            reply.TemplateText = reply.TemplateText.Replace("%COST%", cost);
            reply.TemplateText = reply.TemplateText.Replace("%BALANCE%", balance.ToString("N2"));
            reply.TemplateText = reply.TemplateText.Replace("%SALEVALUE%", saleValue.ToString("N2"));

            SMS sms = new()
            {
                Direction = false,
                Mobile = request.DealerMobile.ToMobile(),
                Priority = new Priority() { PriorityId = (byte)Priorities.Normal },
                State = new State() { StateID = (byte)States.Pending },
                SMSID_In = null,
                SMSText = reply.TemplateText,
            };
            var result = await dbContext.SMSs.AddAsync(sms);
            if (result.IsT0) return true;
            return new AppException("Error Sending Confirmation", result.AsT1.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new AppException("Error Sending Confirmation", ex.Message);

        }

    }

}

