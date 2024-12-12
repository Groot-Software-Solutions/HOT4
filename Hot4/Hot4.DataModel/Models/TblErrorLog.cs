namespace Hot4.DataModel.Models;

public partial class TblErrorLog
{
    public required long ErrorId { get; set; }

    public DateTime LogDate { get; set; }

    public int CheckId { get; set; }

    public int ErrorCount { get; set; }

    public required string ErrorData { get; set; }
}
