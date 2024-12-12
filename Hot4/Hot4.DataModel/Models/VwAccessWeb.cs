namespace Hot4.DataModel.Models;

public partial class VwAccessWeb
{
    public long AccessId { get; set; }

    public long AccountId { get; set; }

    public byte ChannelId { get; set; }

    public required string AccessCode { get; set; }

    public required string AccessPassword { get; set; }

    public bool? Deleted { get; set; }

    public string? AccessName { get; set; }

    public string? WebBackground { get; set; }
}
