using System.ComponentModel.DataAnnotations;

namespace Hot.Econet.Prepaid.Models;

public class AccountBalance
{
    [Display(Name = "Amount")]
    [Required(ErrorMessage = "Amount is Required")]
    public long Amount { get; set; }

    [Display(Name = "Currency Code")]
    [Required(ErrorMessage = "Currency Code is Required")]
    public int Currency { get; set; }

    [Key]
    [Display(Name = "Account Type Id")]
    [Required(ErrorMessage = "Account Type Id is Required")]
    public int AccountType { get; set; }

    public enum AccountTypes
    {
        Company_Account = 100,
        Company_Available = 101,
        User_Available = 102
    }


}




