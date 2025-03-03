using MediatR;
using Sage.Application.Actions;
using Sage.Application.Entities.BankAccounts;
using Sage.Application.Entities.Companys;
using Sage.Application.Entities.Currencys;
using Sage.Application.Entities.CustomerCategorys;
using Sage.Application.Entities.Customers;
using Sage.Application.Entities.Items;
using Sage.Application.Entities.SalesRepresentatives;
using Sage.Application.Entities.TaxTypes;
using Sage.Domain.Entities;
using System.ComponentModel;

namespace ConsoleAppUSD
{
    internal class App
    {

        readonly IMediator mediator;

        public App(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public void Run()
        {
            GenerateInvoiceMenu(); //uncomment to run

            //Console.WriteLine("Updating Accounts");
            //_ = mediator.Send(new AddNewHotAccountsToSageCommand()).Result;
            //_ = mediator.Send(new UpdateHotAccountsWithSageIdCommand(true)).Result;

            //var result = mediator.Send(new GenerateNewSageInvoicesCommand(date, date, Sage.Domain.Enums.CurrencyOptions.ZWL)).Result;
            //var list = mediator.Send(new CompanyGetAllQuery()).Result;
            //list.ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}"));

            //var xlist = mediator.Send(new BankAccountGetAllQuery()).Result;
            //xlist.ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}"));

            //var ylist = mediator.Send(new CustomerGetAllQuery()).Result;
           // ylist.Where(a=>a.Name.ToLower().Contains("Cash")).ToList().ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}"));
           // ylist.Where(a => a.Name.ToLower().Contains("mishdor")).ToList().ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}"));
            
            //ylist.Where(a => a.Name.ToLower().Contains("Say and Play Therapy")).ToList().ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}")); 
            //ylist.ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}"));

            //var list = mediator.Send(new CustomerCategoryGetQuery()).Result;
            //list.ForEach(x => Console.WriteLine($"\"{x.Description}\":{x.ID}"));

            //var zlist = mediator.Send(new CurrencyGetAllQuery()).Result;
            //zlist.ForEach(x => Console.WriteLine($"{x.ID}:{x.Code}:{x.Description}"));

            //var xlist = mediator.Send(new ItemGetQuery()).Result;
            //xlist.ForEach(x => Console.WriteLine($"{x.ID}:{x.Code}:{x.Description}"));

            //var account = mediator.Send(new CustomerGetByIdQuery(35961118)).Result;
            //Console.WriteLine($"{account.Name}:{account.Balance}:{account.ContactName}");

            //var ylist = mediator.Send(new TaxTypeGetAllQuery()).Result;
            //ylist.ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}:{x.Percentage}"));

            //var zlist = mediator.Send(new SalesRepresentativeGetAllQuery()).Result;
            //zlist.ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}:{x.Email}"));

            //var list = mediator.Send(new CustomerGetAllQuery()).Result;
            //list.Where(r=>r.NumericField1==0&& r.CurrencyId ==151)
            //    .OrderBy(r=>r.Name)
            //    .ToList()
            //    .ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}:{x.Email}"));

            //USDAccountLink().ToList().ForEach(a =>
            //{
            //    var account = mediator.Send(new CustomerGetByIdQuery(a.Key)).Result;
            //    Console.WriteLine($"{account.Name}:{account.Balance}:{account.ContactName}");
            //    account.NumericField1 = a.Value;
            //    var result = mediator.Send(new CustomerSaveCommand(account)).Result;
            //    Console.WriteLine($"Saved Link for {a.Key}");
            //});
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }

        private void GenerateInvoiceMenu()
        {
            Console.WriteLine("USD Sage Invoice Generator");
            Console.WriteLine();
            DateTime? date = null;
            while (date is null)
            {
                Console.Write("Please Enter Date to Add (DD/MM/YY): ");
                var dateInput = Console.ReadLine();
                try
                {
                    date = Convert.ToDateTime(dateInput);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Date Entered");
                }
            }

            Console.WriteLine("Updating Accounts");
            _ = mediator.Send(new AddNewHotAccountsToSageCommand()).Result;
            Console.WriteLine(" Adding SAGE ID to Hot4.AccountID"); //KMR 21/01/25
            _ = mediator.Send(new UpdateHotAccountsWithSageIdCommand(true)).Result;
            while (true)
            {
                Console.WriteLine($"Adding USD Invoices for {date: dd MMM yyy}");
                var result = mediator.Send(
                    new GenerateNewSageInvoicesCommand(date ?? DateTime.Now, date ?? DateTime.Now, Sage.Domain.Enums.CurrencyOptions.USD)
                    ).Result;
                Console.WriteLine(result);
                Console.WriteLine("Do you want to add another day (Y/n):");
                var completed = Console.ReadLine();
                if (completed.ToLower() != "y") break;

                date = null;
                while (date is null)
                {
                    Console.Write("Please Enter Date to Add (DD/MM/YY): ");
                    var dateInput = Console.ReadLine();
                    try
                    {
                        date = Convert.ToDateTime(dateInput);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid Date Entered");
                    }
                }
            }
        }

    }
}