namespace Hot4.DataModel.Models;

public partial class TblErrorLogContact
{
    public int ErrorLogtypeId { get; set; }

    public int TestType { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactMobile { get; set; }
}
