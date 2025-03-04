using Hot.SMPP.Interfaces;
using Inetlab.SMPP.Builders;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.PDU;
using Inetlab.SMPP;
using System.Security.Authentication;
using Hot.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Hot.SMPP.Extensions;
using Hot.Domain.Entities;
using Hot.Domain.Enums;

namespace Hot.SMPP.Service;

public class SMPPService : ISMPPService
{
    private List<Domain.Entities.SMPP> sMPPs = new();
    private readonly Dictionary<SmppClient, Domain.Entities.SMPP> clientSMPPs = new();
    private readonly IDbContext context;
    private readonly ILogger<SMPPService> logger;
    public SMPPService(IDbContext context, ILogger<SMPPService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task StartAsync()
    {
        await Initialize();
    }
    public async Task StopAsync()
    {
        logger.LogInformation("SMPP Service Stopping at {time}", DateTimeOffset.Now);
        foreach (var client in clientSMPPs)
        {
            await Disconnect(client.Key);
        }
        logger.LogInformation("SMPP Service stoped at: {time}", DateTimeOffset.Now);
    }
    public async Task RebindAsync()
    {
        foreach (var client in clientSMPPs)
        {
            await Rebind(client);
        }

    }
    public async Task SendPendingSMSAsync()
    {
        if (!CanSend())
        {
            logger.LogError("No Clients available to send SMSs.");
            return;
        }
        var response = await context.SMSs.OutboxAsync();
        if (response.IsT1)
        {
            logger.LogError(response.AsT1, "Pending SMS Load Error: {message}", response.AsT1.Message);
            return;
        }
        var smss = response.AsT0;

        foreach (var item in smss)
        {
            var client = GetClient();
            var smpp = GetSMPP(client);
            var sourceAddress = new SmeAddress(smpp.SourceAddress, (AddressTON)smpp.SourceAddressTon, (AddressNPI)smpp.SourceAddressNpi);
            var destinationAddress = new SmeAddress(item.Mobile.ToMSIDNMobileNumber(), (AddressTON)smpp.DestinationAddressTon, (AddressNPI)smpp.DestinationAddressNpi);

            ISubmitSmBuilder builder = Inetlab.SMPP.SMS.ForSubmit()
               .From(sourceAddress)
               .To(destinationAddress)
               .Coding(DataCodings.Default)
               .Text(item.SMSText);

            try
            {
                IList<SubmitSmResp> resp = await client.SubmitAsync(builder);

                if (!resp.All(x => x.Header.Status == CommandStatus.ESME_ROK))
                {
                    logger.LogWarning("Submit failed. Status: {data}", string.Join(",", resp.Select(x => x.Header.Status.ToString())));
                    item.State = new State() { StateID = (int)States.Failure };
                }
                else
                {
                    item.State = new State() { StateID = (int)States.Success };
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Submit failed. Error: {Message}", ex.Message);
                item.State = new State() { StateID = (int)States.Failure };
            }
            finally
            {
                try
                {
                    await context.SMSs.UpdateAsync(item);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to update SMS {SMSID} - {Message}", item.SMSID, ex.Message);
                }
            }
        }
    }
    public bool CanSend()
    {
        var clients = GetConnectedClients();
        if (!clients.Any()) return false;
        var canSendingSMPPs = clientSMPPs.Values.Where(c => c.AllowSend);
        if (!canSendingSMPPs.Any()) return false;
        return true;
    }

    private async Task Initialize()
    {
        logger.LogInformation("SMPP Service Starting at {time}", DateTimeOffset.Now);
        var SMPPResult = context.SMPPs.ListAsync();
        sMPPs = SMPPResult.Result.AsT0.Where(s => s.SmppEnabled).ToList();

        foreach (var smpp in sMPPs)
        {
            SmppClient _client = new();
            if (smpp.AllowReceive) _client.evDeliverSm += OnDeliverSmAsync;
            await Connect(smpp, _client);
            ;
            clientSMPPs.Add(_client, smpp);
        }
        logger.LogInformation("SMPP Started with {clients} SMPP Clients at {time}", clientSMPPs.Count, DateTimeOffset.Now);
        logger.LogInformation("Sending on {Senders} SMPP(s), Receiving on {Receivers} SMPP(s)", ClientsThatCanSend().Count, ClientsThatCanRecieve().Count);

    }
    private async Task Rebind(KeyValuePair<SmppClient, Domain.Entities.SMPP> client)
    {
        try
        {
            await Disconnect(client.Key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error disconnecting SMPP connection");
        }
        try
        {
            await Connect(client.Value, client.Key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error connecting to SMPP");
        }
    }
    private async Task Disconnect(SmppClient client)
    {
        logger.LogInformation("Disconnecting SMPP Client");
        if (!(client.Status == ConnectionStatus.Open || client.Status == ConnectionStatus.Bound))
        {
            logger.LogInformation("SMPP Client not connected - Connection State {Status}", client.Status);
            return;
        }

        if (client.Status == ConnectionStatus.Bound) await client.UnbindAsync();
        if (client.Status == ConnectionStatus.Open) await client.DisconnectAsync();

        if (client.Status == ConnectionStatus.Closed)
        {
            logger.LogInformation("SMPP Client Disconnected");
            return;
        }
        logger.LogInformation("SMPP Client failed to  Disconnected");
    }
    private async Task Connect(Domain.Entities.SMPP smpp, SmppClient _client)
    {
        logger.LogInformation("Attempting to connect to {Name} SMPP - {Client}", smpp.SmppName, _client.ClientIdentifier());
        _client.EsmeAddress = new SmeAddress(smpp.SourceAddress, (AddressTON)Convert.ToByte(smpp.SourceAddressTon), (AddressNPI)Convert.ToByte(smpp.SourceAddressNpi));
        _client.SystemType = smpp.SystemType;
        _client.EnabledSslProtocols = SslProtocols.None;
        _client.ConnectionRecovery = true;
        _client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(10);
        _client.SendSpeedLimit = LimitRate.NoLimit;

        if (await _client.ConnectAsync(smpp.RemoteHost, smpp.RemotePort))
        {
            BindResp bindResp = await _client.BindAsync(smpp.SystemID, smpp.SmppPassword, ConnectionMode.Transceiver);
            if (bindResp.Header.Status == CommandStatus.ESME_ROK)
            {
                logger.LogInformation("Bind succeeded: Status: {status}, SystemId: {systemid}", bindResp.Header.Status, bindResp.SystemId);
            }
            else
            {
                logger.LogInformation("Bind Result: {Message}", bindResp.Header.Status);
            }
        }
        logger.LogInformation("SMPP Connection Status - {Name} {State}", smpp.SmppName, _client.Status);
    }
    private List<SmppClient> ClientsThatCanSend()
    {
        var clients = GetConnectedClients();
        if (!clients.Any()) return new();
        var canSendingSMPPs = clientSMPPs.Where(c => c.Value.AllowSend).Select(c => c.Key);
        if (!canSendingSMPPs.Any()) return new();
        return canSendingSMPPs.ToList();
    }
    private List<SmppClient> ClientsThatCanRecieve()
    {
        var clients = GetConnectedClients();
        if (!clients.Any()) return new();
        var canReceiveSMPPs = clients.Where(c => GetSMPP(c).AllowReceive);
        if (!canReceiveSMPPs.Any()) return new();
        return canReceiveSMPPs.ToList();
    }
    private Domain.Entities.SMPP GetSMPP(SmppClient c)
    {
        return clientSMPPs[c];
    }
    private List<SmppClient> GetConnectedClients()
    {
        return clientSMPPs
            .Where(c => c.Key.Status == ConnectionStatus.Bound)
            .Select(c=> c.Key)
            .ToList();
    }
    private SmppClient GetClient()
    {
        var list = ClientsThatCanSend();
        return list.First();
    }
    private async void OnDeliverSmAsync(object o, DeliverSm deliverSm)
    {
        if (deliverSm.MessageType == MessageTypes.SMSCDeliveryReceipt)
        {
            logger.LogInformation("Delivery Receipt received");
        }
        else
        {

            logger.LogInformation("Incoming SMS received");
            var client = (SmppClient)o;
            var smpp = clientSMPPs[client];
            await SaveSMStoDatabaseAsync(deliverSm, (smpp ?? new()).SmppID);
        }
    }
    private async Task SaveSMStoDatabaseAsync(DeliverSm sms, int smppID)
    {
        var smsObject = new Domain.Entities.SMS
        {
            Direction = true,
            InsertDate = DateTime.Now,
            Mobile = sms.SourceMobile().ToMobileNumber(),
            SMSID_In = null,
            SMSText = sms.Text(),
            SmppID = smppID,
            State = new State() { StateID = (int)States.Pending },
            Priority = new Priority() { PriorityId = (int)Priorities.Normal }, 
            SMSDate = DateTime.Now,
        };
        var AddSMSResult = await context.SMSs.AddAsync(smsObject);
        if (AddSMSResult.IsT1) logger.LogError(AddSMSResult.AsT1, "SMS Save Error: {data}", AddSMSResult.AsT1.Message);
        logger.LogInformation("SMS Saved to DB {SMSID}", AddSMSResult.AsT0);
    }
}
