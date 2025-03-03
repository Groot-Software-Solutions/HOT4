namespace Hot.Application.Actions.SMSActions;
public record SendPinToDealerSMSCommand (Pin Pin, string Mobile): IRequest<OneOf<bool, AppException>>;
        public class SendPinToDealerSMSCommandHandler : IRequestHandler<SendPinToDealerSMSCommand, OneOf<bool, AppException>>
        {
            private readonly IDbContext dbContext;
            private readonly ILogger<SendPinToDealerSMSCommandHandler> logger;

            public SendPinToDealerSMSCommandHandler(IDbContext dbContext, ILogger<SendPinToDealerSMSCommandHandler> logger)
            {
                this.dbContext = dbContext;
                this.logger = logger;
            }

            public async Task<OneOf<bool, AppException>> Handle(SendPinToDealerSMSCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var brandList = (await dbContext.Brands.ListAsync()).ResultOrNull();
                    var brand = brandList.Where(x => x.BrandId==request.Pin.BrandId).FirstOrDefault()?? new Brand() {BrandName="Pin Voucher"};    
                    
                    Template reply = (await dbContext.Templates.GetAsync((int)Templates.SuccessfulRechargePin_Dealer)).ResultOrNull();

                    reply.TemplateText = reply.TemplateText.Replace("%PIN%", request.Pin.PinNumber);
                    reply.TemplateText = reply.TemplateText.Replace("%PINVALUE%", request.Pin.PinValue.ToString("N2"));
                    reply.TemplateText = reply.TemplateText.Replace("%REF%", request.Pin.PinRef);
                    reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.Mobile);
                    reply.TemplateText = reply.TemplateText.Replace("%BRAND%", brand.BrandName);

                    SMS sms = new()
                    {
                        Direction = false,
                        Mobile = request.Mobile.ToMobile(),
                        Priority = new Priority() { PriorityId = (byte)Priorities.Normal },
                        State = new State() { StateID = (byte)States.Pending },
                        SMSID_In = (long?)null,
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
