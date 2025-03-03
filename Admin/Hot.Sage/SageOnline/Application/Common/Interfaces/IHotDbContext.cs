using Hot.Domain.Entities;
using Sage.Domain.Entities;

namespace Sage.Application.Common.Interfaces
{
    public interface IHotDbContext
    {
        public Task<List<Customer>> GetNewAccountsAsync();
        public Task<List<CustomerReceipt>> GetReceiptsAsync(DateTime start, DateTime end);
        public Task<List<CustomerReceipt>> GetReceiptsZWLAsync(DateTime start, DateTime end);
        public Task<List<CustomerReceipt>> GetReceiptsUSDAsync(DateTime start, DateTime end);
        public Task<Address> GetAddress(long AccountId);
        public Task<bool> SaveAddress(Address Address);
        public Task<bool> SaveSageReceiptId(long PaymentID, string SageReceiptId);
        public Task<bool> SaveSageBatchReceiptId(string PaymentBatchRef, string SageReceiptId);// Added to make and save a batch receipt id and add that to
                                                                                               // HOT database in the case of Cash Sales accounts having too
                                                                                               // many receipts - eg. EcoCash, even CBZ and OneMoney.
                                                                                               // Each PatmentID will get a SageReceiptID and the Batch name made
                                                                                               // for the trx to be able to rever to it.
                                                                                               // KMR and RH 20/01/2025
    }
}

