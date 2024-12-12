namespace Hot4.DataModel.Models;

public partial class VwAccountDetail
{
    public int ProfileId { get; set; }

    public required string AccountName { get; set; }

    public required string NationalId { get; set; }

    public required string Email { get; set; }

    public required string ReferredBy { get; set; }

    public byte ChannelId { get; set; }

    public required string AccessCode { get; set; }

    public long AccountId { get; set; }
}
