Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Configuration
Imports System.Threading
Imports Hot.EconetBundle
Imports Hot.Data
Imports EcoCashAPI
Imports System.Security.Cryptography
Imports System.Text

Public Class xProcess

#Region " Members "
    Private _Config As xConfig
    Private _Conn As String
    Private WithEvents _Timer As Timers.Timer

    Private RunningThreads As New List(Of Thread)
    Private PendingTransactions As List(Of xSMS)
    ReadOnly LimitOfRunningThreads As Integer = ConfigurationManager.AppSettings("maximumThreads")
    ReadOnly LimitWaitPeriod As Integer = ConfigurationManager.AppSettings("LimitWaitPeriod")
    ReadOnly MaxResetPinAmount As Integer = ConfigurationManager.AppSettings("MaxResetPinAmount")
    ReadOnly EconetLimitsEnabled As Boolean = ConfigurationManager.AppSettings("EconetLimitsEnabled")
#End Region

#Region " Initialise "
    Public Sub StartProcess(ByVal Conn As String)
        Try
            _Conn = Conn

            'Set Config
            Using sqlConn As New SqlConnection(_Conn)
                sqlConn.Open()
                _Config = xConfigAdapter.Config(sqlConn)
                sqlConn.Close()
            End Using

            'Start Timer
            _Timer = New System.Timers.Timer With {
                .Interval = ConfigurationManager.AppSettings("refreshTimer"),
                .Enabled = True
            }
            _Timer.Start()

        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "StartProcess", ex.ToString, 0)
        End Try
    End Sub
    Public Sub StopProcess()
        _Timer.Stop()
    End Sub

#End Region

#Region " Common "
    Private Sub Log(ByVal LogModule As String, ByVal LogObject As String, ByVal LogMethod As String, ByVal LogDescription As String, SmsId As Integer)
        Try
            Using sqlConn As New SqlConnection(_Conn)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    Dim iLog As New xLog With {
                        .LogModule = LogModule,
                        .LogObject = LogObject,
                        .LogMethod = LogMethod,
                        .LogDescription = LogDescription,
                        .IDNumber = SmsId
                    }
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
        Console.Write(LogMethod & " - " & LogDescription)
    End Sub
    Private Function FixMessageDelimiter(ByVal Msg As String) As String

        Msg = Msg.Trim
        Msg = Replace(Msg, "#", " ")
        Msg = Replace(Msg, "*", " ")
        Msg = Replace(Msg, ",", " ")
        Msg = Replace(Msg, "-", " ")
        'Msg = Replace(Msg, "DEPRECATED", "0000")
        While InStr(Msg, "  ") > 0
            Msg = Replace(Msg, "  ", " ")
        End While
        If Msg Is Nothing Then Msg = " "
        Return Msg
    End Function

    Private Function CheckAccess(ByRef iSMS As xSMS, ByVal sqlConn As SqlConnection) As xAccess
        Dim iAccess As xAccess = xAccessAdapter.SelectCode(iSMS.Mobile, sqlConn)
        If iAccess Is Nothing Then
            'List of Replies
            'Reply Not Registered 
            'Reply Registration Help
            Dim iList As New List(Of xTemplate) From {
                xTemplateAdapter.SelectRow(xTemplate.Templates.FailedNotRegistered, sqlConn),
                xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRegister, sqlConn)
            }

            'Process Replies
            Reply(iSMS, iList, sqlConn)
        End If
        Return iAccess
    End Function

    Private Function ValidPassword(iSMS As xSMS, iAccess As xAccess) As Boolean
        If iAccess.AccessPassword = iSMS.SMSText.Split(" ")(3) Then Return True
        If PasswordHasher.VerifyPassword(iSMS.SMSText.Split(" ")(3), iAccess.PasswordSalt, iAccess.PasswordHash) Then Return True
        If iAccess.AccessCode = "0772397464" Then
            Log("SMSHandler", "PasswordHashing", "ValidPassword",
                $"Hash Data Compared - {PasswordHasher.GenerateHash(iSMS.SMSText.Split(" ")(3), iAccess.PasswordSalt)}:{iAccess.PasswordHash}", iSMS.SMSID)
        End If
        Return False
    End Function

    Private Sub ClearPassword(iSMS As xSMS, HadValidPassword As Boolean, sqlcon As SqlConnection)
        Dim smsText = iSMS.SMSText
        iSMS.SMSText = smsText.Substring(0, smsText.LastIndexOf(smsText.Split(" ")(3))) + " " + IIf(HadValidPassword, "[Valid PIN]", "[Wrong PIN]")
        xSMSAdapter.Save(iSMS, sqlcon)

    End Sub

#End Region

#Region " SMS Processing Loop "
    Private Sub Timer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles _Timer.Elapsed
        Try
            'Stop the Timer
            _Timer.Stop()

            Using sqlConn As New SqlConnection(_Conn)
                sqlConn.Open()
                PendingTransactions = xSMSAdapter.Inbox(sqlConn)


                For Each iMobile In (From iSMS As xSMS In PendingTransactions Select iSMS.Mobile Distinct).ToList()
                    While (RunningThreads.Count >= LimitOfRunningThreads)
                        RemoveCompleteThreads()
                        Thread.Sleep(LimitWaitPeriod)
                    End While
                    Dim TransactionThread As New Thread(
                    Sub()

                        Try
                            For Each iSMS As xSMS In
                               (From sms As xSMS In PendingTransactions Where sms.Mobile = iMobile Select sms Order By sms.SMSDate).ToList()

                                'Fix Message Delimiter
                                iSMS.SMSText = FixMessageDelimiter(iSMS.SMSText)
                                If Not sqlConn.State = ConnectionState.Open Then sqlConn.Open()
                                'Identify and Handle Type Of Request 
                                Select Case xHotTypeAdapter.Identify(iSMS.SMSText.Split(" ")(0), iSMS.SMSText.Split(" ").Length, sqlConn)
                                    Case xHotType.HotTypes.Balance
                                        HandleBalance(iSMS, sqlConn)
                                    Case xHotType.HotTypes.Help
                                        HandleHelp(iSMS, sqlConn)
                                    Case xHotType.HotTypes.Recharge
                                        HandleRecharge(iSMS, sqlConn)
                                    Case xHotType.HotTypes.Registration
                                        HandleRegistration(iSMS, sqlConn)
                                    Case xHotType.HotTypes.Resend
                                        HandleResend(iSMS, sqlConn)
                                    Case xHotType.HotTypes.Transfer
                                        HandleTransfer(iSMS, sqlConn)
                                    Case xHotType.HotTypes.Answer
                                        HandleAnswer(iSMS, sqlConn)
                                    Case xHotType.HotTypes.EcoCash
                                        HandleEcoCash(iSMS, sqlConn)
                                    Case xHotType.HotTypes.EcoChargeSelf, xHotType.HotTypes.EcoChargeOther
                                        HandleSelfTopUp(iSMS, sqlConn)
                                    Case xHotType.HotTypes.Unknown
                                        HandleUnknown(iSMS, sqlConn)
                                    Case Else
                                        HandleUnknown(iSMS, sqlConn)
                                End Select

                            Next
                        Catch ex As Exception
                            Log("SMS Handler Service", "xProcess", "Timer_Elapsed", ex.ToString, 0)
                            'Thread.CurrentThread.Abort()
                        End Try

                    End Sub
                    )
                    TransactionThread.Start()
                    RunningThreads.Add(TransactionThread)

                Next

                While (RunningThreads.Count >= 1)
                    RemoveCompleteThreads()
                    Thread.Sleep(LimitWaitPeriod)
                End While
                sqlConn.Close()
            End Using
        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "Timer_Elapsed", ex.ToString, 0)
        Finally
            'Restart the Timer
            PendingTransactions.Clear()
            _Timer.Start()
        End Try
    End Sub
    Private Sub RemoveCompleteThreads()
        Dim CompletedThreads = New List(Of Thread)
        RunningThreads.ForEach(Sub(x) If (x.ThreadState = ThreadState.Stopped) Then CompletedThreads.Add(x))
        CompletedThreads.ForEach(Sub(x) RunningThreads.Remove(x))
        CompletedThreads.Clear()
    End Sub

#End Region

#Region " Reply "
    Private Sub Reply(ByRef iSMS As xSMS, ByVal iList As List(Of xTemplate), ByVal sqlConn As SqlConnection)
        For Each iReply As xTemplate In iList
            Dim iSMSReply As New xSMS With {
                .Direction = False,
                .Mobile = iSMS.Mobile,
                .SMSID_In = iSMS.SMSID,
                .SMSText = iReply.TemplateText
            }
            iSMSReply.Priority.PriorityID = xPriority.Priorities.Normal
            iSMSReply.State.StateID = xState.States.Pending
            xSMSAdapter.Save(iSMSReply, sqlConn)
        Next
        iSMS.State.StateID = xState.States.Success
        xSMSAdapter.Save(iSMS, sqlConn)
    End Sub
    Private Sub ReplyCustomer(ByVal Recipient As String, ByVal iSMS As xSMS, ByVal iList As List(Of xTemplate), ByVal sqlConn As SqlConnection)
        For Each iReply As xTemplate In iList
            Dim iSMSReply As New xSMS With {
                .Direction = False,
                .Mobile = Recipient,
                .SMSText = iReply.TemplateText,
                .SMSDate = Now,
                .SMSID_In = iSMS.SMSID
            }
            iSMSReply.Priority.PriorityID = xPriority.Priorities.Normal
            iSMSReply.State.StateID = xState.States.Pending

            xSMSAdapter.Save(iSMSReply, sqlConn)
        Next
    End Sub
#End Region

#Region " Handle Unknown "
    Private Sub HandleUnknown(ByRef iSMS As xSMS, ByVal sqlConn As SqlConnection)
        Try
            Dim iList As New List(Of xTemplate)

            'Reply with Unknown Message
            Dim iTemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.UnknownRequest, sqlConn)
            iTemplate.TemplateText = iTemplate.TemplateText.Replace("%MESSAGE%", iSMS.SMSText)
            iList.Add(iTemplate)

            'Reply Help Register and Default - Remove 6/3/20 by BZ requested VG 
            'iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRegister, sqlConn))
            'iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpDefault, sqlConn))
            Reply(iSMS, iList, sqlConn)
        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "HandleUnknown", ex.ToString, iSMS.SMSID)
        End Try
    End Sub
#End Region

#Region " Handle Registration "
    Private Function Registration_Duplicate(ByRef iSMS As xSMS, ByVal sqlConn As SqlConnection) As Boolean
        Dim iExists As xAccess = xAccessAdapter.SelectCode(iSMS.Mobile, sqlConn)
        If iExists IsNot Nothing Then
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(iExists.AccountID, sqlConn)
            If iAccount.Profile.ProfileID = ProfileID_SelfTopUp Then
                RegisterSelfTopUp(iSMS, sqlConn)
            Else
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRegistrationDuplicate, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%NAME%", iAccount.AccountName)
                Dim iList As New List(Of xTemplate) From {
                    iReply
                }
                Reply(iSMS, iList, sqlConn)
            End If
            Return True
        End If
        Return False
    End Function

    Private Sub RegisterSelfTopUp(iSMS As xSMS, sqlConn As SqlConnection)

        If Not iSMS.SMSText.Split.Length > 2 Then
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRegister, sqlConn)
            Dim iList As New List(Of xTemplate) From {
                iReply
            }
            Reply(iSMS, iList, sqlConn)

        Else
            Dim iAccess As xAccess = xAccessAdapter.SelectCode(iSMS.Mobile, sqlConn)

            'Create New Account        
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)

            iAccount.AccountName = iSMS.SMSText.Split(" ")(2) & ", " & iSMS.SMSText.Split(" ")(1)
            iAccount.NationalID = iSMS.SMSText.Split(" ")(3)
            iAccount.Email = ""
            iAccount.ReferredBy = iSMS.Mobile

            If iSMS.SMSText.Split.Length > 3 Then
                Dim count As Integer = 0
                For i As Integer = 0 To iSMS.SMSText.Length - 1
                    If iSMS.SMSText(i) = " " Then
                        count += 1
                        If count = 3 Then
                            iAccount.NationalID = iSMS.SMSText.Substring(i)
                        End If
                    End If
                Next
            End If

            iAccount.Profile.ProfileID = _Config.ProfileID_NewSMSDealer


            xAccountAdapter.Save(iAccount, sqlConn,)

            'Create New Access
            Dim rnd As New Random()

            iAccess.AccessPassword = rnd.Next(1, 9999).ToString.PadLeft(4, "0")

            xAccessAdapter.Save(iAccess, sqlConn)

            'Successful Reply
            'With Password
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRegistration, sqlConn)
            iReply.TemplateText = iReply.TemplateText.Replace("%PASSWORD%", iAccess.AccessPassword)
            'Send General Help 
            Dim iList As New List(Of xTemplate) From {
                iReply,
                xTemplateAdapter.SelectRow(xTemplate.Templates.HelpEcoCash, sqlConn),
                xTemplateAdapter.SelectRow(xTemplate.Templates.HelpBank, sqlConn),
                 xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRecharge, sqlConn)
            }
            Reply(iSMS, iList, sqlConn)
        End If

    End Sub

    Private Sub HandleRegistration(ByRef iSMS As xSMS, ByVal sqlConn As SqlConnection)
        Try
            'Check Duplicate Registration
            If Registration_Duplicate(iSMS, sqlConn) Then
                Log("SMS Handler Service", "xProcess", "HandleRegistration", "Duplicate found:" + iSMS.Mobile.ToString, iSMS.SMSID)
                Exit Sub
            End If
            ' Check Wrong Formated Registration
            If Not iSMS.SMSText.Split.Length > 3 Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRegister, sqlConn)
                Dim iList As New List(Of xTemplate) From {
                    iReply
                }
                Reply(iSMS, iList, sqlConn)

            Else

                'Create New Account        
                Dim iAccount As New xAccount With {
                    .AccountName = iSMS.SMSText.Split(" ")(2) & ", " & iSMS.SMSText.Split(" ")(1),
                    .NationalID = iSMS.SMSText.Split(" ")(3),
                    .Email = "",
                    .ReferredBy = iSMS.Mobile
                }
                If iSMS.SMSText.Split.Length > 4 Then
                    Dim count As Integer = 0
                    For i As Integer = 0 To iSMS.SMSText.Length - 1
                        If iSMS.SMSText(i) = " " Then
                            count += 1
                            If count = 3 Then
                                iAccount.NationalID = iSMS.SMSText.Substring(i)
                            End If
                        End If
                    Next
                End If

                iAccount.Profile.ProfileID = _Config.ProfileID_NewSMSDealer

                'Econet BA Handling
                If iSMS.SMSText.ToUpper.StartsWith("BA") Then
                    Const EconetBAProfileID As Integer = 7
                    iAccount.Profile.ProfileID = EconetBAProfileID
                    iAccount.ReferredBy = "Econet-BA"
                End If
                xAccountAdapter.Save(iAccount, sqlConn,)

                'Create New Access
                Dim rnd As New Random()
                Dim iAccess As New xAccess With {
                    .AccountID = iAccount.AccountID,
                    .AccessCode = iSMS.Mobile,
                    .AccessPassword = rnd.Next(1, 9999).ToString.PadLeft(4, "0")
                }
                iAccess.Channel.ChannelID = xChannel.Channels.SMS
                xAccessAdapter.Save(iAccess, sqlConn)

                'Successful Reply
                'With Password
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRegistration, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%PASSWORD%", iAccess.AccessPassword)
                'Send General Help 
                Dim iList As New List(Of xTemplate) From {
                    iReply,
                    xTemplateAdapter.SelectRow(xTemplate.Templates.HelpEcoCash, sqlConn),
                    xTemplateAdapter.SelectRow(xTemplate.Templates.HelpBank, sqlConn),
                    xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRecharge, sqlConn)
                }
                'Send Help on Ecocash 
                Reply(iSMS, iList, sqlConn)
            End If

        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "HandleRegistration", ex.ToString & "-" & iSMS.SMSID, iSMS.SMSID)
        End Try
    End Sub



#End Region

#Region " Handle Balance "
    Private Sub HandleBalance(ByRef iSMS As xSMS, ByVal sqlConn As SqlConnection)
        Try
            'Get Access Code
            Dim iAccess As xAccess = CheckAccess(iSMS, sqlConn)
            If iAccess IsNot Nothing Then
                'List of Replies
                Dim iList As New List(Of xTemplate)

                'Get Account with Balance & Sale Value
                Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)

                'Reply Balance
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", FormatNumber(iAccount.Balance, 2))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", FormatNumber(iAccount.SaleValue, 2))
                iReply.TemplateText = iReply.TemplateText.Replace("%ZESABALANCE%", FormatNumber(iAccount.ZESABalance, 2))
                iReply.TemplateText = iReply.TemplateText.Replace("%USDBALANCE%", FormatNumber(iAccount.USDBalance, 2))

                iList.Add(iReply)

                'Process Replies
                Reply(iSMS, iList, sqlConn)
            End If
        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "HandleBalance", ex.ToString, iSMS.SMSID)
        End Try
    End Sub
#End Region

#Region " Handle Help "
    Private Sub HandleHelp(ByRef iSMS As xSMS, ByVal sqlConn As SqlConnection)
        Try
            'If Help Request has additional text other than "?" or "Help" then handle specific query else handle default help
            Dim Query As String = ""
            If iSMS.SMSText.Trim.Split(" ").Length > 1 Then Query = iSMS.SMSText.Split(" ")(1)

            'Reply List
            Dim iList As New List(Of xTemplate)

            'Handle Help Types
            Select Case Query.ToUpper
                Case "BANK", "BANKS"
                    iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpBank, sqlConn))
                Case "STOCK", "STOCKS"
                    iList = HelpPinStock(sqlConn)
                Case "DISC", "DISCOUNT", "COM", "COMM", "COMMISSION"
                    iList = HelpDiscount(iSMS, sqlConn)
                Case "RECHARGE", "HOT"
                    iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRecharge, sqlConn))
                Case "REG", "REGISTER"
                    iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRegister, sqlConn))
                Case "ECO", "ECOCASH", "EC0", "EC0CASH"
                    iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpEcoCash, sqlConn))
                Case "PIN", "RESETPIN"
                    If Not ResetPinIfPossible(iSMS, sqlConn) Then
                        iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpResetPin, sqlConn))
                    End If
                Case "LIMIT", "LIMITS"
                    iList = HelpLimit(iSMS, sqlConn)
                Case "ZESA", "ZETDC"
                    iList = HelpZesa(iSMS, sqlConn)
                Case Else
                    iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpDefault, sqlConn))
                    iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRecharge, sqlConn))
            End Select

            'Process Replies
            Reply(iSMS, iList, sqlConn)
        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "HandleHelp", ex.ToString, iSMS.SMSID)
        End Try
    End Sub

    Private Function HelpZesa(iSMS As xSMS, sqlConn As SqlConnection) As List(Of xTemplate)
        Dim iList As New List(Of xTemplate)

        iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpZESA, sqlConn))
        Return iList


    End Function

    Private Function HelpLimit(iSMS As xSMS, sqlConn As SqlConnection) As List(Of xTemplate)
        Dim iList As New List(Of xTemplate)
        'Check for Account
        Dim iAccess As xAccess = xAccessAdapter.SelectCode(iSMS.Mobile, sqlConn)
        If iAccess Is Nothing Then
            iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.FailedNotRegistered, sqlConn))
            iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRegister, sqlConn))
            Return iList
        End If

        Dim iLimit = xLimitAdapter.GetLimit(1, iAccess.AccountID, sqlConn)
        Dim iTemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.HelpLimit, sqlConn)
        iTemplate.TemplateText = iTemplate.TemplateText.Replace("%LIMITMONTHLY%", $"{iLimit.MonthlyLimit:#,##0.00}")
        iTemplate.TemplateText = iTemplate.TemplateText.Replace("%LIMITREMAINING%", $"{iLimit.LimitRemaining:#,##0.00}")
        iList.Add(iTemplate)
        Return iList
    End Function

    Private Function HelpDiscount(ByVal iSMS As xSMS, ByVal sqlConn As SqlConnection) As List(Of xTemplate)

        'Reply List
        Dim iList As New List(Of xTemplate)

        'Check for Account
        Dim iAccess As xAccess = xAccessAdapter.SelectCode(iSMS.Mobile, sqlConn)
        If iAccess Is Nothing Then
            iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.FailedNotRegistered, sqlConn))
            iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRegister, sqlConn))
            Return iList
        End If

        'Get Discounts
        Dim iTemplate As New xTemplate With {
            .TemplateText = "Discount"
        }

        'Build SMS Text
        Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
        For Each iDiscount As xProfileDiscount In xProfileAdapter.Discounts(iAccount.Profile.ProfileID, sqlConn)
            Dim DiscountItem As String = vbNewLine & iDiscount.Brand.BrandName & " " & FormatNumber(iDiscount.Discount, 1) & "%"
            If iTemplate.TemplateText.Length + DiscountItem.Length < 160 Then
                iTemplate.TemplateText &= DiscountItem
            Else
                iList.Add(iTemplate)
                iTemplate = New xTemplate With {
                    .TemplateText = "Discount"
                }
                iTemplate.TemplateText &= DiscountItem
            End If
        Next
        iList.Add(iTemplate)

        Return iList
    End Function
    Private Function HelpPinStock(ByVal sqlConn As SqlConnection) As List(Of xTemplate)
        Dim iList As New List(Of xTemplate)
        Dim iTemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.HelpStock, sqlConn)
        iTemplate.TemplateText = "Stocks:"
        Dim CurrentBrand As String = ""
        For Each iStock As xPinStock In xPinStockAdapter.Stock(sqlConn)
            If CurrentBrand <> iStock.BrandName Then
                CurrentBrand = iStock.BrandName
                iTemplate.TemplateText &= vbNewLine & CurrentBrand & " $"
            End If
            If iStock.PinValue Mod 1 = 0 Then
                iTemplate.TemplateText &= ", " & FormatNumber(iStock.PinValue, 0)
            Else
                iTemplate.TemplateText &= ", " & FormatNumber(iStock.PinValue, 1)
            End If
        Next
        iList.Add(iTemplate)
        Return iList
    End Function
#End Region

#Region " Handle Transfer "
    Private Function TransferValid(ByRef iSMS As xSMS, ByVal iAccess As xAccess, ByVal iAccount As xAccount, ByVal sqlConn As SqlConnection) As xAccess
        'List of Replies
        Dim iList As New List(Of xTemplate)

        'Validate Amount
        If Not IsNumeric(iSMS.SMSText.Split(" ")(1)) Then
            iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.FailedTransferFormat, sqlConn))
            Reply(iSMS, iList, sqlConn)
            Return Nothing
        End If

        'Check Transfer To Mobile
        Dim iAccessTo As xAccess = xAccessAdapter.SelectCode(iSMS.SMSText.Split(" ")(2), sqlConn)
        If iAccessTo Is Nothing Then
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedTransferMobile, sqlConn)
            iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iSMS.SMSText.Split(" ")(2))
            iList.Add(iReply)
            Reply(iSMS, iList, sqlConn)
            Return Nothing
        End If

        'Validate Access Password        
        If Not ValidPassword(iSMS, iAccess) Then
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedTransferAccessCode, sqlConn)
            iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", FormatNumber(iSMS.SMSText.Split(" ")(1), 2))
            iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iSMS.SMSText.Split(" ")(2))
            iList.Add(iReply)
            Reply(iSMS, iList, sqlConn)
            ClearPassword(iSMS, False, sqlConn)
            Return Nothing
        End If

        ClearPassword(iSMS, True, sqlConn)

        'Validate Amount Range
        If CDec(iSMS.SMSText.Split(" ")(1)) < _Config.MinTransfer Then
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedTransferMin, sqlConn)
            iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iAccessTo.AccessCode)
            iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", FormatNumber(CDec(iSMS.SMSText.Split(" ")(1)), 0))
            iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", FormatNumber(_Config.MinTransfer, 0))
            iList.Add(iReply)
            Reply(iSMS, iList, sqlConn)
            Return Nothing
        End If

        'Check Balance
        If iAccount.Balance < CDec(iSMS.SMSText.Split(" ")(1)) Then
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedTransferBalance, sqlConn)
            iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iAccessTo.AccessCode)
            iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", FormatNumber(CDec(iSMS.SMSText.Split(" ")(1)), 0))
            iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", FormatNumber(iAccount.Balance, 2))
            iList.Add(iReply)
            Reply(iSMS, iList, sqlConn)
            Return Nothing
        End If

        Return iAccessTo
    End Function



    Private Sub HandleTransfer(ByRef iSMS As xSMS, ByVal sqlConn As SqlConnection)
        Try
            'Get Access Code
            Dim iAccess As xAccess = CheckAccess(iSMS, sqlConn)
            If iAccess IsNot Nothing Then
                'Get Account        
                Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)

                'Validate Transfer
                Dim iAccessTo As xAccess = TransferValid(iSMS, iAccess, iAccount, sqlConn)
                If iAccessTo IsNot Nothing Then

                    'Amount
                    Dim Amount As Decimal = CDec(iSMS.SMSText.Split(" ")(1))

                    'Payment From
                    Dim iPaymentFrom As New xPayment With {
                        .Reference = "Transfer, $" & FormatNumber(Amount, 0) & " from " & iAccess.AccessCode & " to " & iAccessTo.AccessCode & ", " & Format(Now, "dd MMM yyyy HH:mm"),
                        .AccountID = iAccess.AccountID,
                        .Amount = (0 - Amount),
                        .PaymentDate = Format(Now, "dd MMM yyyy HH:mm:ss"),
                        .LastUser = "HOT Service"
                    }
                    iPaymentFrom.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.HOTTransfer
                    iPaymentFrom.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.HOTDealer
                    xPaymentAdapter.Save(iPaymentFrom, sqlConn)

                    'Payment To
                    Dim iPaymentTo As New xPayment With {
                        .Reference = iPaymentFrom.Reference,
                        .AccountID = iAccessTo.AccountID,
                        .Amount = Amount,
                        .PaymentDate = iPaymentFrom.PaymentDate,
                        .LastUser = iPaymentFrom.LastUser,
                        .PaymentType = iPaymentFrom.PaymentType,
                        .PaymentSource = iPaymentFrom.PaymentSource
                    }
                    xPaymentAdapter.Save(iPaymentTo, sqlConn)

                    'Transfer
                    Dim iTransfer As New xTransfer With {
                        .Amount = Amount,
                        .PaymentID_From = iPaymentFrom.PaymentID,
                        .PaymentID_To = iPaymentTo.PaymentID,
                        .TransferDate = Now,
                        .SMSID = iSMS.SMSID
                    }
                    iTransfer.Channel.ChannelID = xChannel.Channels.SMS
                    xTransferAdapter.Save(iTransfer, sqlConn)

                    'Sender Reply
                    'Refresh Balance
                    iAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
                    Dim iReplySender As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulTransferSender, sqlConn)
                    iReplySender.TemplateText = iReplySender.TemplateText.Replace("%AMOUNT%", FormatNumber(iTransfer.Amount, 2))
                    iReplySender.TemplateText = iReplySender.TemplateText.Replace("%BALANCE%", FormatNumber(iAccount.Balance, 2))
                    iReplySender.TemplateText = iReplySender.TemplateText.Replace("%MOBILE%", iAccessTo.AccessCode)
                    Dim iList As New List(Of xTemplate) From {
                        iReplySender
                    }
                    Reply(iSMS, iList, sqlConn)

                    'Receiver Reply                    
                    Dim iAccountTo As xAccount = xAccountAdapter.SelectRow(iAccessTo.AccountID, sqlConn)
                    Dim iReplyCustomer As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulTransferReceiver, sqlConn)
                    iReplyCustomer.TemplateText = iReplyCustomer.TemplateText.Replace("%REF%", iPaymentTo.Reference)
                    iReplyCustomer.TemplateText = iReplyCustomer.TemplateText.Replace("%BALANCE%", FormatNumber(iAccountTo.Balance, 2))
                    iReplyCustomer.TemplateText = iReplyCustomer.TemplateText.Replace("%SALEVALUE%", FormatNumber(iAccountTo.SaleValue, 2))
                    Dim iListCustomer As New List(Of xTemplate) From {
                        iReplyCustomer
                    }
                    ReplyCustomer(iAccessTo.AccessCode, iSMS, iListCustomer, sqlConn)

                End If
            End If
        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "HandleTransfer", ex.ToString, iSMS.SMSID)
        End Try

    End Sub
#End Region

#Region " Handle Resend "
    Private Sub HandleResend(ByRef iSMS As xSMS, ByVal sqlConn As SqlConnection)
        Try
            Dim RechargeMobile As String = ""
            If iSMS.SMSText.Split(" ").Length > 1 Then
                RechargeMobile = iSMS.SMSText.Split(" ", 2, StringSplitOptions.RemoveEmptyEntries)(1)
                'ElseIf iSMS.SMSText.Split(" ").Length > 2 Then
                '   RechargeMobile = iSMS.SMSText.Split(" ", 2, StringSplitOptions.RemoveEmptyEntries) '(1)' & " " & iSMS.SMSText.Split(" ")(2)
            End If
            xSMSAdapter.Resend(iSMS.Mobile, RechargeMobile, sqlConn)
            iSMS.State.StateID = xState.States.Success
            xSMSAdapter.Save(iSMS, sqlConn)
        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "HandleResend", ex.ToString, iSMS.SMSID)
        End Try
    End Sub
#End Region

#Region " Handle Recharge "
    Private Sub HandleRecharge(ByVal iSMS As xSMS, ByVal sqlConn As SqlConnection)
        Try
            ' Log("SMS Handler Service", "xProcess", "HandleRecharge", "Recharge Started")
            'Replies list
            Dim iList As New List(Of xTemplate)

            'Initialise Recharge
            Dim iRecharge As New xRecharge

            'Check Duplicate
            If xSMSAdapter.Duplicate(iSMS, sqlConn) IsNot Nothing Then
                Dim iTemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedDuplicate, sqlConn)
                iTemplate.TemplateText = iTemplate.TemplateText.Replace("%MESSAGE%", iSMS.SMSText)
                iList.Add(iTemplate)
                Reply(iSMS, iList, sqlConn)
                Exit Sub
            End If

            'Check Account
            Dim iAccess As xAccess = xAccessAdapter.SelectCode(iSMS.Mobile, sqlConn)
            If iAccess Is Nothing Then
                iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.FailedNotRegistered, sqlConn))
                iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRegister, sqlConn))
                Reply(iSMS, iList, sqlConn)
                Exit Sub
            End If
            iRecharge.AccessID = iAccess.AccessID


            If Not ValidPassword(iSMS, iAccess) Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedAccessCode, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", FormatNumber(iSMS.SMSText.Split(" ")(1), 2))
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iSMS.SMSText.Split(" ")(2))
                iList.Add(iReply)
                Reply(iSMS, iList, sqlConn)
                ClearPassword(iSMS, False, sqlConn)
                Exit Sub
            End If
            ClearPassword(iSMS, True, sqlConn)
            'Get the mobile and brand to be recharged
            iRecharge.Mobile = iSMS.SMSText.Split(" ")(2)

            ' Set variable for Data Bundles 
            Dim IsDataBundle As Boolean = False
            Dim bundle As New Models.BundleProduct

            'Validate Amount is numeric
            If Not IsNumeric(iSMS.SMSText.Split(" ")(1)) Then
                'Check if valid Data bundle Code
                bundle = Repository.BundleRepository.Get(iSMS.SMSText.Split(" ")(1), sqlConn)
                If Not bundle Is Nothing Then
                    IsDataBundle = True
                    iSMS.SMSText.Replace(iSMS.SMSText.Split(" ")(1), (bundle.Amount / 100))
                Else

                    iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeFormat, sqlConn))
                    Reply(iSMS, iList, sqlConn)
                    Exit Sub
                End If
            End If
            iRecharge.Amount = CDec(iSMS.SMSText.Split(" ")(1))


            'Get Network of Recharging Mobile
            Dim iNetwork As xNetwork = xNetwork_Data.Identify(iRecharge.Mobile, sqlConn)

            Dim FailedRechargeMobile As Boolean = False
            'by KMR 16/12/12 adding Africom network
            If iNetwork Is Nothing Then
                FailedRechargeMobile = True
            Else
                'Check Mobile Length for Econet, NetOne and Telecel
                'If iNetwork.NetworkID <= 3 And iRecharge.Mobile.Length <> 10 Then FailedRechargeMobile = True
                'If iNetwork.NetworkID = 7 And iRecharge.Mobile.Length <> 10 Then FailedRechargeMobile = True

                ''Check Mobile Length for Africom, Umax and Powertel
                'If iNetwork.NetworkID >= 4 And iRecharge.Mobile.Length <> 11 Then FailedRechargeMobile = True
                ''reply if number invalid and exit
                Select Case iNetwork.NetworkID
                    Case xNetwork.Networks.Econet, xNetwork.Networks.Econet078, xNetwork.Networks.NetOne, xNetwork.Networks.Telecel
                        'kmr 17/02/25 add NetOne 072
                        If iRecharge.Mobile.Length <> 10 Then FailedRechargeMobile = True
                    Case Else
                        If iRecharge.Mobile.Length <> 11 Then FailedRechargeMobile = True
                End Select

                ' Get Data Bundle 
                If IsDataBundle Then
                    Select Case iNetwork.NetworkID
                        Case xNetwork.Networks.Econet, xNetwork.Networks.Econet078
                        Case Else
                            FailedRechargeMobile = True
                    End Select
                End If

                'Get Brand of Recharging Mobile
                Dim suffix = iRecharge.Mobile.Substring(iRecharge.Mobile.Length - 1, 1).ToUpper()
                If Not IsNumeric(suffix) Then
                    If (iNetwork.NetworkID = xNetwork.Networks.Econet078 And suffix = "U") Then iNetwork.NetworkID = xNetwork.Networks.Econet
                    'KMR 17/02/25 probably need the same switch for NetOne
                    iRecharge.Brand = xBrandAdapter.GetBrand(iNetwork, iRecharge.Mobile.Substring(iRecharge.Mobile.Length - 1, 1).ToUpper(), sqlConn)

                    'Strip Brand Suffix from Mobile (if applicable)
                    iRecharge.Mobile = iRecharge.Mobile.Substring(0, iRecharge.Mobile.Length - 1)
                    FailedRechargeMobile = False
                Else
                    iRecharge.Brand = xBrandAdapter.GetBrand(iNetwork, " ", sqlConn)
                End If

                If IsDataBundle Then
                    Dim BrandsById As Dictionary(Of Integer, xBrand) = New Dictionary(Of Integer, xBrand)
                    For Each b As xBrand In xBrandAdapter.List(sqlConn)
                        BrandsById.Add(b.BrandID, b)
                    Next
                    iRecharge.Brand = BrandsById(bundle.BrandId)
                    iRecharge.Mobile += " " + bundle.ProductCode
                End If

                If iRecharge.Brand Is Nothing Then FailedRechargeMobile = True


            End If

            If FailedRechargeMobile Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
                iList.Add(iReply)
                Reply(iSMS, iList, sqlConn)
                Exit Sub
            End If


            'AMOUNT checking


            'Validate Amount Range
            'Select Case iRecharge.Brand.BrandID
            'Case xBrand.Brands.Juice
            '    If iRecharge.Amount < 0.1 Or iRecharge.Amount > _Config.MaxRecharge Then
            '        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeRange, sqlConn)
            '        iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", "0.10")
            '        iReply.TemplateText = iReply.TemplateText.Replace("%MAX%", FormatNumber(_Config.MaxRecharge, 2))
            '        iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", FormatNumber(iRecharge.Amount, 2))
            '        iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
            '        iList.Add(iReply)
            '        Reply(iSMS, iList, sqlConn)
            '        Exit Sub
            '    End If
            'Case xBrand.Brands.Africom
            '    If iRecharge.Amount < 1 Or iRecharge.Amount > _Config.MaxRecharge Then
            '        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeRange, sqlConn)
            '        iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", "1.00")
            '        iReply.TemplateText = iReply.TemplateText.Replace("%MAX%", FormatNumber(_Config.MaxRecharge, 2))
            '        iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", FormatNumber(iRecharge.Amount, 2))
            '        iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
            '        iList.Add(iReply)
            '        Reply(iSMS, iList, sqlConn)
            '        Exit Sub
            '    End If
            'Case Else 'Econet, NetOne and the rest
            'If iRecharge.Amount < _Config.MinRecharge Or iRecharge.Amount > _Config.MaxRecharge Then
            '    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeRange, sqlConn)
            '    iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", FormatNumber(_Config.MinRecharge, 2))
            '    iReply.TemplateText = iReply.TemplateText.Replace("%MAX%", FormatNumber(_Config.MaxRecharge, 2))
            '    iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", FormatNumber(iRecharge.Amount, 2))
            '    iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
            '    iList.Add(iReply)
            '    Reply(iSMS, iList, sqlConn)
            '    Exit Sub
            'End If
            'End Select

            If (iRecharge.Amount < _Config.MinRecharge Or iRecharge.Amount > _Config.MaxRecharge) And Not IsDataBundle Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeRange, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", FormatNumber(_Config.MinRecharge, 2))
                iReply.TemplateText = iReply.TemplateText.Replace("%MAX%", FormatNumber(_Config.MaxRecharge, 2))
                iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", FormatNumber(iRecharge.Amount, 2))
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
                iList.Add(iReply)
                Reply(iSMS, iList, sqlConn)
                Exit Sub
            End If


            Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
            If EconetLimitsEnabled And IsEconetZWLTransaction(iRecharge) Then
                Dim network As xNetwork = xNetwork_Data.Identify(iRecharge.Mobile, sqlConn)
                Dim limit As xLimit = xLimitAdapter.GetLimit(network.NetworkID, iAccount.AccountID, sqlConn)
                If limit IsNot Nothing Then
                    If limit.LimitRemaining < iRecharge.Amount Then
                        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(
                                    If(limit.LimitTypeId = 1, xTemplate.Templates.FailedEconetDailyLimit, xTemplate.Templates.FailedEconetMonthlyLimit), sqlConn)
                        iReply.TemplateText = iReply.TemplateText.Replace("%LIMIT%", limit.LimitRemaining.ToString("#,##0.00"))
                        iList.Add(iReply)
                        Reply(iSMS, iList, sqlConn)
                        Exit Sub
                    End If
                End If
            End If

            'Get Discount
            iRecharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, iRecharge.Brand.BrandID, sqlConn).Discount

            'Check Available Sale Value vs Recharge Amount   
            Dim Cost As Decimal = iRecharge.Amount - (iRecharge.Amount * (iRecharge.Discount / 100))
            Dim Balance As Decimal = iAccount.Balance
            If iRecharge.Brand.BrandID = xBrand.Brands.EconetUSD Or
                iRecharge.Brand.BrandID = xBrand.Brands.NetoneUSD Or
                 iRecharge.Brand.BrandID = xBrand.Brands.TelecelUSD Then
                Balance = iAccount.USDBalance
            End If
            Dim Salevalue As Decimal = Balance + (Balance * (iRecharge.Discount / 100))
            Dim HasSufficientFunds As Boolean = Not (Balance < Cost)

            If Not HasSufficientFunds Then
                Dim iReplyBalance As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReplyBalance.TemplateText = iReplyBalance.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
                iReplyBalance.TemplateText = iReplyBalance.TemplateText.Replace("%BALANCE%", FormatNumber(Balance, 2))
                iReplyBalance.TemplateText = iReplyBalance.TemplateText.Replace("%SALEVALUE%", FormatNumber(Salevalue, 2))
                iList.Add(iReplyBalance)
                Reply(iSMS, iList, sqlConn)
                Exit Sub
            End If


            'Insert recharge and handover to Recharge Service
            iSMS.State.State = xState.States.Busy
            xSMSAdapter.Save(iSMS, sqlConn)
            iRecharge.State.StateID = xState.States.Pending
            xRechargeAdapter.Save(iRecharge, sqlConn, iSMS.SMSID)
        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "HandleRecharge", ex.ToString, iSMS.SMSID)
        End Try
    End Sub

    Private Function IsEconetZWLTransaction(iRecharge As xRecharge) As Boolean
        If iRecharge.Brand.BrandID = xBrand.Brands.Econet078 Or
                iRecharge.Brand.BrandID = xBrand.Brands.EconetPlatform Or
                iRecharge.Brand.BrandID = xBrand.Brands.EconetBB Or
                 iRecharge.Brand.BrandID = xBrand.Brands.EconetData Or
                 iRecharge.Brand.BrandID = xBrand.Brands.EconetFacebook Or
                 iRecharge.Brand.BrandID = xBrand.Brands.EconetInstagram Or
                 iRecharge.Brand.BrandID = xBrand.Brands.EconetTwitter Or
                 iRecharge.Brand.BrandID = xBrand.Brands.EconetTXT Or
                 iRecharge.Brand.BrandID = xBrand.Brands.EconetWhatsapp Then Return True

        Return False
    End Function
#End Region

#Region "Handle Answers"
    Private Sub HandleAnswer(ByRef iSMS As xSMS, ByVal sqlConn As SqlConnection)
        Try
            'If Answer is more than blank then thank for reply
            'Reply List
            Dim iList As New List(Of xTemplate)
            If Left(iSMS.SMSText, 3).ToUpper = "OPT" Then
                If iSMS.SMSText.ToUpper = "OPT OUT" Or iSMS.SMSText.ToUpper = "OPTOUT" Then
                    Dim iTemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.AnswerOK, sqlConn)
                    iTemplate.TemplateText = "You have chosen to Opt Out of future information or competitions with HOT recharge. Thank you for your business. HOT Recharge 10th Anniversary Celebrations"
                    iList.Add(iTemplate)
                Else
                    Dim iTemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.AnswerOK, sqlConn)
                    iTemplate.TemplateText = "You have chosen to Opt In to get future information & competitions with HOT recharge. Thank you from HOT Recharge and the 10th Anniversary Celebrations"
                    iList.Add(iTemplate)
                End If

            Else
                If iSMS.SMSText.Trim.Split(" ").Length > 1 Then
                    Dim iTemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.AnswerOK, sqlConn)
                    iTemplate.TemplateText = Replace(iTemplate.TemplateText, "%MESSAGE%", iSMS.SMSText.Split(" ")(1))
                    iList.Add(iTemplate)
                Else
                    Dim iTemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.AnswerWrong, sqlConn)
                    'iTemplate.TemplateText = Replace(iTemplate.TemplateText, "%MESSAGE%", iSMS.SMSText.Split(" ")(1))
                    iList.Add(iTemplate)
                End If
            End If
            'Process Replies
            Reply(iSMS, iList, sqlConn)
        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "HandleAnswer", ex.ToString, iSMS.SMSID)
        End Try

    End Sub
#End Region

#Region "Handle EcoCash"
    Private Sub HandleEcoCash(ByRef iSMS As xSMS, ByVal sqlConn As SqlConnection)
        Try
            If ConfigurationManager.AppSettings("DisableEcoCash") Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.PaymentFailed, sqlConn)
                iReply.TemplateText = "EcoCash request Failed: Ecocash platform down, please try again later. HOT Recharge"
                'List of Replies to Process
                Dim iList As New List(Of xTemplate) From {
                    iReply
                }
                Reply(iSMS, iList, sqlConn)
                Exit Sub
            End If

            'check that an account exists for the number
            Dim iAccess As xAccess = CheckAccess(iSMS, sqlConn)
            If iAccess Is Nothing Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedNotRegistered, sqlConn)
                'List of Replies
                Dim iList As New List(Of xTemplate) From {
                    iReply
                }
                'Process Replies
                Reply(iSMS, iList, sqlConn)
            Else
                If iSMS.SMSText.Split(" ").Length <> 2 Then
BadSMS:
                    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeFormat, sqlConn)
                    iReply.TemplateText = "HOT Recharge cannot understand your message:" & iSMS.SMSText & ", sms ECOCASH Amount - HOT Recharge" 'List of Replies
                    Dim iList As New List(Of xTemplate) From {
                        iReply
                    }
                    'Process Replies
                    Reply(iSMS, iList, sqlConn)

                Else
                    If Not IsNumeric(iSMS.SMSText.Split(" ")(1)) And Not iSMS.SMSText.ToLower().Split(" ")(1).EndsWith("u") Then GoTo BadSMS

                    Dim IsUSD As Boolean = False
                    If iSMS.SMSText.ToLower().Split(" ")(1).EndsWith("u") Then
                        IsUSD = True
                        iSMS.SMSText = iSMS.SMSText.ToLower().Replace("u", "")
                    End If

                    If ConfigurationManager.AppSettings("AllowZWLEcoCash") = False And Not IsUSD Then
                        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.PaymentFailed, sqlConn)
                        iReply.TemplateText = "EcoCash ZWL funding is currently not available. Please try 'ECOCASH [Amount]U' if you are trying to fund USD. HOT Recharge"
                        'List of Replies to Process
                        Dim iList As New List(Of xTemplate) From {
                    iReply
                }
                        Reply(iSMS, iList, sqlConn)
                        Exit Sub
                    End If
                    'check the minimum transaction amount
                    'deduct any commissions in the payment when successful, not now.
                    'get the current batch ID if one was done today, else create a new one
                    Dim iBanktrxBatch As New xBankTrxBatch With {
                        .BankID = xBank.Banks.EcoMerchant,
                        .LastUser = "SMSUser",
                        .BatchDate = Now,
                        .BatchReference = "SMSMerchant " & Format(DateTime.Now(), "dd mmm yyyy")
                    }
                    iBanktrxBatch = xBankTrxBatchAdapter.GetCurrentBatch(iBanktrxBatch, sqlConn)

                    'record the EcoCash request as a pending bank transaction for another handler to pick up
                    Dim iBankTrx As New xBankTrx With {
                        .BankTrxBatchID = iBanktrxBatch.BankTrxBatchID,
                        .Amount = iSMS.SMSText.Split(" ")(1),
                        .TrxDate = DateTime.Now,
                        .RefName = If(IsUSD, $"USD-{iAccess.AccessID}", iSMS.SMSID),
                        .Identifier = iSMS.Mobile,
                        .Branch = If(IsUSD, $"API-{iAccess.AccessID}", "na"),
                        .BankRef = "pending",
                        .Balance = 0
                    }
                    iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.EcoCashPending
                    iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Pending

                    xBankTrxAdapter.Insert(iBankTrx, sqlConn)


                    Dim ecocashservice = New xEcoCashAPI()
                    If IsUSD Then
                        Dim APIUsername As String = ConfigurationManager.AppSettings("EcocashAPIUsernameUSD")
                        Dim APIPassword As String = ConfigurationManager.AppSettings("EcocashAPIPasswordUSD")
                        Dim APIMechantCode As String = ConfigurationManager.AppSettings("MerchantCodeUSD")
                        Dim APIMerchantPIN As String = ConfigurationManager.AppSettings("MerchantPINUSD")
                        Dim APIMerchantNumber As String = ConfigurationManager.AppSettings("MerchantNumberUSD")
                        ecocashservice = New xEcoCashAPI(APIUsername, APIPassword, APIMechantCode, APIMerchantPIN, APIMerchantNumber)

                    End If
                    Dim iEcoCashRequestTransaction As xEcoCashTransaction = ecocashservice.ChargeNumber(iBankTrx.Identifier, iBankTrx.Amount, iBankTrx.BankTrxID, IsUSD)
                    iBankTrx.BankRef = iEcoCashRequestTransaction.ecocashReference

                    If iEcoCashRequestTransaction.ValidResponse = True And iEcoCashRequestTransaction.ErrorData = "" Then
                        iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.BusyConfirming
                        'Log("SMS Handler Service", "xProcess", "HandleEcoCash", iBankTrx.BankTrxID & ":" & iEcoCashRequestTransaction.ErrorData)
                        'iReply.TemplateText = "EcoCash request Received for $" & FormatNumber(iBankTrx.Amount, 0) & " Please dial *151*200# and follow instructions to authorise your payment to Comm Shop"
                    Else
                        iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Failed
                        iBankTrx.BankRef = iEcoCashRequestTransaction.ErrorData
                        Log("SMS Handler Service", "xProcess", "HandleEcoCash", iBankTrx.BankTrxID & ":" & iEcoCashRequestTransaction.transactionOperationStatus & ":" & iEcoCashRequestTransaction.ErrorData, iSMS.SMSID)
                        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.PaymentFailed, sqlConn)
                        If iEcoCashRequestTransaction.ErrorData.Contains("The underlying connection was closed: An unexpecte") Then
                            iEcoCashRequestTransaction.ErrorData = "Ecocash platform down, please try again later. HOT Recharge"
                        End If
                        iReply.TemplateText = "EcoCash request Failed: " & iEcoCashRequestTransaction.ErrorData & ". Please try again. HOT Recharge"
                        'List of Replies to Process
                        Dim iList As New List(Of xTemplate) From {
                        iReply
                    }
                        Reply(iSMS, iList, sqlConn)
                    End If
                    xBankTrxAdapter.Save(iBankTrx, sqlConn)


                    iSMS.State.StateID = xState.States.Success
                    xSMSAdapter.Save(iSMS, sqlConn)
                End If
            End If

        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "HandleEcoCash", ex.ToString, iSMS.SMSID)
        End Try
    End Sub
#End Region

#Region "Handle Self Top Up"
    Private ReadOnly ProfileID_SelfTopUp As Integer = 6
    Private Sub HandleSelfTopUp(iSMS As xSMS, sqlConn As SqlConnection)
        Try

            If ConfigurationManager.AppSettings("DisableEcoCash") Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.PaymentFailed, sqlConn)
                iReply.TemplateText = "EcoCash request Failed: Ecocash platform down, please try again later. HOT Recharge"
                'List of Replies to Process
                Dim iList As New List(Of xTemplate) From {
                        iReply
                    }
                Reply(iSMS, iList, sqlConn)
                Exit Sub
            End If
            Dim Mobile = If(iSMS.SMSText.Split(" ").Length = 3, iSMS.SMSText.Split(" ")(2), iSMS.Mobile)

            If ConfigurationManager.AppSettings("TelecelDisabled") And Mobile.StartsWith("073") Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeVASDisabled, sqlConn)
                iReply.TemplateText = "Please note Telecel is down and currently unable to restore their systems. Please do not try to process transactions until further notice 
HOT Recharge"
                'List of Replies to Process
                Dim iList As New List(Of xTemplate) From {
                        iReply
                    }
                Reply(iSMS, iList, sqlConn)
                Exit Sub
            End If

            Dim IsUSD As Boolean = False
            If iSMS.SMSText.ToLower().Split(" ")(1).EndsWith("u") Then
                IsUSD = True
                iSMS.SMSText = iSMS.SMSText.ToLower().Replace("u", "")
            End If
            If ConfigurationManager.AppSettings("AllowZWLEcoCash") = False And Not IsUSD Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.PaymentFailed, sqlConn)
                iReply.TemplateText = "EcoCash ZWL funding is currently not available. Please try 'ECOCASH [Amount]U' if you are trying to fund USD. HOT Recharge"
                'List of Replies to Process
                Dim iList As New List(Of xTemplate) From {
                    iReply
                }
                Reply(iSMS, iList, sqlConn)
                Exit Sub
            End If
            Dim iResponses As New List(Of xTemplate)
            'Check that an account exists for the number
            Dim iAccess As xAccess = xAccessAdapter.SelectCode(iSMS.Mobile, sqlConn)
            Dim iAccount As xAccount
            If iAccess Is Nothing Then
                ' Create New Account
                iAccount = New xAccount With {
                    .AccountName = "SelfTopUp-" + iSMS.Mobile,
                    .NationalID = iSMS.Mobile,
                    .Email = "",
                    .ReferredBy = iSMS.Mobile
                }
                iAccount.Profile.ProfileID = ProfileID_SelfTopUp
                xAccountAdapter.Save(iAccount, sqlConn)

                'Create New Access
                Dim rnd As New Random()
                iAccess = New xAccess With {
                    .AccountID = iAccount.AccountID,
                    .AccessPassword = rnd.Next(1, 9999).ToString.PadLeft(4, "0"),
                    .AccessCode = iSMS.Mobile
                }
                iAccess.Channel.ChannelID = xChannel.Channels.SMS
                xAccessAdapter.Save(iAccess, sqlConn)

            Else
                iAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
                ' BZ removed to allow dealsers to Top up directly from ecocash 
                ' If Not iAccount.Profile.ProfileID = ProfileID_SelfTopUp Then HandleUnknown(iSMS, sqlConn)
                ' Exit Sub
            End If

            ' Validate Amount 
            If Not IsNumeric(iSMS.SMSText.Split(" ")(1)) Then _
                iResponses.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeFormat, sqlConn)) : GoTo Reply

            'Initialize Recharge Object
            Dim iRecharge As New xRecharge With
                {
                    .AccessID = iAccess.AccessID,
                    .Amount = CDec(iSMS.SMSText.Split(" ")(1)),
                    .Mobile = If(iSMS.SMSText.Split(" ").Length = 3, iSMS.SMSText.Split(" ")(2), iSMS.Mobile),
                    .RechargeDate = Date.Now
                }

            ' Check If recharge is within acceptable amount
            If iRecharge.Amount < _Config.MinRecharge Or iRecharge.Amount > _Config.MaxRecharge Then _
                InvalidAmountResponse(iRecharge, iResponses, sqlConn) : GoTo Reply

            ' Check if valid Number
            If IsInvalidMobileNumber(sqlConn, iRecharge) Then _
                InvalidMobileResponse(iSMS, iRecharge, iResponses, sqlConn) : GoTo Reply

            Log("SMS Handler Service", "xProcess", "HandleSelfTopUp", $"Is Econet:{IsEconetZWLTransaction(iRecharge)}|{EconetLimitsEnabled}", iSMS.SMSID)

            If EconetLimitsEnabled And IsEconetZWLTransaction(iRecharge) Then
                Dim network As xNetwork = xNetwork_Data.Identify(iRecharge.Mobile, sqlConn)
                Dim limit As xLimit = xLimitAdapter.GetLimit(network.NetworkID, iAccount.AccountID, sqlConn)
                If limit IsNot Nothing Then
                    If limit.LimitRemaining < iRecharge.Amount Then
                        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(
                                    If(limit.LimitTypeId = 1, xTemplate.Templates.FailedEconetDailyLimit, xTemplate.Templates.FailedEconetMonthlyLimit), sqlConn)
                        iReply.TemplateText = iReply.TemplateText.Replace("%LIMIT%", limit.LimitRemaining.ToString("#,##0.00"))
                        iResponses.Add(iReply)
                        GoTo Reply
                    End If
                End If
            End If

            ' Check if Account has enough fund for transaction
            If iAccount.SaleValue < CDec(iSMS.SMSText.Split(" ")(1)) Or Not iAccount.Profile.ProfileID = ProfileID_SelfTopUp Then
                ' Fund Account
                RequestEcocashFundAccount(iSMS, iAccount, iResponses, sqlConn)
                ' Transaction will be completed by Ecocash handler.
                Exit Sub
            End If

            'Get Discount
            iRecharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, iRecharge.Brand.BrandID, sqlConn).Discount

            'Insert recharge and handover to Recharge Service
            iSMS.State.State = xState.States.Busy
            xSMSAdapter.Save(iSMS, sqlConn)
            iRecharge.State.StateID = xState.States.Pending
            xRechargeAdapter.Save(iRecharge, sqlConn, iSMS.SMSID)

            Exit Sub
Reply:
            Reply(iSMS, iResponses, sqlConn)
        Catch ex As Exception
            Log("SMS Handler Service", "xProcess", "HandleSelfTopUp", ex.ToString, iSMS.SMSID)
        End Try
    End Sub

    Private Sub RequestEcocashFundAccount(iSMS As xSMS, iAccount As xAccount, ByRef iList As List(Of xTemplate), sqlConn As SqlConnection)
        'get the current batch ID if one was done today, else create a new one
        Dim iBanktrxBatch As New xBankTrxBatch With {
            .BankID = xBank.Banks.EcoMerchant,
            .LastUser = "SMSUser",
            .BatchDate = Now,
            .BatchReference = "SMSMerchant " & Format(DateTime.Now(), "dd mmm yyyy")
        }
        iBanktrxBatch = xBankTrxBatchAdapter.GetCurrentBatch(iBanktrxBatch, sqlConn)
        Dim ecocashamount As Decimal = CDec(iSMS.SMSText.Split(" ")(1)) - iAccount.Balance
        If ecocashamount <= 0 Then ecocashamount = CDec(iSMS.SMSText.Split(" ")(1))
        'record the EcoCash request as a pending bank transaction for another handler to pick up
        Dim iBankTrx As New xBankTrx With {
            .BankTrxBatchID = iBanktrxBatch.BankTrxBatchID,
            .Amount = ecocashamount,
            .TrxDate = DateTime.Now,
            .RefName = iSMS.SMSID,
            .Identifier = iSMS.Mobile,
            .Branch = "self",
            .BankRef = "pending",
            .Balance = 0
        }
        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.EcoCashPending
        iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Pending

        xBankTrxAdapter.Insert(iBankTrx, sqlConn)
        Dim iEcoCashRequestTransaction As xEcoCashTransaction = (New xEcoCashAPI()).ChargeNumber(iSMS.Mobile, iBankTrx.Amount, iBankTrx.BankTrxID)
        iBankTrx.BankRef = iEcoCashRequestTransaction.ecocashReference

        If iEcoCashRequestTransaction.ValidResponse = True And iEcoCashRequestTransaction.ErrorData = "" Then
            iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.BusyConfirming
            'Log("SMS Handler Service", "xProcess", "HandleEcoCash", iBankTrx.BankTrxID & ":" & iEcoCashRequestTransaction.ErrorData)
            'iReply.TemplateText = "EcoCash request Received for $" & FormatNumber(iBankTrx.Amount, 0) & " Please dial *151*200# and follow instructions to authorise your payment to Comm Shop"
        Else
            iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Failed
            iBankTrx.BankRef = iEcoCashRequestTransaction.ErrorData
            Log("SMS Handler Service", "xProcess", "RequestEcocashFundAccount", iBankTrx.BankTrxID & ":" & iEcoCashRequestTransaction.transactionOperationStatus & ":" & iEcoCashRequestTransaction.ErrorData, iSMS.SMSID)
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.PaymentFailed, sqlConn)
            iReply.TemplateText = "EcoCash request Failed: " & iEcoCashRequestTransaction.ErrorData & ". Please try again. HOT Recharge"
            'List of Replies to Process
            iList.Add(iReply)
            Reply(iSMS, iList, sqlConn)
        End If
        xBankTrxAdapter.Save(iBankTrx, sqlConn)

        iSMS.State.StateID = xState.States.Success
        xSMSAdapter.Save(iSMS, sqlConn)

    End Sub

    Private Sub InvalidMobileResponse(iSMS As xSMS, iRecharge As xRecharge, ByRef iList As List(Of xTemplate), sqlconn As SqlConnection)
        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlconn)
        iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
        iList.Add(iReply)
    End Sub

    Private Sub InvalidAmountResponse(iRecharge As xRecharge, ByRef iList As List(Of xTemplate), sqlconn As SqlConnection)
        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeRange, sqlconn)
        iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", FormatNumber(_Config.MinRecharge, 2))
        iReply.TemplateText = iReply.TemplateText.Replace("%MAX%", FormatNumber(_Config.MaxRecharge, 2))
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
                iRecharge.Brand = xBrandAdapter.GetBrand(network, suffix, sqlConn)
            Catch ex As Exception
                If network.NetworkID = xNetwork.Networks.Econet078 Then
                    Dim EconetNetwork As xNetwork = xNetwork_Data.Identify("0772000000", sqlConn)
                    iRecharge.Brand = xBrandAdapter.GetBrand(EconetNetwork, suffix, sqlConn)
                Else
                    Throw ex
                End If
            End Try
        Else
            iRecharge.Brand = xBrandAdapter.GetBrand(network, " ", sqlConn)
        End If

        If iRecharge.Brand Is Nothing Then Return True
        If Not IsNumeric(iRecharge.Mobile) Then Return True
        Return False
    End Function


#End Region

#Region "Handle Reset Pin"

    Function ResetPinIfPossible(ByRef iSMS As xSMS, ByVal sqlConn As SqlConnection) As Boolean

        'Replies list
        Dim iList As New List(Of xTemplate)

        Dim iAccess As xAccess = xAccessAdapter.SelectCode(iSMS.Mobile, sqlConn)
        If iAccess Is Nothing Then
            iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.FailedNotRegistered, sqlConn))
            iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRegister, sqlConn))
            Reply(iSMS, iList, sqlConn)
            Return False
        End If

        Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
        If iAccount.Balance >= MaxResetPinAmount Then Return False

        'Create New Access Password
        Dim rnd As New Random()
        iAccess.AccessPassword = rnd.Next(1, 9999).ToString.PadLeft(4, "0")
        iAccess.Channel.ChannelID = xChannel.Channels.SMS
        xAccessAdapter.Save(iAccess, sqlConn)

        'Successful Reply
        'With Password
        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRegistration, sqlConn)
        iReply.TemplateText = iReply.TemplateText.Replace("%PASSWORD%", iAccess.AccessPassword)
        'Send General Help 
        iList.Add(iReply)
        'Send Help on Pins
        'iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRechargePins, sqlConn))
        Reply(iSMS, iList, sqlConn)

        Return True
    End Function

#End Region


End Class
