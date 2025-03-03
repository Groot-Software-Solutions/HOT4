Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports HOT5.Common
Imports System.Data.SqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class EcocashBank
    Inherits System.Web.Services.WebService
    Private Const ConnString As String = "data source=HOT5;initial catalog=HOT4;persist Security Info=True; User ID=iis_hot_web;Password=H0t5$t93nn6#08642"

    <WebMethod()>
    Public Function BankStatementLine(ByVal AccessCode As String, ByVal AccessPassword As String,
                                      ByVal Amount As Decimal, ByVal TrxDate As Date, ByVal Mobile As String, ByVal ApprovalCode As String,
                                      ByVal EndBalance As Decimal) As ReturnObject
        Dim iRet As New ReturnObject
        If Not (AccessCode = "bryan@hot.co.zw" And AccessPassword = "Ecocash789661HomeStmt") Then
            iRet.ReturnCode = ReturnObject.Returncodes.Rejected
            Return iRet
        End If
        Using sqlConn As New SqlConnection(ConnString)

            sqlConn.Open()
            Dim iBanktrxBatch As New xBankTrxBatch With {
                        .BankID = xBank.Banks.EcoMerchant,
                        .LastUser = "SMSUser",
                        .BatchDate = Now,
                        .BatchReference = "SMSManual " & Format(DateTime.Now(), "dd mmm yyyy")
                    }
            iBanktrxBatch = xBankTrxBatchAdapter.GetCurrentBatch(iBanktrxBatch, sqlConn)

            'record the EcoCash request as a pending bank transaction for another handler to pick up
            Dim iBankTrx As New xBankTrx With {
                        .BankTrxBatchID = iBanktrxBatch.BankTrxBatchID,
                        .Amount = Amount,
                        .TrxDate = TrxDate,
                        .RefName = ApprovalCode,
                        .Identifier = Mobile,
                        .Branch = "na",
                        .BankRef = ApprovalCode,
                        .Balance = EndBalance
                    }
            iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.EcoCashPending
            iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.BusyConfirming

            If Not HasEcocashTranPending(iBankTrx, sqlConn) Then
                xBankTrxAdapter.Insert(iBankTrx, sqlConn)
                iRet.ReturnMsg = "BankTrxInsert"
            Else
                iRet.ReturnMsg = "Online Payment Pending"
            End If

            If iBankTrx.BankTrxID <> 0 Then
                iBankTrx = xBankTrxAdapter.GetFromBankTrxID(iBankTrx.BankTrxID, sqlConn)
                If Not iBankTrx.BankTrxID = 0 Then
                    If Not (iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Success Or iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.ToBeAllocated) Then
                        iRet.ReturnMsg = EcoCash_ProcessRequest(iBankTrx, sqlConn)
                    Else
                        iRet.ReturnMsg = "Payment Already Processed"
                    End If
                    iRet.ReturnValue = iBankTrx.BankTrxID
                Else
                    iRet.ReturnMsg = "Exception Logged"
                End If
            End If

            iRet.ReturnCode = If(iRet.ReturnMsg = "Completed", ReturnObject.Returncodes.Success, ReturnObject.Returncodes.Pending)
            sqlConn.Close()
        End Using
        Return iRet
    End Function

    Function EcoCash_ProcessRequest(ByVal iBankTrx As xBankTrx, ByVal sqlConn As SqlConnection) As String
        Dim Result As String

        iBankTrx.BankTrxState = New xBankTrxState With {.BankTrxStateID = xBankTrxState.BankTrxStates.ToBeAllocated, .BankTrxState = CType(xBankTrxState.BankTrxStates.ToBeAllocated, String)}
        xBankTrxAdapter.Save(iBankTrx, sqlConn)

        Dim iAccess As xAccess
        ' Handle Not Standard Payemnts
        If Regex.IsMatch(iBankTrx.BankRef, "^API-\d\d\d\d\d*$", RegexOptions.IgnoreCase) Then
            iAccess = xAccessAdapter.SelectRow(iBankTrx.BankRef.Split("-")(1), sqlConn)
        Else
            iAccess = xAccessAdapter.SelectCode(iBankTrx.Identifier, sqlConn)
        End If

        If Not (iAccess Is Nothing) Then
            Dim iPayment As New xPayment
            iPayment.Reference = "EcoCash payment successful. EcoCash Ref: " + iBankTrx.BankRef
            iPayment.AccountID = iAccess.AccountID
            iPayment.Amount = iBankTrx.Amount
            iPayment.PaymentDate = iBankTrx.TrxDate
            iPayment.LastUser = "EcoCash Web(Stmt Manual)"
            iPayment.PaymentType = New xPaymentType With {.PaymentType = CType(xPaymentType.PaymentTypes.BankAuto, String), .PaymentTypeID = xPaymentType.PaymentTypes.BankAuto}
            iPayment.PaymentSource = New xPaymentSource With {.PaymentSource = CType(xPaymentSource.PaymentSources.EcoCash, String), .PaymentSourceID = xPaymentSource.PaymentSources.EcoCash}
            xPaymentAdapter.Save(iPayment, sqlConn)

            iBankTrx.PaymentID = iPayment.PaymentID
            iBankTrx.BankTrxState = New xBankTrxState With {.BankTrxStateID = xBankTrxState.BankTrxStates.Success, .BankTrxState = CType(xBankTrxState.BankTrxStates.Success, String)}
            xBankTrxAdapter.Save(iBankTrx, sqlConn)

            Dim iSMS As New xSMS
            iSMS.Direction = False
            iSMS.Mobile = iBankTrx.Identifier
            iSMS.Priority.PriorityID = xPriority.Priorities.Normal
            iSMS.State.StateID = xState.States.Pending
            iSMS.SMSDate = Now
            iSMS.SMSText = "Ecocash Payment Received" & vbNewLine &
                                "Amount: " & FormatNumber(iPayment.Amount, 2) & vbNewLine &
                                "Balance: " & FormatNumber(xAccountAdapter.SelectRow(iPayment.AccountID, sqlConn).Balance, 2) & vbNewLine &
                                "Ref: " & iPayment.Reference & vbNewLine &
                                "Source: " & iPayment.PaymentSource.PaymentSource & vbNewLine &
                                "HOT Recharge - your favourite service"
            xSMSAdapter.Save(iSMS, sqlConn)
            Result = "Completed"
            If LCase(iBankTrx.Branch.Replace(" ", "")) = "self" Then
                Log("HOT5.Web.Ecocash", "ReturnedPostData", "EcoCash_ProcessRequest", "Attempting Self top Up " & iBankTrx.BankTrxID)
                ProcessSelfTopUp(iBankTrx, iAccess, sqlConn)
            End If
            If Regex.IsMatch(iBankTrx.BankRef, "^API-\d\d\d\d\d*$", RegexOptions.IgnoreCase) Then
                Log("HOT5.Web.Ecocash", "ReturnedPostData", "EcoCash_ProcessRequest", "Processed API EcocashRequest webVersion " & iBankTrx.BankTrxID)
                'ProcessAPIEcocashRequest(econetResponse, sqlConn, iAccess)
            End If

        Else
            Result = "Invalid Access"
        End If

        Return Result
    End Function

    Private Function HasEcocashTranPending(iBankTrx As xBankTrx, sqlConn As SqlConnection) As Boolean
        Dim HasOnlineEcocashPending As Integer = 0
        Using sqlCmd As New SqlCommand("xBankTrx_HasEcocashTranPending", sqlConn)
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("Mobile", iBankTrx.Identifier)
            sqlCmd.Parameters.AddWithValue("Amount", iBankTrx.Amount)
            sqlCmd.Parameters.AddWithValue("date", iBankTrx.TrxDate)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                HasOnlineEcocashPending = sqlRdr("TransactionsPending")
                sqlRdr.Close()
            End Using
        End Using
        Return HasOnlineEcocashPending <> 0
    End Function

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

    Private Sub ProcessSelfTopUp(iBankTrx As xBankTrx, iAccess As xAccess, sqlConn As SqlConnection)
        Try
            Dim iResponses As New List(Of xTemplate)
            'Get original SMS

            Dim iSMS As New xSMS
            Dim query As String = "select * from vwsms where smsid = " + iBankTrx.RefName
            Using sqlcommand As New SqlCommand(query, sqlConn)
                sqlcommand.CommandType = System.Data.CommandType.Text
                Using sqlRdr As SqlDataReader = sqlcommand.ExecuteReader()
                    While sqlRdr.Read
                        iSMS = New xSMS(sqlRdr)
                    End While
                    sqlRdr.Close()
                End Using
            End Using
            'Initialize Recharge Object
            Dim iRecharge As New xRecharge With
                {
                    .AccessID = iAccess.AccessID,
                    .Amount = CDec(iSMS.SMSText.Split(" ")(1)),
                    .Mobile = If(iSMS.SMSText.Split(" ").Length = 3, iSMS.SMSText.Split(" ")(2), iSMS.Mobile),
                    .RechargeDate = Date.Now
                }
            ' Check If recharge is within acceptable amount
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            If iRecharge.Amount < iConfig.MinRecharge Or iRecharge.Amount > iConfig.MaxRecharge Then _
                InvalidAmountResponse(iRecharge, iResponses, sqlConn) : GoTo reply

            ' Check if valid Number
            If IsInvalidMobileNumber(sqlConn, iRecharge) Then _
                InvalidMobileResponse(iSMS, iRecharge, iResponses, sqlConn) : GoTo reply

            'Get Discount
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
            iRecharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, iRecharge.Brand.BrandID, sqlConn).Discount
            'Insert recharge and handover to Recharge Service
            iSMS.State.State = xState.States.Busy
            xSMSAdapter.Save(iSMS, sqlConn)
            iRecharge.State.StateID = xState.States.Pending
            xRechargeAdapter.Save(iRecharge, sqlConn, iSMS.SMSID)
            Log("HOT5.Web.Ecocash", "ReturnedPostData", "ProcessSelfTopUp", "Recharge Completed: RechargeId " & iRecharge.RechargeID)
            Exit Sub
reply:
            Reply(iSMS, iResponses, sqlConn)
        Catch ex As Exception
            Log("HOT5.Web.Ecocash", "ReturnedPostData", "ProcessSelfTopUp", "Exception:" & ex.ToString & " Data:" & iBankTrx.BankTrxID.ToString)
        End Try

    End Sub

    Private Shared Sub ProcessAPIEcocashRequest(econetResponse As EcoCashAPI.xEcoCashTransaction, sqlConn As SqlConnection, iAccess As xAccess)
        Dim iAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
        'If Not iAccount Is Nothing Then
        '    Dim client = New HttpClient() With {
        '        .BaseAddress = New Uri(iAccount.Email)
        '    }
        '    Dim response As HttpResponseMessage =
        '        client.PostAsync("", New StringContent(JsonConvert.SerializeObject(econetResponse), System.Text.Encoding.UTF8, "application/json")
        '                        ).Result

        'End If
    End Sub

    Private Sub InvalidMobileResponse(iSMS As xSMS, iRecharge As xRecharge, ByRef iList As List(Of xTemplate), sqlconn As SqlConnection)
        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlconn)
        iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
        iList.Add(iReply)
    End Sub

    Private Sub InvalidAmountResponse(iRecharge As xRecharge, ByRef iList As List(Of xTemplate), sqlconn As SqlConnection)
        Dim iConfig As xConfig = xConfigAdapter.Config(sqlconn)
        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeRange, sqlconn)
        iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", FormatNumber(iConfig.MinRecharge, 2))
        iReply.TemplateText = iReply.TemplateText.Replace("%MAX%", FormatNumber(iConfig.MaxRecharge, 2))
        iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", FormatNumber(iRecharge.Amount, 2))
        iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
        iList.Add(iReply)
    End Sub

    Private Function IsInvalidMobileNumber(sqlConn As SqlConnection, ByRef iRecharge As xRecharge) As Boolean
        Dim network As xNetwork = xNetwork_Data.Identify(iRecharge.Mobile, sqlConn)
        If network Is Nothing Then Return True

        Dim suffix = Regex.Match(iRecharge.Mobile, "\D+").Value.Replace(" ", "")
        iRecharge.Mobile = Regex.Match(iRecharge.Mobile, "\d*").Value 'Strip Brand Suffix from Mobile (if applicable)

        Select Case network.NetworkID
            Case xNetwork.Networks.Econet, xNetwork.Networks.Econet078, xNetwork.Networks.NetOne, xNetwork.Networks.Telecel
                If iRecharge.Mobile.Length <> 10 Then Return True
            Case Else
                If iRecharge.Mobile.Length <> 11 Then Return True
        End Select

        If Not IsNumeric(suffix) Then
            Try
                iRecharge.Brand = xBrandAdapter.Identify(network, suffix, sqlConn)
            Catch ex As Exception
                If network.NetworkID = xNetwork.Networks.Econet078 Then
                    Dim EconetNetwork As xNetwork = xNetwork_Data.Identify("0772000000", sqlConn)
                    iRecharge.Brand = xBrandAdapter.Identify(EconetNetwork, suffix, sqlConn)
                Else
                    Throw ex
                End If
            End Try
        Else
            iRecharge.Brand = xBrandAdapter.Identify(network, " ", sqlConn)
        End If

        If iRecharge.Brand Is Nothing Then Return True
        If Not IsNumeric(iRecharge.Mobile) Then Return True
        Return False
    End Function

    Private Sub Reply(ByRef iSMS As xSMS, ByVal iList As List(Of xTemplate), ByVal sqlConn As SqlConnection)
        For Each iReply As xTemplate In iList
            Dim iSMSReply As New xSMS
            iSMSReply.Direction = False
            iSMSReply.Mobile = iSMS.Mobile
            iSMSReply.Priority.PriorityID = xPriority.Priorities.Normal
            iSMSReply.SMSID_In = iSMS.SMSID
            iSMSReply.SMSText = iReply.TemplateText
            iSMSReply.State.StateID = xState.States.Pending
            xSMSAdapter.Save(iSMSReply, sqlConn)
        Next
        iSMS.State.StateID = xState.States.Success
        xSMSAdapter.Save(iSMS, sqlConn)
    End Sub


End Class