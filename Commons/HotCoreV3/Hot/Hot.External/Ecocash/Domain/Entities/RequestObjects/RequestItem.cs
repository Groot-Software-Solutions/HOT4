#pragma warning disable IDE1006 // Naming Styles
using Hot.Ecocash.Application.Common;
using Hot.Ecocash.Domain.Enums; 

namespace Hot.Ecocash.Domain.Entities;
public class RequestItem
{
    internal string _endUserId = "";

    internal TransactionOperationStatus _transactionOperationStatus = TransactionOperationStatus.Charged;

    public RequestItem(ServiceOptions options)
    {
        merchantCode = options.MechantCode;
        merchantNumber = options.MerchantNumber;
        merchantPin = options.MerchantPin;
        notifyUrl = options.HotRechargeReturnURL;
    }

    public string clientCorrelator { get; set; }

    public string endUserId
    {
        get
        {
            return "tel:" + _endUserId;
        }

        set
        {
            if (!value.StartsWith("+") & value.StartsWith("0"))
                value = string.Concat("+263", value.AsSpan(1));
            _endUserId = value;
        }
    }


    public string merchantCode { get; set; }


    public string merchantPin { get; set; }

    public string merchantNumber { get; set; }

    public PaymentAmount paymentAmount { get; set; }

    public string referenceCode { get; set; }

    public string transactionOperationStatus
    {
        get
        {
            return Enum.GetName(typeof(TransactionOperationStatus), _transactionOperationStatus);
        }
        set
        {
            _ = Enum.TryParse(value, out _transactionOperationStatus);
        }
    }
     
    public string currencyCode { get; set; }
    public string notifyUrl { get; set; }

    public string remark { get; set; }
}
#pragma warning restore IDE1006 // Naming Styles

