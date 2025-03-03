using Hot.Application.Common.Extensions;

namespace Hot.Application.Actions.SMSActions;

public record SendConfirmationSMStoCustomerUtilityCommand(string TargetMobile, decimal Amount, decimal Balance,
    string Company, string AccountName, string AccountNumber, Networks Networks,decimal Units, string? CustomSMS = null)
: IRequest<OneOf<bool, AppException>>;

public class SendConfirmationSMStoCustomerUtilityCommandHandler : IRequestHandler<SendConfirmationSMStoCustomerUtilityCommand, OneOf<bool, AppException>>
{
    private readonly IDbContext dbContext;
    private readonly ILogger<SendConfirmationSMStoCustomerUtilityCommandHandler> logger;

    public SendConfirmationSMStoCustomerUtilityCommandHandler(IDbContext dbContext, ILogger<SendConfirmationSMStoCustomerUtilityCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<bool, AppException>> Handle(SendConfirmationSMStoCustomerUtilityCommand request, CancellationToken cancellationToken)
    {
        try
        { 
            var template = request.Networks switch
            {
                Networks.Nyaradzo => Templates.SuccessfulNyaradzoPayment_Customer,
                Networks.ZESA => Templates.SuccessfulZESATokenPurchase_Customer,
                _ => Templates.SuccessfulRechargeVAS_Customer
            };
            Template? reply;
            reply = (request.CustomSMS is null)
                 ? TemplateExtensions.GetTemplate(dbContext, template)
                 : new Template() { TemplateText = request.CustomSMS };
            if (reply is null) return new AppException("Error Sending Confirmation", "Failed to load template");
           
            reply.SetCustomerName(request.AccountName);
            reply.SetAmount(request.Amount);
            reply.SetBalance(request.Balance, true);
            reply.SetAccountNumber(request.AccountNumber)
                .SetMeter(request.AccountNumber);
            reply.SetAccountName(request.Company);
            reply.SetUnits(request.Units);
            
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

