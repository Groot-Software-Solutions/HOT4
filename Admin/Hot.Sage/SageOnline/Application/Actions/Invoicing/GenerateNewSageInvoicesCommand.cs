using MediatR;
using Sage.Application.Common.Helpers;
using Sage.Application.Common.Interfaces;
using Sage.Application.Common.Models;
using Sage.Application.Entities.Allocations;
using Sage.Application.Entities.CustomerReceipts;
using Sage.Application.Entities.Customers;
using Sage.Application.Entities.TaxInvoices;
using Sage.Domain.Entities;
using Sage.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sage.Application.Actions
{

    public class GenerateNewSageInvoicesCommand : IRequest<bool>
    {
        public GenerateNewSageInvoicesCommand(DateTime startDate, DateTime endDate, CurrencyOptions currency = CurrencyOptions.All)
        {
            StartDate = startDate;
            EndDate = endDate;
            Currency = currency;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CurrencyOptions Currency { get; set; } = CurrencyOptions.All;

        public class GenerateNewSageInvoicesCommandHandler : IRequestHandler<GenerateNewSageInvoicesCommand, bool>
        {
            private const int transactionDelay = 200;
            readonly IHotDbContext hotDbContext;
            readonly IMediator mediator;

            public GenerateNewSageInvoicesCommandHandler(IHotDbContext hotDbContext, IMediator mediator)
            {
                this.hotDbContext = hotDbContext;
                this.mediator = mediator;
            }

            public async Task<bool> Handle(GenerateNewSageInvoicesCommand request, CancellationToken cancellationToken)
            {
                //End date handling
                request.EndDate = request.StartDate.AddDays(1);
                List<CustomerReceipt> Receipts = new();
                try
                {
                    Receipts = request.Currency switch
                    {
                        CurrencyOptions.ZWL => await hotDbContext.GetReceiptsZWLAsync(request.StartDate, request.EndDate),
                        CurrencyOptions.USD => await hotDbContext.GetReceiptsUSDAsync(request.StartDate, request.EndDate),
                        _ => await hotDbContext.GetReceiptsAsync(request.StartDate, request.EndDate)
                    };
                }
                catch (Exception ex)
                {

                    throw;
                }
                AssignCustomerDetails(ref Receipts);
                Receipts = await SaveReceiptsAsync(Receipts);
                List<Allocation> Allocations = await SaveInvoices(Receipts);

                var SavedAllocations = await SaveAllocations(Allocations);
                return true;// Re-added this line and closing brace to get build to work - RH 17/02/2025
            }

            public async Task  UpdateReceiptPayments(List<CustomerReceipt> receipts)
            {
                foreach (var item in receipts)
                {
                    var isValid = long.TryParse(item.Reference, out long PaymentID);
                    if (isValid)
                    {
                        var response  = await hotDbContext.SaveSageReceiptId(PaymentID, item.DocumentNumber.ToString()); //KMR 15/01/2025 item.id changed to documentnumber                        if (!response) await Console.Out.WriteLineAsync( $"{PaymentID}-{item}");
                        Console.WriteLine($"Payment  {PaymentID} updated with Receipt {item.DocumentNumber}");
                    }
                    else 
                    {
                        if (item.Reference.ToLower().Contains("batch")) // If this is a Batch trx then 
                        {
                            var response = await hotDbContext.SaveSageBatchReceiptId(item.Reference, item.DocumentNumber.ToString() + "-" + item.Reference);
                            Console.WriteLine($"Batch  {item.Reference} ALL updated with Receipt {item.DocumentNumber}");
                        }
                        
                    }
                   
                }
                
            }

            private async Task<List<Allocation>> SaveAllocations(List<Allocation> Allocations)
            {
                var list = new List<Allocation>();
                foreach (var item in Allocations)
                {
                    var result = await mediator.Send(new AllocationSaveCommand(item));
                    list.Add(result);
                    Log($"Saved Allocation: {item.ID} - Receipt:{item.SourceDocumentId} allocated to Invoice:{item.AllocatedToDocumentId}");

                }
                return list;
            }

            private async Task<List<Allocation>> SaveInvoices(List<CustomerReceipt> Receipts)
            {

                var AirtimeInvoicesData = GetInvoicesFromReceipts(Receipts.Where(r => !IsZesaReceipt(r)).ToList());
                var ZesaInvoices = GetInvoicesFromReceipts(Receipts.Where(r => IsZesaReceipt(r)).ToList());
                var Invoices = ZesaInvoices;
                Invoices.AddRange(AirtimeInvoicesData);

                var Allocations = new List<Allocation>();
                Log($"Attempting to save {Invoices.Count()}invoices");
                foreach (var Invoice in Invoices)
                {
                    Log($"Saved Invoice: {Invoice.Item1.Id} - {Invoice.Item1.Customer.Name} - {Invoice.Item1.Date:g}");
                    try
                    {
                        var result = await mediator.Send(new TaxInvoiceSaveCommand(Invoice.Item1));
                        Log($"Saved Invoice: {result.Id} - {result.Customer.Name} - {result.Date:g}");
                        Invoice.Item2.ForEach(receipt =>
                                Allocations.Add(
                                    new Allocation()
                                    {
                                        SourceDocumentId = receipt.Id,
                                        AllocatedToDocumentId = result.Id,
                                        Type = 1,
                                        Total = receipt.Total
                                    }));
                        Thread.Sleep(transactionDelay);
                    }
                    catch (Exception ex)
                    {
                        Log($"Saved Invoice Failed: {Invoice.Item1.Id} - {Invoice.Item1.Customer.Name} - {Invoice.Item1.Date:g}");
                    }
                }
                return Allocations;
            }

            private TaxInvoice CreateInvoice(long CustomerId, DateTime InvoiceDate, List<CustomerReceipt> Receipts)
            {
                return new TaxInvoice()
                {
                    CustomerID = CustomerId,
                    Inclusive = true,
                    Date = InvoiceDate,
                    DueDate = InvoiceDate,
                    Lines = Receipts.Select(line =>
                        new CommercialDocumentLine()
                        {
                            Quantity = 1,
                            SelectionId = IsZesaReceipt(line) ? IsNyaradzoReceipt(line) ? SageSystemOptions.HotUtilityItemId : SageSystemOptions.HotZesaItemId : SageSystemOptions.HotAirtimeItemId,
                            TaxTypeId = GetTaxTypeId(line),
                            UnitPriceInclusive = line.Total,
                            Description = IsZesaReceipt(line) ? "ZETDC Prepaid Tokens for Meters" : "Airtime for All Zimbabwean Networks"
                        }).ToList(),
                    SalesRepresentativeId = SageSystemOptions.HotSalesRepId,
                    Customer = Receipts[0].Customer,
                    TaxReference = Receipts[0].Customer.TaxReference,
                    DeliveryAddress01 = Receipts[0].Customer.DeliveryAddress01,
                    DeliveryAddress02 = Receipts[0].Customer.DeliveryAddress02,
                    DeliveryAddress03 = Receipts[0].Customer.DeliveryAddress03,
                    DeliveryAddress04 = Receipts[0].Customer.DeliveryAddress04,
                    PostalAddress01 = Receipts[0].Customer.PostalAddress01,
                    PostalAddress02 = Receipts[0].Customer.PostalAddress02,
                    PostalAddress03 = Receipts[0].Customer.PostalAddress03,
                    PostalAddress04 = Receipts[0].Customer.PostalAddress04
                };
            }

            private int GetTaxTypeId(CustomerReceipt line)
            {
                if (IsZesaReceipt(line))
                {
                    if (IsNyaradzoReceipt(line)) return SageSystemOptions.HotUtilityItemId;
                    return SageSystemOptions.HotZesaTaxId;
                }
                if (IsUSDReceipt(line)) return SageSystemOptions.HotUSDTaxId;
                return SageSystemOptions.HotDefaultTaxId;
            }

            private List<Tuple<TaxInvoice, List<CustomerReceipt>>> GetInvoicesFromReceipts(List<CustomerReceipt> Receipts)
            {
                var Invoices = Receipts
                    .Where(i => !IsHotReceipt(i))
                    .GroupBy(r => new { r.CustomerId, r.Date }, r => r, (key, g) => new { Key = key, Receipts = g.ToList() })
                    .Select(i => Tuple.Create(CreateInvoice(i.Key.CustomerId, i.Key.Date, i.Receipts), i.Receipts))
                    .ToList();

                var HotInvoices = Receipts
                    .Where(i => IsHotReceipt(i));

                if (HotInvoices.Count() > 0)
                {
                    var HotInvoice = HotInvoices
                   .GroupBy(r => new { r.CustomerId, r.Date }, r => r, (key, g) => new { Key = key, Receipts = g.ToList() })
                   .ToList()
                   .First();
                    if (HotInvoice.Receipts.Where(c => IsEcocashReceipt(c)).Any())
                    {
                        Invoices.Add(Tuple.Create(CreateInvoice(
                                               HotInvoice.Key.CustomerId,
                                               HotInvoice.Key.Date,
                                               HotInvoice.Receipts.Where(c => IsEcocashReceipt(c)).ToList()
                                           ), HotInvoice.Receipts.Where(c => IsEcocashReceipt(c)).ToList()));
                    }
                    if (HotInvoice.Receipts.Where(c => !IsEcocashReceipt(c)).Any()) // KMR 06/02/2025 BZ treats EcoCash receipt differently because of volume?
                    {
                        Invoices.Add(Tuple.Create(CreateInvoice(
                                                HotInvoice.Key.CustomerId,
                                                HotInvoice.Key.Date,
                                                HotInvoice.Receipts.Where(c => !IsEcocashReceipt(c)).ToList()
                                            ), HotInvoice.Receipts.Where(c => !IsEcocashReceipt(c)).ToList()));
                    }
                }
                return Invoices;
            }

            private bool IsEcocashReceipt(CustomerReceipt c)
            {
                return c.BankAccountId == (int)Bank.Eco_Cash;
            }

            private bool IsHotReceipt(CustomerReceipt i)
            {
                return i.CustomerId == SageSystemOptions.HotCustomerId;
            }
            private bool IsUSDReceipt(CustomerReceipt i)
            {
                return i.Customer.CurrencyId == 151;
            }

            private bool IsZesaReceipt(CustomerReceipt i)
            {
                var split = i.Description.Split("-");// KMR changed the delimiter from a "," to a "|" BZ using the data in the description to determine if a Utility item or not! stupid, not using the Taxable Item in the REceipt object.
                // Description = $"{Taxable}|{PaymentSource}|{PaymentTypeID}|{BankTrxDate}|"
                //if (!(split.Length > 1)) return false;// take this out since the split will be more than 1 always
                if (split[2] == ((int)HotPaymentTypes.ZESA).ToString()) return true;
                if (split[2] == ((int)HotPaymentTypes.Nyaradzo).ToString()) return true;
                if (split[2] == ((int)HotPaymentTypes.USDUtility).ToString()) return true;
                //if (split[0] == 0 for taxable then  return true; this space could be used to insert an item code for multiple products in future
                return false;
            }

            private bool IsNyaradzoReceipt(CustomerReceipt i)
            {
                var split = i.Description.Split(",");
                if (!(split.Count() > 1)) return false;
                if (split[1] == ((int)HotPaymentTypes.Nyaradzo).ToString()) return true;
                return false;
            }

            private async Task<List<CustomerReceipt>> SaveReceiptsAsync(List<CustomerReceipt> Receipts)
            {
                var SavedReceipts = new List<CustomerReceipt>();
                foreach (var receipt in Receipts)
                {
                    
                    Log(Message: $"Attempting to save Receipt: {receipt.Id} - Reciept {receipt.DocumentNumber} - {receipt.Customer.Name} - Amount {receipt.Total} - {receipt.Date:g}");

                    var result = await mediator.Send(new CustomerReceiptSaveCommand(receipt));
                    result.Customer = receipt.Customer;
                    SavedReceipts.Add(result);
                    Log(Message: $"Receipt Saved: {result.Id} - {result.Customer.Name} - {result.Date:g}");
                    Thread.Sleep(transactionDelay);
                }
                return SavedReceipts;
            }

            private void Log(string Message)
            {
                Console.WriteLine(Message);
            }

            private void AssignCustomerDetails(ref List<CustomerReceipt> Receipts)
            {
                var CustomerList = mediator.Send(new CustomerGetAllQuery()).Result;
                var Customers = new List<Customer>();
                foreach (var id in Receipts.Select(s => s.CustomerId).Distinct().ToList())
                {
                    var customer = CustomerList.FirstOrDefault(c => c.ID == id) ?? mediator.Send(new CustomerGetByIdQuery(id)).Result;
                    if (id == customer.ID)
                        Customers.Add(customer);
                    //Thread.Sleep(transactionDelay);
                }
                Receipts.ForEach(r => r.Customer = Customers.Where(c => c.ID == r.CustomerId).FirstOrDefault());
                Receipts = Receipts.OrderByDescending(r => r.Customer.Balance).OrderBy(r => r.Customer.Name).ToList();

            }
        }
    }
}
