using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblSmpp
{
    [Key]
    public byte SmppId { get; set; }

    public required string SmppName { get; set; }

    public bool AllowSend { get; set; }

    public bool AllowReceive { get; set; }

    public bool SmppEnabled { get; set; }

    public int DestinationAddressNpi { get; set; }

    public int DestinationAddressTon { get; set; }

    public required string SourceAddress { get; set; }

    public int SourceAddressNpi { get; set; }

    public int SourceAddressTon { get; set; }

    public int SmppTimeout { get; set; }

    public required string RemoteHost { get; set; }

    public int RemotePort { get; set; }

    public required string SystemId { get; set; }

    public required string SmppPassword { get; set; }

    public required string AddressRange { get; set; }

    public int InterfaceVersion { get; set; }

    public required string SystemType { get; set; }

    public required string EconetPrefix { get; set; }

    public required string NetOnePrefix { get; set; }

    public required string TelecelPrefix { get; set; }

    public virtual ICollection<TblSms> TblSms { get; set; } = new List<TblSms>();
}
