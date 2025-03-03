using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using Microsoft.Extensions.Configuration;
using TelOne.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace TelOne.Application.Commands.Accounts
{
    public class PayBillCommand : IRequest<OneOf<PayBillResponse, APIException, string>>
    {
        public PayBillCommand(PayBillRequest item)
        {
            Item = item;
        }

        public PayBillRequest Item { get; set; }

        public class PayBillCommandHandler : IRequestHandler<PayBillCommand, OneOf<PayBillResponse, APIException, string>>
        {
            private readonly IAPIService apiService;
            private readonly IConfiguration configuration;
            private IAPIService _service;

            public PayBillCommandHandler(IAPIService service)
            {
                _service = service;
            }

            public PayBillCommandHandler(IAPIService apiService, IConfiguration configuration)
            {
                this.apiService = apiService;
                this.configuration = configuration;
            }

            public async Task<OneOf<PayBillResponse, APIException, string>> Handle(PayBillCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var result =  await apiService
                        .Post<PayBillResponse, PayBillRequest>
                        ("payBill", request.Item);
                    if (result.IsT0)
                    {
                        try
                        {
                            var item = result.AsT0;
                            var requestBal = new Request()
                            {
                                APIKey = configuration["APIKey"],
                                AccountSid = configuration["AccountSid"],
                            };
                            var balanceResponse = await apiService.Post<List<AccountBalance>, Request>("QueryMerchantBalance", requestBal);
                            if (balanceResponse.IsT0)
                            {
                                item.Balance = balanceResponse.AsT0.Where(b => b.Currency.ToUpper().Contains("ZiG")).FirstOrDefault().Balance;
                                return item;
                            }
                        }
                        catch (Exception) { }
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw new APIException("Telone", ex.Message);
                }
            }
        }
    }
}
