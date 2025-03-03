Imports log4net
Imports Microsoft.VisualBasic
Imports HOT5.Common

Public Class Common
    'Public Const Conn As String = "data source=HOT5;initial catalog=HOT4;persist Security Info=True; User ID=iis_hot_web;Password=H0t5$t93nn6#08642"
    'Public Const EconetIPAdd = "10.10.11.60"
    Public Shared Conn As String = ConfigurationManager.AppSettings.Get("SQLCon")
    Public Shared EconetIPAdd = ConfigurationManager.AppSettings.Get("HotServerIP")
    'uid=showzim;pwd=D4nc3r;"
    'Public Const Conn As String = "data source=10.10.11.60;initial catalog=HOT4;uid=hotadmin;pwd=3c0n3753rv3r"
    Public Structure xSystemStatus
        Public Status As String
        Public Message As String

    End Structure
    Public Structure xReportItem
        Public ItemType As Integer
        Public TransactionDate As String
        Public Mobile As String
        Public Amount As Decimal
        Public State As String
        Public AccessCode As String
        Public AccessName As String
        Public Reference As String
        Public TranType As String
        Public Source As String                                
    End Structure
    Public Shared Function EmailAddressCheck(ByVal emailAddress As String) As Boolean

        Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
        Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)
        If emailAddressMatch.Success Then
            EmailAddressCheck = True
        Else
            EmailAddressCheck = False
        End If

    End Function

    Public Shared Function IsValidNumber(Number As String) As Boolean
        Dim isValid As Match = Regex.Match(Number, "^[0](((71)|(73)|(77)|(78))\d|(8644))\d\d\d\d\d\d$")
        Return If(isValid.Success, True, False)
    End Function
    Public Shared Function Sanitize(ByVal input As String) As String
        input = Regex.Escape(input.Replace(";", ""))
        Return input.Replace("\", "")
    End Function

    'Private Function Transfer(ByVal iTransferTo As Long, ByVal iAmount As Decimal) As Boolean

    '    Try
    '        Using sqlConn As New SqlConnection(Conn)

    '            sqlConn.Open()
    '            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
    '            If iAmount < iConfig.MinTransfer Then
    '                Throw New Exception("Minimum transfer value is " & FormatNumber(iConfig.MinTransfer, 0))
    '            End If

    '            Dim aCookie As HttpCookie = Request.Cookies("HOTLogin")
    '            Dim iAccount As xAccount = xAccountAdapter.SelectRow(aCookie.Item("AccountID"), sqlConn)

    '            If iAmount > iAccount.Balance Then
    '                Throw New Exception("Insufficient Balance")
    '            End If

    '            Dim iTransferFrom As Long = CLng(iAccount.AccountID)
    '            Dim iAccess As xAccess = Session.Item("HOTAccess")

    '            Dim iAccountTo As xAccount = xAccountAdapter.SelectRow(iTransferTo, sqlConn)
    '            Dim iAccessTo As xAccess = xAccessAdapter.SelectRow(xAccessAdapter.AdminSelect(iTransferTo, sqlConn), sqlConn)
    '            Dim Amount As Decimal = iAmount

    '            'Payment From
    '            Dim iPaymentFrom As New xPayment
    '            iPaymentFrom.Reference = "Transfer, $" & FormatNumber(Amount, 0) & " from " & iAccount.AccountName & " to " & iAccountTo.AccountName & ", " & Format(Now, "dd MMM yyyy HH:mm")
    '            iPaymentFrom.AccountID = iAccess.AccountID
    '            iPaymentFrom.Amount = (0 - Amount)
    '            iPaymentFrom.PaymentDate = Format(Now, "dd MMM yyyy HH:mm:ss")
    '            iPaymentFrom.LastUser = "HOT Web Service"
    '            iPaymentFrom.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.HOTTransfer
    '            iPaymentFrom.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.HOTDealer
    '            xPaymentAdapter.Save(iPaymentFrom, sqlConn)

    '            'Payment To
    '            Dim iPaymentTo As New xPayment
    '            iPaymentTo.Reference = iPaymentFrom.Reference
    '            iPaymentTo.AccountID = iAccessTo.AccountID
    '            iPaymentTo.Amount = Amount
    '            iPaymentTo.PaymentDate = iPaymentFrom.PaymentDate
    '            iPaymentTo.LastUser = iPaymentFrom.LastUser
    '            iPaymentTo.PaymentType = iPaymentFrom.PaymentType
    '            iPaymentTo.PaymentSource = iPaymentFrom.PaymentSource
    '            xPaymentAdapter.Save(iPaymentTo, sqlConn)

    '            'Transfer
    '            Dim iTransfer As New xTransfer
    '            iTransfer.Amount = Amount
    '            iTransfer.Channel.ChannelID = xChannel.Channels.Web
    '            iTransfer.PaymentID_From = iPaymentFrom.PaymentID
    '            iTransfer.PaymentID_To = iPaymentTo.PaymentID
    '            iTransfer.TransferDate = Now
    '            iTransfer.SMSID = 0
    '            xTransferAdapter.Save(iTransfer, sqlConn)

    '            'Refresh Balance
    '            iAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
    '            txtAmount.Text = ""
    '            BRtxtAmount.Text = ""
    '            ShowInfo("Transfer from " + iAccount.AccountName + " to " + iAccountTo.AccountName + " for " + FormatCurrency(iAmount, 2, TriState.True, , TriState.True) + " has been processed.", Me)
    '        End Using


    '        Return True
    '    Catch ex As Exception
    '        ShowEx(ex.Message, Me)
    '        Return True
    '    End Try
    'End Function

End Class
