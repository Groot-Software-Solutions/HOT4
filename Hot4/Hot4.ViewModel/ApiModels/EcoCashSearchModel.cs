using System.ComponentModel.DataAnnotations;

namespace Hot4.ViewModel.ApiModels
{
    public class EcoCashSearchModel
    {
        [StringLength(50)]
        public string Mobile { get; set; }
        public decimal Amount { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
    }
}
