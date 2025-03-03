Imports System.Data.SqlClient

Public Class xTemplate

    Public Enum Templates As Integer
        UnknownRequest = 1

        HelpRegister = 101
        HelpRecharge = 102
        HelpRechargePins = 103
        HelpStock = 104
        HelpBank = 105
        HelpDefault = 106
        HelpPinReset = 107
        HelpResetPin = 108
        HelpEcoCash = 109
        HelpLimit = 110
        HelpZESA = 111

        FailedRegistrationFormat = 201
        FailedRegistrationDuplicate = 202
        FailedNotRegistered = 203
        FailedRechargeFormat = 204
        FailedRechargeRange = 205
        FailedRechargeVASDisabled = 206
        FailedAccessCode = 207
        FailedRechargeBalance = 208
        FailedPinDenomination = 209
        FailedVAS = 210
        FailedTransferFormat = 211
        FailedTransferMobile = 212
        FailedTransferMin = 213
        FailedTransferBalance = 214
        FailedResend = 215
        FailedDuplicate = 216
        FailedRechargeMobile = 217
        FailedTransferAccessCode = 218
        FailedWebException = 219
        FailedWebLogin = 220
        FailedPhoneBalance = 221
        FailedWebRechargeMinMax = 222
        FailedZESAInvalidMeterNumber = 223
        FailedEconetDailyLimit = 224
        FailedEconetMonthlyLimit = 225
        FailedEconetWeeklyLimit = 226

        FailedUnsupportedBrand = 230


        SuccessfulRegistration = 301
        SuccessfulBalance = 302
        SuccessfulRecharge_Dealer_Header = 303
        SuccessfulRechargePin_Customer_Header = 304
        SuccessfulRechargePin_Dealer = 305
        SuccessfulRechargePin_Customer = 306
        SuccessfulRechargeVAS_Customer = 307
        SuccessfulTransferReceiver = 308
        SuccessfulTransferSender = 309
        SuccessfulReversalDealer = 310
        SuccessfulReversalCustomer = 311
        SuccessfulRechargeReceived = 312
        SuccessfulZESATokenPurchase_Dealer = 313
        SuccessfulZESATokenPurchase_Customer = 314
        SuccessfulZESAStandardTemplate = 315
        PendingZESATokenPurchase_API = 316
        PendingZESATokenPurchase_Dealer = 317
        SuccessfulNyaradzo = 318
        SuccessfulWebPhoneBalance = 402
        SuccessfulWebServicePinHeader = 404
        SuccessfulWebServiceVasCustomer = 407
        SuccessfulWebServiceDataCustomer = 408

        PaymentSuccessful = 500
        PaymentFailed = 501
        BulkSmsSuccessful = 502
        BulkSmsFailed = 503

        AnswerOK = 600
        AnswerWrong = 601

        NetworkTimeout = 700
        NetworkConnectionIssue = 701
        NetworkWebserviceUnavailable = 702
        NetworkWebserviceError = 703
        NetworkGeneralError = 704

        AuditingAgentReferenceNotFound = 800

    End Enum

    Private _TemplateID As Integer
    Public Property TemplateID() As Integer
        Get
            Return _TemplateID
        End Get
        Set(ByVal value As Integer)
            _TemplateID = value
        End Set
    End Property

    Private _TemplateName As String
    Public Property TemplateName() As String
        Get
            Return _TemplateName
        End Get
        Set(ByVal value As String)
            _TemplateName = value
        End Set
    End Property

    Private _TemplateText As String
    Public Property TemplateText() As String
        Get
            Return _TemplateText
        End Get
        Set(ByVal value As String)
            _TemplateText = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _TemplateID = sqlRdr("TemplateID")
        _TemplateName = sqlRdr("TemplateName")
        _TemplateText = sqlRdr("TemplateText")
    End Sub
End Class
Public Class xTemplateAdapter
    Public Shared Function SelectRow(ByVal TemplateID As Integer, ByVal sqlConn As SqlConnection) As xTemplate
        Dim iRow As xTemplate = Nothing
        Using sqlCmd As New SqlCommand("xTemplate_Select", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("TemplateID", TemplateID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iRow = New xTemplate(sqlRdr)
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function
End Class