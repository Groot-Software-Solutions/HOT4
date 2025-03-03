using Hot.Domain.Entities;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Application.Common.Models;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage.Infrastructure.Services
{
    public class HotDbContext : IHotDbContext 
    {
        readonly IDbHelper context;

        public HotDbContext(IDbHelper context)
        {
            this.context = context;
        }

         public async Task<List<Customer>> GetNewAccountsAsync()
        {
            string Query = @"xSageAccounts_ListNew";
            var result  =   await context.Query<HotSageAccount>(Query);
            return result.Select(c => c.Customer).ToList();
        }

        public async Task<List<CustomerReceipt>> GetReceiptsAsync(DateTime startdate, DateTime enddate)
        {
            //enddate = startdate.AddDays(1); //Unneccessary as enddate is set by calling function in GenerateNewSageInvoicesCommandHandler  - RH 17/02/2025
            string Query = @"xSageGenerateReceipts @Startdate, @endDate";
            var result = await context.Query<HotSageReceipt,object>(Query, new {startdate, enddate});
            return result.Select(c => c.Receipt).ToList(); //added the tblBankTrx.Trx Date as a string to the Description for Recons KMR & RH 20/01/2025
        }
        public async Task<List<CustomerReceipt>> GetReceiptsZWLAsync(DateTime startdate, DateTime enddate)
        {
            //enddate = startdate.AddDays(1); //Unneccessary as enddate is set by calling function in GenerateNewSageInvoicesCommandHandler - RH 17/02/2025
            string Query = @"xSageGenerateReceiptsZWL @Startdate, @endDate";
            var result = await context.Query<HotSageReceipt, object>(Query, new { startdate, enddate });
            return result.Select(c => c.Receipt).ToList();
        }
        public async Task<List<CustomerReceipt>> GetReceiptsUSDAsync(DateTime startdate, DateTime enddate)
        {
            string Query = @"xSageGenerateReceiptsUSD @Startdate, @endDate";
            var result = await context.Query<HotSageReceipt, object>(Query, new { startdate, enddate });
            return result.Select(c => c.Receipt).ToList();
        }

        public async Task<Address> GetAddress(long AccountId)
        {
            if (AccountId == 0) throw new NotFoundException("Address",AccountId);
            string Query = @"xAddress_Select @AccountId";
            var result = await context.QuerySingle<Address,object>(Query,  new { AccountId });
            return result;
        }

        public async Task<bool> SaveAddress(Address Address)
        {
            string Query = @"xAddress_Save @AccountID,@Address1,@Address2,@City,@ContactName,@ContactNumber,@VatNumber,@Latitude,@Longitude,@SageID,@InvoiceFreq,@SageIDUsd";
            var result = await context.Execute(Query, Address);
            return result;
        }

        public async Task<bool> SaveSageReceiptId(long PaymentID, string SageReceiptId)
        {
            string Query = @"xPayment_Save_SageReceipt @PaymentId,@SageReceiptId";
            var result = await context.Execute(Query, new { PaymentID, SageReceiptId });
            return result;
        }
        public async Task<bool> SaveSageBatchReceiptId(string PaymentBatchRef, string SageReceiptId)
        {
            string Query = @"xPayment_Save_SageBatchReceipt @PaymentBatchRef,@SageReceiptId";
            var result = await context.Execute(Query, new { PaymentBatchRef, SageReceiptId });
            return result;
        }
    }
}
