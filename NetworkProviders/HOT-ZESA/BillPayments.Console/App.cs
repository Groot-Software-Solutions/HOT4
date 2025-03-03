using BillPayments.Domain.Models;
using BillPayments.Domain.Enums;
using MediatR;
using System;
using BillPayments.Domain.Helpers;
using BillPayments.Application.Commands;

namespace BillPayments.Console
{
    public class App
    {
        const string VendorNumber = "VE20245865801";
        const string AccountNumber= "0120245865817";
        
        readonly IMediator mediator;

        public App(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public void Run()
        {
            //GetVendorBalance();

            GetCutomerInfo();

            //PurchaseToken();
             

            // var jobId = BackgroundJob.Schedule(() => ResendPurchaseRequest(purchaseTokenCommand) , TimeSpan.FromSeconds(7));

            System.Console.WriteLine("Press any key to close");
            System.Console.ReadKey();
        }

        private  void PurchaseToken()
        {
            //var purchaseTokenCommand = new PurchaseTokenCommand
            //{
            //    Mti = "0200",
            //    VendorReference = "031423091358",
            //    ProcessingCode = "U50000",
            //    TransactionAmount = 5000,
            //    TransmissionDate = DateTime.Now.ToString("MMddyyyyHHmmss"),
            //    VendorNumber = "V3003616720091",
            //    TerminalId = "POS001",
            //    MerchantName = "ZETDC",
            //    UtilityAccount = "41234567890",
            //    Agregator = "POWERTEL",
            //    ProductName = "ZETDC_PREPAID",
            //    CurrencyCode = "ZWL"
            //};

            //var purchaseTokenResponse = mediator.Send(purchaseTokenCommand).GetAwaiter().GetResult();
        }

        private  void GetCutomerInfo()
        {
            var customerInfoResponse = mediator.Send(new GetCustomerInfoCommand( new CustomerInfoRequest
            { 
                TransactionAmount = AmountHelper.BillPaymentAmount(20), 
                UtilityAccount = "07074496105"
            })).GetAwaiter().GetResult();
            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(customerInfoResponse));

        }

        private  void GetVendorBalance()
        {
            var vendorBalanceResponse = mediator.Send(new GetVendorBalanceCommand( new VendorBalanceRequest
            { 
                VendorNumber = VendorNumber,
                AccountNumber = AccountNumber,
                CurrencyCode = CurrencyCodes.ZWG,  
                VendorReference = Guid.NewGuid().ToString()
            })).GetAwaiter().GetResult();
            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(vendorBalanceResponse));
            System.Console.WriteLine(VendorBalanceHelper.GetBalanceAmount(vendorBalanceResponse.VendorBalance));
        }

    }
}
