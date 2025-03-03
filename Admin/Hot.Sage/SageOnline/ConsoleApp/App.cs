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
using System;
using System.Threading.Channels;
using static Sage.Application.Entities.Companys.CompanyGetAllQuery;

namespace ConsoleApp
{
    public class App
    {

        readonly IMediator mediator;

        public App(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public void Run()
        {

            GenerateInvoiceMenu(); //uncomment to run
            //UpdateAcccounts();
            DebugOptions();
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }

        private void UpdateAcccounts() {
            _ = mediator.Send(new AddNewHotAccountsToSageCommand()).Result;
            _ = mediator.Send(new UpdateHotAccountsWithSageIdCommand()).Result;
        }

        private void DebugOptions()
        {
            //var result = mediator.Send(new CustomerCategoryGetQuery()).Result;
            //result.ForEach(c => Console.WriteLine($"\"{c.Description}\":{c.ID},"));


            //var result = mediator.Send(new SalesRepresentativeGetAllQuery()).Result;
            //result.ForEach(c => Console.WriteLine($"{c.ID}-{c.Name}-{c.FirstName}"));

            //var result =  mediator.Send(new CompanyGetAllQuery()).Result;
            //result.ForEach(c => Console.WriteLine($"{c.ID}-{c.Name}"));

            //result = mediator.Send(new UpdateHotAccountsWithSageIdCommand()).Result;

            //DateTime date = Convert.ToDateTime("10/31/2023");
            //var result = mediator.Send(new GenerateNewSageInvoicesCommand(date, date, Sage.Domain.Enums.CurrencyOptions.ZWL)).Result;
            //Console.WriteLine(result);


            //var result = mediator.Send(new GenerateNewSageInvoicesCommand(date, date, Sage.Domain.Enums.CurrencyOptions.ZWL)).Result;

            //var list = mediator.Send(new BankAccountGetAllQuery()).Result;
            //list.ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}"));

            //var list = mediator.Send(new CustomerGetAllQuery()).Result;
            //list.Where(c=>c.Name.StartsWith("Smart"))
            //    .ToList()
            //    .ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}"));

            //var ylist = mediator.Send(new CurrencyGetAllQuery()).Result;
            //ylist.ForEach(x => Console.WriteLine($"{x.ID}:{x.Code}:{x.Description}"));

            //var zlist = mediator.Send(new ItemGetQuery()).Result;
            //zlist.ForEach(x => Console.WriteLine($"{x.ID}:{x.Code}:{x.Description}"));

            //var account = mediator.Send(new CustomerGetByIdQuery(54642472)).Result;
            //Console.WriteLine($"{account.Name}:{account.Balance}:{account.ContactName}");

            //var tlist = mediator.Send(new TaxTypeGetAllQuery()).Result;
           // tlist.ForEach(x => Console.WriteLine($"{x.ID}:{x.Name}:{x.Percentage}"));

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
        }

        private void GenerateInvoiceMenu()
        {
            Console.WriteLine("ZWG Sage Invoice Generator");
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
            Console.WriteLine(" Adding SAGE ID to Hot4.AccountID");//KMR 21/01/25
            _ = mediator.Send(new UpdateHotAccountsWithSageIdCommand()).Result;
            while (true)
            {
                Console.WriteLine($"Adding ZWG Invoices for {date: dd MMM yyy}");
                var result = mediator.Send(
                    new GenerateNewSageInvoicesCommand(date ?? DateTime.Now, date ?? DateTime.Now, Sage.Domain.Enums.CurrencyOptions.ZWL)
                    ).Result;
                Console.WriteLine(result);
                Console.WriteLine($"completed: {date: dd MMM yyyy} Do you want to add another day (Y/n):");
                var completed = Console.ReadLine();
                if (completed.ToLower() != "y") break;

                date = null;
                while (date is null)
                {
                    Console.Write("Enter (n) to stop or Please Enter Date to Add (DD/MM/YY): ");
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
