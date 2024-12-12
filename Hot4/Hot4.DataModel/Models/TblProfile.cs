using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblProfile
{
    [Key]
    public int ProfileId { get; set; }

    public required string ProfileName { get; set; }

    public virtual ICollection<TblAccount> TblAccounts { get; set; } = new List<TblAccount>();

    public virtual ICollection<TblProfileDiscount> TblProfileDiscounts { get; set; } = new List<TblProfileDiscount>();
}
