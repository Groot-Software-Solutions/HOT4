using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using Hot.Ecocash.Application.Common.Extensions;
using Hot.Ecocash.Application.Common.Interfaces; 
using MediatR;
using OneOf;

namespace Hot.Ecocash.Application.Actions;

public record ProcessPendingEcocashPaymentsCommand : IRequest<OneOf<List<BankPaymentResult>, AppException>>;
public class ProcessPendingEcocashPaymentsCommandHandler : IRequestHandler<ProcessPendingEcocashPaymentsCommand, OneOf<List<BankPaymentResult>, AppException>>
{
    private readonly IDbContext context;
    private readonly IMediator mediator;
    private readonly IEcocashServiceFactory ecocash;

    public ProcessPendingEcocashPaymentsCommandHandler(IDbContext context, IMediator mediator, IEcocashServiceFactory ecocash)
    {
        this.context = context;
        this.mediator = mediator;
        this.ecocash = ecocash;
    }

    public async Task<OneOf<List<BankPaymentResult>, AppException>> Handle(ProcessPendingEcocashPaymentsCommand request, CancellationToken cancellationToken)
    {
        
        var paymentsListResult = await context.BankTrxs.ListPendingEcocashAsync();
        if (paymentsListResult.IsT1) return new AppException("Failed to get pending payments", paymentsListResult.AsT1);
        var paymentsList = paymentsListResult.AsT0; 

        List<BankPaymentResult> result = new();
        foreach (var item in paymentsList)
        {
            var service = ecocash.GetService(item.GetEcocashAccountType());
            var queryResponse = await service.QueryTransactionAsync(item.Identifier, item.BankTrxID.ToString());
            if (queryResponse is null || !(queryResponse.ValidResponse)) continue;
            var ecocashTransaction = queryResponse.Item;
            var response = await mediator.Send(new CompleteEcocashCommand(ecocashTransaction,"Ecocash.Service"), cancellationToken);
            result.Add(
                new(
                    item.BankTrxID,
                    response.Match(
                    completed => completed.Successful
                        ? $"BankTrx completed successfully : {((completed.Payment is null) ? "Payment failed at provider" : $"PaymentId-{completed?.Payment?.PaymentId}")}"
                        : $"Banktrx unchanged result currently : {completed.ErrorData}",
                    notfound => notfound.Message,
                    error => error.Message)
            ));

        }
        return result;
    } 
}

