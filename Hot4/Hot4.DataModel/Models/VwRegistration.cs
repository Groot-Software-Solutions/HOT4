namespace Hot4.DataModel.Models;

public partial class VwRegistration
{
    public int ProfileId { get; set; }

    public required string AccountName { get; set; }

    public required string NationalId { get; set; }

    public required string Email { get; set; }

    public required string ReferredBy { get; set; }

    public long AccountId { get; set; }

    public DateTime Smsdate { get; set; }

    public required string Smstext { get; set; }

    public bool Direction { get; set; }

    public required string Mobile { get; set; }

    public DateTime? InsertDate { get; set; }
}
