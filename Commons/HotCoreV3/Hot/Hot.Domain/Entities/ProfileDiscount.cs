using System.ComponentModel.DataAnnotations;

namespace Hot.Domain.Entities
{
    public class ProfileDiscount
    {
        [Key]
        [Display(Name = "Discount Id")]
        public int ProfileDiscountId { get; set; }

        [Display(Name = "Profile")]
        [Required(ErrorMessage = "Profile is required")]
        public int ProfileId { get; set; }

        [Display(Name = "Brand")]
        [Required(ErrorMessage = "Brand is required")]
        public byte BrandId { get; set; }

        [Required(ErrorMessage = "Discount is required")]        
        [Display(Name = "Discount %")]
        //[DataType(DataType.Currency)]
        public decimal Discount { get; set; }
    }
}
