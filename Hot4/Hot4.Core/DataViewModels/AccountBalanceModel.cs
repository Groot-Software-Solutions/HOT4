using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hot4.Core.DataViewModels
{
    public class AccountBalanceModel
    {
        public long AccountID { get; set; }

        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = ("#,##0.00"))]
        public decimal? Balance { get; set; }

        [DisplayName("Sale Value")]
        [DisplayFormat(DataFormatString = ("#,##0.00"))]
        public decimal? SaleValue { get; set; }

        [DisplayName("ZESA Balance")]
        [DisplayFormat(DataFormatString = ("#,##0.00"))]
        public decimal? ZESABalance { get; set; }

        [DisplayName("USD Balance")]
        [DisplayFormat(DataFormatString = ("#,##0.00"))]
        public decimal? USDBalance { get; set; }

        [DisplayName("USD Utility Balance")]
        [DisplayFormat(DataFormatString = ("#,##0.00"))]
        public decimal? USDUtilityBalance { get; set; }
    }
}
