using BillPayments.Application.Common.Exceptions;
using BillPayments.Application.Common.Interfaces;
using BillPayments.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BillPayments.Application.Commands
{
    public class MakePaymentCommand : IRequest<PaymentResponse>
    {
        public PaymentRequest Item { get; set; }

        public MakePaymentCommand(PaymentRequest item)
        {
            Item = item;
        }

        public class MakePaymentCommandHandler : IRequestHandler<MakePaymentCommand, PaymentResponse>
        {
            private readonly IAPIService apihelper;

            public MakePaymentCommandHandler(IAPIService apihelper)
            {
                this.apihelper = apihelper;
            }
            public async Task<PaymentResponse> Handle(MakePaymentCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    return await apihelper.APIPostCall<PaymentResponse, PaymentRequest>("vend", request.Item);
                }
                catch (Exception ex)
                {
                    throw new APIException(request.Item.MerchantName, ex.Message);
                }
            }
        }
    }
}
