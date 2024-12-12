using Hot4.Core.DataViewModels;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class SMPPRepository : RepositoryBase<TblSmpp>, ISMPPRepository
    {
        public SMPPRepository(HotDbContext context) : base(context) { }
        public async Task<List<SMPPModel>> ListSMPP()
        {
            return await GetAll().Select(d => new SMPPModel
            {

                AddressRange = d.AddressRange,
                AllowSend = d.AllowSend,
                AllowReceive = d.AllowReceive,
                DestinationAddressNpi = d.DestinationAddressNpi,
                DestinationAddressTon = d.DestinationAddressTon,
                SourceAddress = d.SourceAddress,
                SourceAddressTon = d.SourceAddressTon,
                EconetPrefix = d.EconetPrefix,
                SourceAddressNpi = d.SourceAddressNpi,
                InterfaceVersion = d.InterfaceVersion,
                NetOnePrefix = d.NetOnePrefix,
                RemoteHost = d.RemoteHost,
                RemotePort = d.RemotePort,
                SmppEnabled = d.SmppEnabled,
                SmppID = d.SmppId,
                SmppName = d.SmppName,
                SmppPassword = d.SmppPassword,
                SmppTimeout = d.SmppTimeout,
                SystemID = d.SystemId,
                SystemType = d.SystemType,
                TelecelPrefix = d.TelecelPrefix,
            }).ToListAsync();


        }
    }
}
