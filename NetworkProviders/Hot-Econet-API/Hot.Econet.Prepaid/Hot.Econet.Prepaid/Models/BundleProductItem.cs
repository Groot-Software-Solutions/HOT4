using System.ComponentModel.DataAnnotations;

namespace Hot.Econet.Prepaid.Models;

public class BundleProductItem : Bundle
{
    public int Quantity;

    [Display(Name = "Provider Code")]
    public int ProviderCode { get; set; }

    [Display(Name = "Currency", Prompt = "Currency of amount")]
    [Required(ErrorMessage = "Currency is Required")]
    public int Currency { get; set; }

}




