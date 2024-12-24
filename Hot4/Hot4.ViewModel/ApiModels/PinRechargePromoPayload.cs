using System.ComponentModel.DataAnnotations;

namespace Hot4.ViewModel.ApiModels
{
    public class PinRechargePromoPayload
    {
        [StringLength(50)]
        public string AccessCode { get; set; }
        public int Quantity { get; set; }
        public decimal PinValue { get; set; }
        public byte BrandId { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }

    }
}
