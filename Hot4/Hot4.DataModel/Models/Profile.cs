using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Profile
{
    [Key]
    public int ProfileId { get; set; }

    public required string ProfileName { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<ProfileDiscount> ProfileDiscounts { get; set; } = new List<ProfileDiscount>();
}
