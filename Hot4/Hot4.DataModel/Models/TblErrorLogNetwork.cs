namespace Hot4.DataModel.Models;

public partial class TblErrorLogNetwork
{
    public int ErrorLogNetworkId { get; set; }

    public required string NetworkName { get; set; }

    public int HotBrandId { get; set; }
}
