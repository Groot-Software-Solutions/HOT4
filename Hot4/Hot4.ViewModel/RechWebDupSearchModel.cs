using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.ViewModel
{
    public class RechWebDupSearchModel
    {
        public long AccessId { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
    }
}
