using System;
using System.ComponentModel.DataAnnotations;

namespace Hot.Domain.Entities
{
    public class Recharge
    {
        [Key]
        [Display(Name = "Id")]
        public long RechargeId { get; set; }

        [Display(Name = "Recharge State")]
        [Required(ErrorMessage = "Recharge State is required")]
        public byte StateId { get; set; }

        [Display(Name = "Access")]
        [Required(ErrorMessage = "Access is required")]
        public long AccessId { get; set; }

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Amount is required")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "Discount")]
        [Required(ErrorMessage = "Discount is required")]
        [DataType(DataType.Currency)]
        public decimal Discount { get; set; }

        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "Mobile is required")]
        [StringLength(50, ErrorMessage = "Mobile may not exceed 50 characters")]
        public string Mobile { get; set; } = string.Empty;  

        [Display(Name = "Brand")]
        [Required(ErrorMessage = "Brand is required")]
        public byte BrandId { get; set; }

        [Display(Name = "Recharge Date")]
        [Required(ErrorMessage = "Recharge Date is required")]
        [DataType(DataType.DateTime)]
        public DateTime RechargeDate { get; set; }

        [Display(Name = "Insert Date")]        
        [DataType(DataType.DateTime)]
        public DateTime? InsertDate { get; set; }
    }
}
