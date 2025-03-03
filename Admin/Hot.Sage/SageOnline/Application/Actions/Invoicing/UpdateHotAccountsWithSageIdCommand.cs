using MediatR;
using Microsoft.Extensions.Logging;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Application.Common.Models;
using Sage.Application.Entities.Customers;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Actions;
 
public record UpdateHotAccountsWithSageIdCommand(bool IsUsd = false) : IRequest<bool>;
    public class UpdateHotAccountsWithSageIdCommandHandler : IRequestHandler<UpdateHotAccountsWithSageIdCommand, bool>
    {
        private readonly IHotDbContext hotDbContext;
        private readonly IMediator mediator;
        private readonly ILogger<UpdateHotAccountsWithSageIdCommandHandler> logger;

        public UpdateHotAccountsWithSageIdCommandHandler(IHotDbContext hotDbContext, IMediator mediator, ILogger<UpdateHotAccountsWithSageIdCommandHandler> logger)
        {
            this.hotDbContext = hotDbContext;
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task<bool> Handle(UpdateHotAccountsWithSageIdCommand request, CancellationToken cancellationToken)
        {
            var SageCustomers = await mediator.Send(new CustomerGetAllQuery(),cancellationToken);
            await UpdateHotCustomerAccounts(SageCustomers, request.IsUsd);
            return true; // Add
        }

        

        private async Task UpdateHotCustomerAccounts(List<Customer> SageCustomers,bool IsUsd)
        {
        //var newava = string.Join("\r\n", SageCustomers.Select(c => $"{c.ID},{c.Name},{c.NumericField1}"));
            foreach (var item in SageCustomers)
            {
                try
                {
                    var AccountId = (long)item.NumericField1;
                    if (AccountId == 0) continue;
                    var customerAddress = await hotDbContext.GetAddress(AccountId);

                    if ((customerAddress.SageID is null || customerAddress.SageID == 0) && item.CurrencyId == 169) //KMR 06/02/25 added is null and || OR 
                    {
                       customerAddress.SageID = item.ID; 
                       customerAddress.AccountID = AccountId;
                        _ = await hotDbContext.SaveAddress(customerAddress);
                        logger.LogInformation("ZWL Account Updated - {ID} - {Name} - {SageID}", customerAddress.AccountID, item.Name, item.ID);
                    Console.WriteLine($"ZWL Account Updated - {customerAddress.AccountID} - {item.Name} - {item.ID}");
                    }
                    if ((customerAddress.SageIDUsd is null || customerAddress.SageIDUsd == 0) && IsUsd)
                    {
                        customerAddress.SageIDUsd = item.ID;
                        customerAddress.AccountID = AccountId;
                        _ = await hotDbContext.SaveAddress(customerAddress);
                        logger.LogInformation("USD Account Updated - {ID} - {Name} - {SageID}", customerAddress.AccountID, item.Name, item.ID);
                    Console.WriteLine($"USD Account Updated - - {customerAddress.AccountID} - {item.Name} - {item.ID}");
                    }
                    
                }
                catch (NotFoundException ex)
                {
                    Console.WriteLine($"CheckingLink for {item.NumericField1}-{item.Name}({item.ID})");
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }

            }
        }
    }

