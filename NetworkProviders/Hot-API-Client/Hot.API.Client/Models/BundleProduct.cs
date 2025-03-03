using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hot.API.Client.Entities;

namespace Hot.API.Client.Models
{
    public class BundleProduct : Entities.Bundle
    {
        [Key]
        [Display(Name = "Id")]
        public int BundleId { get; set; }

        [Display(Name = "Brand Id")]
        public int BrandId { get; set; }

        [Display(Name = "Network")]
        public string Network { get; set; }

    }
}
