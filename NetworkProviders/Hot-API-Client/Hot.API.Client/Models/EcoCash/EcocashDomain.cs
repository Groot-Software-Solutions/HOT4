#pragma warning disable IDE1006 // Naming Styles

using Hot.API.Client.Ecocash.Domain.Enums;
using System; 


namespace Hot.API.Client.Ecocash.Domain.Entities
{
    public class ResponseItem
    {
        public int id { get; set; } = 0;
        public decimal version { get; set; } = 0m;
    }
    public class Transaction : ResponseItem
    {
        public string clientCorrelator { get; set; } = "";
       
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

        //public long endTime { get; set; }
        //public long startTime { get; set; }
        //public DateTime EndDate
        //{
        //    get => ToDateTime(endTime);
        //}
        //public DateTime StartDate
        //{
        //    get => ToDateTime(startTime);
        //}

        public static DateTime ToDateTime(long unixtime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(unixtime);
        }

    }

    public partial class PaymentAmountResponse : ResponseItem
    {
        private string _totalAmountCharged = "";
        public ChargingInformationResponse charginginformation = new ChargingInformationResponse();
        public ChargingMetaDataResponse chargeMetaData = new ChargingMetaDataResponse();

        public string totalAmountCharged
        {
            get
            {
                return _totalAmountCharged;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "";
                if (value.Contains("null"))
                    value = "";
                _totalAmountCharged = value;
            }
        }
    }
    public class ChargingMetaDataResponse : ChargingMetaData
    {
        public int id { get; set; } = 0;
        public decimal version { get; set; } = 0m;
        private string _serviceId { get; set; } = "";
        public string purchaseCategoryCode { get; set; } = "Airtime";

        public string serviceId
        {
            get
            {
                return _serviceId;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "";
                if (value.Contains("null"))
                    value = "";
                _serviceId = value;
            }
        }
    }


    public class ChargingInformationResponse : ChargingInformation
    {
        public int id { get; set; } = 0;
        public decimal version { get; set; } = 0m;
    }
}

namespace Hot.API.Client.Ecocash.Domain.Entities
{
    public class RequestRefund : RequestItem
    {
        public string originalEcocashReference { get; set; }
        public string tranType { get; set; } = "REF";
        public string superMerchantName { get; set; } = "CommShop";
        public string merchantName { get; set; } = "HotRecharge";

    }
    public class RequestItem
    {
        internal string _endUserId = "";

        internal TransactionOperationStatus _transactionOperationStatus = TransactionOperationStatus.Charged;


        public string clientCorrelator { get; set; }

        public string endUserId
        {
            get
            {
                return "tel:" + _endUserId;
            }

            set
            {
                if (!value.StartsWith("+") && value.StartsWith("0"))
                    value = "+263" + value[1..];
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
                Enum.TryParse(value, out _transactionOperationStatus);
            }
        }

        public string notifyUrl { get; set; }

        public string remark { get; set; }
    }
    public class PaymentAmount
    {
        public ChargingInformation charginginformation { get; set; } = new ChargingInformation();
        public ChargingMetaData chargeMetaData { get; set; } = new ChargingMetaData();

        public PaymentAmount(decimal amount, string desc)
        {
            charginginformation.amount = amount;
            charginginformation.description = desc;
        }
        public PaymentAmount(decimal amount, string desc, string onBeHalfOf) : this(amount, desc)
        {
            chargeMetaData.onBeHalfOf = onBeHalfOf;

        }
    }
    public class ChargingInformation
    {
        public decimal amount { get; set; } = 0m;
        public string description { get; set; } = "";

        private Currencies _currency = Currencies.USD;
        public string currency
        {
            get
            {
                return Enum.GetName(typeof(Currencies), _currency);
            }
            set
            {
                Enum.TryParse(value, out _currency);
            }
        }
    }
    public class ChargingMetaData
    {
        private PaymentChannel _channel = PaymentChannel.SMS;
        public string channel
        {
            get
            {
                return Enum.GetName(typeof(PaymentChannel), _channel);
            }

            set
            {
                Enum.TryParse(value, out _channel);
            }
        }

        public string onBeHalfOf { get; set; } = "CommShop";
    }
}

namespace Hot.API.Client.Ecocash.Domain.Enums
{
    public enum Currencies : int
    {
        USD = 1
    }
    public enum PaymentChannel : int
    {
        WAP = 1,
        Web = 2,
        SMS = 3
    }
    public enum TransactionOperationStatus : int
    {
        Charged = 1,
        Refunded = 2,
        Refund = 7,
        ListTransaction = 3,
        Processing = 4,
        Denied = 5,
        Refused = 6
    }
}


#pragma warning restore IDE1006 // Naming Styles