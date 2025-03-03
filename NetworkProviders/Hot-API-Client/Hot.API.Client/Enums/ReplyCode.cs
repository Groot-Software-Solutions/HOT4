using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client
{
    public enum ReplyCode
    {
        Success = 2,
        FailedNetworkPrepaidPlatform = 210,
        FailedRechargeAmountLimits = 222,
        FailedRechargeInvalidEndUserOrNetwork = 217,
        FailedInsufficeintWalletBalance = 208,
        FailedRechargePlatformDisabled = 206,
        FailedPinDenomination = 209,
        FailedWebException = 219,
        FailedWebLogin = 220,
        FailedBalanceRequest = 221,
        FailedBulkPinDenomination = 223,
        FailedDuplicate = 216,
        FailedTransfer = 218,
        SuccessfulRegistration = 301,
        SuccessfulBalance = 302,
        SuccessfulRecharge = 303,
        SuccessfulRechargePin = 304,
        SuccessfulRechargePinDealer = 305,
        SuccessfulRechargePinCustomer = 306,
        SuccessfulRechargeVASCustomer = 307,
        SuccessfulTransferReceiver = 308,
        SuccessfulTransferSender = 309,
        SuccessfulReversalDealer = 310,
        SuccessfulReversalCustomer = 311,
        SuccessRechargeReceived = 312,
        SuccessfulWebPhoneBalance = 402,
        SuccessfulWebPinCustomerHeader = 404,
        SuccessfulWebVASCustomer = 407,
        SuccessfulBulkEvdPinRequest = 408,
        PaymentSuccessful = 500,
        PaymentFailed = 501,
        NetworkTimeout = 700,
        NetworkConnectionIssue = 701,
        NetworkWebserviceUnavailable = 702,
        NetworkWebserviceError = 703,
        NetworkGeneralError = 704,


    }
}
