Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Api.Backend.Models
Imports Hot.Core
Imports Hot.Core.Brands
Imports Hot.Data
Imports Hot.Api.Backend.Ecocash

Public Class DealerService

    Private ReadOnly _applicationName As String = Assembly.GetCallingAssembly().GetName().Name
    Private ReadOnly _typeName As String = [GetType]().Name
    Private ReadOnly _isTestMode As Boolean
    Private ReadOnly _referencePrefix = ConfigurationManager.AppSettings("referencePrefix")

    Private ReadOnly _connString = Config.GetConnectionString()
    Private ReadOnly ProfileID_SelfTopUp As Integer = 6
    ReadOnly _webRequest As WebRequestService = New WebRequestService(_connString)
    ReadOnly _agentservice As AgentService = New AgentService(Config.IsTestMode())

    Private ReadOnly _econetLimitsEnabled = ConfigurationManager.AppSettings("EnableEconetLimits")

    Sub New(isTestMode As Boolean)
        _isTestMode = isTestMode
    End Sub

#Region "   Selftop Up Data   "

    Public Function SelfTopUpData(request As SelfTopUpDataRequest, context As Context) As SelfTopUpResponse
        Dim response As New SelfTopUpResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.EcoChargeSelf, True)
        response = _selfTopUpData(request)
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.EcoChargeSelf, False)
        Return response
    End Function



    Private Function _selfTopUpData(request As SelfTopUpDataRequest) As SelfTopUpResponse
        Dim response As SelfTopUpResponse = New SelfTopUpResponse()
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            Try
                sqlConn.Open()
                Dim TargetNumber As String = Sanitize(request.TargetMobile).Replace("-", "")
                Dim EcocashNumber As String = Sanitize(request.BillerMobile).Replace("-", "")
                Dim Amount As Decimal = request.Amount
                If ConfigurationManager.AppSettings("DisableEcoCash") Then
                    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "EcoCash request Failed: Ecocash platform down, please try again later. HOT Recharge"}
                End If

                If Not IsValidNumber(EcocashNumber) And IsEcoCashNumber(EcocashNumber) Then
                    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "Please put in a valid ecocash mobile number"}
                End If

                If Not IsValidNumber(TargetNumber) Then
                    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "Please put in a valid target mobile number"}
                End If
                ' Check If recharge is within acceptable amount
                Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
                If request.Amount < iConfig.MinRecharge Or request.Amount > iConfig.MaxRecharge Then
                    Dim reply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
                    reply.TemplateText = reply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(iConfig.MinRecharge, 2))
                    reply.TemplateText = reply.TemplateText.Replace("%MAX%", Formatting.FormatAmount(iConfig.MaxRecharge, 2))

                    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = reply.TemplateText}
                End If
                Dim suffix As String = GetSuffix(request.BrandId, sqlConn)
                Return CreateSelfTopUp(EcocashNumber, TargetNumber + suffix, Amount)

                'If Not IsAllowedtoSelfTopUp(EcocashNumber) Then
                '    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "Invalid Account Type please use standard method"}
                'End If

                '        Dim iResponses As New List(Of xTemplate)
                '        'Check that an account exists for the number
                '        Dim iAccess As xAccess = xAccessAdapter.SelectCode(request.BillerMobile, sqlConn)
                '        Dim iAccount As xAccount
                '        If iAccess Is Nothing Then
                '            ' Create New Account
                '            iAccount = New xAccount With {
                '    .AccountName = "SelfTopUp-" + request.BillerMobile,
                '    .NationalID = request.BillerMobile,
                '    .Email = "",
                '    .ReferredBy = request.BillerMobile
                '}
                '            iAccount.Profile.ProfileID = ProfileID_SelfTopUp
                '            xAccountAdapter.Save(iAccount, sqlConn)

                '            'Create New Access
                '            Dim rnd As New Random()
                '            iAccess = New xAccess With {
                '    .AccountID = iAccount.AccountID,
                '    .AccessPassword = rnd.Next(1, 9999).ToString.PadLeft(4, "0"),
                '    .AccessCode = request.BillerMobile
                '}
                '            iAccess.Channel.ChannelID = xChannel.Channels.SMS
                '            xAccessAdapter.Save(iAccess, sqlConn)
                '        Else
                '            iAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)

                '        End If

                '        ' Check if Account has enough fund for transaction
                '        If iAccount.SaleValue < request.Amount Or iAccount.Profile.ProfileID <> ProfileID_SelfTopUp Then
                '            ' Fund Account
                '            Return RequestEcocashFundAccount(request, iAccount, sqlConn)
                '        End If

                '        Dim replytemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRechargeVAS_Customer, sqlConn)

                '        Dim rechargerequest = New RechargePinlessRequest() With {
                '            .Amount = request.Amount,
                '            .TargetMobile = request.TargetMobile,
                '            .CustomerSMS = replytemplate.TemplateText
                '        }
                '        Dim cloneContext = New Context With {
                '            .AccessCode = iAccess.AccessCode,
                '            .AccessPassword = iAccess.AccessPassword,
                '            .AccessId = iAccess.AccessID,
                '            .AgentReference = DateTime.Now.ToString(),
                '            .AccountId = iAccount.AccountID
                '        }

                '        _agentservice.Recharge(rechargerequest, cloneContext)

                ''Initialize Recharge Object
                'Dim iRecharge As New xRecharge With
                '    {
                '        .AccessID = iAccess.AccessID,
                '        .Amount = request.Amount,
                '        .Mobile = request.TargetMobile,
                '        .RechargeDate = Date.Now
                '    }


                '' Check if valid Number
                'If IsInvalidMobileNumber(sqlConn, iRecharge) Then
                '    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "Please put in a valid target mobile number"}

                'End If

                ''Get Discount
                'iRecharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, iRecharge.Brand.BrandID, sqlConn).Discount

                ''Insert recharge and handover to Recharge Service 
                'iRecharge.State.StateID = xState.States.Pending
                'xRechargeAdapter.Save(iRecharge, sqlConn)

            Catch ex As Exception
                Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "Error processing transaction please retry later"}
            End Try
        End Using
        Return response
    End Function

    Private Function GetSuffix(brandId As Integer, sqlConn As SqlConnection) As String
        Dim list = xBrandAdapter.List(sqlConn)
        Dim item = From a In list
                   Where a.BrandID = brandId
                   Select a.BrandSuffix
        Return item.SingleOrDefault()
    End Function



#End Region


#Region "   SelfTopUp   "

    Public Function SelfTopUp(request As SelfTopUpRequest, context As Context) As SelfTopUpResponse
        Dim response As New SelfTopUpResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.EcoChargeSelf, True)
        response = _selfTopUp(request)
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.EcoChargeSelf, False)
        Return response
    End Function


    Private Function CreateSelfTopUp(ecocashNumber As String, targetNumber As String, amount As Decimal) As SelfTopUpResponse
        Dim response As SelfTopUpResponse = New SelfTopUpResponse()
        Try
            Using sqlConn As New SqlConnection(Config.GetConnectionString())
                sqlConn.Open()
                Dim iSMS As xSMS = New xSMS() With {
                    .SmppID = 1,
                    .Direction = True,
                    .Mobile = ecocashNumber,
                    .SMSText = "TopUp " + amount.ToString() + " " + targetNumber
                }

                xSMSAdapter.Save(iSMS, sqlConn)
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRechargeReceived, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", "")
                response.ReplyCode = 2
                response.ReplyMsg = iReply.TemplateText
            End Using
        Catch ex As Exception
            Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "Error processing transaction please retry later"}
        End Try
        Return response
    End Function

    Private Function _selfTopUp(request As SelfTopUpRequest) As SelfTopUpResponse
        Dim response As SelfTopUpResponse = New SelfTopUpResponse()
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            Try
                sqlConn.Open()
                Dim TargetNumber As String = Sanitize(request.TargetMobile).Replace("-", "")
                Dim EcocashNumber As String = Sanitize(request.BillerMobile).Replace("-", "")
                Dim Amount As Decimal = request.Amount
                If ConfigurationManager.AppSettings("DisableEcoCash") Then
                    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "EcoCash request Failed: Ecocash platform down, please try again later. HOT Recharge"}
                End If

                If Not IsValidNumber(EcocashNumber) And IsEcoCashNumber(EcocashNumber) Then
                    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "Please put in a valid ecocash mobile number"}
                End If

                If Not IsValidNumber(TargetNumber) Then
                    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "Please put in a valid target mobile number"}
                End If
                ' Check If recharge is within acceptable amount
                Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
                If request.Amount < iConfig.MinRecharge Or request.Amount > iConfig.MaxRecharge Then
                    Dim reply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
                    reply.TemplateText = reply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(iConfig.MinRecharge, 2))
                    reply.TemplateText = reply.TemplateText.Replace("%MAX%", Formatting.FormatAmount(iConfig.MaxRecharge, 2))

                    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = reply.TemplateText}
                End If

                Dim iAccess As xAccess = xAccessAdapter.SelectCode(request.BillerMobile, sqlConn)

                If _econetLimitsEnabled And IsEconetZWLTransaction(request.TargetMobile) And iAccess IsNot Nothing Then
                    Dim network As xNetwork = xNetwork_Data.Identify(TargetNumber, sqlConn)
                    Dim limit As xLimit = xLimitAdapter.GetLimit(1, iAccess.AccountID, sqlConn)
                    If limit IsNot Nothing Then
                        If limit.LimitRemaining < request.Amount And Not limit.LimitRemaining = -1 Then
                            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(
                                    If(limit.LimitTypeId = 1, xTemplate.Templates.FailedEconetDailyLimit, xTemplate.Templates.FailedEconetMonthlyLimit), sqlConn)
                            iReply.TemplateText = iReply.TemplateText.Replace("%LIMIT%", limit.LimitRemaining.ToString("#,##0.00"))
                            Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = iReply.TemplateText}
                        End If
                    End If
                End If

                Return CreateSelfTopUp(EcocashNumber, TargetNumber, Amount)

                'If Not IsAllowedtoSelfTopUp(EcocashNumber) Then
                '    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "Invalid Account Type please use standard method"}
                'End If

                '        Dim iResponses As New List(Of xTemplate)
                '        'Check that an account exists for the number
                '        Dim iAccess As xAccess = xAccessAdapter.SelectCode(request.BillerMobile, sqlConn)
                '        Dim iAccount As xAccount
                '        If iAccess Is Nothing Then
                '            ' Create New Account
                '            iAccount = New xAccount With {
                '    .AccountName = "SelfTopUp-" + request.BillerMobile,
                '    .NationalID = request.BillerMobile,
                '    .Email = "",
                '    .ReferredBy = request.BillerMobile
                '}
                '            iAccount.Profile.ProfileID = ProfileID_SelfTopUp
                '            xAccountAdapter.Save(iAccount, sqlConn)

                '            'Create New Access
                '            Dim rnd As New Random()
                '            iAccess = New xAccess With {
                '    .AccountID = iAccount.AccountID,
                '    .AccessPassword = rnd.Next(1, 9999).ToString.PadLeft(4, "0"),
                '    .AccessCode = request.BillerMobile
                '}
                '            iAccess.Channel.ChannelID = xChannel.Channels.SMS
                '            xAccessAdapter.Save(iAccess, sqlConn)
                '        Else
                '            iAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)

                '        End If

                '        ' Check if Account has enough fund for transaction
                '        If iAccount.SaleValue < request.Amount Or iAccount.Profile.ProfileID <> ProfileID_SelfTopUp Then
                '            ' Fund Account
                '            Return RequestEcocashFundAccount(request, iAccount, sqlConn)
                '        End If

                '        Dim replytemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRechargeVAS_Customer, sqlConn)

                '        Dim rechargerequest = New RechargePinlessRequest() With {
                '            .Amount = request.Amount,
                '            .TargetMobile = request.TargetMobile,
                '            .CustomerSMS = replytemplate.TemplateText
                '        }
                '        Dim cloneContext = New Context With {
                '            .AccessCode = iAccess.AccessCode,
                '            .AccessPassword = iAccess.AccessPassword,
                '            .AccessId = iAccess.AccessID,
                '            .AgentReference = DateTime.Now.ToString(),
                '            .AccountId = iAccount.AccountID
                '        }

                '        _agentservice.Recharge(rechargerequest, cloneContext)

                ''Initialize Recharge Object
                'Dim iRecharge As New xRecharge With
                '    {
                '        .AccessID = iAccess.AccessID,
                '        .Amount = request.Amount,
                '        .Mobile = request.TargetMobile,
                '        .RechargeDate = Date.Now
                '    }


                '' Check if valid Number
                'If IsInvalidMobileNumber(sqlConn, iRecharge) Then
                '    Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "Please put in a valid target mobile number"}

                'End If

                ''Get Discount
                'iRecharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, iRecharge.Brand.BrandID, sqlConn).Discount

                ''Insert recharge and handover to Recharge Service 
                'iRecharge.State.StateID = xState.States.Pending
                'xRechargeAdapter.Save(iRecharge, sqlConn)

            Catch ex As Exception
                Return New SelfTopUpResponse() With {.ReplyCode = 3, .ReplyMsg = "Error processing transaction please retry later"}
            End Try
        End Using
        Return response
    End Function
    Private Function RequestEcocashFundAccount(request As SelfTopUpRequest, iAccount As xAccount, sqlConn As SqlConnection) As SelfTopUpResponse
        Dim response As SelfTopUpResponse = New SelfTopUpResponse
        'get the current batch ID if one was done today, else create a new one
        Dim iBanktrxBatch As New xBankTrxBatch With {
            .BankID = xBank.Banks.EcoMerchant,
            .LastUser = "SMSUser",
            .BatchDate = Now,
            .BatchReference = "SMSMerchant " & Format(DateTime.Now(), "dd mmm yyyy")
        }
        iBanktrxBatch = xBankTrxBatchAdapter.GetCurrentBatch(iBanktrxBatch, sqlConn)
        Dim ecocashamount As Decimal = request.Amount - iAccount.Balance
        If ecocashamount <= 0 Then ecocashamount = request.Amount

        'record the EcoCash request as a pending bank transaction for another handler to pick up
        Dim iBankTrx As New xBankTrx With {
            .BankTrxBatchID = iBanktrxBatch.BankTrxBatchID,
            .Amount = Math.Round(ecocashamount, 2),
            .TrxDate = DateTime.Now,
            .RefName = $"{DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalSeconds()}",
            .Identifier = request.BillerMobile,
            .Branch = "self",
            .BankRef = "pending",
            .Balance = 0
        }
        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.EcoCashPending
        iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Pending

        xBankTrxAdapter.Insert(iBankTrx, sqlConn)
        Dim client As New EcocashAPI.EcocashSoapClient()

        Dim result As EcocashAPI.EcocashResult = client.Charge("Hot263180", 1, request.BillerMobile, iBankTrx.BankTrxID.ToString, iBankTrx.Amount, "HotRecharge")
        Dim iEcoCashRequestTransaction = result.Item


        If result.ValidResponse = True Then
            iBankTrx.BankRef = iEcoCashRequestTransaction.EcocashReference
            iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.BusyConfirming
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRechargeReceived, sqlConn)
            iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", "")
            response.ReplyCode = 2
            response.ReplyMsg = iReply.TemplateText
        Else
            iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Failed
            iBankTrx.BankRef = result.ErrorData


            response.ReplyMsg = "EcoCash request Failed: " & result.ErrorData & ". Please try again. HOT Recharge"

        End If
        xBankTrxAdapter.Save(iBankTrx, sqlConn)

        Return response

    End Function


    Private Function IsAllowedtoSelfTopUp(ecocashNumber As String) As Boolean
        Try
            Using sqlConn As New SqlConnection(Config.GetConnectionString())
                sqlConn.Open()
                Dim iAccess As xAccess = xAccessAdapter.SelectCode(ecocashNumber, sqlConn)
                If iAccess Is Nothing Then Return True
                Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
                If iAccount.Profile.ProfileID = 6 Then Return True
            End Using
        Catch ex As Exception
        End Try
        Return False
    End Function

    Private Function IsEcoCashNumber(ecocashNumber As String) As Boolean
        If ecocashNumber.StartsWith("077") Then Return True
        If ecocashNumber.StartsWith("078") Then Return True
        Return False
    End Function

    Public Shared Function IsValidNumber(Number As String) As Boolean
        Dim isValid As Match = Regex.Match(Number, "^[0](((71)|(73)|(77)|(78))\d|(8644))\d\d\d\d\d\d$")
        Return If(isValid.Success, True, False)
    End Function

    Public Shared Function Sanitize(ByVal input As String) As String
        input = Regex.Escape(input.Replace(";", ""))
        Return input.Replace("\", "")
    End Function

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

#Region "   Reset Password   "

    Public Function ResetPassword(request As PinResetRequest, context As Context) As PinResetResponse

        Dim response As New PinResetResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Registration, True)
        response = _resetPassword(request, context)
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.Registration, False)
        Return response

    End Function

    Private Function _resetPassword(request As PinResetRequest, context As Context) As PinResetResponse
        If Not (context.AccessCode = "wbot@hot.co.zw" And context.AccessPassword = "WBot78966100") Then
            Return PinResetErorrMessage($"Unauthorised {request.TargetNumber } by {request.RequestingNumber}", context.AgentReference)
        End If

        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            Try
                sqlConn.Open()
                Dim iAccess As xAccess = xAccessAdapter.SelectCode(request.TargetNumber, sqlConn)

                If iAccess Is Nothing Then
                    Return PinResetErorrMessage($"No valid Account for {request.TargetNumber}", context.AgentReference)
                End If

                If Not iAccess.Channel.ChannelID = xChannel.Channels.SMS Then
                    Return PinResetErorrMessage($"Only Phone Numbers can be reset via API. {request.TargetNumber } was not recognised as a valid number.", context.AgentReference)
                End If

                If Not IsAllowedtoResetPin(request, iAccess, sqlConn) Then
                    Return PinResetErorrMessage($"The details for {request.TargetNumber } did not match the details set by {request.RequestingNumber} ", context.AgentReference)
                End If

                Return ProcessResetPin(request, context, iAccess, sqlConn)

            Catch
                Return PinResetErorrMessage($"Error occurred trying to reset password for {request.TargetNumber } requested by {request.RequestingNumber}", context.AgentReference)
            End Try
        End Using

        ' Return New PinResetResponse
    End Function

    Private Function IsAllowedtoResetPin(request As PinResetRequest, iAccess As xAccess, sqlConn As SqlConnection) As Boolean
        Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
        Dim searchstring = $"{iAccount.AccountName}{iAccount.NationalID}{iAccount.Email}".Replace(" ", "")
        Dim matchscore As Integer = 0

        If searchstring.Contains(request.IDNumber.Replace(" ", "")) Then
            matchscore += 5
        Else
            For Each idpart In request.IDNumber.Replace(" ", "").Split("-")
                If searchstring.Contains(idpart) And idpart.Length > 4 Then matchscore += 2
            Next
        End If

        For Each name In request.Names.Split(" ")
            If searchstring.Contains(name) Then matchscore += 5
        Next

        If iAccess.AccessCode = request.RequestingNumber Then matchscore += 10
        If iAccount.Balance < 1000 Then matchscore += 5

        Return matchscore > 10
    End Function

    Private Function ProcessResetPin(request As PinResetRequest, context As Context, iAccess As xAccess, sqlConn As SqlConnection) As PinResetResponse
        Dim rnd As New Random
        Dim iPassword As String = rnd.Next(1, 9999).ToString.PadLeft(4, "0")
        iAccess.AccessPassword = iPassword

        Dim sqltrans As SqlTransaction = sqlConn.BeginTransaction
        Try
            xAccessAdapter.PasswordChange(iAccess, iAccess.AccessPassword, sqlConn, sqltrans)

            ' !!!!!!!!
            ' Work around for Temporarily not using encrpyted Password to be removed when we swap over

            Dim sqlCmd As New SqlCommand("xAccess_PasswordChange", sqlConn, sqltrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("@AccessID", iAccess.AccessID)
            sqlCmd.Parameters.AddWithValue("@NewPassword", iPassword)
            sqlCmd.ExecuteNonQuery()

            '!!!!!!!



            sqltrans.Commit()

            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRegistration, sqlConn)
            iReply.TemplateText = iReply.TemplateText.Replace("%PASSWORD%", iPassword)

            Dim iSMSReply As New xSMS With {
            .Direction = False,
            .Mobile = iAccess.AccessCode,
            .SMSDate = Now,
            .SMSText = iReply.TemplateText
        }
            iSMSReply.Priority.PriorityID = xPriority.Priorities.Normal
            iSMSReply.State.StateID = xState.States.Pending
            xSMSAdapter.Save(iSMSReply, sqlConn)
            Return New PinResetResponse With {
            .ReplyCode = 2,
            .AgentReference = context.AgentReference,
            .ReplyMsg = $"Pincode successfully reset for {request.TargetNumber } by {request.RequestingNumber}"
            }
        Catch ex As Exception
            sqltrans.Rollback()
            Throw ex
        End Try
    End Function

    Private Shared Function PinResetErorrMessage(ErrorMessage As String, AgentReference As String) As PinResetResponse
        Return New PinResetResponse With {
                                        .ReplyCode = -1,
                                        .AgentReference = AgentReference,
                                        .ReplyMsg = ErrorMessage
                                        }
    End Function

#End Region


#Region "   Registration   "

    Private ReadOnly _RegistrationAPIKey = ConfigurationManager.AppSettings("TransferAPIKey")

    Private Function Registration_Duplicate(ByRef iSMS As RegistrationRequest, ByVal sqlConn As SqlConnection) As Boolean
        Dim iExists As xAccess = xAccessAdapter.SelectCode(iSMS.Mobile, sqlConn)
        If iExists IsNot Nothing Then
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(iExists.AccountID, sqlConn)
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRegistrationDuplicate, sqlConn)
            iReply.TemplateText = iReply.TemplateText.Replace("%NAME%", iAccount.AccountName)
            Dim iList As New List(Of xTemplate) From {
                    iReply
                }
            ReplyCustomer(iSMS.Mobile, iList, sqlConn)

            Return True
        End If
        Return False
    End Function

    Public Function HandleRegistration(req As registrationRequest, context As Context) As Response
        If req.RegistrationAPIKey = _RegistrationAPIKey Then
            Using sqlConn As New SqlConnection(Config.GetConnectionString())
                sqlConn.Open()

                If Registration_Duplicate(req, sqlConn) Then
                    Return New TransferResponse With {
                        .AgentReference = context.AgentReference,
                        .ReplyCode = 1,
                        .ReplyMsg = "Account is already registered."
                    }
                End If

                If Not Registration_IsValid(req, sqlConn) Then
                    Return New TransferResponse With {
                        .AgentReference = context.AgentReference,
                        .ReplyCode = 1,
                        .ReplyMsg = "Invalid Registration request, please check details submited."
                    }
                End If

                If Not Registration_IsReferredValid(req, sqlConn) Then
                    Return New TransferResponse With {
                        .AgentReference = context.AgentReference,
                        .ReplyCode = 1,
                        .ReplyMsg = "The referred by dealer number is invalid"
                    }
                End If

                Dim transaction As SqlTransaction = sqlConn.BeginTransaction
                Try

                    Dim iAccount As New xAccount
                    iAccount.AccountName = $"{Sanitize(req.Surname)}, {Sanitize(req.Name)}"
                    iAccount.Email = Sanitize(req.Email)
                    iAccount.NationalID = Sanitize(req.IDNumber)
                    iAccount.ReferredBy = Sanitize(req.ReferredBy)
                    iAccount.Profile = New xProfile
                    iAccount.Profile.ProfileID = 1
                    xAccountAdapter.Save(iAccount, sqlConn, transaction)

                    Dim iAccess As New xAccess
                    iAccess.AccessCode = req.Mobile
                    Dim rnd As New Random()
                    iAccess.AccessPassword = rnd.Next(1, 9999).ToString.PadLeft(4, "0")
                    iAccess.AccountID = iAccount.AccountID
                    iAccess.Channel.ChannelID = xChannel.Channels.SMS
                    xAccessAdapter.Save(iAccess, sqlConn, transaction)

                    transaction.Commit()
                    'Successful Reply
                    'With Password
                    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRegistration, sqlConn)
                    iReply.TemplateText = iReply.TemplateText.Replace("%PASSWORD%", iAccess.AccessPassword)
                    'Send General Help 
                    Dim iList As New List(Of xTemplate) From {
                        iReply,
                        xTemplateAdapter.SelectRow(xTemplate.Templates.HelpRecharge, sqlConn)
                    }
                    Reply(iAccess, iList, sqlConn)

                    sqlConn.Close()
                    Return New Response With {
                        .AgentReference = context.AgentReference,
                        .ReplyCode = 2,
                        .ReplyMsg = "Account Registered. Pin has bee sent to you mobile number"}
                Catch ex As Exception
                    transaction.Rollback()
                    Return New TransferResponse With {
               .AgentReference = context.AgentReference,
               .ReplyCode = 1,
               .ReplyMsg = "Error occured during registration please try again."
           }
                End Try

            End Using
        Else
            Return New TransferResponse With {
                .AgentReference = context.AgentReference,
                .ReplyCode = 1,
                .ReplyMsg = "Internal Server Error - Not Implemented"
            }
        End If

    End Function

    Private Function Registration_IsReferredValid(req As RegistrationRequest, sqlConn As SqlConnection) As Boolean
        Dim refer = xAccessAdapter.SelectCode(req.ReferredBy, sqlConn)
        If refer Is Nothing Then Return False
        Return True

    End Function

    Private Function Registration_IsValid(req As RegistrationRequest, sqlConn As SqlConnection) As Boolean
        If Not ((req.Name.Length() + req.Surname.Length()) > 5) Then Return False
        If Not req.IDNumber.Length > 7 Then Return False
        If Not req.ReferredBy.Length > 9 Then Return False


        Return True
    End Function



#End Region


#Region "   Transfers   "

    Private ReadOnly _minTransfer = ConfigurationManager.AppSettings("minTransfer")
    Private ReadOnly _maxTransfer = ConfigurationManager.AppSettings("maxTransfer")
    Private ReadOnly _TransferAPIKey = ConfigurationManager.AppSettings("TransferAPIKey")

    Public Function HandleTransfer(request As TransferRequest, context As Context) As TransferResponse
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Transfer, True)
        Dim response As TransferResponse = _handleTransfer(request, context)
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.Transfer, False)
        Return response
    End Function

    Private Function _handleTransfer(ByRef request As TransferRequest, context As Context) As TransferResponse

        If request.APIKey <> _TransferAPIKey Then
            Return New TransferResponse With {
                .AgentReference = context.AgentReference,
                .ReplyCode = 2,
                .ReplyMsg = "Internal Server Error - Not Implemented"
            }
        End If

        Try
            Using sqlConn As New SqlConnection(_connString)
                sqlConn.Open()
                'List of Replies
                Dim iList As New List(Of xTemplate)
                'Get Access Code
                Dim iAccess As xAccess = xAccessAdapter.SelectRow(context.AccessId, sqlConn)
                If iAccess IsNot Nothing Then
                    'Get Account        
                    Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)

                    'Validate Amount
                    If Not IsNumeric(request.Amount) Then
                        iList.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.FailedTransferFormat, sqlConn))
                        Return TransferErrorResponse(iList, context)

                    End If

                    'Check Transfer To Mobile
                    Dim iAccessTo As xAccess = xAccessAdapter.SelectCode(request.ToNumber, sqlConn)
                    If iAccessTo Is Nothing Then
                        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedTransferMobile, sqlConn)
                        iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", request.ToNumber)
                        iList.Add(iReply)
                        Return TransferErrorResponse(iList, context)
                    End If

                    'Validate Amount Range
                    If CDec(request.Amount) < _minTransfer Or request.Amount > _maxTransfer Then
                        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedTransferMin, sqlConn)
                        iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iAccessTo.AccessCode)
                        iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", FormatNumber(CDec(request.Amount), 0))
                        iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", FormatNumber(_minTransfer, 0))
                        iList.Add(iReply)
                        Return TransferErrorResponse(iList, context)

                    End If


                    'Check Balance
                    If iAccount.Balance < CDec(request.Amount) Then
                        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedTransferBalance, sqlConn)
                        iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iAccessTo.AccessCode)
                        iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", FormatNumber(CDec(request.Amount), 0))
                        iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", FormatNumber(iAccount.Balance, 2))
                        iList.Add(iReply)
                        Return TransferErrorResponse(iList, context)

                    End If

                    'Amount
                    Dim Amount As Decimal = request.Amount

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
                    .SMSID = 0
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
                    iList = New List(Of xTemplate) From {
                        iReplySender
                    }
                    Reply(iAccess, iList, sqlConn)

                    'Receiver Reply                    
                    Dim iAccountTo As xAccount = xAccountAdapter.SelectRow(iAccessTo.AccountID, sqlConn)
                    Dim iReplyCustomer As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulTransferReceiver, sqlConn)
                    iReplyCustomer.TemplateText = iReplyCustomer.TemplateText.Replace("%REF%", iPaymentTo.Reference)
                    iReplyCustomer.TemplateText = iReplyCustomer.TemplateText.Replace("%BALANCE%", FormatNumber(iAccountTo.Balance, 2))
                    iReplyCustomer.TemplateText = iReplyCustomer.TemplateText.Replace("%SALEVALUE%", FormatNumber(iAccountTo.SaleValue, 2))
                    Dim iListCustomer As New List(Of xTemplate) From {
                    iReplyCustomer
                }
                    ReplyCustomer(iAccessTo.AccessCode, iListCustomer, sqlConn)


                End If
                Return New TransferResponse With {
            .AgentReference = context.AgentReference,
            .ReplyCode = 2,
            .ReplyMsg = iList.FirstOrDefault().TemplateText
            }


            End Using

        Catch ex As Exception
            LogError("HandleTransfer", ex)
            Return New TransferResponse With {
            .AgentReference = context.AgentReference,
            .ReplyCode = 3,
            .ReplyMsg = "Error Occured during transfer please try again later"}

        End Try

    End Function

    Private Sub LogError(methodName As String, ex As Exception, Optional idType As String = Nothing, Optional idNumber As Long = Nothing)
        xLog_Data.Save(_applicationName, _typeName, methodName, ex, _connString, idType, idNumber)
    End Sub

    Private Function TransferErrorResponse(iList As List(Of xTemplate), context As Context) As TransferResponse
        Return New TransferResponse With {
            .ReplyCode = 1,
            .AgentReference = context.AgentReference,
            .ReplyMsg = iList.FirstOrDefault().TemplateText
        }
    End Function



#End Region

#Region "   Common   "

    Private Function IsEconetZWLTransaction(mobile As String) As Boolean
        If mobile.StartsWith("077") Or mobile.StartsWith("078") Then Return True
        Return False
    End Function

    Private Sub SaveWebRequest(context As Context, replyCode As Integer, replyMsg As String, type As xHotType.HotTypes, isRequest As Boolean, Optional rechargeId As Integer? = Nothing)

        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = replyMsg,
            .ReturnCode = replyCode,
            .ChannelID = xChannel.Channels.Web,
            .StateID = xState.States.Success,
            .RechargeID = rechargeId,
            .HotTypeID = type
        }
        web.WalletBalance = GetWalletBalance(context.AccountId)
        _webRequest.Save(web, isRequest)
    End Sub

    Private Function GetWalletBalance(accountId As Long) As Decimal
        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim account = xAccountAdapter.SelectRow(accountId, sqlConn)
            Return account.Balance
        End Using
    End Function

#Region " Reply "
    Private Sub Reply(context As xAccess, ByVal iList As List(Of xTemplate), ByVal sqlConn As SqlConnection)
        If context.Channel.ChannelID = xChannel.Channels.SMS Then
            For Each iReply As xTemplate In iList
                Dim iSMSReply As New xSMS With {
                .Direction = False,
                .Mobile = context.AccessCode,
                .SMSID_In = Nothing,
                .SMSText = iReply.TemplateText
            }
                iSMSReply.Priority.PriorityID = xPriority.Priorities.Normal
                iSMSReply.State.StateID = xState.States.Pending
                xSMSAdapter.Save(iSMSReply, sqlConn)
            Next
        End If

    End Sub
    Private Sub ReplyCustomer(ByVal Recipient As String, ByVal iList As List(Of xTemplate), ByVal sqlConn As SqlConnection)
        For Each iReply As xTemplate In iList
            Dim iSMSReply As New xSMS With {
                .Direction = False,
                .Mobile = Recipient,
                .SMSText = iReply.TemplateText,
                .SMSDate = Now,
                .SMSID_In = Nothing
            }
            iSMSReply.Priority.PriorityID = xPriority.Priorities.Normal
            iSMSReply.State.StateID = xState.States.Pending

            xSMSAdapter.Save(iSMSReply, sqlConn)
        Next
    End Sub
#End Region
#End Region


End Class




