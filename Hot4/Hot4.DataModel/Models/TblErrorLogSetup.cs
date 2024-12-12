namespace Hot4.DataModel.Models;

public partial class TblErrorLogSetup
{

    public int ErrorLogCheckId { get; set; }

    public required string Server { get; set; }

    public required string Name { get; set; }

    public int Network { get; set; }

    public int TestType { get; set; }

    public string? HostAddress { get; set; }

    public string? Url { get; set; }

    public string? Port { get; set; }

    public int? Latency { get; set; }

    public string? ExpectedContent { get; set; }

    public int CountThreshold { get; set; }

    public int ErrorInterval { get; set; }

    public int CheckInterval { get; set; }

    public bool Enabled { get; set; }
}
