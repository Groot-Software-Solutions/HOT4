using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblAddress
{
    [Key]
    public long AccountId { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? ContactName { get; set; }

    public string? ContactNumber { get; set; }

    public string? VatNumber { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public long? SageId { get; set; }

    public byte? InvoiceFreq { get; set; }

    public long? SageIdusd { get; set; }

    public bool? Confirmed { get; set; }

    public virtual TblAccount Account { get; set; }
}
