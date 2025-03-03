using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Hot.API.Client.Entities
{
    public class Bundle
    {

        [Display(Name = "Product Code")]
        [StringLength(10, ErrorMessage = "Product Code can not exceed 10 characters in length")]
        [Required(ErrorMessage = "Product Code is Required")]
        public string ProductCode { get; set; }

        [Display(Name = "Amount", Prompt = "Bundle value in cents")]
        [Required(ErrorMessage = "Amount is Required")]
        public int Amount { get; set; }

        [Display(Name = "Bundle Name")]
        [StringLength(50, ErrorMessage = "Bundle Name can not exceed 50 characters in length")]
        [Required(ErrorMessage = "Bundle name is Required")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [StringLength(250, ErrorMessage = "Bundle Description can not exceed 250 characters in length")]
        public string Description { get; set; }

        [Display(Name = "Period", Prompt = "Bundle validity period in days")]
        public int ValidityPeriod { get; set; }

    }
}
