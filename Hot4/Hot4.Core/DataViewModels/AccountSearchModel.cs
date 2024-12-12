using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hot4.Core.DataViewModels
{
    public class AccountSearchModel
    {
        [DisplayName("Account Id")]
        public long AccountID { get; set; }

        [DisplayName("Account Name")]
        public string AccountName { get; set; }

        [DisplayName("Profile")]
        public string ProfileName { get; set; }

        [DisplayName("National Id")]
        public string NationalID { get; set; }

        public string Email { get; set; }

        [DisplayName("Referred By")]
        public string ReferredBy { get; set; }

        [DisplayName("ZiG VAT")]
        [DisplayFormat(DataFormatString = ("#,##0.00"))]
        public decimal ZiGVAT { get; set; }

        [DisplayName("ZiG VAT Exempt")]
        [DisplayFormat(DataFormatString = ("#,##0.00"))]
        public decimal ZiGVATExempt { get; set; }

        [DisplayName("USD VAT")]
        [DisplayFormat(DataFormatString = ("#,##0.00"))]
        public decimal USDVAT { get; set; }

        [DisplayName("USD VAT Exempt")]
        [DisplayFormat(DataFormatString = ("#,##0.00"))]
        public decimal USDVATExempt { get; set; }
    }
}
