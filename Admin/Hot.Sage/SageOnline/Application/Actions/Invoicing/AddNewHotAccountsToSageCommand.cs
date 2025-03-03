using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Application.Common.Models;
using Sage.Application.Entities.Customers;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Sage.Application.Actions
{
    public class AddNewHotAccountsToSageCommand : IRequest<bool>
    {

        public class AddNewhotAcconntsToSageCommandHandler : IRequestHandler<AddNewHotAccountsToSageCommand, bool>
        {
            private const int transactionDelay = 300;
            readonly IHotDbContext hotDbContext;
            readonly IMediator mediator;

            public AddNewhotAcconntsToSageCommandHandler(IHotDbContext hotDbContext, IMediator mediator)
            {
                this.hotDbContext = hotDbContext;
                this.mediator = mediator;
            }

            public async Task<bool> Handle(AddNewHotAccountsToSageCommand request, CancellationToken cancellationToken)
            {
                List<Customer> Accounts = await hotDbContext.GetNewAccountsAsync();
                List<Customer> SavedCustomers = SaveCustomers(Accounts);
                await UpdateHotCustomerAccounts(SavedCustomers);

                return SavedCustomers.Count == Accounts.Count;
            }

            private List<Customer> SaveCustomers(List<Customer> Accounts)
            {
                var SavedAccounts = new List<Customer>();
                foreach (var account in Accounts)
                {
                    try
                    {
                        Console.WriteLine($"Saving a a new Customer to SAGE - {account.ID} - {account.Name}");
                        var result = mediator.Send(new CustomerSaveCommand(account)).Result;
                        if (result is not null) SavedAccounts.Add(result);
                        Thread.Sleep(transactionDelay);
                        Console.WriteLine($"Saved a SAGE Customer {result.ID} - {result.Name}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        // throw;
                    }
                }


                return SavedAccounts;
            }

            private async Task UpdateHotCustomerAccounts(List<Customer> SageCustomers)
            {
                foreach (var item in SageCustomers)
                {
                    try
                    {
                        var customerAddress = await hotDbContext.GetAddress((long)item.NumericField1);

                        if (customerAddress.SageID == 0 || customerAddress.SageIDUsd == 0)
                        {
                            if (item.CurrencyId == 169) customerAddress.SageID = item.ID;
                            if (item.CurrencyId == 151) customerAddress.SageIDUsd = item.ID;
                            customerAddress.AccountID = (long)item.NumericField1;
                            _ = await hotDbContext.SaveAddress(customerAddress);
                            Console.WriteLine("Account Added - {ID} - {Name} - {SageID}", customerAddress.AccountID, item.Name, item.ID);
                        }
                    }
                    catch (NotFoundException ex)
                    {
                        Console.WriteLine($"CheckingLink for {item.Name}({item.ID})");
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                    }

                }
            }

        }
    }

}