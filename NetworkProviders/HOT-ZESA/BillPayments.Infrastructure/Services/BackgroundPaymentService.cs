using BillPayments.Application.Commands;
using BillPayments.Application.Common.Interfaces;
using BillPayments.Application.Common.Models;
using BillPayments.Application.Services;
using BillPayments.Domain.Enums;
using BillPayments.Domain.Helpers;
using BillPayments.Domain.Models;
using BillPayments.Domain.Models.PurchaseToken;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BillPayments.Application.Services
{
    public class BackgroundPaymentService : IBackgroundPaymentService
    {
        private readonly IBackgroundTaskService _backgroundTaskService;
        private readonly IMediator _mediator; 
        private readonly IAPIService _apiservice; 
        private readonly JsonSerializerOptions _serializerOptions;


        public BackgroundPaymentService(IBackgroundTaskService backgroundTaskService, IMediator mediator, IAPIService apiservice)
        {
            _backgroundTaskService = backgroundTaskService;
            _mediator = mediator; 
            _apiservice = apiservice;
            apiservice.APIName = "HOT";
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
           
        }

        public async Task Run()
        {
            var tasks = await _backgroundTaskService.GetUnprocessedTasks();
            if (tasks.Any())
            {
                foreach (var task in tasks)
                {
                    switch (task.EntityType)
                    {
                        case "BillPayments.Domain.Models.PurchaseTokenRequest":
                            var request = JsonSerializer.Deserialize<PurchaseTokenRequest>(task.EntityBody, _serializerOptions); 
                            var response = await _mediator.Send(new ResendPurchaseTokenCommand(new ResendPurchaseTokenRequest
                            {
                                OriginalReference = request.VendorReference,
                                VendorReference = Guid.NewGuid().ToString(),
                                TransactionAmount = request.TransactionAmount,
                                VendorNumber = request.VendorNumber,
                                MerchantName = request.MerchantName,
                                ProductName = request.ProductName,
                                UtilityAccount = request.UtilityAccount,
                                CurrencyCode = request.CurrencyCode, 
                            }));
                            task.LastResponse = JsonSerializer.Serialize<PurchaseTokenResponse>(response, _serializerOptions);
                         

                            switch (response.ResponseCode)
                            {
                                case "00":
                                    try
                                    {
                                        var retryupdate = new HotRetyUpdate()
                                        {
                                            RechargeId = Convert.ToInt32(request.VendorReference.Replace("Hot-", "")),
                                            PurchaseToken = PurchaseTokenHelper.GetPurchaseToken(response)
                                        };
                                        var reply = _apiservice.APIPostCall<string, HotRetyUpdate>("/api/v1/agents/complete-zesa", retryupdate, "");

                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex; //_logger.
                                    } 
                                    task.RetrySucceeded = true;
                                    break;
                                case "09":
                                    task.RetrySucceeded = false;
                                    break;
                                default:
                                    task.RetrySucceeded = true;
                                    try
                                    {
                                        var retryupdate = new HotRetyUpdate()
                                        {
                                            RechargeId = Convert.ToInt32(request.VendorReference.Replace("Hot-", "")),
                                            PurchaseToken = PurchaseTokenHelper.GetPurchaseToken(response)
                                        };
                                        var reply = _apiservice.APIPostCall<string, HotRetyUpdate>("/api/v1/agents/complete-zesa", retryupdate, "");

                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex; //_logger.
                                    } 
                                    break;
                            }

                            task.DateOfLastRetry = DateTime.Now;
                            await _backgroundTaskService.SaveTask(task);
                            break;

                        default:
                            break;
                    }

                }
            }
        }

    }
}
