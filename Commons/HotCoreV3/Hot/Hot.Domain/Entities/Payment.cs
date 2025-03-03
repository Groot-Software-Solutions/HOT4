using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace Hot.Domain.Entities
{
    public class Payment
    {
        [Key]
        [Display(Name = "Id")]
        public long PaymentId { get; set; }

        [Display(Name = "Account Id")]
        public long AccountId { get; set; }

        [Required(ErrorMessage = "Payment Type is required")]
        [Display(Name = "Payment Type")]
        public byte PaymentTypeId { get; set; }

        [Required(ErrorMessage = "Payment Source is required")]
        [Display(Name = "Payment Source")]
        public byte PaymentSourceId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment Date is required")]
        [Display(Name = "Payment Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Reference is required")]
        [Display(Name = "Reference")]
        public string Reference { get; set; } = string.Empty;

        [Display(Name = "Last User")]
        public string LastUser { get; set; } = string.Empty;
    }
}
