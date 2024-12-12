namespace Hot4.DataModel.Models;

public partial class VwErrorStatus
{
    public int ErrorLogCheckId { get; set; }

    public required string Server { get; set; }

    public required string Name { get; set; }

    public int Network { get; set; }

    public int TestType { get; set; }

    public bool Enabled { get; set; }

    public long? ErrorId { get; set; }

    public DateTime? LogDate { get; set; }

    public int? CheckId { get; set; }

    public int? LogErrorCount { get; set; }

    public string? ErrorData { get; set; }
}
