Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports HOT5.Common
Imports System.Data.SqlClient


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class CBZ

    Inherits System.Web.Services.WebService
    Public Const ConnString As String = "data source=HOT5;initial catalog=HOT4;persist Security Info=True; User ID=iis_hot_web;Password=H0t5$t93nn6#08642"
    Public Const AllowAutoPayments As Boolean = False
    Public Const AllowLargePayments As Boolean = False
    Public Const LargePaymentAmount As Decimal = 100000

    <WebMethod()>
    Public Function BankStatementLine(ByVal AccessCode As String, ByVal AccessPassword As String,
                                      ByVal Amount As Decimal, ByVal TrxDate As Date, ByVal CBZBankTrxType As String,
                                      ByVal ClientIdentifier As String, ByVal ClientReference As String, ByVal Branch As String,
                                      ByVal BankReferenceID As String, ByVal EndBalance As Decimal) _
    As ReturnObject
        Dim iRet As New ReturnObject
        Dim iBankTrx As New xBankTrx
        Dim iBanktrxBatch As New xBankTrxBatch

        Try
            Using sqlConn As New SqlConnection(ConnString)
                sqlConn.Open()
                Dim iAccess As xAccess = xAccessAdapter.SelectLogin(AccessCode, AccessPassword, sqlConn)
                If iAccess Is Nothing Or Not (AccessCode.ToUpper() = "cbz@hot.co.zw".ToUpper()) Then
                    iRet.ReturnCode = -1
                    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebLogin, sqlConn)
                    iRet.ReturnMsg = iReply.TemplateID & "," & iReply.TemplateText
                    iRet.ReturnValue = 0
                    Return iRet
                End If
                'Get the requesting site's address to check 
                Log("WebService", "Banks", "CBZ", "Secure:" & Context.Request.IsSecureConnection.ToString & "| remote_addr:" &
                    Context.Request.ServerVariables("remote_addr") & "| Cookie:" & Context.Request.ServerVariables("CERT_COOKIE") &
                    "| ALL_RAW:" & Context.Request.ServerVariables("ALL_RAW") & " | AccessCode: " & AccessCode & "| " & BankReferenceID &
                    "| ClientIdentifier:" & ClientIdentifier & "| ClientReference: " & ClientReference & "| TrxType: " & CBZBankTrxType &
                    "| Amount:" & Amount)
                'get the current batch ID if one was done today, else create a new one
                iBanktrxBatch.BankID = xBank.Banks.CBZ
                iBanktrxBatch.LastUser = "IIS_Hot_Web"
                iBanktrxBatch.BatchDate = Now
                iBanktrxBatch.BatchReference = "CBZwebPost:" & Format(DateTime.Now(), "dd/MMM/yyyy")
                iBanktrxBatch = xBankTrxBatchAdapter.GetCurrentBatch(iBanktrxBatch, sqlConn)

                'record the EcoCash request as a pending bank transaction for another handler to pick up
                iBankTrx.BankTrxBatchID = iBanktrxBatch.BankTrxBatchID
                Select Case UCase(CBZBankTrxType)
                    Case "CASH DEPOSIT", "CASH DEPOSIT M"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.CashDeposit
                    Case "REVERSAL"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.CashdepositReversal
                    Case "CASH WITHDRAWAL", "ATM WDL"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.CashWithdrawal
                    Case "CHEQUE DEPOSIT"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.ChequeDeposit
                    Case "CHEQUE WITHDRAWAL"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.ChequePayment
                    Case "INTERNET BANKING INTER A/C TRANSFER"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.eBankingTrfr
                    Case "OTT COMMISSION CHARGE", "CASH WITHDRAWAL CHARGE"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.EmailCharge
                    Case "DEBIT", "SUNDRY DEBIT",
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.JnlDebit
                    Case "DEBIT REVERSAL"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.JnlDebitReversal
                    Case "TRANSFER CHARGE", "RTGS TRF CHARGE", "INTERMEDIATED MONEY TRANSFER TAX", "TRANSFER HANDLING FEE"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.RtgsCharge
                    Case "INTERNET BANKING THIRD PARTY PMT", "TELEGRAPHIC TRANSFER", "RTGS TRF",
                        "ATM FUNDS TRANSFER", "PAYMENT AS PER YOUR INSTRUCTIONS"
                        If Amount < 0 Then
                            iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.RTGSPayment
                        Else
                            iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.RTGSReceipt
                        End If
                    Case "INCOMING BANK TRANFER", "INCOMING INTERBANK FUNDS TRANSFER", "SUNDRY CREDIT"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.SalaryCredit
                    Case Else
                        If Amount < 0 Then
                            iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.MiscDebit
                            ClientReference = CBZBankTrxType & ": " & ClientReference
                        Else
                            iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.MiscCredit
                            ClientReference = CBZBankTrxType & ": " & ClientReference
                        End If
                End Select

                iBankTrx.RefName = ClientReference
                iBankTrx.BankTrxState.BankTrxStateID = If(AllowAutoPayments, xBankTrxState.BankTrxStates.Pending, xBankTrxState.BankTrxStates.ToBeAllocated)
                iBankTrx.Amount = Amount
                iBankTrx.TrxDate = TrxDate
                iBankTrx.Identifier = ClientIdentifier
                iBankTrx.Branch = Branch
                iBankTrx.BankRef = BankReferenceID
                iBankTrx.Balance = EndBalance
                xBankTrxAdapter.Insert(iBankTrx, sqlConn)

                iRet.ReturnCode = ReturnObject.Returncodes.Success
                iRet.ReturnMsg = "Bank Statement Line loaded"
                iRet.ReturnValue = iBankTrx.BankTrxID
                sqlConn.Close()
            End Using


        Catch ex As Exception
            Log("Webservice", "CBZ", "BankStatementLine", ex.Message)
            iRet.ReturnCode = ReturnObject.Returncodes.Failed
            iRet.ReturnMsg = "Insert failed"
            iRet.ReturnValue = -1
        Finally

        End Try



        Try
            If AllowAutoPayments Then
                InsertPayment(iBankTrx)
            End If

        Catch ex As Exception
            Log("Webservice", "CBZ", "Call Payment Insert", ex.Message)

        End Try
        Return iRet
    End Function

    <WebMethod()>
    Public Function BankStatementLineMultiCurrency(ByVal AccessCode As String, ByVal AccessPassword As String,
                                      ByVal Amount As Decimal, ByVal TrxDate As Date, ByVal CBZBankTrxType As String,
                                      ByVal ClientIdentifier As String, ByVal ClientReference As String, ByVal Branch As String,
                                      ByVal BankReferenceID As String, ByVal EndBalance As Decimal, ByVal Currency As String) _
    As ReturnObject
        Dim iRet As New ReturnObject
        Dim iBankTrx As New xBankTrx
        Dim iBanktrxBatch As New xBankTrxBatch

        Try
            Using sqlConn As New SqlConnection(ConnString)
                sqlConn.Open()
                Dim iAccess As xAccess = xAccessAdapter.SelectLogin(AccessCode, AccessPassword, sqlConn)
                If iAccess Is Nothing Or Not (AccessCode.ToUpper() = "cbz@hot.co.zw".ToUpper()) Then
                    iRet.ReturnCode = -1
                    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebLogin, sqlConn)
                    iRet.ReturnMsg = iReply.TemplateID & "," & iReply.TemplateText
                    iRet.ReturnValue = 0
                    Return iRet
                End If
                'Get the requesting site's address to check 
                Log("WebService", "Banks", "CBZ", "Secure:" & Context.Request.IsSecureConnection.ToString & "| remote_addr:" &
                    Context.Request.ServerVariables("remote_addr") & "| Cookie:" & Context.Request.ServerVariables("CERT_COOKIE") &
                    "| ALL_RAW:" & Context.Request.ServerVariables("ALL_RAW") & " | AccessCode: " & AccessCode & "| " & BankReferenceID &
                    "| ClientIdentifier:" & ClientIdentifier & "| ClientReference: " & ClientReference & "| TrxType: " & CBZBankTrxType &
                    "| Amount:" & Amount)
                'get the current batch ID if one was done today, else create a new one
                iBanktrxBatch.BankID = xBank.Banks.CBZ
                iBanktrxBatch.LastUser = "IIS_Hot_Web"
                iBanktrxBatch.BatchDate = Now
                iBanktrxBatch.BatchReference = "CBZwebPost:" & Format(DateTime.Now(), "dd/MMM/yyyy")
                iBanktrxBatch = xBankTrxBatchAdapter.GetCurrentBatch(iBanktrxBatch, sqlConn)

                'record the EcoCash request as a pending bank transaction for another handler to pick up
                iBankTrx.BankTrxBatchID = iBanktrxBatch.BankTrxBatchID
                Select Case UCase(CBZBankTrxType)
                    Case "CASH DEPOSIT", "CASH DEPOSIT M"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.CashDeposit
                    Case "REVERSAL"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.CashdepositReversal
                    Case "CASH WITHDRAWAL", "ATM WDL"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.CashWithdrawal
                    Case "CHEQUE DEPOSIT"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.ChequeDeposit
                    Case "CHEQUE WITHDRAWAL"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.ChequePayment
                    Case "INTERNET BANKING INTER A/C TRANSFER"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.eBankingTrfr
                    Case "OTT COMMISSION CHARGE", "CASH WITHDRAWAL CHARGE"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.EmailCharge
                    Case "DEBIT", "SUNDRY DEBIT",
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.JnlDebit
                    Case "DEBIT REVERSAL"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.JnlDebitReversal
                    Case "TRANSFER CHARGE", "RTGS TRF CHARGE", "INTERMEDIATED MONEY TRANSFER TAX", "TRANSFER HANDLING FEE"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.RtgsCharge
                    Case "INTERNET BANKING THIRD PARTY PMT", "TELEGRAPHIC TRANSFER", "RTGS TRF",
                        "ATM FUNDS TRANSFER", "PAYMENT AS PER YOUR INSTRUCTIONS"
                        If Amount < 0 Then
                            iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.RTGSPayment
                        Else
                            iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.RTGSReceipt
                        End If
                    Case "INCOMING BANK TRANFER", "INCOMING INTERBANK FUNDS TRANSFER", "SUNDRY CREDIT"
                        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.SalaryCredit
                    Case Else
                        If Amount < 0 Then
                            iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.MiscDebit
                            ClientReference = CBZBankTrxType & ": " & ClientReference
                        Else
                            iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.MiscCredit
                            ClientReference = CBZBankTrxType & ": " & ClientReference
                        End If
                End Select

                iBankTrx.RefName = ClientReference
                iBankTrx.BankTrxState.BankTrxStateID = If(AllowAutoPayments, xBankTrxState.BankTrxStates.Pending, xBankTrxState.BankTrxStates.ToBeAllocated)
                iBankTrx.Amount = Amount
                iBankTrx.TrxDate = TrxDate
                iBankTrx.Identifier = ClientIdentifier
                iBankTrx.Branch = Branch
                iBankTrx.BankRef = BankReferenceID
                iBankTrx.Balance = EndBalance
                xBankTrxAdapter.Insert(iBankTrx, sqlConn)

                iRet.ReturnCode = ReturnObject.Returncodes.Success
                iRet.ReturnMsg = "Bank Statement Line loaded"
                iRet.ReturnValue = iBankTrx.BankTrxID
                sqlConn.Close()
            End Using


        Catch ex As Exception
            Log("Webservice", "CBZ", "BankStatementLine", ex.Message)
            iRet.ReturnCode = ReturnObject.Returncodes.Failed
            iRet.ReturnMsg = "Insert failed"
            iRet.ReturnValue = -1
        Finally

        End Try



        Try
            If AllowAutoPayments Then
                InsertPayment(iBankTrx, Currency)
            End If

        Catch ex As Exception
            Log("Webservice", "CBZ", "Call Payment Insert", ex.Message)

        End Try
        Return iRet
    End Function

    'If iTrx.Identifier.Length < 10 Or Not iTrx.Identifier.StartsWith("07") Then
    '                            iTrx.Identifier = ""
    ''Dim emailID As New List(Of String)
    '                            For Each emailID As String In i.TransactionDescription.Split(" ")
    '                                If System.Text.RegularExpressions.Regex.Match(emailID, "^(("".+?"")|([0-9a-zA-Z](((\.(?!\.))|([-!#\$%&'\*\+/=\?\^`\{\}\|~\w]))*[0-9a-zA-Z])*))@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$", RegexOptions.IgnoreCase).Success Then
    '                                    iTrx.Identifier = emailID
    '                                End If
    '                            Next
    '                        End If


    'Used for all banks
    Private Sub InsertPayment(ByVal iBankTrx As xBankTrx, Optional ByVal currency As String = "ZiG")

        Using sqlConn As New SqlConnection(ConnString)
            sqlConn.Open()

            Dim updatedBanktrx As xBankTrx = xBankTrxAdapter.GetFromBankTrxID(iBankTrx.BankTrxID, sqlConn)
            If Not (updatedBanktrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Pending And (updatedBanktrx.PaymentID = 0 Or updatedBanktrx.PaymentID Is Nothing)) Then Exit Sub

            Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction

            Try
                'Check if transaction identifier is an access code
                If IsNumeric(iBankTrx.Identifier) Then
                    If iBankTrx.Identifier.Length = 12 Then

                        If Left(iBankTrx.Identifier, 3) = "263" Then
                            iBankTrx.Identifier = "0" & Right(iBankTrx.Identifier, 9)
                        Else

                        End If
                    End If
                End If

                Dim iAccess As xAccess = xAccessAdapter.SelectCode(iBankTrx.Identifier, sqlConn, sqlTrans)
                'If it is a valid access code
                If AllowLargePayments = False And iBankTrx.Amount > LargePaymentAmount Then

                Else
                    If iAccess IsNot Nothing Then
                    'Insert Payment
                    Dim iPayment As New xPayment
                    iPayment.PaymentID = 0
                    iPayment.AccountID = iAccess.AccountID
                    iPayment.Amount = iBankTrx.Amount
                    iPayment.LastUser = "iis_hot_web"
                    iPayment.PaymentDate = Now
                    iPayment.PaymentSource.PaymentSourceID = If(currency = "USD", 22, xPaymentSource.PaymentSources.CBZ)
                    iPayment.PaymentType.PaymentTypeID = If(currency = "USD", 17, xPaymentType.PaymentTypes.BankAuto)
                    iPayment.Reference = iBankTrx.RefName
                    xPaymentAdapter.Save(iPayment, sqlConn, sqlTrans)
                    iBankTrx.PaymentID = iPayment.PaymentID
                    iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Success
                    xBankTrxAdapter.Save(iBankTrx, sqlConn, sqlTrans)

                    'SMS
                    Dim iSMS As New xSMS
                    iSMS.Direction = False
                    iSMS.Mobile = iAccess.AccessCode
                    iSMS.Priority.PriorityID = xPriority.Priorities.Normal
                    iSMS.State.StateID = xState.States.Pending
                    iSMS.SMSDate = Now
                    iSMS.SMSText = "Payment Received: " & iPayment.PaymentSource.PaymentSource & vbNewLine &
                            "Amount: " & FormatNumber(iPayment.Amount, 2) & vbNewLine &
                            "Balance: " & FormatNumber(xAccountAdapter.SelectRow(iPayment.AccountID, sqlConn, sqlTrans).Balance, 2) & vbNewLine &
                            "Ref: " & iPayment.Reference & vbNewLine &
                            "Source: CBZ " & iBankTrx.Branch & vbNewLine &
                            "HOT Recharge - your favourite service" 'Removed as not getting Text for iPayment.PaymentSource.PaymentSource 
                    xSMSAdapter.Save(iSMS, sqlConn, sqlTrans)
                Else
                    iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.ToBeAllocated
                    xBankTrxAdapter.UpdateState(iBankTrx, sqlConn, sqlTrans)
                End If
                    sqlTrans.Commit()

                End If
            Catch ex As Exception
                Log("Webservice", "CBZ", "InsertPayment", iBankTrx.BankTrxID & "|" & ex.InnerException.ToString)
                sqlTrans.Rollback()
            Finally
                sqlConn.Close()
            End Try
            sqlConn.Close()
        End Using
    End Sub

    Private Sub Log(ByVal LogModule As String, ByVal LogObject As String, ByVal LogMethod As String, ByVal LogDescription As String)
        Try
            Using sqlConn As New SqlConnection(ConnString)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    Dim iLog As New xLog
                    iLog.LogModule = LogModule
                    iLog.LogObject = LogObject
                    iLog.LogMethod = LogMethod
                    iLog.LogDescription = LogDescription
                    xLog_Data.Save(iLog, sqlConn, sqlTrans)
                    sqlTrans.Commit()
                Catch ex As Exception
                    sqlTrans.Rollback()
                    Throw ex
                Finally
                    sqlConn.Close()
                End Try
            End Using
        Catch ex As Exception
            '#Catasrophic Failure
        End Try
    End Sub

End Class