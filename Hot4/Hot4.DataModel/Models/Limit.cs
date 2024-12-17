using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Limit
{
    [Key]
    public long LimitId { get; set; }

    public byte NetworkId { get; set; }

    public long AccountId { get; set; }

    public int LimitTypeId { get; set; }

    public double DailyLimit { get; set; }

    public double MonthlyLimit { get; set; }
}
