using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.ViewModel
{
    public class BankTransactionSearchModel
    {
        public byte BankId { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        public DateTime TrxDate { get; set; }
        [StringLength(50)]
        public string BankRef { get; set; }
        [Column(TypeName = "money")]
        public decimal Balance { get; set; }
    }
}
