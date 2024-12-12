namespace Hot4.DataModel.Models;

public partial class ZtblStat
{
    public long Accountid { get; set; }

    public int? Rmonth { get; set; }

    public decimal? Mvalue { get; set; }

    public int? Mcount { get; set; }

    public required string Band { get; set; }
}
