Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Reflection
Imports Hot.Core
Imports Hot.Core.Brands
Imports Hot.Data

Public Class xProcess

#Region " Members "

    Private _config As xConfig
    Private _conn As String
    Private WithEvents _timer As Timers.Timer
    Private ReadOnly _applicationName as String = Assembly.GetCallingAssembly().GetName().Name
    Private ReadOnly _typeName as String = [GetType]().Name
    Private _isTestMode As Boolean
    Private ReadOnly _referencePrefix = ConfigurationManager.AppSettings("referencePrefix")

#End Region

#Region " Initialise "

    Public Sub StartProcess(conn As String, Optional isTestMode As Boolean = False)
        _isTestMode = isTestMode
        Try
            _conn = Conn

            'Set Config
            Using sqlConn As New SqlConnection(_conn)
                sqlConn.Open()
                _config = xConfigAdapter.Config(sqlConn)
                sqlConn.Close()
            End Using

            'Start Timer
            _Timer = New Timers.Timer
            _Timer.Interval = 500
            _Timer.Enabled = True
            _Timer.Start()

        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex)
        End Try
    End Sub

    Public Sub StopProcess()
        If _Timer IsNot Nothing Then
            _Timer.Stop()
        End If
    End Sub

#End Region


#Region " Process "

    Private Sub Timer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles _timer.Elapsed
        Try
            _Timer.Stop()
            Using sqlConn As New SqlConnection(_Conn)
                sqlConn.Open()
                Dim pending As IList = xRechargeAdapter.Pending(GetBrandIds(), sqlConn)
                If pending.Count > 0 Then Console.WriteLine("Recharging:" & pending.Count)
                For Each iRecharge As xRecharge In pending
                    Recharge(iRecharge, sqlConn)
                Next
                sqlConn.Close()
            End Using
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex)
        Finally
            _Timer.Start()
        End Try
    End Sub

    Private Function GetBrandIds() As IList(Of Integer)
        Return ConfigurationManager.AppSettings("brandIds").Split(",").Select(Function(s) Convert.ToInt32(s) ).ToList()
    End Function


    Private Sub Recharge(iRecharge As xRecharge, sqlConn As SqlConnection)
        Try
            'Set Recharge state to failed by default
            iRecharge.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(iRecharge, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge
            'look out for duplicate listings of brands as both platform and pins!!!!!
            Select Case iRecharge.Brand.BrandID
                Case xBrand.Brands.EconetPlatform, xBrand.Brands.Econet078, xBrand.Brands.EconetBB,
                    xBrand.Brands.EconetTXT, xBrand.Brands.TelecelBB, xBrand.Brands.TelecelTXT
                    RechargeEconet(iRecharge, sqlConn)
                Case xBrand.Brands.EasyCall
                    RechargeNetOne(iRecharge, sqlConn)
                Case xBrand.Brands.Juice
                    RechargeTelecel(iRecharge, sqlConn)
                Case xBrand.Brands.Africom
                    RechargeAfricom(iRecharge, sqlConn)
                Case Else
                    RechargePin(iRecharge, sqlConn)
            End Select

        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", iRecharge.RechargeID)
        End Try
    End Sub

    Private Sub RechargeNetOne(iRecharge As xRecharge, sqlConn As SqlConnection)
        Try
            Dim netOne As NetOne = new NetOne(sqlConn, _applicationName, ConfigurationManager.AppSettings("NetOne_Endpoint"), _isTestMode, _referencePrefix, False)
            Dim response As ServiceRechargeResponse = netOne.RechargePrepaid(iRecharge)
            HandleServiceRechargeResponse(sqlConn, iRecharge, response)
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", iRecharge.RechargeID)
        End Try
    End Sub

    Private Sub RechargeEconet(iRecharge As xRecharge, sqlConn As SqlConnection)
        Try
            Dim econet As Econet = new Econet(sqlConn, _applicationName,  ConfigurationManager.AppSettings("Econet_Endpoint"), _isTestMode, _referencePrefix, False)
            Dim response As ServiceRechargeResponse = econet.RechargePrepaid(iRecharge)
            HandleServiceRechargeResponse(sqlConn, iRecharge, response)
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", iRecharge.RechargeID)
        End Try
    End Sub

    Private Sub RechargeTelecel(iRecharge As xRecharge, sqlConn As SqlConnection)
        Try
            Dim telecel As Telecel = new Telecel(sqlConn, _applicationName,  ConfigurationManager.AppSettings("TeleCel_Endpoint"), _isTestMode, _referencePrefix, False)
            Dim response As ServiceRechargeResponse = telecel.RechargePrepaid(iRecharge)
            If iRecharge.IsSuccessFul Then
                Console.WriteLine("Recharge Successful. Replying")
                ReplyPrepaid(iRecharge, response.RechargePrepaid, sqlConn)
            Else
                Console.WriteLine("Recharge Not Successful. Trying Pin.")
                RechargePin(iRecharge, sqlConn)
            End If
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", iRecharge.RechargeID)
        End Try
    End Sub


    Private Sub RechargeAfricom(ByVal iRecharge As xRecharge, ByVal sqlConn As SqlConnection)
        Try
            Dim africom As Africom = new Africom(sqlConn, _applicationName,  ConfigurationManager.AppSettings("Africom_Endpoint"), _isTestMode, _referencePrefix, False)
            Dim response As ServiceRechargeResponse = africom.RechargePrepaid(iRecharge)
            ' response.RechargePrepaid.Narrative
            HandleServiceRechargeResponse(sqlConn, iRecharge, response)
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", iRecharge.RechargeID)
        End Try
    End Sub


    Private Sub RechargePin(iRecharge As xRecharge, sqlConn As SqlConnection)
        Try
            Dim pin As Pin = new Pin(sqlConn, _applicationName)
            Dim response As PinRechargeResponse = pin.RechargePin(iRecharge)
            If response.Success Then
                Console.WriteLine("Recharge Successful. Replying")
                If response.AccessChannel = xChannel.Channels.SMS Then
                    Reply(response.Sms, response.Templates, sqlConn)
                End If
                ' Not sure if simply replying is good if unsuccessful??
                ReplyCustomer(iRecharge.Mobile, response.CustomerTemplates, sqlConn, response.Sms)
            Else
                Reply(response.Sms, response.Templates, sqlConn)
            End If
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", iRecharge.RechargeID)
        End Try
    End Sub

    Private Sub HandleServiceRechargeResponse(sqlConn As SqlConnection, iRecharge As xRecharge,
                                              response As ServiceRechargeResponse)
        If iRecharge.IsSuccessFul Then
            Console.WriteLine("Recharge Successful. Replying")
            ReplyPrepaid(iRecharge, response.RechargePrepaid, sqlConn)
        Else
            Console.WriteLine("Recharge Failed. Replying")
            ReplyPrepaidFailed(iRecharge, sqlConn)
        End If
    End Sub

    Private Sub LogError(methodName As String, ex As Exception, Optional idType As String = Nothing,
                         Optional idNumber As Long = Nothing)
        xLog_Data.Save(_applicationName, _typeName, methodName, ex, _conn, idType, idNumber)
    End Sub

    Private Sub ReplyPrepaid(ByVal iRecharge As xRecharge, ByVal iRechargePrepaid As xRechargePrepaid,
                             ByVal sqlConn As SqlConnection)
        
        Dim iAccess As xAccess = xAccessAdapter.SelectRow(iRecharge.AccessID, sqlConn)
        Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
        Dim sms As New xSMS
        Select Case iAccess.Channel.ChannelID
            Case xChannel.Channels.SMS
                If iRecharge.Amount > 0 Then 'Credit
                    'Get Recharge SMS
                    sms = xRechargeAdapter.SelectSMS(iRecharge.RechargeID, sqlConn)
                    sms = SendDealerCreditResponse(iRecharge, sqlConn, iAccount, sms)
                    SendCustomerCreditResponse(iRecharge, sqlConn, sms, iRechargePrepaid)
                Else 'Debit response          
                    Try
                        'there is no incoming SMS for this trx, so fill with expected
                        iAccess = xAccessAdapter.SelectRow(iRecharge.AccessID, sqlConn)
                        sms.Mobile = iAccess.AccessCode
                        sms.SMSID = 0
                        sms = SendDealerDebitResponse(iRecharge, sqlConn, iAccount, sms)
                        SendCustomerDebitResponse(iRecharge, sqlConn, sms, iRechargePrepaid)
                    Catch ex As Exception
                        xLog_Data.Save(_applicationName, _typeName, MethodBase.GetCurrentMethod().Name,
                                       ex.ToString & "Acc" & iAccess.AccessCode & "iSMS" & sms.Mobile, _conn,
                                       "RechargeID", iRecharge.RechargeID)
                    End Try
                End If
            Case xChannel.Channels.Email, xChannel.Channels.Web

                'Send Customer response only 
                Dim iReplyCustomer As xTemplate =
                        xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRechargeVAS_Customer, sqlConn)
                iReplyCustomer.TemplateText = iReplyCustomer.TemplateText.Replace("%AMOUNT%",
                                                                                  Formatting.FormatAmount(
                                                                                      iRecharge.Amount))
                iReplyCustomer.TemplateText = iReplyCustomer.TemplateText.Replace("%BALANCE%",
                                                                                  Formatting.FormatAmount(
                                                                                      iRechargePrepaid.FinalBalance))
                Dim iListCustomer As New List(Of xTemplate)
                iListCustomer.Add(iReplyCustomer)
                sms.SMSID = 0
                ReplyCustomer(iRecharge.Mobile, iListCustomer, sqlConn, sms)
        End Select
    End Sub

    Private Sub SendCustomerCreditResponse(recharge As xRecharge, sqlConn As SqlConnection, sms As xSMS, iRechargePrepaid As xRechargePrepaid)

        Dim template As xTemplate =  xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRechargeVAS_Customer, sqlConn)
        Dim text As String = template.TemplateText
        text =text.Replace("%AMOUNT%", Formatting.FormatAmount(recharge.Amount))
        If iRechargePrepaid.FinalBalance = 0 Then
            text = text.Replace("%BALANCE%", "Unknown")
        Else
            text = text.Replace("%BALANCE%", Formatting.FormatAmount(iRechargePrepaid.FinalBalance))
        End If

        template.TemplateText = text
        Dim templates As New List(Of xTemplate)
        templates.Add(template)
        ReplyCustomer(recharge.Mobile, templates, sqlConn, sms)
    End Sub

    Private Function SendDealerCreditResponse(iRecharge As xRecharge, sqlConn As SqlConnection, iAccount As xAccount, sms As xSMS) As xSMS

        Dim template As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRecharge_Dealer_Header, sqlConn)
        Dim text As String = template.TemplateText
        text = text.Replace("%MOBILE%",iRecharge.Mobile)
        text = text.Replace("%AMOUNT%",Formatting.FormatAmount(iRecharge.Amount))
        text = text.Replace("%DISCOUNT%", Formatting.FormatAmount(iRecharge.Discount, decimalPlaces := 1))
        text = text.Replace("%COST%",Formatting.FormatAmount(iRecharge.Amount*(1 - iRecharge.Discount/100), formatForDealerCost := true))
        text = text.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.Balance))
        text = text.Replace("%SALEVALUE%", Formatting.FormatAmount( iAccount.SaleValue))
        Dim iListDealer As New List(Of xTemplate)
        template.TemplateText = text
        iListDealer.Add(template)
        Reply(sms, iListDealer, sqlConn)
        Return sms
    End Function

    Private Sub SendCustomerDebitResponse(recharge As xRecharge, sqlConn As SqlConnection, sms As xSMS, rechargePrepaid As xRechargePrepaid)

        Dim template As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulReversalCustomer, sqlConn)
        Dim text As String = template.TemplateText
        text = text.Replace("%AMOUNT%", Formatting.FormatAmount(recharge.Amount))
        text = text.Replace("%BALANCE%",Formatting.FormatAmount(rechargePrepaid.FinalBalance))
        Dim iListCustomer As New List(Of xTemplate)
        template.TemplateText = text
        iListCustomer.Add(template)
        ReplyCustomer(recharge.Mobile, iListCustomer, sqlConn, sms)
    End Sub

    Private Function SendDealerDebitResponse(iRecharge As xRecharge, sqlConn As SqlConnection, iAccount As xAccount, sms As xSMS) As xSMS

        Dim template As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulReversalDealer, sqlConn)
        Dim text As String = template.TemplateText
        text = text.Replace("%MOBILE%", iRecharge.Mobile)
        text = text.Replace("%AMOUNT%", Formatting.FormatAmount(iRecharge.Amount))
        text = text.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.Balance))
        text = text.Replace("%SALEVALUE%", Formatting.FormatAmount(iAccount.SaleValue))
        template.TemplateText = text
        Dim templates As New List(Of xTemplate)
        templates.Add(template)
        Reply(sms, templates, sqlConn)
        Return sms
    End Function

    Private Sub ReplyPrepaidFailed(iRecharge As xRecharge, sqlConn As SqlConnection)

        'Get Access Row
        Dim iAccess As xAccess = xAccessAdapter.SelectRow(iRecharge.AccessID, sqlConn)

        'Get Recharge SMS
        Dim iSMS As xSMS = xRechargeAdapter.SelectSMS(iRecharge.RechargeID, sqlConn)

        'Reply depending on channel transaction type
        Select Case CType(iAccess.Channel.ChannelID, xChannel.Channels)
            Case xChannel.Channels.SMS

                ' 'Get Account
                'Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
               
                'Send Dealer Response                        
                Dim iReplyDealerHeader As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeVASDisabled, sqlConn)
                iReplyDealerHeader.TemplateText = iReplyDealerHeader.TemplateText.Replace("%MESSAGE%", iSMS.SMSText)
                Dim iListDealer As New List(Of xTemplate)
                iListDealer.Add(iReplyDealerHeader)
                Reply(iSMS, iListDealer, sqlConn)


        End Select
    End Sub

#End Region

#Region " Reply "

    Private Sub Reply(ByRef iSMS As xSMS, iList As List(Of xTemplate), sqlConn As SqlConnection)
        For Each iReply As xTemplate In iList
            Dim iSmsReply As New xSMS
            iSmsReply.Direction = False
            iSmsReply.Mobile = iSMS.Mobile
            iSmsReply.Priority.PriorityID = xPriority.Priorities.Normal
            If iSMS.SMSID <> 0 Then iSmsReply.SMSID_In = iSMS.SMSID
            iSmsReply.SMSText = iReply.TemplateText
            iSmsReply.State.StateID = xState.States.Pending
            xSMSAdapter.Save(iSmsReply, sqlConn)
        Next
        'Set the incoming SMS to Success as the whole process is now complete
        If iSMS.SMSID <> 0 Then
            iSMS.State.StateID = xState.States.Success
            xSMSAdapter.Save(iSMS, sqlConn)
        End If
    End Sub

    Private Sub ReplyCustomer(recipient As String, iList As List(Of xTemplate), sqlConn As SqlConnection,
                              Optional ByVal iSms As xSMS = Nothing)
        For Each iReply As xTemplate In iList
            Dim iSmsReply As New xSMS
            iSmsReply.Direction = False
            iSmsReply.Mobile = Recipient
            iSmsReply.SMSText = iReply.TemplateText
            iSmsReply.Priority.PriorityID = xPriority.Priorities.Normal
            iSmsReply.State.StateID = xState.States.Pending
            iSmsReply.SMSDate = Now
            If iSMS.SMSID <> 0 Then iSmsReply.SMSID_In = iSMS.SMSID
            xSMSAdapter.Save(iSmsReply, sqlConn)
        Next
    End Sub

#End Region
End Class


