#pragma warning disable IDE1006 // Naming Styles
using Hot.Application.Common.Extensions; 
using Hot.Ecocash.Domain.Enums;
using System;

namespace Hot.Ecocash.Domain.Entities;
public class Transaction : ResponseItem
{
     
    public string clientCorrelator { get; set; } = "";
    public long endTime { get; set; }
    public long startTime { get; set; }
    public string notifyUrl { get; set; } = "";
    public string referenceCode { get; set; } = "";
    public string endUserId { get; set; } = "";
    public string serverReferenceCode { get; set; } = "";
    public string transactionOperationStatus { get; set; } = "";
    public PaymentAmountResponse paymentAmount { get; set; } = new PaymentAmountResponse();
    public string ecocashReference { get; set; } = "";
    public string merchantCode { get; set; } = "";
    public string merchantPin { get; set; } = "";

    public string merchantNumber { get; set; } = "";

    public string notificationFormat { get; set; } = "";
    public string originalServerReferenceCode { get; set; } = "";

    private Currencies _currencyCode = Currencies.ZiG;
    public string currencyCode
    {
        get => Enum.GetName(typeof(Currencies), _currencyCode).Replace("_", "-");
        set
        {
            _ = Enum.TryParse(value.Replace("-", "_"), out _currencyCode);
        }
    }
    public DateTime EndDate
    {
        get => endTime.ToDateTimeFromUnixTime();
    }

    public DateTime StartDate
    {
        get => startTime.ToDateTimeFromUnixTime();
    }





}

#pragma warning restore IDE1006 // Naming Styles
