using System.ComponentModel.DataAnnotations;

namespace Hot.Econet.Prepaid.Models;

public class BundleProduct :  Bundle
{
    [Key]
    [Display(Name = "Id")]
    public int BundleId { get; set; }

    [Display(Name = "Brand Id")]
    public int BrandId { get; set; }

    [Display(Name = "Network")]
    public string Network { get; set; } = string.Empty;

}




