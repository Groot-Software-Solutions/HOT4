namespace Hot4.DataModel.Models;

public partial class VwErrorDetailVp
{
    public DateTime LogDate { get; set; }

    public int ErrorCount { get; set; }

    public required string ErrorData { get; set; }

    public required string Server { get; set; }

    public required string Name { get; set; }

    public int Network { get; set; }

    public string? HostAddress { get; set; }

    public string? Url { get; set; }

    public string? Port { get; set; }

    public int? Latency { get; set; }

    public string? ExpectedContent { get; set; }

    public int CountThreshold { get; set; }

    public int ErrorInterval { get; set; }

    public int CheckInterval { get; set; }

    public bool Enabled { get; set; }

    public required string TestTypeName { get; set; }

    public long ErrorId { get; set; }

    public int CheckId { get; set; }
}
