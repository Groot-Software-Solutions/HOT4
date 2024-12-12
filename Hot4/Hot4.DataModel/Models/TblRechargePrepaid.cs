using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblRechargePrepaid
{
    [Key]
    public long RechargeId { get; set; }

    public bool DebitCredit { get; set; }

    public required string ReturnCode { get; set; }

    public required string Narrative { get; set; }

    public decimal InitialBalance { get; set; }

    public decimal FinalBalance { get; set; }

    public required string Reference { get; set; }

    public decimal? InitialWallet { get; set; }

    public decimal? FinalWallet { get; set; }

    public DateTime? Window { get; set; }

    public string? Data { get; set; }

    public string? Sms { get; set; }

    public virtual TblRecharge Recharge { get; set; }
}
