using Sage.Application.Common.Helpers;
using Sage.Domain.Entities;
using Sage.Domain.Enums; 

namespace Sage.Application.Common.Models
{
    public class HotSageReceipt
    {
        public long AccountID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string VatNumber { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int SageID { get; set; }
        public byte? InvoiceFreq { get; set; }
        //public long PaymentID { get; set; }
        public string PaymentID { get; set; }
        public int PaymentTypeID { get; set; }
        public int PaymentSourceID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Reference { get; set; }
        public string LastUser { get; set; }
        public string PaymentSource { get; set; }
        public string SageReceiptId { get; set; } = string.Empty;
        public int Taxable { get; set; } //KMR 06/02/2025 Missing the Taxable column from the stored procedure
        public string BankTrxDate { get; set; } = string.Empty;

        public CustomerReceipt Receipt { 
            get 
            {
                {
                    return new CustomerReceipt()
                    {
                        CustomerId = SageID, 
                        Total = Math.Round(Amount,2, MidpointRounding.ToNegativeInfinity),
                        BankAccountId = HotSageHelper.GetBankFromSource(PaymentSourceID),
                        PaymentMethod = PaymentMethod.Cash,
                        Date = PaymentDate,
                        Description = $"{Taxable}-{PaymentSource}-{PaymentTypeID}-{BankTrxDate}-", //To make more descriptive for Recons added the
                                                                                            //tblBankTrx.TrxDate if available - KMR & RH added 20/01/2025
                        Comments = Reference,
                        Reference = PaymentID, 

                    };
                } 
            } 
        }


    }
}
