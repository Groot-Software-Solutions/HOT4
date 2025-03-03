namespace Hot.Application.Actions.SMSActions;

public record SendConfirmationSMStoCustomerDataCommand(string TargetMobile, string BundleName, decimal Amount,
     string? DealerName = null, string? CustomSMS = null)
: IRequest<OneOf<bool, AppException>>;


public class SendConfirmationSMStoCustomerDataCommandHandler : IRequestHandler<SendConfirmationSMStoCustomerDataCommand, OneOf<bool, AppException>>
{
    private readonly IDbContext dbContext;
    private readonly ILogger<SendConfirmationSMStoCustomerDataCommandHandler> logger;

    public SendConfirmationSMStoCustomerDataCommandHandler(IDbContext dbContext, ILogger<SendConfirmationSMStoCustomerDataCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<bool, AppException>> Handle(SendConfirmationSMStoCustomerDataCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Template? reply;
            var template = Templates.SuccessfulWebServiceDataCustomer;
            reply = (request.CustomSMS is null)
                 ? TemplateExtensions.GetTemplate(dbContext, template)
                 : new Template() { TemplateText = request.CustomSMS };

            if (reply is null) return new AppException("Error Sending Confirmation", "Failed to load template");
            //reply.SetCurrency()
            reply.SetAmount(request.Amount);
            reply.SetBundle(request.BundleName);
            reply.SetAccountName(request.DealerName ?? "");

            SMS sms = new()
            {
                Direction = false,
                Mobile = request.TargetMobile.ToMobile(),
                Priority = new Priority() { PriorityId = (byte)Priorities.Normal },
                State = new State() { StateID = (byte)States.Pending },
                SMSID_In = null,
                SMSText = reply.TemplateText,
            };

            var result = await dbContext.SMSs.AddAsync(sms);
            if (result.IsT1) return new AppException("Error Sending Confirmation", result.AsT1.Message);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Sending Confirmation SMS - {message}", ex.Message);
            return new AppException("Error Sending Confirmation", ex.Message);

        }

    }

}