Imports System.Data.SqlClient
Imports HOT5.Common
Imports EcoCashAPI
Imports Newtonsoft.Json

Partial Class EcoCashNotify
    Inherits System.Web.UI.Page
    Public Const ConnString As String = "data source=HOT5;initial catalog=HOT4;persist Security Info=True; User ID=iis_hot_web;Password=H0t5$t93nn6#08642"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim econetResponse As xEcoCashTransaction = Nothing
        Dim iBankTrx As xBankTrx = Nothing
        Dim reader As New System.IO.StreamReader(HttpContext.Current.Request.InputStream)
        Dim dataFromPost As String = reader.ReadToEnd()
        Try
            '217.15.118.42 Ecocash Server IP
            'If String.IsNullOrEmpty(dataFromPost) Then Exit Sub
            'Invalid IP Address
            'Hot Tran data
            Log("HOT5.Web.Ecocash", "ReturnPostData", "Page_Load", dataFromPost)
            If Request.UserHostAddress <> "217.15.118.42" And GetIPAddress().StartsWith("192.168.104.") Then
                Log("HOT5.Web.Ecocash", "ReturnPostData", "Page_Load_IP_Check", "Invalid IP Address:" + GetIPAddress() + "-" + Request.UserHostAddress + " Data:" + dataFromPost)
                Exit Sub
            End If
            If Not String.IsNullOrEmpty(dataFromPost) Then
                dataFromPost = IIf(dataFromPost.StartsWith("="), dataFromPost.Remove(0, 1), dataFromPost)

                dataFromPost = HttpUtility.UrlDecode(dataFromPost)
                'lbl1.Text = dataFromPost

                Using sqlConn As New SqlConnection(ConnString)
                    sqlConn.Open()

                    Try
                        econetResponse = JsonConvert.DeserializeObject(dataFromPost, GetType(xEcoCashTransaction))
                        'Check Econet response has correct API config data to check if Econet is sender
                        If econetResponse.merchantCode = xEcoCashAPI.APIMechantCode _
                            And econetResponse.merchantNumber = xEcoCashAPI.APIMerchantNumber _
                            Then

                            iBankTrx = xBankTrxAdapter.GetFromBankTrxID(econetResponse.clientCorrelator, sqlConn)

                            If Not iBankTrx.BankTrxID = 0 Then
                                If Not (iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Success Or iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.ToBeAllocated) Then
                                    lbl1.Text = "Status: " + EcoCash_ProcessRequest(iBankTrx, econetResponse, sqlConn) + "<br/>Data Received: " + dataFromPost
                                Else
                                    lbl1.Text = "Status: Payment Already Processed" + "<br/>Data Received: " + dataFromPost
                                End If
                            Else
                                lbl1.Text = "Status: Exception Logged"
                                'log exception
                                Log("HOT5.Web.Ecocash", "ReturnPostData", "Page_Load", "Invalid transaction Returned" & dataFromPost)

                            End If
                        ElseIf econetResponse.version = 2 Then
                            Log("HOT5.Web.Ecocash", "ReturnPostData", "Page_Load", "Duplicate submitted" & dataFromPost)
                        Else
                            Log("HOT5.Web.Ecocash", "ReturnPostData", "Page_Load", "Invalid Data Submitted: " & dataFromPost)
                        End If
                    Catch ex As Exception
                        lbl1.Text = "Error: " + ex.Message + "<br/>" + "Data: " + dataFromPost
                        Log("HOT5.Web.Ecocash", "ReturnedPostData", "Page_Load-Parsing", "Exception:" & ex.ToString & " Data:" & dataFromPost)
                    End Try
                    sqlConn.Close()
                End Using
            Else
                'log exception
                Log("HOT5.Web.Ecocash", "ReturnPostData", "Page_Load", "Empty Post Receieved")
            End If


        Catch ex As Exception
            Log("HOT5.Web.Ecocash", "ReturnedPostData", "Page_Load-LoadData ", "Exception:" & ex.ToString & " Data:" & dataFromPost)
        End Try
    End Sub

    Function EcoCash_ProcessRequest(ByVal iBankTrx As xBankTrx, ByVal econetResponse As xEcoCashTransaction, ByVal sqlConn As SqlConnection) As String
        Dim Result As String
        Select Case UCase(econetResponse.transactionOperationStatus)
            Case "COMPLETED"
                'hot tran done
                iBankTrx.BankTrxState = New xBankTrxState With {.BankTrxStateID = xBankTrxState.BankTrxStates.ToBeAllocated, .BankTrxState = CType(xBankTrxState.BankTrxStates.ToBeAllocated, String)}
                xBankTrxAdapter.Save(iBankTrx, sqlConn)

                Dim iAccess As xAccess = xAccessAdapter.SelectCode(iBankTrx.Identifier, sqlConn)

                If Not (iAccess Is Nothing) Then
                    Dim iPayment As New xPayment
                    iPayment.Reference = "EcoCash Payment successful. EcoCash Ref: " + econetResponse.ecocashReference
                    iPayment.AccountID = iAccess.AccountID
                    iPayment.Amount = iBankTrx.Amount
                    iPayment.PaymentDate = iBankTrx.TrxDate
                    iPayment.LastUser = "EcoCash Web(Notify Page)"
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


                Else
                        Result = "Invalid Access"
                End If

            Case UCase("PENDING SUBSCRIBER VALIDATION"), UCase("Processing Charge")
                'ignore 
                Result = "Pending"
            Case UCase("FAILED")
                ' hot update denied
                iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Failed
                xBankTrxAdapter.UpdateState(iBankTrx, sqlConn)
                ' Send SMS to Requesting user to notify them that they need to resend if they want to continue

                Result = "FAILED"
            Case Else
                'log status
                Result = econetResponse.transactionOperationStatus
        End Select
        Return Result
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

    Private Function GetIPAddress() As String
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If String.IsNullOrEmpty(sIPAddress) Then
            Return context.Request.ServerVariables("REMOTE_ADDR")
        Else
            Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
            Return ipArray(0)
        End If
    End Function

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
