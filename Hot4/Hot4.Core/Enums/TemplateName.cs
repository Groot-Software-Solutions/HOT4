﻿namespace Hot4.Core.Enums
{

    public enum TemplateName : int
    {
        UnknownRequest = 1,
        HelpRegister = 101,
        HelpRecharge = 102,
        HelpRechargePins = 103,
        HelpStock = 104,
        HelpBank = 105,
        HelpDefault = 106,
        HelpEcoCash = 109,
        HelpResetPin = 108,
        HelpDiscount = 109,
        HelpPinStock = 110,
        FailedRegistrationFormat = 201,
        FailedRegistrationDuplicate = 202,
        FailedNotRegistered = 203,
        FailedRechargeFormat = 204,
        FailedRechargeRange = 205,
        FailedRechargeVASDisabled = 206,
        FailedAccessCode = 207,
        FailedRechargeBalance = 208,
        FailedPinDenomination = 209,
        FailedVAS = 210,
        FailedTransferFormat = 211,
        FailedTransferMobile = 212,
        FailedTransferMin = 213,
        FailedTransferBalance = 214,
        FailedResend = 215,
        FailedDuplicate = 216,
        FailedRechargeMobile = 217,
        FailedTransferAccessCode = 218,
        FailedWebException = 219,
        FailedWebLogin = 220,
        FailedPhoneBalance = 221,
        FailedWebRechargeMinMax = 222,
        FailedZESAInvalidMeterNumber = 223,
        FailedUnsupportedBrand = 230,
        SuccessfulRegistration = 301,
        SuccessfulBalance = 302,
        SuccessfulRecharge_Dealer_Header = 303,
        SuccessfulRechargePin_Customer_Header = 304,
        SuccessfulRechargePin_Dealer = 305,
        SuccessfulRechargePin_Customer = 306,
        SuccessfulRechargeVAS_Customer = 307,
        SuccessfulTransferReceiver = 308,
        SuccessfulTransferSender = 309,
        SuccessfulReversalDealer = 310,
        SuccessfulReversalCustomer = 311,
        SuccessfulRechargeReceived = 312,
        SuccessfulZESATokenPurchase_Dealer = 313,
        SuccessfulZESATokenPurchase_Customer = 314,
        SuccessfulZESAStandardTemplate = 315,
        PendingZESATokenPurchase_API = 316,
        PendingZESATokenPurchase_Dealer = 317,
        SuccessfulNyaradzoPayment_Customer = 318,
        SuccessfulReservationConfirmation = 319,

        SuccessfulWebPhoneBalance = 402,
        SuccessfulWebServicePinHeader = 404,
        SuccessfulWebServiceVasCustomer = 407,
        SuccessfulWebServiceDataCustomer = 408,
        PaymentSuccessful = 500,
        PaymentFailed = 501,
        BulkSmsSuccessful = 502,
        BulkSmsFailed = 503,
        AnswerOK = 600,
        AnswerWrong = 601,
        NetworkTimeout = 700,
        NetworkConnectionIssue = 701,
        NetworkWebserviceUnavailable = 702,
        NetworkWebserviceError = 703,
        NetworkGeneralError = 704,
        AuditingAgentReferenceNotFound = 800,
    }
}