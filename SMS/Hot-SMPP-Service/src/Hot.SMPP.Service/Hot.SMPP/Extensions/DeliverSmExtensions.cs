using Inetlab.SMPP.Common;
using Inetlab.SMPP.PDU;
using Inetlab.SMPP;

namespace Hot.SMPP.Extensions;

public static class DeliverSmExtensions
{
    public static string Text(this DeliverSm deliverSm) => deliverSm.GetMessageText(EncodingMapper.Default);

    public static string SourceMobile(this DeliverSm deliverSm) => deliverSm.SourceAddress.Address;

    public static string DestinationMobile(this DeliverSm deliverSm) => deliverSm.DestinationAddress.Address;

    public static string ToMSIDNMobileNumber(this string Number)
    {
        if (Number.StartsWith("+263")) return $"263{Number[4..]}";
        if (Number.StartsWith("0")) return $"263{Number[1..]}";
        if (Number.StartsWith("6")) return $"266{Number}";
        if (Number.StartsWith("+266")) return $"266{Number[4..]}";
        return Number;
    }
    public static string ToMobileNumber(this string Number)
    {
        if (Number.StartsWith("263")) return $"0{Number[3..]}";
        if (Number.StartsWith("+263")) return $"0{Number[4..]}";
        if (Number.StartsWith("+266")) return $"0{Number[4..]}";
        if (Number.StartsWith("266")) return $"0{Number[3..]}";
        return Number;
    }

    public static string ClientIdentifier(this SmppClient _client)
    {
        return $"{_client.RemoteEndPoint}-{_client.SystemID}";
    }
}