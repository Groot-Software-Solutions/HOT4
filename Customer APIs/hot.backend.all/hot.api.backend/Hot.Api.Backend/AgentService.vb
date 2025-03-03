Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Api.Backend.Models
Imports Hot.Core
Imports Hot.Core.Brands
Imports Hot.Data

Public Class AgentService

    Private ReadOnly _applicationName As String = Assembly.GetCallingAssembly().GetName().Name
    Private ReadOnly _typeName As String = [GetType]().Name
    Private ReadOnly _isTestMode As Boolean
    Private ReadOnly _referencePrefix = ConfigurationManager.AppSettings("referencePrefix")
    Private ReadOnly _connString = Config.GetConnectionString()
    Private ReadOnly _econetLimitsEnabled = ConfigurationManager.AppSettings("EnableLimits")
    Private ReadOnly _econetZWLDealersEnabled = ConfigurationManager.AppSettings("EconetZWLDealersEnabled")
    Private ReadOnly _netoneZWLDealersEnabled = ConfigurationManager.AppSettings("NetoneZWLDealersEnabled")
    Private ReadOnly _econetUSDOnlyAccounts As String = ConfigurationManager.AppSettings("EconetUSDOnlyAccounts")
    Private ReadOnly _netoneUSDOnlyAccounts As String = ConfigurationManager.AppSettings("NetoneUSDOnlyAccounts")
    Private Shared ReadOnly _zesaFeesEnabled As String = ConfigurationManager.AppSettings("ZESAFeeEnabled")
    Private Shared ReadOnly _zesaFeeAmount As String = ConfigurationManager.AppSettings("ZESAFeeAmount")

    ReadOnly _webRequest As WebRequestService = New WebRequestService(_connString)


    Sub New(isTestMode As Boolean)
        _isTestMode = isTestMode
    End Sub

#Region "   USD   "

    Public Function WalletBalanceUSD(context As Context) As WalletBalanceResponse
        Dim response As New WalletBalanceResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Balance, True)
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulBalance, sqlConn)
            iReply.TemplateText = "Your HOT USD Balance is $ %BALANCE%. You can sell approximately $ %SALEVALUE%."
            iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.USDBalance))
            iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(iAccount.USDBalance))
            response.ReplyCode = 2
            response.ReplyMsg = iReply.TemplateText
            response.WalletBalance = iAccount.USDBalance
            sqlConn.Close()
        End Using
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.Balance, False)
        Return response
    End Function

    Public Function QueryUSDEvdStock(context As Context) As QueryEVDResponse
        Dim response As New QueryEVDResponse(context.AgentReference)

        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Answer, True)
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            response = _queryusdevdstock(context)
            AddTelecelPinStock(response, sqlConn, True)
            sqlConn.Close()
        End Using
        SaveWebRequest(context, 200, response.ReplyMsg, xHotType.HotTypes.Answer, False)
        Return response

    End Function

    Private Shared Sub AddTelecelPinStock(response As QueryEVDResponse, sqlConn As SqlConnection, Optional Usd As Boolean = False)
        Dim brand = If(Usd, xBrand.Brands.TelecelUSD, xBrand.Brands.Juice)
        For Each iStock As xPinStock In xPinStockAdapter.Stock(sqlConn)
            If iStock.BrandId = brand Then
                response.InStock.Add(
                New NetoneAPI.PinStockModel() With {
                    .BrandId = iStock.BrandId,
                    .BrandName = iStock.BrandName,
                    .PinValue = iStock.PinValue,
                    .Stock = 1000
                 })
            End If
        Next
    End Sub

    Private Function _queryusdevdstock(context As Context) As QueryEVDResponse
        Dim response As New QueryEVDResponse(context.AgentReference)
        Try
            Using sqlconn As New SqlConnection(_connString)
                sqlconn.Open()
                For Each iStock As xPinStock In xPinStockAdapter.Stock(sqlconn)
                    If iStock.BrandId = xBrand.Brands.EconetUSD Or
                       iStock.BrandId = xBrand.Brands.NetoneUSD Or
                       iStock.BrandId = xBrand.Brands.TelecelUSD Then
                        response.InStock.Add(
                        New NetoneAPI.PinStockModel() With {
                            .BrandId = iStock.BrandId,
                            .BrandName = iStock.BrandName,
                            .PinValue = iStock.PinValue,
                            .Stock = 1000
                         })
                    End If
                Next

            End Using
            response.ReplyMsg = If(response.InStock.Count > 0, "EVD PIN stock request successful", "We have received your EVD stock request but do not have correct stock")
            response.ReplyCode = 2 ' Success
        Catch ex As Exception
            response.ReplyMsg = "We have received your EVD stock request but failed to query stock."
            response.ReplyCode = 223 ' Failure
        End Try

        Return response
    End Function


    Public Function RechargeUSDEvd(req As RechargeEvdRequest, context As Context) As BulkEvdResponse
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, xState.States.Busy)
        Dim response = _rechargeusdevd(req, rech, context)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function

    Private Function _rechargeusdevd(request As RechargeEvdRequest, recharge As xRecharge, context As Context) As BulkEvdResponse
        Dim response As New BulkEvdResponse(context.AgentReference)

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            recharge.AccessID = context.AccessId
            recharge.Amount = request.Quantity * request.Denomination
            recharge.Denomination = request.Denomination
            recharge.Quantity = request.Quantity
            recharge.Mobile = request.TargetNumber
            recharge.Brand = xBrandAdapter.GetBrand(request.BrandId, sqlConn)


            If recharge.Mobile.Length <> 10 Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Validate Amount Range
            If recharge.Amount < iConfig.MinRecharge Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(iConfig.MinRecharge, 2))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If




            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            'Get Discount
            recharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, recharge.Brand.BrandID, sqlConn).Discount

            Dim Balance As Decimal = Nothing
            Dim Salevalue As Decimal = Nothing
            Dim HasSufficientFunds As Boolean = Nothing
            CalculateRechargeCost(recharge, iAccount, Balance, Salevalue, HasSufficientFunds)

            If Not HasSufficientFunds Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(Balance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(Salevalue))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If


            'Insert recharge
            recharge.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(recharge, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge
            Select Case recharge.Brand.BrandID
                Case xBrand.Brands.EconetUSD, xBrand.Brands.NetoneUSD, xBrand.Brands.TelecelUSD
                    Return PinsEVDRecharge(recharge, sqlConn)
                Case Else
                    Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, recharge.Brand.BrandID & " brand code is not currently supported") ' return RetailRechargePin(recharge, sqlConn)
            End Select
        End Using


        Return response
    End Function


    Private Shared Sub CalculateRechargeCost(recharge As xRecharge, iAccount As xAccount, ByRef Balance As Decimal, ByRef Salevalue As Decimal, ByRef HasSufficientFunds As Boolean)
        Dim Cost As Decimal = recharge.Amount - (recharge.Amount * (recharge.Discount / 100))
        Balance = iAccount.Balance

        If recharge.Brand.BrandID = xBrand.Brands.EconetUSD _
            Or recharge.Brand.BrandID = xBrand.Brands.NetoneUSD _
            Or recharge.Brand.BrandID = xBrand.Brands.TelecelUSD _
            Or recharge.Brand.BrandID = xBrand.Brands.TeloneUSD Then
            Balance = iAccount.USDBalance
        End If
        If recharge.Brand.BrandID = xBrand.Brands.ZETDC _
            Or recharge.Brand.BrandID = xBrand.Brands.Nyaradzo Then
            Balance = iAccount.ZESABalance
            If recharge.Brand.BrandID = xBrand.Brands.ZETDC And _zesaFeesEnabled Then Balance -= _zesaFeeAmount
        End If
        If recharge.Brand.BrandID = xBrand.Brands.ZETDCUSD _
            Or recharge.Brand.BrandID = xBrand.Brands.NyaradzoUSD Then
            Balance = iAccount.USDUtilityBalance
        End If
        Salevalue = Balance + (Balance * (recharge.Discount / 100))
        HasSufficientFunds = Not (Balance < Cost)
    End Sub

    Private Function PinsEVDRecharge(recharge As xRecharge, sqlConn As SqlConnection) As BulkEvdResponse
        Dim network = New Pin(sqlConn, _applicationName)
        Try
            Dim response As PinRechargeResponse = network.RechargePin(recharge)

            If recharge.IsSuccessFul Then
                Console.WriteLine("Recharge Successful. Replying")


                Dim templates = response.CustomerTemplates
                ReplyCustomer(recharge.Mobile, templates, sqlConn)

                Dim iAccess As xAccess = xAccessAdapter.SelectRow(recharge.AccessID, sqlConn)
                Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)

                Return New BulkEvdResponse("") With {
                    .Pins = GetListofPins(response.Pins),
                    .Amount = recharge.Amount,
                    .Discount = recharge.Discount,
                    .RechargeID = recharge.RechargeID,
                    .ReplyCode = 2,
                    .ReplyMsg = response.Templates.First().TemplateText,
                    .WalletBalance = iAccount.USDBalance
                    }
            Else
                Console.WriteLine("Recharge Failed. Replying")
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)
                ' GET TEXT FROM WEBSERVICE CUSTOMER
                iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", response.Templates.First().TemplateText)
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
            Throw
        End Try
    End Function

    Private Function GetListofPins(pins As List(Of xPin)) As List(Of String)
        'PIN,REF,VALUE,DATE
        Dim result As New List(Of String)
        For Each pin As xPin In pins
            result.Add($"{pin.Pin},{pin.PinRef},{pin.PinValue:0.00},{pin.PinExpiry:MM/dd/yyyy}")
        Next
        Return result
    End Function

#End Region

#Region "   Ecocash    "
    Public Function RequestEcocashFundAccount(request As EcocashFundingRequest, context As Context, sqlConn As SqlConnection) As EcocashAPI.Transaction
        'get the current batch ID if one was done today, else create a new one
        Dim iBanktrxBatch As New xBankTrxBatch With {
            .BankID = xBank.Banks.EcoMerchant,
            .LastUser = "SMSUser",
            .BatchDate = Now,
            .BatchReference = "SMSMerchant " & Format(DateTime.Now(), "dd-MMM-yyyy")
        }
        iBanktrxBatch = xBankTrxBatchAdapter.GetCurrentBatch(iBanktrxBatch, sqlConn)

        'record the EcoCash request as a pending bank transaction for another handler to pick up
        Dim iBankTrx As New xBankTrx With {
            .BankTrxBatchID = iBanktrxBatch.BankTrxBatchID,
            .Amount = Math.Round(request.Amount, 2),
            .TrxDate = Date.Now,
            .RefName = GetEcoCashRef(request, context),
            .Identifier = request.TargetMobile,
            .Branch = $"API-{context.AccessId}",
            .BankRef = context.AccessId,
            .Balance = 0
        }
        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.EcoCashPending
        iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Pending

        xBankTrxAdapter.Insert(iBankTrx, sqlConn)

        Dim client As New EcocashAPI.EcocashSoapClient

        Dim result As EcocashAPI.EcocashResult = client.Charge("Hot263180", request.Account, iBankTrx.Identifier, iBankTrx.BankTrxID, iBankTrx.Amount, request.OnBehalfOf)
        Dim iEcoCashRequestTransaction As EcocashAPI.Transaction = If(result.ValidResponse, result.Item, New EcocashAPI.Transaction())

        iBankTrx.BankRef = iEcoCashRequestTransaction.EcocashReference

        If result.ValidResponse Then
            iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.BusyConfirming
            'Log("SMS Handler Service", "xProcess", "HandleEcoCash", iBankTrx.BankTrxID & ":" & iEcoCashRequestTransaction.ErrorData)
            'iReply.TemplateText = "EcoCash request Received for $" & FormatNumber(iBankTrx.Amount, 0) & " Please dial *151*200# and follow instructions to authorise your payment to Comm Shop"
        Else
            iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Failed
            iBankTrx.BankRef = result.ErrorData
        End If
        xBankTrxAdapter.Save(iBankTrx, sqlConn)
        AnnoymizeEcocashTransaction(iEcoCashRequestTransaction)
        Return iEcoCashRequestTransaction

    End Function

    Private Shared Function GetEcoCashRef(request As EcocashFundingRequest, context As Context) As String
        If request.Account = 4 Then Return $"UUSD-{context.AccessId}"
        If request.Account = 3 Then Return $"USD-{context.AccessId}"
        If request.Account = 2 Then Return $"ZESA-{context.AccessId}"
        Return $"API-{context.AccessId}"
    End Function

    Private Shared Sub AnnoymizeEcocashTransaction(ByRef iEcoCashRequestTransaction As EcocashAPI.Transaction)
        iEcoCashRequestTransaction.MerchantPin = "0000"
        iEcoCashRequestTransaction.MerchantNumber = "0772929223"
    End Sub

    Public Function EcocashFundAccount(req As EcocashFundingRequest, context As Context) As EcocashAPI.Transaction
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Return RequestEcocashFundAccount(req, context, sqlConn)
            sqlConn.Close()
        End Using
    End Function

#End Region

#Region "   OneMoney   "
    Public Function OneMoneyFundAccount(req As EcocashFundingRequest, context As Context) As EcocashAPI.Transaction
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Return RequestOneMoneyFundAccount(req, context, sqlConn)
            sqlConn.Close()
        End Using
    End Function

    Public Function RequestOneMoneyFundAccount(request As EcocashFundingRequest, context As Context, sqlConn As SqlConnection) As EcocashAPI.Transaction
        'get the current batch ID if one was done today, else create a new one
        Dim iBanktrxBatch As New xBankTrxBatch With {
            .BankID = If(request.Account = 3, xBank.Banks.OneMoneyUSD, xBank.Banks.OneMoney),
            .LastUser = "SMSUser",
            .BatchDate = Now,
            .BatchReference = "SMSMerchant " & Format(DateTime.Now(), "dd-MMM-yyyy")
        }
        iBanktrxBatch = xBankTrxBatchAdapter.GetCurrentBatch(iBanktrxBatch, sqlConn)

        'record the EcoCash request as a pending bank transaction for another handler to pick up
        Dim iBankTrx As New xBankTrx With {
            .BankTrxBatchID = iBanktrxBatch.BankTrxBatchID,
            .Amount = Math.Round(request.Amount, 2),
            .TrxDate = Date.Now,
            .RefName = GetEcoCashRef(request, context),
            .Identifier = request.TargetMobile,
            .Branch = $"API-{context.AccessId}",
            .BankRef = context.AccessId,
            .Balance = 0
        }
        iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.OneMoney
        iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Pending

        xBankTrxAdapter.Insert(iBankTrx, sqlConn)

        Dim iEcoCashRequestTransaction As New EcocashAPI.Transaction() With {
            .ClientCorrelator = iBankTrx.BankTrxID,
            .PaymentAmount = New EcocashAPI.PaymentAmountResponse() With {.TotalAmountCharged = iBankTrx.Amount.ToString()},
            .TransactionOperationStatus = "PENDING",
            .EndUserId = iBankTrx.Identifier,
            .Id = iBankTrx.BankTrxID
        }

        AnnoymizeEcocashTransaction(iEcoCashRequestTransaction)
        Return iEcoCashRequestTransaction

    End Function
#End Region

#Region "   Data   "

    Public Function GetAllDataBundles(context As Context, Optional UsdBundles As Boolean = False) As GetBundleResponse
        Dim response As New GetBundleResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Answer, True)
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Dim UsdBundleList = EconetBundle.Repository.BundleRepository.List(sqlConn).Where(Function(x) x.BrandId = xBrand.Brands.EconetUSD Or x.BrandId = xBrand.Brands.NetoneUSD).ToList()
            Dim ZwlBundleList = EconetBundle.Repository.BundleRepository.List(sqlConn).Where(Function(x) Not (x.BrandId = xBrand.Brands.EconetUSD And x.BrandId = xBrand.Brands.NetoneUSD)).ToList()
            response.Bundles = If(UsdBundles, UsdBundleList, ZwlBundleList)
            response.Bundles = response.Bundles _
                .OrderBy((Function(x) x.BrandId)) _
                .ThenBy(Function(x) x.Network) _
                .ThenBy(Function(x) x.ProductCode.Substring(0, 3)) _
                .ThenBy(Function(x) x.ValidityPeriod) _
                .ThenBy(Function(x) x.Amount) _
                .ToList()
            response.ReplyCode = 2 ' Success
            sqlConn.Close()
        End Using
        SaveWebRequest(context, 200, response.Bundles.ToString(), xHotType.HotTypes.Answer, False)
        Return response
    End Function

    Public Function GetEconetDataBundles(context As Context, Optional UsdBundles As Boolean = False) As GetBundleResponse
        Dim response As New GetBundleResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Answer, True)
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Dim UsdBundleList = EconetBundle.Repository.BundleRepository.List(sqlConn).Where(Function(x) x.BrandId = xBrand.Brands.EconetUSD).ToList()
            Dim ZwlBundleList = EconetBundle.Repository.BundleRepository.List(sqlConn).Where(Function(x) Not x.BrandId = xBrand.Brands.EconetUSD).ToList()
            response.Bundles = If(UsdBundles, UsdBundleList, ZwlBundleList)
            Dim econetBrands As List(Of Integer) = New List(Of Integer) From {xBrand.Brands.EconetUSD, xBrand.Brands.Econet078, xBrand.Brands.EconetBB, xBrand.Brands.EconetData, xBrand.Brands.EconetFacebook, xBrand.Brands.EconetFacebook, xBrand.Brands.EconetInstagram, xBrand.Brands.EconetTwitter, xBrand.Brands.EconetTXT, xBrand.Brands.EconetWhatsapp, xBrand.Brands.Text}
            response.Bundles = response.Bundles.Where(Function(x) econetBrands.Contains(x.BrandId)).ToList()
            response.ReplyCode = 2 ' Success
            sqlConn.Close()
        End Using
        SaveWebRequest(context, 200, response.Bundles.ToString(), xHotType.HotTypes.Answer, False)
        Return response
    End Function

    Public Function GetNetoneDataBundles(context As Context, Optional UsdBundles As Boolean = False) As GetBundleResponse
        Dim response As New GetBundleResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Answer, True)
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Dim UsdBundleList = EconetBundle.Repository.BundleRepository.List(sqlConn).Where(Function(x) x.BrandId = xBrand.Brands.EconetUSD).ToList()
            Dim ZwlBundleList = EconetBundle.Repository.BundleRepository.List(sqlConn).Where(Function(x) Not x.BrandId = xBrand.Brands.EconetUSD).ToList()
            response.Bundles = If(UsdBundles, UsdBundleList, ZwlBundleList)
            Dim NetoneBrands As List(Of Integer) = New List(Of Integer) From {xBrand.Brands.NetoneData, xBrand.Brands.NetoneOneFi, xBrand.Brands.NetoneOneFusion, xBrand.Brands.NetoneSMS, xBrand.Brands.NetoneSocial, xBrand.Brands.NetoneUSD, xBrand.Brands.NetoneWhatsApp}
            response.Bundles = response.Bundles.Where(Function(x) NetoneBrands.Contains(x.BrandId)).ToList()
            response.ReplyCode = 2 ' Success
            sqlConn.Close()
        End Using
        SaveWebRequest(context, 200, response.Bundles.ToString(), xHotType.HotTypes.Answer, False)
        Return response
    End Function



    Public Function RechargeData(request As RechargeDataRequest, context As Context, Optional UsdTransaction As Boolean = False) As RechargeResponseModel
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, xState.States.Busy)
        Dim response = _recharge(request, rech, context, UsdTransaction)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function


    Private Sub SavePreRechargeRequest(context As Context, state As xState.States)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = Nothing,
            .ReturnCode = state,
            .ChannelID = xChannel.Channels.Web,
            .StateID = state,
            .Amount = 0,
            .WalletBalance = GetWalletBalance(context.AccountId),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, True)
    End Sub


    Private Function _recharge(request As RechargeDataRequest, ByRef recharge As xRecharge, context As Context, Optional usdtransaction As Boolean = False) As RechargeResponseModel

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            Dim bundle As EconetBundle.Models.BundleProduct = EconetBundle.Repository.BundleRepository.Get(request.ProductCode, sqlConn)

            recharge.AccessID = context.AccessId

            'Validate Bundle 
            If bundle Is Nothing Then Return InvalidBundleResponse(sqlConn, recharge, iConfig)
            If (CDec(bundle.Amount / 100) <> request.Amount And CDec(bundle.Amount) <> request.Amount) And request.Amount <> 0 Then Return InvalidBundleResponse(sqlConn, recharge, iConfig)

            recharge.Amount = (bundle.Amount / 100)
            recharge.Mobile = request.TargetMobile  ' + bundle.ProductCode.Substring(0, 1).PadLeft(2)
            xBrandAdapter.List(sqlConn)
            recharge.Brand = xBrandAdapter.BrandsById(bundle.BrandId)
            recharge.Brand.BrandSuffix = bundle.ProductCode

            If recharge.Brand.WalletTypeId = 3 And Not usdtransaction Then
                Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, bundle.ProductCode & " is not currently supported for ZiG transactions")
            End If

            If Not recharge.Brand.WalletTypeId = 3 And usdtransaction Then
                Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, bundle.ProductCode & " is not currently supported for USD transactions")
            End If

            Try
                Dim network As xNetwork = xNetwork_Data.Identify(recharge.Mobile, sqlConn)
                If network Is Nothing Then GoTo InvalidNumberError
                Select Case network.NetworkID
                    Case xNetwork.Networks.Econet, xNetwork.Networks.Econet078
                        If recharge.Mobile.Length <> 10 Then GoTo InvalidNumberError
                        If recharge.Brand.Network.NetworkID <> xNetwork.Networks.Econet Then Throw New Exception("Failed Unsupported Brand")
                    Case xNetwork.Networks.NetOne
                        If recharge.Mobile.Length <> 10 Then GoTo InvalidNumberError
                        If recharge.Brand.Network.NetworkID <> network.NetworkID Then Throw New Exception("Failed Unsupported Brand")
                    Case Else
                        Throw New Exception("Failed Unsupported Brand")
                End Select

                Exit Try
InvalidNumberError:
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)

            Catch ex As Exception
                Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, bundle.ProductCode & " is not currently supported for " & recharge.Mobile)
            End Try

            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            If _econetLimitsEnabled And IsEconetZWLTransaction(recharge) Then
                Dim network As xNetwork = xNetwork_Data.Identify(recharge.Mobile, sqlConn)
                Dim limit As xLimit = xLimitAdapter.GetLimit(network.NetworkID, iAccount.AccountID, sqlConn)
                If limit IsNot Nothing Then
                    If limit.LimitRemaining < recharge.Amount And Not limit.LimitRemaining = -1 Then
                        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(
                                    If(limit.LimitTypeId = 1, xTemplate.Templates.FailedEconetDailyLimit, xTemplate.Templates.FailedEconetMonthlyLimit), sqlConn)
                        iReply.TemplateText = iReply.TemplateText.Replace("%LIMIT%", limit.LimitRemaining.ToString("#,##0.00"))
                        Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
                    End If
                End If
            End If

            If DealerEconetZWLNotAllowed(recharge, iAccount) Or DealerNetoneZWLNotAllowed(recharge, iAccount) Then

                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeVASDisabled, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("Your SMS: %MESSAGE%", "")
                Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
            End If

            Try
                'Get Discount
                recharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, recharge.Brand.BrandID, sqlConn).Discount

            Catch ex As Exception
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeVASDisabled, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("Your SMS: %MESSAGE%", "")
                Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
            End Try

            Dim Balance As Decimal = Nothing
            Dim Salevalue As Decimal = Nothing
            Dim HasSufficientFunds As Boolean = Nothing
            CalculateRechargeCost(recharge, iAccount, Balance, Salevalue, HasSufficientFunds)

            If Not HasSufficientFunds Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(Balance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(Salevalue))
                Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
            End If



            'Sanity Check 
            If request.ProductCode = recharge.Brand.BrandSuffix Then
                'Insert recharge
                recharge.State.StateID = xState.States.Busy
                xRechargeAdapter.Save(recharge, sqlConn)

                'Determine whether to dispense pins or do direct prepaid recharge
                Select Case recharge.Brand.BrandID
                    Case xBrand.Brands.EconetData, xBrand.Brands.EconetWhatsapp, xBrand.Brands.EconetFacebook,
                         xBrand.Brands.EconetBB, xBrand.Brands.EconetInstagram, xBrand.Brands.EconetTwitter, xBrand.Brands.Text
                        Return EconetRetailDataRecharge(recharge, request.CustomerSMS, sqlConn)
                    Case xBrand.Brands.EconetUSD
                        Return EconetRetailDataRecharge(recharge, request.CustomerSMS, sqlConn)
                    Case xBrand.Brands.NetoneOneFi, xBrand.Brands.NetoneOneFusion, xBrand.Brands.NetoneWhatsApp,
                         xBrand.Brands.NetoneSocial, xBrand.Brands.NetoneData, xBrand.Brands.NetoneSMS
                        Return NetoneRetailDataRecharge(recharge, request.CustomerSMS, sqlConn)
                    Case xBrand.Brands.NetoneUSD
                        Return NetoneRetailDataUSDRecharge(recharge, request.CustomerSMS, sqlConn)
                    Case Else
                        Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, recharge.Brand.BrandSuffix & " product code is not currently supported") ' return RetailRechargePin(recharge, sqlConn)
                End Select
            Else
                Return ErrorResponse(xTemplate.Templates.FailedWebException, "Error:219, General Bundle Exception.")
            End If


        End Using
    End Function
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
    Private Function IsNetoneZWLTransaction(iRecharge As xRecharge) As Boolean
        If iRecharge.Brand.BrandID = xBrand.Brands.NetoneData Or
                iRecharge.Brand.BrandID = xBrand.Brands.NetoneOneFi Or
                iRecharge.Brand.BrandID = xBrand.Brands.NetoneOneFusion Or
                 iRecharge.Brand.BrandID = xBrand.Brands.NetoneSMS Or
                 iRecharge.Brand.BrandID = xBrand.Brands.NetoneSocial Or
                 iRecharge.Brand.BrandID = xBrand.Brands.NetoneWhatsApp Or
                 iRecharge.Brand.BrandID = xBrand.Brands.EasyCall Then Return True

        Return False
    End Function
    Private Function InvalidBundleResponse(sqlConn As SqlConnection, iRecharge As xRecharge, iConfig As xConfig) As RechargeResponseModel
        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedUnsupportedBrand, sqlConn)
        Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
    End Function

    Private Function EconetRetailDataRecharge(recharge As xRecharge, customSms As String, sqlConn As SqlConnection) As RechargeResponseModel
        Dim econetdata = New EconetData(sqlConn, _applicationName, Endpoint("EconetBundle_Endpoint"), _isTestMode, _referencePrefix, True)
        Return RechargeRetail(econetdata, recharge, customSms, sqlConn)
    End Function
    Private Function NetoneRetailDataRecharge(recharge As xRecharge, customSms As String, sqlConn As SqlConnection) As RechargeResponseModel
        Dim client = New NetoneData(sqlConn, _applicationName, Endpoint("NetoneBundle_Endpoint"), _isTestMode, _referencePrefix, True)
        Return RechargeRetail(client, recharge, customSms, sqlConn)
    End Function
    Private Function NetoneRetailDataUSDRecharge(recharge As xRecharge, customSms As String, sqlConn As SqlConnection) As RechargeResponseModel
        Dim client = New NetoneDataUSD(sqlConn, _applicationName, Endpoint("NetoneBundle_Endpoint"), _isTestMode, _referencePrefix, True)
        Return RechargeRetail(client, recharge, customSms, sqlConn)
    End Function

#End Region

#Region "   Data Pins   "
    Public Function QueryEvdTran(rechargeId As Long, context As Context) As BulkEvdResponse
        Return _queryevdtran(rechargeId, context)
    End Function

    Public Function QueryEvdStock(context As Context) As QueryEVDResponse
        Dim response As New QueryEVDResponse(context.AgentReference)

        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Answer, True)
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            response = _queryevdstock(context)
            AddTelecelPinStock(response, sqlConn)
            sqlConn.Close()
        End Using
        SaveWebRequest(context, 200, response.ReplyMsg, xHotType.HotTypes.Answer, False)
        Return response

    End Function


    Public Function BulkEvd(request As BulkEvdRequest, context As Context) As BulkEvdResponse
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, xState.States.Busy)
        Dim response = _bulkevd(request, rech, context)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function

    Public Function RechargeEvd(request As RechargeEvdRequest, context As Context) As BulkEvdResponse
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, xState.States.Busy)
        Dim response = _rechargeevd(request, rech, context)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function

    Public Sub SavePostRechargeRequest(context As Context, response As BulkEvdResponse, stateID As Integer)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = response.ReplyMsg,
            .ReturnCode = response.ReplyCode,
            .ChannelID = xChannel.Channels.Web,
            .StateID = stateID,
            .Amount = response.Amount,
            .Discount = response.Discount,
            .RechargeID = response.RechargeID,
            .WalletBalance = GetWalletBalance(context.AccountId),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, False)
    End Sub

    Private Function _queryevdtran(rechargeId As Long, context As Context) As BulkEvdResponse
        Dim response As New BulkEvdResponse(context.AgentReference)
        'Dim iPinrechargeservice As New Hot.Lib.Services.PinRechargeService(_connString)
        'Dim iTemplateService As New Hot.Lib.Services.TemplateService(_connString)
        'Dim iBrandService As New Hot.Lib.Services.BrandService(_connString)
        'Try
        '    Dim pins As List(Of [Lib].Models.PinRechargeModel) = iPinrechargeservice.Query(context.AccessCode, rechargeId)
        '    response.Pins = (From pin As [Lib].Models.PinRechargeModel In pins
        '                     Select String.Concat(New String() {
        '                        pin.PinNumber, ",",
        '                        pin.PinRef, ",",
        '                        pin.PinValue.ToString("#.00"), ",",
        '                        pin.PinExpiry.ToShortDateString
        '                        })
        '                     ).ToList()
        '    response.ReplyMsg = (From t As Hot.Lib.Entities.Template In iTemplateService.List()
        '                         Where t.TemplateId = Hot.Lib.Entities.Template.Templates.SuccessfulBulkEVDPinRequest
        '                         Select t.TemplateText).FirstOrDefault()
        '    response.ReplyMsg = response.ReplyMsg.Replace("%QUANTITY%", FormatNumber(response.Pins.Count,,,, TriState.True))
        '    response.ReplyMsg = response.ReplyMsg.Replace("%DENOMINATION%", FormatNumber(pins(0).PinValue,, TriState.True))
        '    response.ReplyMsg = response.ReplyMsg.Replace("%BRANDID%", pins(0).BrandId)
        '    response.ReplyMsg = response.ReplyMsg.Replace("%BRAND%", (From b As Hot.Lib.Entities.Brand In iBrandService.List()
        '                                                              Where b.BrandId = pins(0).BrandId
        '                                                              Select b.BrandName).FirstOrDefault())

        '    response.RechargeID = If(response.Pins.Count > 0, pins(0).RechargeId, 0)
        '    response.Amount = pins(0).PinValue * pins.Count
        '    response.WalletBalance = (New Hot.Lib.Services.AccountService(_connString)).Balance(context.AccountId)
        '    ' Old Library
        '    Using sqlConn As New SqlConnection(_connString)
        '        sqlConn.Open()
        '        Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
        '        'Get Discount
        '        response.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, pins(0).BrandId, sqlConn).Discount
        '        sqlConn.Close()
        '    End Using
        '    ' End old library

        '    response.ReplyCode = 2 ' Success 
        'Catch ex As Exception
        '    response.ReplyMsg = ex.Message
        '    response.ReplyCode = -1 ' Failure
        'End Try


        Return response
    End Function

    Private Function _queryevdstock(context As Context) As QueryEVDResponse
        Dim response As New QueryEVDResponse(context.AgentReference)
        Try
            Using sqlconn As New SqlConnection(_connString)
                Dim netone = New NetonePins(sqlconn, _applicationName, Endpoint("NetOne_Endpoint"), _isTestMode, _referencePrefix, True)
                Dim Query = netone.QueryEVDStock()
                response.InStock = Query
            End Using
            response.ReplyMsg = If(response.InStock.Count > 0, "Bulk EVD PIN stock request successful", "We have received your Bulk EVD request but do not have correct stock")
            response.ReplyCode = 2 ' Success
        Catch ex As Exception
            response.ReplyMsg = "We have received your Bulk EVD stock request but failed to query stock."
            response.ReplyCode = 223 ' Failure
        End Try

        Return response
    End Function

    Private Function _rechargeevd(request As RechargeEvdRequest, Recharge As xRecharge, context As Context) As BulkEvdResponse
        Dim response As New BulkEvdResponse(context.AgentReference)

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            Recharge.AccessID = context.AccessId
            Recharge.Amount = request.Quantity * request.Denomination
            Recharge.Denomination = request.Denomination
            Recharge.Quantity = request.Quantity
            Recharge.Mobile = request.TargetNumber
            Recharge.Brand = xBrandAdapter.GetBrand(request.BrandId, sqlConn)


            If Recharge.Mobile.Length <> 10 Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", Recharge.Mobile)
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Validate Amount Range
            If Recharge.Amount < iConfig.MinRecharge Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(iConfig.MinRecharge, 2))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Check Available Sale Value vs Recharge Amount        
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            If iAccount.SaleValue < Recharge.Amount Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", Recharge.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.Balance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(iAccount.SaleValue))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Get Discount
            Recharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, Recharge.Brand.BrandID, sqlConn).Discount


            'Insert recharge
            Recharge.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(Recharge, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge
            Select Case Recharge.Brand.BrandID
                Case xBrand.Brands.EasyCallEVD, xBrand.Brands.NetoneOneFusion, xBrand.Brands.NetoneOneFi
                    Return NetOnePinsEVDRecharge(Recharge, sqlConn)
                Case xBrand.Brands.Juice
                    Return PinsEVDRecharge(Recharge, sqlConn)
                Case Else
                    Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, Recharge.Brand.BrandID & " brand code is not currently supported") ' return RetailRechargePin(recharge, sqlConn)
            End Select
        End Using


        Return response
    End Function

    Private Function _bulkevd(request As BulkEvdRequest, Recharge As xRecharge, context As Context) As BulkEvdResponse
        Dim response As New BulkEvdResponse(context.AgentReference)

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            Recharge.AccessID = context.AccessId
            Recharge.Amount = request.Quantity * request.Denomination
            Recharge.Denomination = request.Denomination
            Recharge.Quantity = request.Quantity
            Recharge.Mobile = "BulkPinSale"
            Recharge.Brand = xBrandAdapter.GetBrand(request.BrandId, sqlConn)

            'Validate Amount Range
            If Recharge.Amount < iConfig.MinRecharge Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(iConfig.MinRecharge, 2))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Check Available Sale Value vs Recharge Amount        
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            If iAccount.SaleValue < Recharge.Amount Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", Recharge.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.Balance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(iAccount.SaleValue))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Get Discount
            Recharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, Recharge.Brand.BrandID, sqlConn).Discount


            'Insert recharge
            Recharge.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(Recharge, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge
            Select Case Recharge.Brand.BrandID
                Case xBrand.Brands.EasyCallEVD, xBrand.Brands.NetoneOneFusion, xBrand.Brands.NetoneOneFi
                    Return NetOnePinsBulkRecharge(Recharge, sqlConn)
                Case Else
                    Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, Recharge.Brand.BrandID & " brand code is not currently supported") ' return RetailRechargePin(recharge, sqlConn)
            End Select
        End Using
        'Dim iPinrechargeservice As New Hot.Lib.Services.PinRechargeService(_connString)
        'Dim iTemplateService As New Hot.Lib.Services.TemplateService(_connString)
        'Dim iBrandService As New Hot.Lib.Services.BrandService(_connString)

        'If Not _isLicensed Then
        '    response.ReplyMsg = (From t As Entities.Template In iTemplateService.List()
        '                         Where t.TemplateId = Entities.Template.Templates.FailedRechargeVASDisabled
        '                         Select t.TemplateText).FirstOrDefault().Replace("Your SMS: %MESSAGE%", "")
        '    response.ReplyCode = Entities.Template.Templates.FailedRechargeVASDisabled
        '    Return response
        'End If
        'Try
        '    Dim pins As List(Of [Lib].Models.PinRechargeModel) = iPinrechargeservice.Sale(context.AccessCode, request.Quantity, request.Denomination, request.BrandId)
        '    response.Pins = (From pin As [Lib].Models.PinRechargeModel In pins
        '                     Select String.Concat(New String() {
        '                        pin.PinNumber, ",",
        '                        pin.PinRef, ",",
        '                        pin.PinValue.ToString("#.00"), ",",
        '                        pin.PinExpiry.ToShortDateString
        '                        })
        '                     ).ToList()
        '    response.ReplyMsg = (From t As Hot.Lib.Entities.Template In iTemplateService.List()
        '                         Where t.TemplateId = Hot.Lib.Entities.Template.Templates.SuccessfulBulkEVDPinRequest
        '                         Select t.TemplateText).FirstOrDefault()
        '    response.ReplyMsg = response.ReplyMsg.Replace("%QUANTITY%", FormatNumber(request.Quantity,,,, TriState.True))
        '    response.ReplyMsg = response.ReplyMsg.Replace("%DENOMINATION%", FormatNumber(request.Denomination,, TriState.True))
        '    response.ReplyMsg = response.ReplyMsg.Replace("%BRANDID%", request.BrandId)
        '    response.ReplyMsg = response.ReplyMsg.Replace("%BRAND%", (From b As Hot.Lib.Entities.Brand In iBrandService.List()
        '                                                              Where b.BrandId = request.BrandId
        '                                                              Select b.BrandName).FirstOrDefault())

        '    response.RechargeID = If(pins.Count > 0, pins(0).RechargeId, 0)
        '    response.Amount = request.Denomination * request.Quantity
        '    response.WalletBalance = (New Hot.Lib.Services.AccountService(_connString)).Balance(context.AccountId)
        '    ' Old Library
        '    Using sqlConn As New SqlConnection(_connString)
        '        sqlConn.Open()
        '        Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
        '        'Get Discount
        '        response.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, request.BrandId, sqlConn).Discount
        '        sqlConn.Close()
        '    End Using
        '    ' End old library

        '    response.ReplyCode = 2 ' Success 
        'Catch ex As Exception
        '    response.ReplyMsg = ex.Message
        '    response.ReplyCode = -1 ' Failure
        'End Try


        Return response
    End Function

    Private Function NetOnePinsBulkRecharge(recharge As xRecharge, sqlConn As SqlConnection) As BulkEvdResponse
        Dim network = New NetonePins(sqlConn, _applicationName, Endpoint("Netone_Endpoint"), _isTestMode, _referencePrefix, True)
        Try
            Dim response As ServiceRechargeResponse = network.RechargePrepaid(recharge)

            If recharge.IsSuccessFul Then
                Console.WriteLine("Recharge Successful. Replying")

                Return New BulkEvdResponse("") With {
                    .Pins = response.RechargePrepaid.Narrative.Split(New String() {"E-Top Up"}, StringSplitOptions.None)(1).Split(";").ToList(),
                    .Amount = recharge.Amount,
                    .Discount = recharge.Discount,
                    .RechargeID = recharge.RechargeID,
                    .ReplyCode = 2,
                    .ReplyMsg = response.RechargePrepaid.Narrative.Split(New String() {"E-Top Up"}, StringSplitOptions.None)(0)
                    }
            Else
                Console.WriteLine("Recharge Failed. Replying")
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)
                ' GET TEXT FROM WEBSERVICE CUSTOMER
                iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", response.RechargePrepaid.Reference & ", " & response.RechargePrepaid.ReturnCode & ": " & response.RechargePrepaid.Narrative)
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
            Throw
        End Try
    End Function

    Private Function NetOnePinsEVDRecharge(recharge As xRecharge, sqlConn As SqlConnection) As BulkEvdResponse
        Dim network = New NetonePins(sqlConn, _applicationName, Endpoint("Netone_Endpoint"), _isTestMode, _referencePrefix, True)
        Try
            Dim response As ServiceRechargeResponse = network.RechargePrepaid(recharge)

            If recharge.IsSuccessFul Then
                Console.WriteLine("Recharge Successful. Replying")

                Dim pin = New Pin(sqlConn, "SMSRechargeService")
                Dim templates = pin.GetCustomerReplies(recharge, Pin.GetPinsFromServiceResponse(recharge, response))
                ReplyCustomer(recharge.Mobile, templates, sqlConn)

                Return New BulkEvdResponse("") With {
                    .Pins = response.RechargePrepaid.Narrative.Split(New String() {"E-Top Up"}, StringSplitOptions.None)(1).Split(";").ToList(),
                    .Amount = recharge.Amount,
                    .Discount = recharge.Discount,
                    .RechargeID = recharge.RechargeID,
                    .ReplyCode = 2,
                    .ReplyMsg = response.RechargePrepaid.Narrative.Split(New String() {"E-Top Up"}, StringSplitOptions.None)(0)
                    }
            Else
                Console.WriteLine("Recharge Failed. Replying")
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)
                ' GET TEXT FROM WEBSERVICE CUSTOMER
                iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", response.RechargePrepaid.Reference & ", " & response.RechargePrepaid.ReturnCode & ": " & response.RechargePrepaid.Narrative)
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
            Throw
        End Try
    End Function


#End Region

#Region "   Zesa   "

    Public Function GetCustomerDetails(context As Context, request As ZesaCustomerInfoRequest) As ZesaCustomerResponseModel
        Dim response As New ZesaCustomerResponseModel With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Answer, True)
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Dim network = New ZESA(sqlConn, _applicationName, Endpoint("ZESA_EndPoint"), _isTestMode, _referencePrefix, True)
            Try
                If Not IsNumeric(request.MeterNumber) Then GoTo InvalidMeterNumber
                Dim recharge = New xRecharge With {.State = New xState With {.StateID = 2},
                .AccessID = context.AccessId,
                .Amount = 0,
                .Brand = New xBrand With {.BrandID = xBrand.Brands.ZETDCFees},
                .Discount = 0,
                .Mobile = request.MeterNumber,
                .RechargeDate = Date.Now}
                xRechargeAdapter.Save(recharge, sqlConn)
                Dim iRechargePrepaid As xRechargePrepaid = New xRechargePrepaid()
                iRechargePrepaid.RechargeID = recharge.RechargeID
                iRechargePrepaid.DebitCredit = recharge.Amount >= 0
                iRechargePrepaid.Reference = recharge.Mobile
                iRechargePrepaid.FinalBalance = -1
                iRechargePrepaid.InitialBalance = -1
                iRechargePrepaid.Narrative = IIf(iRechargePrepaid.DebitCredit, "Pending", "Debit Pending")
                iRechargePrepaid.ReturnCode = -1

                iRechargePrepaid.Reference = iRechargePrepaid.Reference
                xRechargePrepaidAdapter.Save(iRechargePrepaid, sqlConn)

                response.CustomerInfo = network.CustomerInfoRequest(iRechargePrepaid, recharge)

                If recharge.IsSuccessFul Then
                    If String.IsNullOrEmpty(response.CustomerInfo.CustomerName) Then
                        Return New ZesaCustomerResponseModel() With {
                    .ReplyCode = 1,
                    .ReplyMsg = $"Invalid Meter Number - {request.MeterNumber}",
                    .Meter = response.CustomerInfo.MeterNumber,
                    .CustomerInfo = response.CustomerInfo
                    }
                    End If
                    Return New ZesaCustomerResponseModel() With {
                    .ReplyCode = 2,
                    .ReplyMsg = response.CustomerInfo.CustomerName,
                    .Meter = response.CustomerInfo.MeterNumber,
                    .CustomerInfo = response.CustomerInfo,
                    .Currency = response.CustomerInfo.Currency,
                    .AgentReference = context.AgentReference
                    }

                Else
InvalidMeterNumber:
                    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)

                    Select Case response.ReplyCode
                        Case 217
                            ' GET TEXT FROM WEBSERVICE CUSTOMER
                            iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", $"Invalid Meter Number - {request.MeterNumber}")
                        Case Else
                            iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", "")

                    End Select
                    Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)

                End If
            Catch ex As Exception
                LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", 0)
                Throw
            End Try
            sqlConn.Close()
        End Using
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.Answer, False)
        Return response
    End Function

    Friend Function QueryZesa(req As QueryZesaRequest, context As Context) As RechargeZesaResponseModel
        Dim response = New RechargeZesaResponseModel()
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(req.RechargeId, sqlConn)
            If rech2 Is Nothing Then Throw New HotRequestException(xTemplate.Templates.FailedWebException, "RechargeId Not Found")
            If Not (rech2.Brand.BrandID = xBrand.Brands.ZETDC Or rech2.Brand.BrandID = xBrand.Brands.ZETDCUSD) Then Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, "RechargeId Not ZESA Transaction")

            Dim rp = xRechargeAdapter.SelectRechargePrepaid(req.RechargeId, sqlConn)
            response = HandleZesaResponse(rech2, sqlConn, "0772000000", New RechargeZesaRequest(), New xAccount(), New ServiceRechargeResponse(rp), False)

        End Using
        Return response
    End Function

    Public Function RechargeZesa(request As RechargeZesaRequest, context As Context, Optional IsUsd As Boolean = False) As RechargeZesaResponseModel
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, request, xState.States.Busy)
        Dim response = _recharge(request, rech, context, IsUsd)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function

    Private Sub SavePreRechargeRequest(context As Context, rechargeRequest As RechargeZesaRequest, state As xState.States)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = Nothing,
            .ReturnCode = state,
            .ChannelID = xChannel.Channels.Web,
            .StateID = state,
            .Amount = rechargeRequest.Amount,
            .WalletBalance = GetWalletBalance(context.AccountId),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, True)
    End Sub

    Private Function _recharge(request As RechargeZesaRequest, ByRef recharge As xRecharge, context As Context, Optional IsUsd As Boolean = False) As RechargeZesaResponseModel
        Dim response As New RechargeZesaResponseModel With {.AgentReference = context.AgentReference}

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            recharge.AccessID = context.AccessId
            recharge.Amount = request.Amount
            recharge.Mobile = request.MeterNumber
            recharge.ProductCode = request.TargetNumber
            recharge.Brand = If(IsUsd, xBrandAdapter.GetBrand(xBrand.Brands.ZETDCUSD, sqlConn), xBrandAdapter.GetBrand(xBrand.Brands.ZETDC, sqlConn))


            If recharge.ProductCode.Length <> 10 Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Validate Amount Range
            If (Not IsUsd And (recharge.Amount < 70 Or recharge.Amount > 18500)) Or (IsUsd And (recharge.Amount < 1 Or recharge.Amount > 10000)) Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(70, 2))
                Throw New HotRequestException(iReply.TemplateID, "Your recharge request was out of the acceptable range. Minimum Recharge 70, Max Recharge 18,500.00 try in the correct range. HOT Recharge\r\n")
            End If

            'Check Available Sale Value vs Recharge Amount        
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)

            Try
                'Get Discount
                recharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, recharge.Brand.BrandID, sqlConn).Discount

            Catch ex As Exception
                Throw New HotRequestException(xTemplate.Templates.FailedRechargeVASDisabled, "Your account has not been setup to sell this brand of product.")
            End Try

            Dim Balance As Decimal = Nothing
            Dim Salevalue As Decimal = Nothing
            Dim HasSufficientFunds As Boolean = Nothing
            CalculateRechargeCost(recharge, iAccount, Balance, Salevalue, HasSufficientFunds)

            If Not HasSufficientFunds Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(Balance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(Salevalue))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Insert recharge
            recharge.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(recharge, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge
            Select Case recharge.Brand.BrandID
                Case xBrand.Brands.ZETDC, xBrand.Brands.ZETDCUSD
                    Return ZESATokenRecharge(recharge, sqlConn, recharge.ProductCode, request, iAccount)
                Case Else
                    Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, recharge.Brand.BrandID & " brand code is not currently supported") ' return RetailRechargePin(recharge, sqlConn)
            End Select
        End Using


        Return response

    End Function

    Private Function ZESATokenRecharge(recharge As xRecharge, sqlConn As SqlConnection, Mobile As String, request As RechargeZesaRequest, iAccount As xAccount) As RechargeZesaResponseModel
        Dim network = New ZESA(sqlConn, _applicationName, Endpoint("ZESA_EndPoint"), _isTestMode, _referencePrefix, True)
        Try
            Dim response As ServiceRechargeResponse = network.RechargePrepaid(recharge)

            Return HandleZesaResponse(recharge, sqlConn, Mobile, request, iAccount, response)
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
            Throw
        End Try
    End Function

    Private Function HandleZesaResponse(recharge As xRecharge, sqlConn As SqlConnection, Mobile As String, request As RechargeZesaRequest, iAccount As xAccount, response As ServiceRechargeResponse, Optional SendResponses As Boolean = True) As RechargeZesaResponseModel
        If recharge.IsSuccessFul Then
            Console.WriteLine("Recharge Successful. Replying")
            Dim result As ZesaAPI.PurchaseTokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(response.RechargePrepaid.Narrative, GetType(ZesaAPI.PurchaseTokenResponse))

            Dim replylist As List(Of xTemplate) = ZESACustomerReplies(sqlConn, result, recharge.Brand.BrandID)

            If Not String.IsNullOrEmpty(request.CustomerSMS) Then
                Dim CustomerAPIResponse As New xTemplate With {.TemplateText = request.CustomerSMS}
                CustomerAPIResponse.TemplateText = CustomerAPIResponse.TemplateText.Replace("%AMOUNT%", request.Amount.ToString("#,##0.00"))
                CustomerAPIResponse.TemplateText = CustomerAPIResponse.TemplateText.Replace("%COMPANYNAME%", iAccount.AccountName)
                CustomerAPIResponse.TemplateText = CustomerAPIResponse.TemplateText.Replace("%KWH%", result.PurchaseToken.Tokens(0).Units.ToString("#,##0.0"))
                CustomerAPIResponse.TemplateText = CustomerAPIResponse.TemplateText.Replace("%ACOUNTNAME%", result.CustomerInfo.CustomerName)
                CustomerAPIResponse.TemplateText = CustomerAPIResponse.TemplateText.Replace("%METERNUMBER%", request.MeterNumber)

                replylist.Add(CustomerAPIResponse)
            End If

            If SendResponses Then ReplyCustomer(Mobile, replylist, sqlConn)

            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulZESATokenPurchase_Dealer, sqlConn)
            iReply.TemplateText = iReply.TemplateText.Replace("%METER%", result.PurchaseToken.MeterNumber)
            iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", result.PurchaseToken.Amount.ToString("#,###.##"))
            iReply.TemplateText = iReply.TemplateText.Replace("%DISCOUNT%", recharge.Discount)

            Return New RechargeZesaResponseModel() With {
            .Amount = recharge.Amount,
            .ProviderFees = 7,
            .Discount = recharge.Discount,
            .RechargeID = recharge.RechargeID,
            .ReplyCode = 2,
            .ReplyMsg = iReply.TemplateText,
            .Meter = result.PurchaseToken.MeterNumber,
            .Tokens = result.PurchaseToken.Tokens,
            .CustomerInfo = result.CustomerInfo,
            .AccountName = result.CustomerInfo.CustomerName,
            .Address = result.CustomerInfo.Address
            }
        ElseIf recharge.State.StateID = xState.States.PendingVerification Then
            Console.WriteLine("Recharge Pending. Replying")
            Dim result As ZesaAPI.PurchaseTokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(response.RechargePrepaid.Narrative, GetType(ZesaAPI.PurchaseTokenResponse))

            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.PendingZESATokenPurchase_API, sqlConn)
            iReply.TemplateText = iReply.TemplateText.Replace("%METER%", result.PurchaseToken.MeterNumber)
            iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", result.PurchaseToken.Amount.ToString("#,##0.00"))

            Return New RechargeZesaResponseModel() With {
            .Amount = recharge.Amount,
            .Discount = recharge.Discount,
            .RechargeID = recharge.RechargeID,
            .ReplyCode = 4,
            .ReplyMsg = iReply.TemplateText,
            .Meter = result.PurchaseToken.MeterNumber,
            .Tokens = result.PurchaseToken.Tokens,
            .CustomerInfo = result.CustomerInfo,
            .AccountName = result.CustomerInfo.CustomerName,
            .Address = result.CustomerInfo.Address
            }


        Else

            Console.WriteLine("Recharge Failed. Replying")
            Dim result As ZesaAPI.PurchaseTokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(response.RechargePrepaid.Narrative, GetType(ZesaAPI.PurchaseTokenResponse))
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)

            Select Case result.ReplyCode
                Case 209
                    iReply.TemplateText = $"Meter Account Currency - {result.ReplyMessage}"
                Case 217
                    iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", $"Invalid Meter Number - {result.PurchaseToken.MeterNumber}")
                Case 222
                    iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", $"Invalid Purchase Amount - {result.PurchaseToken.Amount.ToString("#,##0.00")}")
                Case 704
                    iReply.TemplateText = $"{result.ReplyMessage}"

                Case Else
                    iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", $"{result.ReplyMessage} ")

            End Select
            Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)

        End If
    End Function

    Private Shared Function ZESACustomerReplies(sqlConn As SqlConnection, result As ZesaAPI.PurchaseTokenResponse, BrandId As Integer) As List(Of xTemplate)
        Dim replylist As New List(Of xTemplate)
        If result.PurchaseToken.Tokens.Count = 0 Then
            result.PurchaseToken.Tokens.Add(New ZesaAPI.TokenItem() With {.Token = "Arrears Payment", .Arrears = 0, .Levy = 0, .TaxAmount = 0, .NetAmount = result.PurchaseToken.Amount, .Units = 0})
        End If

        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulZESAStandardTemplate, sqlConn)
        Dim tokens As String = ""
        For Each token As ZesaAPI.TokenItem In result.PurchaseToken.Tokens
            tokens = tokens + If(String.IsNullOrEmpty(tokens), "", vbNewLine) + $"Token: {token.Token}"
        Next
        Dim item = result.PurchaseToken.Tokens.First()
        iReply.TemplateText = iReply.TemplateText.Replace("%METER%", result.PurchaseToken.MeterNumber)
        iReply.TemplateText = iReply.TemplateText.Replace("%TOKEN%", tokens)
        iReply.TemplateText = iReply.TemplateText.Replace("%UNITS%", item.Units.ToString("#,##0.00"))
        iReply.TemplateText = iReply.TemplateText.Replace("%NETAMOUNT%", item.NetAmount.ToString("#,##0.00"))
        iReply.TemplateText = iReply.TemplateText.Replace("%DEBT%", item.Arrears.ToString("#,##0.00"))
        iReply.TemplateText = iReply.TemplateText.Replace("%LEVY%", item.Levy.ToString("#,##0.00"))
        iReply.TemplateText = iReply.TemplateText.Replace("%TAX%", item.TaxAmount.ToString("#,##0.00"))
        iReply.TemplateText = iReply.TemplateText.Replace("%TOTALAMOUNT%", (item.NetAmount + item.Arrears + item.Levy + item.TaxAmount).ToString("#,##0.00"))
        iReply.TemplateText = iReply.TemplateText.Replace("%TOTALAMOUNTPAID%", (result.PurchaseToken.Amount).ToString("#,##0.00"))
        iReply.TemplateText = iReply.TemplateText.Replace("%DATE%", Date.Now.ToString("dd/MM/yy HH:mm"))
        iReply.TemplateText = iReply.TemplateText.Replace("%CURRENCY%", result.CustomerInfo.Currency)
        iReply.TemplateText = iReply.TemplateText.Replace("%CURRENCYPAID%", If(BrandId = xBrand.Brands.ZETDCUSD, "USD", result.CustomerInfo.Currency))
        If result.PurchaseToken.Tokens.Count > 1 Then
            iReply.TemplateText = iReply.TemplateText + vbNewLine + "Please input tokens in the given order"
        End If
        replylist.Add(iReply)


        Dim iReply2 As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulZESATokenPurchase_Customer, sqlConn)
        iReply2.TemplateText = iReply2.TemplateText.Replace("%METER%", result.PurchaseToken.MeterNumber)
        iReply2.TemplateText = iReply2.TemplateText.Replace("%AMOUNT%", result.PurchaseToken.Amount.ToString("#,###.##"))
        iReply2.TemplateText = iReply2.TemplateText.Replace("%NAME%", result.CustomerInfo.CustomerName)
        replylist.Add(iReply2)

        Return replylist
    End Function



    Public Function CompleteRechargeZesa(request As CompleteZesaRequest, context As Context) As RechargeZesaResponseModel
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, request, xState.States.Busy)
        Dim response = _completezesarecharge(request, rech, context)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function


    Private Sub SavePreRechargeRequest(context As Context, rechargeRequest As CompleteZesaRequest, state As xState.States)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = Nothing,
            .ReturnCode = state,
            .ChannelID = xChannel.Channels.Web,
            .StateID = state,
            .Amount = 0,
            .WalletBalance = GetWalletBalance(context.AccountId, 1),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, True)
    End Sub

    Private Sub SavePostRechargeRequest(context As Context, rechargeResponse As RechargeZesaResponseModel, state As xState.States)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = rechargeResponse.ReplyMsg,
            .ReturnCode = rechargeResponse.ReplyCode,
            .ChannelID = xChannel.Channels.Web,
            .StateID = state,
            .Amount = rechargeResponse.Amount,
            .Discount = rechargeResponse.Discount,
            .RechargeID = rechargeResponse.RechargeID,
            .WalletBalance = GetWalletBalance(context.AccountId, 1),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, False)
    End Sub


    Private Function _completezesarecharge(request As CompleteZesaRequest, ByRef recharge As xRecharge, context As Context) As RechargeZesaResponseModel
        Dim response As New RechargeZesaResponseModel With {.AgentReference = context.AgentReference}

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            recharge = xRechargeAdapter.SelectRecharge(request.rechargeId, sqlConn)
            If recharge Is Nothing Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebException, sqlConn)
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If


            'Determine whether to dispense pins or do direct prepaid recharge
            Select Case recharge.Brand.BrandID
                Case xBrand.Brands.ZETDC
                    Return UpdateTokenRecharge(recharge, MakePurchaseTokenItem(request.purchaseToken), sqlConn)
                Case Else
                    Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, recharge.Brand.BrandID & " brand code is not currently supported") ' return RetailRechargePin(recharge, sqlConn)
            End Select
        End Using


        Return response
        'Using sqlConn As New SqlConnection(_connString)
        '    sqlConn.Open()
        '    Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
        '    Return New RechargeZesaResponseModel With {
        '        .AccountName = "Test Account",
        '        .Address = "Test Address Data",
        '        .Fees = 0.1,
        '        .Amount = request.Amount,
        '        .Levy = FormatNumber((request.Amount * 0.06), 2),
        '        .Meter = request.MeterNumber,
        '        .Units = FormatNumber(((request.Amount + 0.00001) / 3.99), 2),
        '        .ReplyCode = 2,
        '        .ReplyMsg = $"Test Implementation - Successful ZESA top up of {FormatNumber(request.Amount, 2)}. Token:{ .Token} for Meter Number:{request.MeterNumber} of { .Units} Units",
        '        .WalletBalance = iAccount.Balance
        '    }
        'End Using


    End Function

    Private Function MakePurchaseTokenItem(purchaseToken As Purchasetoken) As ZesaAPI.PurchaseToken
        Dim result = New ZesaAPI.PurchaseToken() With {
            .Amount = purchaseToken.amount,
            .MeterNumber = purchaseToken.meterNumber,
            .Narrative = purchaseToken.narrative,
            .RawResponse = purchaseToken.rawResponse,
            .Reference = purchaseToken.reference,
            .ResponseCode = purchaseToken.responseCode,
            .VendorReference = purchaseToken.vendorReference,
            .Tokens = New List(Of ZesaAPI.TokenItem)(),
            .[Date] = New ZesaAPI.DateTimeOffset()
        }
        For Each i In purchaseToken.tokens
            result.Tokens.Add(New ZesaAPI.TokenItem() With {
            .Arrears = i.arrears,
            .Levy = i.levy,
            .Token = i.token,
            .NetAmount = i.netAmount,
            .TaxAmount = i.taxAmount,
            .Units = i.units,
            .ZesaReference = i.zesaReference
            })
        Next

        Return result
    End Function

    Private Function UpdateTokenRecharge(recharge As xRecharge, token As ZesaAPI.PurchaseToken, sqlConn As SqlConnection) As RechargeZesaResponseModel
        Dim network = New ZESA(sqlConn, _applicationName, Endpoint("ZESA_EndPoint"), _isTestMode, _referencePrefix, True)
        Try
            Dim response As ServiceRechargeResponse = network.CompleteZesa(recharge, token)

            If recharge.IsSuccessFul Then
                Console.WriteLine("Recharge Successful. Replying")
                Dim result As ZesaAPI.PurchaseTokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(response.RechargePrepaid.Narrative, GetType(ZesaAPI.PurchaseTokenResponse))

                Dim replylist As List(Of xTemplate) = ZESACustomerReplies(sqlConn, result, recharge.Brand.BrandID)
                ReplyCustomer(response.RechargePrepaid.Reference, replylist, sqlConn)

                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulZESATokenPurchase_Dealer, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%METER%", result.PurchaseToken.MeterNumber)
                iReply.TemplateText = iReply.TemplateText.Replace("%AMOUNT%", result.PurchaseToken.Amount.ToString("#,###.##"))
                iReply.TemplateText = iReply.TemplateText.Replace("%DISCOUNT%", recharge.Discount)

                Return New RechargeZesaResponseModel() With {
                    .Amount = recharge.Amount,
                    .Discount = recharge.Discount,
                    .RechargeID = recharge.RechargeID,
                    .ReplyCode = 2,
                    .ReplyMsg = iReply.TemplateText,
                    .Meter = result.PurchaseToken.MeterNumber,
                    .Tokens = result.PurchaseToken.Tokens,
                    .CustomerInfo = result.CustomerInfo
                    }
            Else
                Console.WriteLine("Recharge Failed. Replying")
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)
                ' GET TEXT FROM WEBSERVICE CUSTOMER
                iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", response.RechargePrepaid.Reference & ", " & response.RechargePrepaid.ReturnCode & ": " & response.RechargePrepaid.Narrative)
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
            Throw
        End Try
    End Function

    Public Function WalletBalanceZESA(context As Context) As WalletBalanceResponse
        Dim response As New WalletBalanceResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Balance, True)
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulBalance, sqlConn)

            iReply.TemplateText = "Your HOT Balance is $ %BALANCE%. You can sell approximately $ %SALEVALUE%."
            iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.ZESABalance))
            iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(iAccount.ZESABalance))
            iReply.TemplateText = iReply.TemplateText.Replace("HOT Balance", "HOT ZESA Balance")

            response.ReplyCode = 2
            response.ReplyMsg = iReply.TemplateText
            response.WalletBalance = iAccount.ZESABalance
            sqlConn.Close()
        End Using
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.Balance, False)
        Return response
    End Function
    Public Function WalletBalanceZESAUSD(context As Context) As WalletBalanceResponse
        Dim response As New WalletBalanceResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Balance, True)
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulBalance, sqlConn)

            iReply.TemplateText = "Your HOT Balance is $ %BALANCE%. You can sell approximately $ %SALEVALUE%."
            iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.USDUtilityBalance))
            iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(iAccount.USDUtilityBalance))
            iReply.TemplateText = iReply.TemplateText.Replace("HOT Balance", "HOT ZESA Balance")

            response.ReplyCode = 2
            response.ReplyMsg = iReply.TemplateText
            response.WalletBalance = iAccount.USDUtilityBalance
            sqlConn.Close()
        End Using
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.Balance, False)
        Return response
    End Function

#End Region

#Region "   Telone   "

    Public Function VerifyAdslAccount(req As VerifyAccountADSLRequest, context As Context) As VerifyAccountADSLResponse
        Dim response As New VerifyAccountADSLResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Answer, True)
        response = _verifyAdslAccount(req, response)
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.Answer, False)
        Return response
    End Function

    Private Function _verifyAdslAccount(req As VerifyAccountADSLRequest, response As VerifyAccountADSLResponse) As VerifyAccountADSLResponse
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Dim network = New Telone(sqlConn, _applicationName, Endpoint("TelOne_EndPoint"), _isTestMode, _referencePrefix, True)
            Try

                Dim result = network.VerifyAccount(req.AccountNumber)

                If result.ResponseCode = "00" Then
                    response.AccountNumber = req.AccountNumber
                    response.AccountName = result.AccountName
                    response.AccountAddress = result.AccountAddress
                    response.ReplyMsg = result.ResponseDescription
                    response.ReplyCode = 2
                Else
                    Dim iReply As xTemplate = New xTemplate() With {.TemplateID = xTemplate.Templates.FailedVAS}

                    iReply.TemplateText = $"Invalid Account Number - {req.AccountNumber}"

                    Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)

                End If
            Catch ex As Exception
                LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", 0)
                Throw
            End Try
            sqlConn.Close()
        End Using

        Return response
    End Function


    Public Function RechargeAdsl(req As RechargeADSLRequest, context As Context, Optional isUsd As Boolean = False) As BulkADSLResponse
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, req, xState.States.Busy)
        Dim response = _rechargeadsl(req, rech, context, isUsd)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function

    Private Function _rechargeadsl(request As RechargeADSLRequest, Recharge As xRecharge, context As Context, Optional isUsd As Boolean = False) As BulkADSLResponse
        Dim response As New BulkADSLResponse(context.AgentReference)

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            Dim service = New Telone(sqlConn, _applicationName, Endpoint("TelOne_Endpoint"), _isTestMode, _referencePrefix, True)
            Dim products = service.QueryEVDStock(isUsd)

            If Not products.Any(Function(p) p.ProductId = request.ProductId) Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedUnsupportedBrand, sqlConn)
                ErrorResponse(iReply.TemplateID, iReply.TemplateText)
            End If

            Dim product = products.Where(Function(p) p.ProductId = request.ProductId).FirstOrDefault()

            Recharge.AccessID = context.AccessId
            Recharge.Amount = product.Price
            Recharge.Denomination = request.ProductId
            Recharge.Mobile = request.AccountNumber
            Dim brandid = GetTeloneBrand(request.ProductId, isUsd)
            Recharge.Brand = xBrandAdapter.GetBrand(brandid, sqlConn)


            'If Recharge.Mobile.Length <> 10 Then
            '    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
            '    iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", Recharge.Mobile)
            '    Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            'End If

            'Validate Amount Range
            'If Recharge.Amount < iConfig.MinRecharge Then
            '    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
            '    iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(iConfig.MinRecharge, 2))
            '    Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            'End If


            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            'Get Discount
            Recharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, Recharge.Brand.BrandID, sqlConn).Discount

            Dim Balance As Decimal = Nothing
            Dim Salevalue As Decimal = Nothing
            Dim HasSufficientFunds As Boolean = Nothing
            CalculateRechargeCost(Recharge, iAccount, Balance, Salevalue, HasSufficientFunds)

            If Not HasSufficientFunds Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", Recharge.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(Balance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(Salevalue))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If


            'Insert recharge
            Recharge.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(Recharge, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge
            Select Case Recharge.Brand.BrandID
                Case xBrand.Brands.TeloneVoice, xBrand.Brands.TeloneLTE, xBrand.Brands.TeloneBroadband
                    Return TelOneAdslRecharge(Recharge, sqlConn, request.TargetNumber)
                Case xBrand.Brands.TeloneUSD
                    Return TelOneAdslRechargeUSD(Recharge, sqlConn, request.TargetNumber)
                Case Else
                    Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, Recharge.Brand.BrandID & " brand code is not currently supported") ' return RetailRechargePin(recharge, sqlConn)
            End Select
        End Using


        Return response
    End Function


    Private Function TelOneAdslRechargeUSD(recharge As xRecharge, sqlConn As SqlConnection, Optional mobile As String = "") As BulkADSLResponse
        Dim network = New Telone(sqlConn, _applicationName, Endpoint("Telone_Endpoint"), _isTestMode, _referencePrefix, True)
        Try
            Dim response As ServiceRechargeResponse = network.RechargePrepaid(recharge)

            Return ReplyTelone(recharge, sqlConn, mobile, response)
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
            Throw
        End Try
    End Function
    Private Function TelOneAdslRecharge(recharge As xRecharge, sqlConn As SqlConnection, Optional mobile As String = "") As BulkADSLResponse
        Dim network = New Telone(sqlConn, _applicationName, Endpoint("Telone_Endpoint"), _isTestMode, _referencePrefix, True)
        Try
            Dim response As ServiceRechargeResponse = network.RechargePrepaid(recharge)

            Return ReplyTelone(recharge, sqlConn, mobile, response)
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
            Throw
        End Try
    End Function

    Private Function ReplyTelone(recharge As xRecharge, sqlConn As SqlConnection, mobile As String, response As ServiceRechargeResponse) As BulkADSLResponse
        If recharge.IsSuccessFul Then
            Console.WriteLine("Recharge Successful. Replying")
            Dim rechargeresponse = Newtonsoft.Json.JsonConvert.DeserializeObject(Of TelOneAPI.RechargeAdslAccountResponse)(response.RechargePrepaid.Narrative.Split(New String() {"Raw:"}, StringSplitOptions.None)(1))


            Dim pins = rechargeresponse.Voucher.ToList()


            Dim replylist As New List(Of xTemplate)
            For Each item In pins
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRechargePin_Customer, sqlConn)

                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%PIN%", item.Pin)
                iReply.TemplateText = iReply.TemplateText.Replace("%REF%", item.CardNumber)
                iReply.TemplateText = iReply.TemplateText.Replace("%PINVALUE%", recharge.Amount.ToString("#,##0.00"))

                replylist.Add(iReply)
            Next
            If Not String.IsNullOrWhiteSpace(mobile) Then ReplyCustomer(mobile, replylist, sqlConn)

            Return New BulkADSLResponse("") With {
                .Vouchers = pins,
                .Amount = recharge.Amount,
                .Discount = recharge.Discount,
                .RechargeID = recharge.RechargeID,
                .ReplyCode = 2,
                .ReplyMsg = response.RechargePrepaid.Narrative.Split(New String() {"Raw:"}, StringSplitOptions.None)(0)
                }
        Else
            Console.WriteLine("Recharge Failed. Replying")
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)
            ' GET TEXT FROM WEBSERVICE CUSTOMER
            iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", response.RechargePrepaid.Reference & ", " & response.RechargePrepaid.ReturnCode & ": " & response.RechargePrepaid.Narrative)
            Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
        End If
    End Function

    Private Function GetTeloneBrand(productId As Integer, Optional IsUsd As Boolean = False) As Integer
        If IsUsd Then Return 43
        If productId.ToString.StartsWith("2") Then Return 34
        If productId.ToString.StartsWith("6") Then Return 35
        Return 33
    End Function

    Private Sub SavePreRechargeRequest(context As Context, rechargeRequest As RechargeADSLRequest, state As xState.States)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = Nothing,
            .ReturnCode = state,
            .ChannelID = xChannel.Channels.Web,
            .StateID = state,
            .Amount = 0,
            .WalletBalance = GetWalletBalance(context.AccountId),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, True)
    End Sub

    Private Sub SavePostRechargeRequest(context As Context, response As BulkADSLResponse, stateID As Integer)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = response.ReplyMsg,
            .ReturnCode = response.ReplyCode,
            .ChannelID = xChannel.Channels.Web,
            .StateID = stateID,
            .Amount = response.Amount,
            .Discount = response.Discount,
            .RechargeID = response.RechargeID,
            .WalletBalance = GetWalletBalance(context.AccountId),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, False)
    End Sub


    Public Function BulkAdsl(req As BulkADSLRequest, context As Context) As BulkADSLResponse
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, req, xState.States.Busy)
        Dim response = _bulkadsl(req, rech, context)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function

    Private Function _bulkadsl(request As BulkADSLRequest, recharge As xRecharge, context As Context) As Object
        Dim response As New BulkADSLResponse(context.AgentReference)

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            Dim service = New Telone(sqlConn, _applicationName, Endpoint("TelOne_Endpoint"), _isTestMode, _referencePrefix, True)
            Dim products = service.QueryEVDStock()

            If Not products.Any(Function(p) p.ProductId = request.ProductId) Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedUnsupportedBrand, sqlConn)
                ErrorResponse(iReply.TemplateID, iReply.TemplateText)
            End If

            Dim product = products.Where(Function(p) p.ProductId = request.ProductId).FirstOrDefault()

            recharge.AccessID = context.AccessId
            recharge.Amount = product.Price * request.Quantity
            recharge.Denomination = request.ProductId
            recharge.Quantity = request.Quantity
            recharge.Mobile = "BulkSale"
            recharge.Brand = xBrandAdapter.GetBrand(GetTeloneBrand(request.ProductId), sqlConn)


            'If Recharge.Mobile.Length <> 10 Then
            '    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
            '    iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", Recharge.Mobile)
            '    Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            'End If

            'Validate Amount Range
            'If Recharge.Amount < iConfig.MinRecharge Then
            '    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
            '    iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(iConfig.MinRecharge, 2))
            '    Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            'End If

            'Check Available Sale Value vs Recharge Amount        
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            If iAccount.SaleValue < recharge.Amount Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.Balance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(iAccount.SaleValue))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Get Discount
            recharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, recharge.Brand.BrandID, sqlConn).Discount


            'Insert recharge
            recharge.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(recharge, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge
            Select Case recharge.Brand.BrandID
                Case xBrand.Brands.TeloneVoice, xBrand.Brands.TeloneLTE, xBrand.Brands.TeloneBroadband
                    Return TelOneBulkAdslRecharge(recharge, sqlConn)
                Case Else
                    Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, recharge.Brand.BrandID & " brand code is not currently supported") ' return RetailRechargePin(recharge, sqlConn)
            End Select
        End Using


        Return response
    End Function

    Private Function TelOneBulkAdslRecharge(recharge As xRecharge, sqlConn As SqlConnection) As Object
        Dim network = New Telone(sqlConn, _applicationName, Endpoint("Telone_Endpoint"), _isTestMode, _referencePrefix, True)
        Try
            Dim response As ServiceRechargeResponse = network.BulkRechargePrepaid(recharge)

            If recharge.IsSuccessFul Then
                Console.WriteLine("Recharge Successful. Replying")
                Dim rechargeresponse = Newtonsoft.Json.JsonConvert.DeserializeObject(Of TelOneAPI.PurchaseBroadbandProductsResponse)(
                    response.RechargePrepaid.Narrative.Split(New String() {"Raw:"}, StringSplitOptions.None)(1))

                Dim pins = New List(Of TelOneAPI.Voucher)
                pins.AddRange(rechargeresponse.Vouchers.ToList())
                Return New BulkADSLResponse("") With {
                    .Vouchers = pins,
                    .Amount = recharge.Amount,
                    .Discount = recharge.Discount,
                    .RechargeID = recharge.RechargeID,
                    .ReplyCode = 2,
                    .ReplyMsg = response.RechargePrepaid.Narrative.Split(New String() {"Raw:"}, StringSplitOptions.None)(0)
                    }
            Else
                Console.WriteLine("Recharge Failed. Replying")
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)
                ' GET TEXT FROM WEBSERVICE CUSTOMER
                iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", response.RechargePrepaid.Reference & ", " & response.RechargePrepaid.ReturnCode & ": " & response.RechargePrepaid.Narrative)
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
            Throw
        End Try
    End Function

    Private Sub SavePreRechargeRequest(context As Context, rechargeRequest As BulkADSLRequest, state As xState.States)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = Nothing,
            .ReturnCode = state,
            .ChannelID = xChannel.Channels.Web,
            .StateID = state,
            .Amount = 0,
            .WalletBalance = GetWalletBalance(context.AccountId),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, True)
    End Sub


    Public Function QueryAdslStock(context As Context, Optional IsUSD As Boolean = False) As QueryADSLResponse
        Dim response As New QueryADSLResponse(context.AgentReference)

        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Answer, True)
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            response = _queryadslstock(context, IsUSD)
        End Using
        SaveWebRequest(context, 200, response.ReplyMsg, xHotType.HotTypes.Answer, False)
        Return response
    End Function

    Private Function _queryadslstock(context As Context, Optional IsUSD As Boolean = False) As QueryADSLResponse
        Dim response As New QueryADSLResponse(context.AgentReference)
        Dim TeloneStockErrorMessage = "We have received your Telone Pin request but do not have correct stock"
        Try
            Using sqlconn As New SqlConnection(_connString)
                Dim service = New Telone(sqlconn, _applicationName, Endpoint("TelOne_Endpoint"), _isTestMode, _referencePrefix, True)
                Dim Query = service.QueryEVDStock(IsUSD)
                response.Products = Query
            End Using

            response.ReplyMsg = If(response.Products.Count > 0, "Telone PIN stock request successful", TeloneStockErrorMessage)
            response.ReplyCode = 2 ' Success
        Catch ex As Exception
            response.ReplyMsg = TeloneStockErrorMessage
            response.ReplyCode = 223 ' Failure
        End Try

        Return response
    End Function


    Public Function GetADSLBalance(req As EndUserAdslBalanceRequest, context As Context) As EndUserAdslBalanceResponse
        Dim response As New EndUserAdslBalanceResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Answer, True)
        response = _getADSLBalance(req, response)
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.Answer, False)
        Return response
    End Function

    Private Function _getADSLBalance(req As EndUserAdslBalanceRequest, response As EndUserAdslBalanceResponse) As EndUserAdslBalanceResponse
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Dim network = New Telone(sqlConn, _applicationName, Endpoint("TelOne_EndPoint"), _isTestMode, _referencePrefix, True)
            Try

                Dim result = network.GetEndUserBalance(req.AccountNumber)

                If result <> -1 Then
                    response.AccountNumber = req.AccountNumber
                    response.Balance = result
                    response.ReplyMsg = $"Account Number {req.AccountNumber} has a balance of {result:#,##0.00}"
                    response.ReplyCode = 2
                Else
                    response.AccountNumber = req.AccountNumber
                    response.Balance = 0
                    response.ReplyMsg = $"Unable to get the balance for Account Number {req.AccountNumber}"
                    response.ReplyCode = 1
                End If
                Return response
            Catch ex As Exception
                LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", 0)
                Throw
            End Try
            sqlConn.Close()
        End Using
    End Function


    Public Function PayTeloneBill(req As PayTeloneBillRequest, context As Context) As PayTeloneBillResponse
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, req, xState.States.Busy)
        Dim response As PayTeloneBillResponse = _payTeloneBill(req, rech, context)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function

    Private Function _payTeloneBill(req As PayTeloneBillRequest, rech As xRecharge, context As Context) As PayTeloneBillResponse

        Dim response As New PayTeloneBillResponse(context.AgentReference)

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)

            rech.AccessID = context.AccessId
            rech.Amount = req.Amount
            rech.Brand = xBrandAdapter.GetBrand(xBrand.Brands.TeloneBillPayment, sqlConn)


            'If Recharge.Mobile.Length <> 10 Then
            '    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
            '    iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", Recharge.Mobile)
            '    Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            'End If

            'Validate Amount Range
            If rech.Amount < iConfig.MinRecharge Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(iConfig.MinRecharge, 2))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Check Available Sale Value vs Recharge Amount        
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            If iAccount.SaleValue < rech.Amount Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", rech.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.Balance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(iAccount.SaleValue))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Get Discount
            rech.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, rech.Brand.BrandID, sqlConn).Discount


            'Insert recharge
            rech.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(rech, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge 
            Return TelOneBillPayment(rech, sqlConn, req.TargetNumber)

        End Using


        Return response

    End Function

    Private Function TelOneBillPayment(rech As xRecharge, sqlConn As SqlConnection, targetNumber As String) As PayTeloneBillResponse
        Dim network = New Telone(sqlConn, _applicationName, Endpoint("Telone_Endpoint"), _isTestMode, _referencePrefix, True)
        Try
            Dim response As ServiceRechargeResponse = network.PayAccountBill(rech)

            If rech.IsSuccessFul Then
                Console.WriteLine("Recharge Successful. Replying")
                Dim rechargeresponse = Newtonsoft.Json.JsonConvert.DeserializeObject(Of TelOneAPI.PayBillResponse)(response.RechargePrepaid.Narrative.Split(New String() {"Raw:"}, StringSplitOptions.None)(1))

                Dim replylist As New List(Of xTemplate)

                If Not String.IsNullOrWhiteSpace(targetNumber) Then ReplyCustomer(targetNumber, replylist, sqlConn)

                Return New PayTeloneBillResponse("") With {
                    .Amount = rech.Amount,
                    .Discount = rech.Discount,
                    .RechargeID = rech.RechargeID,
                    .ReplyCode = 2,
                    .ReplyMsg = response.RechargePrepaid.Narrative.Split(New String() {"Raw:"}, StringSplitOptions.None)(0)
                    }
            Else
                Console.WriteLine("Recharge Failed. Replying")
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)
                ' GET TEXT FROM WEBSERVICE CUSTOMER
                iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", response.RechargePrepaid.Reference & ", " & response.RechargePrepaid.ReturnCode & ": " & response.RechargePrepaid.Narrative)
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", rech.RechargeID)
            Throw
        End Try
    End Function

    Private Sub SavePostRechargeRequest(context As Context, ByRef response As PayTeloneBillResponse, stateID As Integer)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = response.ReplyMsg,
            .ReturnCode = response.ReplyCode,
            .ChannelID = xChannel.Channels.Web,
            .StateID = stateID,
            .Amount = response.Amount,
            .Discount = response.Discount,
            .RechargeID = response.RechargeID,
            .WalletBalance = GetWalletBalance(context.AccountId),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        response.WalletBalance = web.WalletBalance
        _webRequest.Save(web, False)
    End Sub

    Private Sub SavePreRechargeRequest(context As Context, req As PayTeloneBillRequest, state As xState.States)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = Nothing,
            .ReturnCode = state,
            .ChannelID = xChannel.Channels.Web,
            .StateID = state,
            .Amount = 0,
            .WalletBalance = GetWalletBalance(context.AccountId),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, True)
    End Sub


    Public Function RechargeTeloneVoip(req As RechargeTeloneVoipRequest, context As Context) As RechargeTeloneVoipResponse
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, req, xState.States.Busy)
        Dim response As RechargeTeloneVoipResponse = _rechargeTeloneVoip(req, rech, context)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function

    Private Function _rechargeTeloneVoip(req As RechargeTeloneVoipRequest, rech As xRecharge, context As Context) As RechargeTeloneVoipResponse
        Dim response As New RechargeTeloneVoipResponse(context.AgentReference)

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)

            rech.AccessID = context.AccessId
            rech.Amount = req.Amount
            rech.Brand = xBrandAdapter.GetBrand(xBrand.Brands.TeloneVoip, sqlConn)


            'If Recharge.Mobile.Length <> 10 Then
            '    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
            '    iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", Recharge.Mobile)
            '    Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            'End If

            'Validate Amount Range
            If rech.Amount < iConfig.MinRecharge Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(iConfig.MinRecharge, 2))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Check Available Sale Value vs Recharge Amount        
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            If iAccount.SaleValue < rech.Amount Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", rech.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.Balance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(iAccount.SaleValue))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Get Discount
            rech.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, rech.Brand.BrandID, sqlConn).Discount


            'Insert recharge
            rech.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(rech, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge 
            Return TelOneVoipPayment(rech, sqlConn, req.TargetNumber)

        End Using


        Return response
    End Function

    Private Function TelOneVoipPayment(rech As xRecharge, sqlConn As SqlConnection, targetNumber As String) As RechargeTeloneVoipResponse
        Dim network = New Telone(sqlConn, _applicationName, Endpoint("Telone_Endpoint"), _isTestMode, _referencePrefix, True)
        Try
            Dim response As ServiceRechargeResponse = network.RechargeVoipAccount(rech)

            If rech.IsSuccessFul Then
                Console.WriteLine("Recharge Successful. Replying")
                Dim rechargeresponse = Newtonsoft.Json.JsonConvert.DeserializeObject(Of TelOneAPI.VoipRechargeResponse)(response.RechargePrepaid.Narrative.Split(New String() {"Raw:"}, StringSplitOptions.None)(1))

                Dim replylist As New List(Of xTemplate)

                If Not String.IsNullOrWhiteSpace(targetNumber) Then ReplyCustomer(targetNumber, replylist, sqlConn)

                Return New RechargeTeloneVoipResponse("") With {
                    .Amount = rech.Amount,
                    .Discount = rech.Discount,
                    .RechargeID = rech.RechargeID,
                    .ReplyCode = 2,
                    .ReplyMsg = response.RechargePrepaid.Narrative.Split(New String() {"Raw:"}, StringSplitOptions.None)(0)
                    }
            Else
                Console.WriteLine("Recharge Failed. Replying")
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)
                ' GET TEXT FROM WEBSERVICE CUSTOMER
                iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", response.RechargePrepaid.Reference & ", " & response.RechargePrepaid.ReturnCode & ": " & response.RechargePrepaid.Narrative)
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", rech.RechargeID)
            Throw
        End Try
    End Function

    Private Sub SavePostRechargeRequest(context As Context, response As RechargeTeloneVoipResponse, stateID As Integer)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = response.ReplyMsg,
            .ReturnCode = response.ReplyCode,
            .ChannelID = xChannel.Channels.Web,
            .StateID = stateID,
            .Amount = response.Amount,
            .Discount = response.Discount,
            .RechargeID = response.RechargeID,
            .WalletBalance = GetWalletBalance(context.AccountId),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        response.WalletBalance = web.WalletBalance
        _webRequest.Save(web, False)
    End Sub

    Private Sub SavePreRechargeRequest(context As Context, req As RechargeTeloneVoipRequest, state As xState.States)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = Nothing,
            .ReturnCode = state,
            .ChannelID = xChannel.Channels.Web,
            .StateID = state,
            .Amount = 0,
            .WalletBalance = GetWalletBalance(context.AccountId),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, True)
    End Sub

#End Region


#Region "   Nyaradzo   "


    Friend Function QueryNyaradzoAccount(policyNumber As String, context As Context) As NyaradzoCustomerResponseModel
        Dim response As New NyaradzoCustomerResponseModel() With {.AgentReference = context.AgentReference}

        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Answer, True)
        response = _querynyaradzoaccount(policyNumber, context)
        SaveWebRequest(context, 200, response.ReplyMsg, xHotType.HotTypes.Answer, False)
        Return response
    End Function

    Private Function _querynyaradzoaccount(policyNumber As String, context As Context) As NyaradzoCustomerResponseModel
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Dim network = New Nyaradzo(sqlConn, _applicationName, Endpoint("Nyaradzo_EndPoint"), _isTestMode, _referencePrefix, True)
            Try
                If Not ValidPolicyNumber(policyNumber) Then GoTo InvalidMeterNumber
                Dim recharge = New xRecharge With {.State = New xState With {.StateID = 2},
                .AccessID = context.AccessId,
                .Amount = 0,
                .Brand = New xBrand With {.BrandID = xBrand.Brands.ZETDCFees},
                .Discount = 0,
                .Mobile = policyNumber,
                .RechargeDate = Date.Now}
                xRechargeAdapter.Save(recharge, sqlConn)
                Dim iRechargePrepaid As xRechargePrepaid = New xRechargePrepaid()
                iRechargePrepaid.RechargeID = recharge.RechargeID
                iRechargePrepaid.DebitCredit = recharge.Amount >= 0
                iRechargePrepaid.Reference = recharge.Mobile
                iRechargePrepaid.FinalBalance = -1
                iRechargePrepaid.InitialBalance = -1
                iRechargePrepaid.Narrative = IIf(iRechargePrepaid.DebitCredit, "Pending", "Debit Pending")
                iRechargePrepaid.ReturnCode = -1

                iRechargePrepaid.Reference = iRechargePrepaid.Reference
                xRechargePrepaidAdapter.Save(iRechargePrepaid, sqlConn)

                Dim response = network.CustomerInfoRequest(iRechargePrepaid, recharge)

                If response.ValidResponse Then

                    Dim result = New NyaradzoCustomerResponseModel With {
                        .AgentReference = context.AgentReference,
                        .AmountToBePaid = response.Item.AmountToBePaid,
                        .Balance = response.Item.Balance,
                        .MOnthlyPremium = response.Item.MonthlyPremium,
                        .PolicyHolderName = response.Item.PolicyHolder,
                        .PolicyNumber = response.Item.PolicyNumber,
                        .Status = response.Item.Status,
                        .ReplyCode = 2,
                        .ReplyMsg = response.Item.ResponseDescription
                    }
                    Return result
                Else
InvalidMeterNumber:
                    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)
                    iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", $"Invalid Meter Number - {policyNumber}")
                    Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
                End If
            Catch ex As Exception
                LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", 0)
                Throw
            End Try
            sqlConn.Close()
        End Using
    End Function

    Private Function ValidPolicyNumber(policyNumber As String) As Boolean
        Return True
    End Function

    Friend Function MakeNyaradzoPayment(request As NyaradzoPaymentRequest, context As Context) As RechargeResponseModel
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, request, xState.States.Busy)
        Dim response = _recharge(request, rech, context)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function

    Private Sub SavePreRechargeRequest(context As Context, rechargeRequest As NyaradzoPaymentRequest, state As xState.States)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = Nothing,
            .ReturnCode = state,
            .ChannelID = xChannel.Channels.Web,
            .StateID = state,
            .Amount = 0,
            .WalletBalance = GetWalletBalance(context.AccountId, 1),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, True)
    End Sub

    Private Function _recharge(request As NyaradzoPaymentRequest, ByRef recharge As xRecharge, context As Context) As RechargeResponseModel

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            recharge.AccessID = context.AccessId
            recharge.Amount = request.Amount
            recharge.Mobile = request.PolicyNumber
            recharge.ProductCode = request.MobileNumber
            recharge.Brand = xBrandAdapter.GetBrand(xBrand.Brands.Nyaradzo, sqlConn)

            If Not String.IsNullOrEmpty(recharge.ProductCode) Then
                If recharge.ProductCode.Length <> 10 Then
                    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
                    iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                    Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
                End If
            End If

            'Validate Amount Range
            If recharge.Amount < 5 Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(50, 2))
                Throw New HotRequestException(iReply.TemplateID, "Your recharge request was out of the acceptable range. Minimum Recharge 20.00, Max Recharge 50,000.00 try in the correct range. HOT Recharge\r\n")
            End If

            'Check Available Sale Value vs Recharge Amount        
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            If iAccount.ZESABalance < recharge.Amount Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.ZESABalance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(iAccount.ZESABalance))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            'Get Discount
            recharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, recharge.Brand.BrandID, sqlConn).Discount


            'Insert recharge
            recharge.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(recharge, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge
            Select Case recharge.Brand.BrandID
                Case xBrand.Brands.Nyaradzo
                    Return NyaradzoRecharge(recharge, request, sqlConn)
                Case Else
                    Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, recharge.Brand.BrandID & " brand code is not currently supported") ' return RetailRechargePin(recharge, sqlConn)
            End Select
        End Using


    End Function

    Private Function NyaradzoRecharge(recharge As xRecharge, request As NyaradzoPaymentRequest, sqlConn As SqlConnection) As RechargeResponseModel
        Dim nyaradzo = New Nyaradzo(sqlConn, _applicationName, Endpoint("Nyaradzo_Endpoint"), _isTestMode, _referencePrefix, True)
        Return RechargeRetail(nyaradzo, recharge, request.CustomerSMS, sqlConn)
    End Function


#End Region
    Public Function WalletBalance(context As Context) As WalletBalanceResponse
        Dim response As New WalletBalanceResponse With {
            .AgentReference = context.AgentReference
        }
        SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.Balance, True)
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulBalance, sqlConn)
            iReply.TemplateText = "Your HOT Balance is $ %BALANCE%. You can sell approximately $ %SALEVALUE%."
            iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(iAccount.Balance))
            iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(iAccount.SaleValue))
            response.ReplyCode = 2
            response.ReplyMsg = iReply.TemplateText
            response.WalletBalance = iAccount.Balance
            sqlConn.Close()
        End Using
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.Balance, False)
        Return response
    End Function

    Public Function EndUserBalance(request As EnduserBalanceRequest, context As Context) As EndUserBalanceResponse
        Dim response = _enduserBalance(request, context)
        SaveWebRequest(context, response.ReplyCode, response.ReplyMsg, xHotType.HotTypes.PhoneBal, False, response.RechargeId)
        Return response
    End Function

    Public Function Recharge(request As RechargePinlessRequest, context As Context) As RechargeResponseModel
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, request, xState.States.Busy)
        Dim response = _recharge(request, rech, context)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function

    Public Function RechargeUSD(request As RechargePinlessRequest, context As Context) As RechargeResponseModel
        Dim rech As New xRecharge
        SavePreRechargeRequest(context, request, xState.States.Busy)
        Dim response = _rechargeusd(request, rech, context)
        response.AgentReference = context.AgentReference
        Dim rech2 As xRecharge
        Using sqlConn As New SqlConnection(Config.GetConnectionString())
            sqlConn.Open()
            rech2 = xRechargeAdapter.SelectRecharge(response.RechargeID, sqlConn)
        End Using
        SavePostRechargeRequest(context, response, rech2.State.StateID)
        Return response
    End Function

    Private Function _enduserBalance(request As EnduserBalanceRequest, context As Context) As EndUserBalanceResponse


        Dim response As New EndUserBalanceResponse(context.AgentReference)
        'This inserts a zero value Recharge in our tblRecharge for tracking and queries Econet 

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()

            'Initialise insertion in our DB of a 0
            Dim iRecharge As New xRecharge With {
                .AccessID = context.AccessId,
                .Amount = 0,
                .Discount = 0,
                .Mobile = request.TargetMobile
            }
            Dim FlickSwitch As Integer = 10552407
            If iRecharge.AccessID = FlickSwitch Then iRecharge.Amount = 0.01

            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            If iAccount.SaleValue <= 0 Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedPhoneBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
                ErrorResponse(iReply.TemplateID, iReply.TemplateText)
            End If

            If IsInvalidMobileNumber(sqlConn, iRecharge) Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedPhoneBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
                ErrorResponse(iReply.TemplateID, iReply.TemplateText)
            End If

            'Get Network of Target Mobile
            Dim iNetwork As xNetwork = xNetwork_Data.Identify(iRecharge.Mobile, sqlConn)
            If iNetwork Is Nothing Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedPhoneBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
                response.ReplyCode = iReply.TemplateID
                response.ReplyMsg = iReply.TemplateText
                response.MobileBalance = 0
                Return response
            End If

            iRecharge.Brand = xBrandAdapter.GetBrand(iNetwork, " ", sqlConn)
            'End If
            If iRecharge.Brand Is Nothing Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedPhoneBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
                response.ReplyCode = iReply.TemplateID
                response.ReplyMsg = iReply.TemplateText
                response.MobileBalance = 0
                Return response
            End If

            'Insert $0 recharge with a busy state - balance query
            iRecharge.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(iRecharge, sqlConn)
            SaveWebRequest(context, Nothing, Nothing, xHotType.HotTypes.PhoneBal, True, iRecharge.RechargeID)

            'Record the Prepaid Platform Transaction reference number at initiation
            Dim iRechargePrepaid As New xRechargePrepaid With {
                .RechargeID = iRecharge.RechargeID,
                .Reference = iRecharge.Mobile & Format(Now, "dd-MM-yyyy HH:mm:ss"),
                .FinalBalance = -1,
                .InitialBalance = -1,
                .Narrative = "Balance Pending",
                .ReturnCode = -1
            }

            'If iRecharge.Brand.BrandID = xBrand.Brands.Econet078 Or iRecharge.Brand.BrandID = xBrand.Brands.EconetPlatform Then


            '    Dim econet = New Econet(sqlConn, _applicationName, Endpoint("Econet_Endpoint"), _isTestMode, _referencePrefix, True)
            '    Dim balance = econet.BalanceRequest(iRechargePrepaid, iRecharge)
            '    ' econet.BalanceRequest

            '    If balance.IsSuccessful Then
            '        iRecharge.State.StateID = xState.States.Success
            '        'Respond
            '        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulWebPhoneBalance, sqlConn)
            '        iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
            '        iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(balance.CurrentBalance))
            '        iReply.TemplateText = iReply.TemplateText.Replace("%EXPIRYDATE%", balance.ExpiryDate)
            '        response.ReplyCode = 2
            '        response.ReplyMsg = iReply.TemplateID & "," & iReply.TemplateText
            '        response.MobileBalance = balance.CurrentBalance
            '        response.WindowPeriod = "Expiry Date: " & balance.ExpiryDate
            '    Else
            '        iRecharge.State.StateID = xState.States.Failure
            '    End If

            'Else
            If iRecharge.Brand.BrandID = xBrand.Brands.EasyCall Then
                Dim netone = New NetOne(sqlConn, _applicationName, Endpoint("NetOne_Endpoint"), _isTestMode, _referencePrefix, True)
                Dim balance = netone.BalanceRequest(iRechargePrepaid, iRecharge)

                If balance.IsSuccessful Then
                    iRecharge.State.StateID = xState.States.Success
                    'Respond
                    Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulWebPhoneBalance, sqlConn)
                    iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
                    iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(balance.CurrentBalance))
                    iReply.TemplateText = iReply.TemplateText.Replace("%EXPIRYDATE%", balance.ExpiryDate)
                    response.ReplyCode = 2
                    response.ReplyMsg = iReply.TemplateID & "," & iReply.TemplateText
                    response.MobileBalance = balance.CurrentBalance
                    response.WindowPeriod = "Expiry Date: " & balance.ExpiryDate
                Else
                    iRecharge.State.StateID = xState.States.Failure
                End If

            Else 'its a Telecel or Africom request which can't be answered
                iRecharge.State.StateID = xState.States.Failure
                iRechargePrepaid.Narrative = "Balance Request not available"
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedPhoneBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", iRecharge.Mobile)
                response.ReplyCode = iReply.TemplateID
                response.ReplyMsg = iReply.TemplateText
                response.MobileBalance = 0
            End If

            'Save RechargePrepaid
            xRechargePrepaidAdapter.Save(iRechargePrepaid, sqlConn)

            'Save Recharge
            iRecharge.RechargeDate = Now
            xRechargeAdapter.Save(iRecharge, sqlConn)
            response.RechargeId = iRecharge.RechargeID
            sqlConn.Close()
        End Using
        Return response
    End Function

    Private Function _recharge(request As RechargePinlessRequest, ByRef recharge As xRecharge, context As Context) As RechargeResponseModel

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            recharge.AccessID = context.AccessId
            recharge.Amount = request.Amount
            recharge.Mobile = request.TargetMobile


            If IsInvalidMobileNumber(sqlConn, recharge) Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
            End If

            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)

            If ShouldBeRechargedAsUSDConverted(iAccount) _
                And (recharge.Brand.BrandID = xBrand.Brands.Econet078 Or recharge.Brand.BrandID = xBrand.Brands.EconetPlatform) Then
                Return _rechargeusd(request, recharge, context)
            End If
            If ShouldBeRechargedAsUSDConvertedNetone(iAccount) _
                And (recharge.Brand.BrandID = xBrand.Brands.EasyCall) Then
                Dim datarequest = New RechargeDataRequest() With {
                    .Amount = request.Amount,
                    .CustomerSMS = request.CustomerSMS,
                    .TargetMobile = request.TargetMobile,
                    .ProductCode = If(request.Amount < 1, $"UA0{$"{request.Amount:0.0}".Replace("0.", "")}", $"UA{request.Amount:0}")}
                Return _recharge(datarequest, recharge, context, True)
            End If

            'Validate Amount Range
            If recharge.Amount < iConfig.MinRecharge Or recharge.Amount > iConfig.MaxRecharge Then
                Return InvalidAmountResponse(sqlConn, recharge, iConfig)
            End If

            'Check Available Sale Value vs Recharge Amount        

            Try
                'Get Discount
                recharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, recharge.Brand.BrandID, sqlConn).Discount

            Catch ex As Exception
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeVASDisabled, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("Your SMS: %MESSAGE%", "")
                Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
            End Try

            Dim Balance As Decimal = Nothing
            Dim Salevalue As Decimal = Nothing
            Dim HasSufficientFunds As Boolean = Nothing
            CalculateRechargeCost(recharge, iAccount, Balance, Salevalue, HasSufficientFunds)

            If Not HasSufficientFunds Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(Balance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(Salevalue))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If

            If DealerEconetZWLNotAllowed(recharge, iAccount) Or DealerNetoneZWLNotAllowed(recharge, iAccount) Then

                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeVASDisabled, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("Your SMS: %MESSAGE%", "")
                Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
            End If


            If _econetLimitsEnabled And (IsEconetZWLTransaction(recharge) Or IsNetoneZWLTransaction(recharge)) Then
                Dim network As xNetwork = xNetwork_Data.Identify(recharge.Mobile, sqlConn)
                Dim limit As xLimit = xLimitAdapter.GetLimit(network.NetworkID, iAccount.AccountID, sqlConn)
                If limit IsNot Nothing Then
                    If limit.LimitRemaining < recharge.Amount And Not limit.LimitRemaining = -1 Then
                        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(
                                    If(limit.LimitTypeId = 1, xTemplate.Templates.FailedEconetDailyLimit, xTemplate.Templates.FailedEconetMonthlyLimit), sqlConn)
                        iReply.TemplateText = iReply.TemplateText.Replace("%LIMIT%", limit.LimitRemaining.ToString("#,##0.00"))
                        iReply.TemplateText = iReply.TemplateText.Replace("%NETWORK%", network.Network)
                        Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
                    End If
                End If
            End If



            'Insert recharge
            recharge.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(recharge, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge
            Select Case recharge.Brand.BrandID
                Case xBrand.Brands.Prepaid, xBrand.Brands.EconetPlatform, xBrand.Brands.Econet078, xBrand.Brands.EconetBB, xBrand.Brands.EconetTXT
                    Return EconetRetailRecharge(recharge, request.CustomerSMS, sqlConn)
                Case xBrand.Brands.EasyCall
                    Return RetailNetOne(recharge, request.CustomerSMS, sqlConn)
                Case xBrand.Brands.Juice
                    Return RetaileJuicePinless(recharge, request.CustomerSMS, sqlConn)
                Case xBrand.Brands.Africom
                    Return RetailAfricom(recharge, request.CustomerSMS, sqlConn)
                Case Else
                    Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, recharge.Brand.BrandID & " brand code is not currently supported") ' return RetailRechargePin(recharge, sqlConn)
            End Select
        End Using
    End Function

    Private Function DealerEconetZWLNotAllowed(recharge As xRecharge, iAccount As xAccount) As Boolean
        Return (iAccount.Profile.ProfileID < 12 Or
            iAccount.Profile.ProfileID > 59 Or iAccount.Profile.ProfileID = 31) And
        _econetZWLDealersEnabled = False And
            IsEconetZWLTransaction(recharge)
    End Function
    Private Function DealerNetoneZWLNotAllowed(recharge As xRecharge, iAccount As xAccount) As Boolean
        Return (iAccount.Profile.ProfileID < 12 Or
            iAccount.Profile.ProfileID > 59 Or iAccount.Profile.ProfileID = 31) And
            _netoneZWLDealersEnabled = False And
            IsNetoneZWLTransaction(recharge)
    End Function

    Private Function ShouldBeRechargedAsUSDConverted(iAccount As xAccount) As Boolean
        For Each accountId In _econetUSDOnlyAccounts.Split(",")
            If iAccount.AccountID = accountId Then Return True
        Next
        Return False
    End Function
    Private Function ShouldBeRechargedAsUSDConvertedNetone(iAccount As xAccount) As Boolean
        For Each accountId In _netoneUSDOnlyAccounts.Split(",")
            If iAccount.AccountID = accountId Then Return True
        Next
        Return False
    End Function

    Private Function _rechargeusd(request As RechargePinlessRequest, ByRef recharge As xRecharge, context As Context) As RechargeResponseModel

        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim iConfig As xConfig = xConfigAdapter.Config(sqlConn)
            recharge.AccessID = context.AccessId
            recharge.Amount = request.Amount
            recharge.Mobile = request.TargetMobile + "U"

            'Validate Amount Range
            If recharge.Amount < iConfig.MinRecharge Or recharge.Amount > iConfig.MaxRecharge Then
                Return InvalidAmountResponse(sqlConn, recharge, iConfig)
            End If

            If IsInvalidMobileNumber(sqlConn, recharge) Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeMobile, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
            End If

            Dim iAccount As xAccount = xAccountAdapter.SelectRow(context.AccountId, sqlConn)
            'Get Discount
            recharge.Discount = xProfileDiscountAdapter.Discount(iAccount.Profile.ProfileID, recharge.Brand.BrandID, sqlConn).Discount

            Dim Balance As Decimal = Nothing
            Dim Salevalue As Decimal = Nothing
            Dim HasSufficientFunds As Boolean = Nothing
            CalculateRechargeCost(recharge, iAccount, Balance, Salevalue, HasSufficientFunds)

            If Not HasSufficientFunds Then
                Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedRechargeBalance, sqlConn)
                iReply.TemplateText = iReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                iReply.TemplateText = iReply.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(Balance))
                iReply.TemplateText = iReply.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(Salevalue))
                Throw New HotRequestException(iReply.TemplateID, iReply.TemplateText)
            End If
            'Insert recharge
            recharge.State.StateID = xState.States.Busy
            xRechargeAdapter.Save(recharge, sqlConn)

            'Determine whether to dispense pins or do direct prepaid recharge
            Select Case recharge.Brand.BrandID
                Case xBrand.Brands.TelecelUSD
                    Return RetaileJuiceUSDPinless(recharge, request.CustomerSMS, sqlConn)
                Case xBrand.Brands.EconetUSD
                    Return RetailEconetUSDPinless(recharge, request.CustomerSMS, sqlConn)
                Case xBrand.Brands.NetoneUSD
                    Return RetailNetoneUSDPinless(recharge, request.CustomerSMS, sqlConn)
                Case Else
                    recharge.State.StateID = xState.States.Failure
                    xRechargeAdapter.Save(recharge, sqlConn)
                    Throw New HotRequestException(xTemplate.Templates.FailedUnsupportedBrand, recharge.Brand.BrandID & " brand code is not currently supported") ' return RetailRechargePin(recharge, sqlConn)
            End Select
        End Using
    End Function


    Public Function QueryTransaction(req As QueryTransactionRequest, context As Context) As QueryTransactionsResponse
        Dim response = New QueryTransactionsResponse
        Using sqlCon As New SqlConnection(_connString)
            sqlCon.Open()
            Dim web = _webRequest.GetRequest(req.AgentReference, context.AccessId)
            If web.RechargeID Then
                Dim rech = xRechargeAdapter.SelectRecharge(web.RechargeID, sqlCon)
                response.ReplyCode = rech.State.StateID
            Else
                response.ReplyCode = web.ReturnCode
            End If
            response.RawReply = ""

            response.ReplyMsg = web.Reply
            response.OriginalAgentReference = web.AgentReference
            response.AgentReference = context.AgentReference

            Return response
        End Using
    End Function

    Private Sub SavePreRechargeRequest(context As Context, rechargeRequest As RechargePinlessRequest, state As xState.States)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = Nothing,
            .ReturnCode = state,
            .ChannelID = xChannel.Channels.Web,
            .StateID = state,
            .Amount = rechargeRequest.Amount,
            .WalletBalance = GetWalletBalance(context.AccountId),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, True)
    End Sub

    Private Sub SavePostRechargeRequest(context As Context, rechargeResponse As RechargeResponseModel, state As xState.States)
        Dim web = New xWebRequest() With {
            .AgentReference = context.AgentReference,
            .AccessID = context.AccessId,
            .Reply = rechargeResponse.ReplyMsg,
            .ReturnCode = rechargeResponse.ReplyCode,
            .ChannelID = xChannel.Channels.Web,
            .StateID = state,
            .Amount = rechargeResponse.Amount,
            .Discount = rechargeResponse.Discount,
            .RechargeID = rechargeResponse.RechargeID,
            .WalletBalance = If(state = xState.States.Success, rechargeResponse.WalletBalance, GetWalletBalance(context.AccountId)),
            .HotTypeID = xHotType.HotTypes.Recharge
        }
        _webRequest.Save(web, False)
    End Sub

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

    Private Function GetWalletBalance(accountId As Long, Optional WalletType As Integer = 0) As Decimal
        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Dim account = xAccountAdapter.SelectRow(accountId, sqlConn)
            Dim Balance = account.Balance
            If WalletType = 2 Then Balance = account.USDBalance 'KMR 13/01/2025- possibly an error here WalletTypeID is 3 for USD, 2 for Utility ZWG (ZESA), 4 for Utility USD (Zesa)
            If WalletType = 1 Then Balance = account.ZESABalance ' KMR 13/01/2025 WalletTypeID error?
            Return Balance
        End Using
    End Function

    ' Move to Core
    Private Function IsInvalidMobileNumber(sqlConn As SqlConnection, iRecharge As xRecharge) As Boolean
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

    Private Function InvalidAmountResponse(sqlConn As SqlConnection, iRecharge As xRecharge, iConfig As xConfig) As RechargeResponseModel
        Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedWebRechargeMinMax, sqlConn)
        iReply.TemplateText = iReply.TemplateText.Replace("%MIN%", Formatting.FormatAmount(iConfig.MinRecharge, 2))
        iReply.TemplateText = iReply.TemplateText.Replace("%MAX%", Formatting.FormatAmount(iConfig.MaxRecharge, 2))
        Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
    End Function

    Private Function ErrorResponse(templateId As Integer, message As String) As RechargeResponseModel
        Throw New HotRequestException(templateId, message)
    End Function

    Private Function SuccessResponse(message As String, discount As Decimal, rechargeId As Integer, amount As Decimal, walletBalance As Decimal, prepaid As xRechargePrepaid) As RechargeResponseModel
        Dim reply = New RechargeResponseModel() With {
            .ReplyCode = 2,
            .ReplyMsg = message,
            .Discount = discount,
            .RechargeID = rechargeId,
            .Amount = amount,
            .WalletBalance = walletBalance,
            .InitialBalance = prepaid.InitialBalance,
            .FinalBalance = prepaid.FinalBalance,
            .Window = prepaid.Window,
            .Data = prepaid.Data,
            .SMS = prepaid.SMS
        }
        Return reply
    End Function

    Private Function Endpoint(name As String) As String
        Return ConfigurationManager.AppSettings(name)
    End Function

    Private Function RechargeRetail(Of T)(network As NetworkBase(Of T), iRecharge As xRecharge, customSms As String, sqlConn As SqlConnection) As RechargeResponseModel
        Try
            Dim response As ServiceRechargeResponse = network.RechargePrepaid(iRecharge)
            response.CustomCustomerCreditSuccessSMS = customSms
            Return HandleRetailServiceResponse(sqlConn, iRecharge, response)
        Catch ex As Exception
            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", iRecharge.RechargeID)
            Throw
        End Try
    End Function

    Private Function HandleRetailServiceResponse(sqlConn As SqlConnection, iRecharge As xRecharge, response As ServiceRechargeResponse) As RechargeResponseModel
        If iRecharge.IsSuccessFul Then
            Console.WriteLine("Recharge Successful. Replying")
            Return RetailReplyPrepaid(iRecharge, response, sqlConn)
        Else
            Console.WriteLine("Recharge Failed. Replying")
            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedVAS, sqlConn)
            ' GET TEXT FROM WEBSERVICE CUSTOMER
            iReply.TemplateText = iReply.TemplateText.Replace("%MESSAGE%", response.RechargePrepaid.Reference & ", " & response.RechargePrepaid.ReturnCode & ": " & response.RechargePrepaid.Narrative)
            Return ErrorResponse(iReply.TemplateID, iReply.TemplateText)
        End If
    End Function


    Private Sub ReplyCustomer(recipient As String, list As List(Of xTemplate), sqlConn As SqlConnection, Optional ByVal sms As xSMS = Nothing)
        For Each iReply As xTemplate In list
            Dim smsReply As New xSMS With {
                .Direction = False,
                .Mobile = recipient,
                .SMSText = iReply.TemplateText,
                .SMSDate = Now
            }
            smsReply.Priority.PriorityID = xPriority.Priorities.Normal
            smsReply.State.StateID = xState.States.Pending

            If sms IsNot Nothing Then
                smsReply.SMSID_In = sms.SMSID
            End If
            xSMSAdapter.Save(smsReply, sqlConn)
        Next
    End Sub


#Region "   RetailRecharge   "
    '
    '    Private Function RetailRechargePin(recharge As xRecharge, sqlConn As SqlConnection) As RechargeResponseModel
    '        Try
    '            Dim pin = new Pin(sqlConn, _applicationName)
    '            Dim response As PinRechargeResponse = pin.RechargePin(recharge)
    '            If response.Success Then
    '                ReplyCustomer(recharge.Mobile, response.CustomerTemplates, sqlConn, response.Sms)
    '                Dim dealerHeader As String = "Recharge to " & recharge.Mobile & " of $" & recharge.Amount & " was successful."
    '                Dim res As RechargeResponseModel = SuccessResponse(dealerHeader, recharge.Discount, recharge.RechargeID, 
    '                                                                   recharge.Amount, response.Account.Balance, response. )
    '                res.Discount = recharge.Discount
    '                return res
    '            Else
    '                Dim err As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.FailedPinDenomination, sqlConn)
    '                return ErrorResponse(err.TemplateID, err.TemplateText)
    '            End If
    '        Catch ex As Exception
    '            LogError(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
    '            Return ErrorResponse(219, ex.Message)
    '        End Try
    '    End Function


    Private Function EconetRetailRecharge(recharge As xRecharge, customSms As String, sqlConn As SqlConnection) As RechargeResponseModel

        Dim econet = New Econet(sqlConn, _applicationName, Endpoint("Econet_Endpoint"), _isTestMode, _referencePrefix, True)
        Return RechargeRetail(econet, recharge, customSms, sqlConn)
    End Function

    Private Function RetailNetOne(recharge As xRecharge, customSms As String, sqlConn As SqlConnection) As RechargeResponseModel
        Dim netOne = New NetOne(sqlConn, _applicationName, Endpoint("NetOne_Endpoint"), _isTestMode, _referencePrefix, True)
        Return RechargeRetail(netOne, recharge, customSms, sqlConn)
    End Function
    Private Function RetailNetOneUSDPinless(recharge As xRecharge, customSms As String, sqlConn As SqlConnection) As RechargeResponseModel
        Dim netOne = New NetOne(sqlConn, _applicationName, Endpoint("NetOne_Endpoint"), _isTestMode, _referencePrefix, True)
        Return RechargeRetail(netOne, recharge, customSms, sqlConn)
    End Function
    Private Function RetailAfricom(recharge As xRecharge, customSms As String, sqlConn As SqlConnection) As RechargeResponseModel
        Dim africom = New Africom(sqlConn, _applicationName, Endpoint("Africom_Endpoint"), _isTestMode, _referencePrefix, True)
        Return RechargeRetail(africom, recharge, customSms, sqlConn)
    End Function
    Private Function RetaileJuicePinless(recharge As xRecharge, customSms As String, sqlConn As SqlConnection) As RechargeResponseModel
        Dim telecel = New Telecel(sqlConn, _applicationName, Endpoint("TeleCel_Endpoint"), _isTestMode, _referencePrefix, True)
        Return RechargeRetail(telecel, recharge, customSms, sqlConn)
    End Function
    Private Function RetaileJuiceUSDPinless(recharge As xRecharge, customSms As String, sqlConn As SqlConnection) As RechargeResponseModel
        Dim telecel = New TelecelUSD(sqlConn, _applicationName, Endpoint("TeleCel_Endpoint"), _isTestMode, _referencePrefix, True)
        Return RechargeRetail(telecel, recharge, customSms, sqlConn)
    End Function
    Private Function RetailEconetUSDPinless(recharge As xRecharge, customSms As String, sqlConn As SqlConnection) As RechargeResponseModel
        Dim econet = New EconetPrepaid(sqlConn, _applicationName, Endpoint("EconetBundle_Endpoint"), _isTestMode, _referencePrefix, True)
        Return RechargeRetail(econet, recharge, customSms, sqlConn)
    End Function
    Private Function RetailReplyPrepaid(recharge As xRecharge, serviceResponse As ServiceRechargeResponse, sqlConn As SqlConnection) As RechargeResponseModel

        Dim iAccess As xAccess = xAccessAdapter.SelectRow(recharge.AccessID, sqlConn)
        Dim accessWeb As xAccessWeb = xAccessWebAdapter.SelectRow(recharge.AccessID, sqlConn)
        Dim account As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
        Dim rechargePrepaid = serviceResponse.RechargePrepaid

        If recharge.Amount > 0 Then 'Credit

            Dim IsBundle As Boolean = False
            Dim BundleName As String = ""
            If (
                (recharge.Brand.BrandID = xBrand.Brands.EconetData) Or
                (recharge.Brand.BrandID = xBrand.Brands.EconetFacebook) Or
                (recharge.Brand.BrandID = xBrand.Brands.EconetWhatsapp) Or
                (recharge.Brand.BrandID = xBrand.Brands.EconetInstagram) Or
                (recharge.Brand.BrandID = xBrand.Brands.EconetTXT) Or
                (recharge.Brand.BrandID = xBrand.Brands.EconetTwitter) Or
                (recharge.Brand.BrandID = xBrand.Brands.NetoneOneFusion) Or
                (recharge.Brand.BrandID = xBrand.Brands.NetoneOneFi) Or
                (recharge.Brand.BrandID = xBrand.Brands.NetoneUSD And Not recharge.Brand.BrandSuffix.ToUpper() = "U") Or
                (recharge.Brand.BrandID = xBrand.Brands.EconetUSD And Not recharge.Brand.BrandSuffix.ToUpper() = "U")
            ) Then
                BundleName = EconetBundle.Repository.BundleRepository.Get(recharge.Brand.BrandSuffix, sqlConn).Name
                IsBundle = True

            End If


            'Send Dealer Response       

            Dim dealerHeader As String =
                    If(IsBundle, "Data ", "") & "Recharge to " & recharge.Mobile & " of " &
                    If(IsBundle, BundleName & " valued at ", "") & "$" & recharge.Amount & " was successful." &
                    If(Not IsBundle, " The initial balance was $" & Formatting.FormatAmount(rechargePrepaid.InitialBalance) & " final balance is $" & Formatting.FormatAmount(rechargePrepaid.FinalBalance), "")

            'Send Customer Response

            Dim iTemplate As xTemplate.Templates = If(IsBundle, xTemplate.Templates.SuccessfulWebServiceDataCustomer, xTemplate.Templates.SuccessfulWebServiceVasCustomer)
            If recharge.Brand.BrandID = xBrand.Brands.Nyaradzo Then
                iTemplate = xTemplate.Templates.SuccessfulNyaradzo
            End If
            Dim customerTemplate As xTemplate = xTemplateAdapter.SelectRow(iTemplate, sqlConn)
            Dim custTxt = customerTemplate.TemplateText


            Dim Senditoo As Long = 10572106 ' Promotion We dont want to be associated with
            Dim veritran As Long = 10534171, veritranWebDev As Long = 10541358 ' Webdev Agreement

            If serviceResponse.CustomCustomerCreditSuccessSMS <> "" Then
                custTxt = serviceResponse.CustomCustomerCreditSuccessSMS
            ElseIf account.AccountID = veritran Then '  10004702 ZimSwitch Shared Services - Veritran mod
                custTxt = accessWeb.AccessName + " topped up your Airtime with $%AMOUNT%." + vbCrLf + "Your new balance is $%BALANCE%"
            End If

            custTxt = custTxt.Replace("%COMPANY%", account.AccountName)
            custTxt = custTxt.Replace("%COMPANYNAME%", account.AccountName)
            custTxt = custTxt.Replace("%AMOUNT%", Formatting.FormatAmount(recharge.Amount))
            custTxt = custTxt.Replace("%INITIALBALANCE%", Formatting.FormatAmount(rechargePrepaid.InitialBalance))
            custTxt = custTxt.Replace("%DATA%", Formatting.FormatAmount(rechargePrepaid.Data))
            custTxt = custTxt.Replace("%TXT%", Formatting.FormatAmount(rechargePrepaid.SMS))
            custTxt = custTxt.Replace("%BUNDLE%", BundleName)
            custTxt = custTxt.Replace("%MOBILE%", recharge.Mobile)


            If accessWeb IsNot Nothing Then
                custTxt = custTxt.Replace("%ACCESSNAME%", accessWeb.AccessName)
                custTxt = custTxt.Replace("%ACCESS%", accessWeb.AccessName)
            End If


            If rechargePrepaid.FinalBalance <> -1 Then
                custTxt = custTxt.Replace("%FINALBALANCE%", Formatting.FormatAmount(rechargePrepaid.FinalBalance))
                custTxt = custTxt.Replace("%BALANCE%", Formatting.FormatAmount(rechargePrepaid.FinalBalance))
            Else
                custTxt = custTxt.Replace("%FINALBALANCE%", "Unknown")
                custTxt = custTxt.Replace("%BALANCE%", "Unknown")
            End If

            If account.AccountID = veritran Or recharge.AccessID = veritranWebDev Or
                account.AccountID = Senditoo _
                Or iAccess.Channel.ChannelID = xChannel.Channels.Email Or iAccess.Channel.ChannelID = xChannel.Channels.ServiceWithNoTag Then
                If custTxt.Length > 160 Then custTxt = custTxt.Substring(0, 160)
            Else
                If custTxt.Length > 135 Then custTxt = custTxt.Substring(0, 135)
                custTxt = custTxt & vbCrLf & "Powered by HOT Recharge"
            End If


            customerTemplate.TemplateText = custTxt
            Dim iListCustomer As New List(Of xTemplate) From {
                customerTemplate
            }

            If Not (({25, 55}.Contains(account.Profile.ProfileID) And iAccess.Channel.ChannelID = xChannel.Channels.Email) _
               Or ({xBrand.Brands.EasyCall}.Contains(recharge.Brand.BrandID))) _
            Then
                If (recharge.Brand.BrandID = xBrand.Brands.Nyaradzo) Then
                    If Not String.IsNullOrEmpty(recharge.ProductCode) Then
                        ReplyCustomer(recharge.ProductCode, iListCustomer, sqlConn)
                    End If
                Else
                    ReplyCustomer(recharge.Mobile, iListCustomer, sqlConn)
                End If
            End If
            Dim balance As Decimal = GetBalance(recharge.Brand.WalletTypeId, account)
            Return SuccessResponse(dealerHeader, recharge.Discount, recharge.RechargeID, recharge.Amount, balance, rechargePrepaid)
        Else 'Debit           
            Dim balance As Decimal = GetBalance(recharge.Brand.WalletTypeId, account)
            'Send Dealer Response                        
            Dim dealerHeader As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulReversalDealer, sqlConn)
            dealerHeader.TemplateText = dealerHeader.TemplateText.Replace("%MOBILE%", recharge.Mobile)
            dealerHeader.TemplateText = dealerHeader.TemplateText.Replace("%AMOUNT%", Formatting.FormatAmount(recharge.Amount))
            dealerHeader.TemplateText = dealerHeader.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(balance))
            dealerHeader.TemplateText = dealerHeader.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(account.SaleValue))


            'Send Customer Response
            Dim iReplyCustomer As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulReversalCustomer, sqlConn)
            iReplyCustomer.TemplateText = iReplyCustomer.TemplateText.Replace("%AMOUNT%", Formatting.FormatAmount(recharge.Amount))
            iReplyCustomer.TemplateText = iReplyCustomer.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(rechargePrepaid.FinalBalance))
            Dim iListCustomer As New List(Of xTemplate) From {
                iReplyCustomer
            }

            ReplyCustomer(recharge.Mobile, iListCustomer, sqlConn)

            Return SuccessResponse(dealerHeader.TemplateID & "," & dealerHeader.TemplateText, recharge.Discount, recharge.RechargeID, recharge.Amount, balance, rechargePrepaid)
        End If
    End Function

    Private Function GetBalance(wallet As Integer, account As xAccount) As Decimal
        Dim balance = account.Balance
        If wallet = 2 Then balance = account.ZESABalance
        If wallet = 3 Then balance = account.USDBalance
        Return balance
    End Function

#End Region


    Private Sub LogError(methodName As String, ex As Exception, Optional idType As String = Nothing, Optional idNumber As Long = Nothing)
        xLog_Data.Save(_applicationName, _typeName, methodName, ex, _connString, idType, idNumber)
    End Sub

End Class