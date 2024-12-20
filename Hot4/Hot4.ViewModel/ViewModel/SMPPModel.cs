namespace Hot4.Core.DataViewModels
{
    public class SMPPModel
    {
        public byte SmppID { get; set; }
        public string SmppName { get; set; }
        public bool AllowSend { get; set; }
        public bool AllowReceive { get; set; }
        public bool SmppEnabled { get; set; }
        public int DestinationAddressNpi { get; set; }
        public int DestinationAddressTon { get; set; }
        public string SourceAddress { get; set; }
        public int SourceAddressNpi { get; set; }
        public int SourceAddressTon { get; set; }
        public int SmppTimeout { get; set; }
        public string RemoteHost { get; set; }
        public int RemotePort { get; set; }
        public string SystemID { get; set; }
        public string SmppPassword { get; set; }
        public string AddressRange { get; set; }
        public int InterfaceVersion { get; set; }
        public string SystemType { get; set; }
        public string EconetPrefix { get; set; }
        public string NetOnePrefix { get; set; }
        public string TelecelPrefix { get; set; }
    }
}
