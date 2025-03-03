Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Core.Brands
Imports Hot.Core.ServiceWrappers
Imports Hot.Core.ZesaAPI
Imports Hot.Data
Imports NSubstitute
Imports System.Text
Imports Newtonsoft.Json

Public Class ZESA
    Inherits NetworkBase(Of ZESASoap)


    Private Const HotAPIKey As String = "Hot263180"
    Private Const TimeoutPeriod As Integer = 180000

    Public Sub New(sqlConn As SqlConnection, applicationName As String, serviceEndpoint As String, isTestMode As Boolean, referencePrefix As String, webServiceOrCorporate As Boolean)
        MyBase.New(sqlConn, applicationName, serviceEndpoint, isTestMode, webServiceOrCorporate, referencePrefix)
    End Sub

    Public Function CompleteZesa(recharge As xRecharge, tokenresponse As PurchaseToken) As ServiceRechargeResponse
        Dim iRechargePrepaid As xRechargePrepaid = xRechargeAdapter.SelectRechargePrepaid(recharge.RechargeID, SqlConn)
        recharge.ProductCode = iRechargePrepaid.Reference
        Dim response As PurchaseTokenResponse = New PurchaseTokenResponse() With {
            .CustomerInfo = New CustomerInfo(),
            .FinalBalance = iRechargePrepaid.FinalBalance,
            .InitialBalance = iRechargePrepaid.InitialBalance,
            .PurchaseToken = tokenresponse,
            .ReplyCode = IIf(tokenresponse.ResponseCode = "00", 2, 3),
            .Reference = iRechargePrepaid.Reference,
            .ReplyMessage = tokenresponse.Narrative
            }

        Try
            Console.WriteLine("Crediting")
            xLog_Data.WriteJsonToConsole(response)

            Console.WriteLine("Response from Netone " & response.ToString)
            SetRechargePrepaid(recharge, iRechargePrepaid, 90, response)


            If recharge.IsSuccessFul() Then
                SavePins(response, recharge, SqlConn.ConnectionString)
                'recharge.Amount += 7  ' Add Charges For E-Solutions
                SaveFees(response, recharge, SqlConn)
            End If
        Catch ex As Exception

            Console.WriteLine("Credit Exception")
            xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
            recharge.State.StateID = xState.States.Failure


        End Try

        Console.WriteLine("Saving prepaid")
        xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)
        Console.WriteLine("Saving Recharge")
        xRechargeAdapter.Save(recharge, SqlConn)
        'If recharge.IsSuccessFul() Then
        '    Dim rechargefee = New xRecharge With {.State = New xState With {.StateID = 2},
        '        .AccessID = recharge.AccessID,
        '        .Amount = 7,
        '        .Brand = New xBrand With {.BrandID = xBrand.Brands.ZETDCFees},
        '        .Discount = 0,
        '        .Mobile = recharge.Mobile,
        '        .RechargeDate = Date.Now}
        '    xRechargeAdapter.Save(rechargefee, SqlConn)
        '    Dim RechargePrepaidfee As xRechargePrepaid = CreateRechargeObject(rechargefee)
        '    RechargePrepaidfee.Narrative = recharge.RechargeID.ToString()
        '    RechargePrepaidfee.Reference = iRechargePrepaid.Reference
        '    xRechargePrepaidAdapter.Save(RechargePrepaidfee, SqlConn)

        'End If
        Return New ServiceRechargeResponse(iRechargePrepaid)
    End Function

    Private Sub CreditSubscriber(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer)
        Dim creditRequest As PurchaseZESATokenByCurrencyRequest = CreateCreditRequest(recharge, rechargePrepaid, numberOfDays)
        Dim networkClient = GetNetworkClient(TimeoutPeriod)
        Try
            Console.WriteLine("Crediting")
            Dim client = networkClient.GetNetwork()
            Dim Response = client.PurchaseZESATokenByCurrency(creditRequest).Body.PurchaseZESATokenByCurrencyResult
            xLog_Data.WriteJsonToConsole(Response)

            Console.WriteLine("Response from Netone " & Response.ToString)
            If Not HasTimeoutErrors(Response.ReplyMessage) Then
                SetRechargePrepaid(recharge, rechargePrepaid, numberOfDays, Response)

            Else
                'QueryRecharge(recharge, rechargePrepaid, numberOfDays, client)hriow 

                Throw New Exception(Response.ReplyMessage)
            End If
            If recharge.IsSuccessFul() Then
                SavePins(Response, recharge, SqlConn.ConnectionString)
                'SaveFees(Response, recharge, SqlConn)
            End If
        Catch ex As Exception

            networkClient.Abort()
            Console.WriteLine("Credit Exception")
            xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
            recharge.State.StateID = xState.States.Failure
            rechargePrepaid.Narrative = "NetoneAPI Error: " & ex.Message

        Finally
            networkClient.Close()
        End Try
    End Sub

    Private Sub SaveFees(response As PurchaseTokenResponse, _recharge As xRecharge, sqlConn As SqlConnection)
        Dim recharge = New xRecharge With {.State = New xState With {.StateID = 2},
                .AccessID = _recharge.AccessID,
                .Amount = 0,
                .Brand = New xBrand With {.BrandID = xBrand.Brands.ZETDCFees},
                .Discount = 0,
                .Mobile = _recharge.Mobile,
                .RechargeDate = Date.Now}
        xRechargeAdapter.Save(recharge, sqlConn)
        Dim iRechargePrepaid As New xRechargePrepaid With {
            .RechargeID = recharge.RechargeID,
            .DebitCredit = recharge.Amount >= 0,
            .Reference = recharge.Mobile,
            .FinalBalance = -1,
            .InitialBalance = -1,
            .Narrative = $"Recharge ID: {_recharge.RechargeID} ",
            .ReturnCode = -1,
            .Data = _recharge.RechargeID,
            .FinalWallet = response.FinalBalance,
            .InitialWallet = response.InitialBalance
        }

        xRechargePrepaidAdapter.Save(iRechargePrepaid, sqlConn)
    End Sub

    Private Sub SavePins(Response As PurchaseTokenResponse, recharge As xRecharge, connectionString As String)
        Using sqlConn As New SqlConnection(connectionString)
            sqlConn.Open()
            Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
            Try
                'Batch
                Dim iBatch As New xPinBatch
                iBatch.PinBatch = $"ZESA-PinBatch-{Date.Now:MMDDYYYY}"
                iBatch.PinBatchType = New xPinBatchType()
                iBatch.PinBatchType.PinBatchTypeID = xPinBatchType.PinBatchTypes.ZESA
                xPinBatchAdapter.Insert(iBatch, sqlConn, sqlTrans)

                Dim iPins As List(Of xPin) = GetPin(Response.PurchaseToken, recharge, iBatch)
                For Each iPin In iPins
                    If Not String.IsNullOrEmpty(iPin.Pin) Then
                        Try
                            xPinAdapter.Insert(iPin, sqlConn, sqlTrans)
                            Dim RP = New DataTable()
                            RP.Columns.Add("RechargeID")
                            RP.Columns.Add("PinID")

                            Dim dr = RP.NewRow()
                            dr("PinID") = iPin.PinID
                            dr("RechargeID") = recharge.RechargeID
                            RP.Rows.Add(dr)

                            Dim objbulk = New SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, sqlTrans)
                            objbulk.DestinationTableName = "tblRechargePin"
                            objbulk.ColumnMappings.Add("RechargeID", "RechargeID")
                            objbulk.ColumnMappings.Add("PinID", "PinID")
                            objbulk.WriteToServer(RP)
                        Catch ex As Exception

                        End Try


                    End If

                Next

                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                'Throw ex
            Finally
                sqlConn.Close()
            End Try
        End Using
    End Sub

    Private Function GetPin(body As PurchaseToken, recharge As xRecharge, iBatch As xPinBatch) As List(Of xPin)
        Dim iPins As New List(Of xPin)
        For Each pin In body.Tokens
            Try
                Dim iPin As New xPin With {
                    .Brand = recharge.Brand,
                    .Pin = pin.Token,
                    .PinRef = pin.ZesaReference,
                    .PinBatch = iBatch,
                    .PinExpiry = Date.Now.AddYears(1),
                    .PinState = New xPinState,
                    .PinValue = body.Amount
                }
                iPin.PinState.PinStateID = xPinState.PinStates.SoldHotBanking
                iPins.Add(iPin)
            Catch ex As Exception
                'Return New xPin()
            End Try
        Next

        Return iPins
    End Function

    Private Function CreateCreditRequest(recharge As xRecharge, iRechargePrepaid As xRechargePrepaid,
                                             numberOfDays As Integer) As PurchaseZESATokenByCurrencyRequest
        Return New PurchaseZESATokenByCurrencyRequest With {
                .Body = New PurchaseZESATokenByCurrencyRequestBody With {
                .Reference = recharge.RechargeID,
                .Amount = recharge.Amount,
                .MeterNumber = recharge.Mobile,
                .AppKey = HotAPIKey,
                .Currency = If(recharge.Brand.BrandID = xBrand.Brands.ZETDCUSD, "USD", "ZWG")
            }}
    End Function


    Public Overrides Function RechargePrepaid(recharge As xRecharge, Optional numberOfDays As Integer = 90) As ServiceRechargeResponse
        ApplyDiscountRules(recharge)
        Dim iRechargePrepaid As xRechargePrepaid = CreateRechargeObject(recharge)
        xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)

        If iRechargePrepaid.DebitCredit Then
            CreditSubscriber(recharge, iRechargePrepaid, numberOfDays)
        Else
            'Debit not possible on Netone 
            iRechargePrepaid.Narrative = "Bundle Debit not possible"
            iRechargePrepaid.ReturnCode = "099"
            recharge.State.StateID = xState.States.Failure
        End If
        Console.WriteLine("Saving prepaid")
        xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)
        Console.WriteLine("Saving Recharge")
        xRechargeAdapter.Save(recharge, SqlConn)
        ' Add E Solution Fees
        'If recharge.IsSuccessFul() Then
        '    Dim rechargefee = New xRecharge With {.State = New xState With {.StateID = 2},
        '        .AccessID = recharge.AccessID,
        '        .Amount = 7,
        '        .Brand = New xBrand With {.BrandID = xBrand.Brands.ZETDCFees},
        '        .Discount = 0,
        '        .Mobile = recharge.Mobile,
        '        .RechargeDate = Date.Now}
        '    xRechargeAdapter.Save(rechargefee, SqlConn)
        '    Dim RechargePrepaidfee As xRechargePrepaid = CreateRechargeObject(rechargefee)
        '    RechargePrepaidfee.Narrative = recharge.RechargeID.ToString()
        '    RechargePrepaidfee.Reference = iRechargePrepaid.Reference
        '    xRechargePrepaidAdapter.Save(RechargePrepaidfee, SqlConn)

        'End If

        Return New ServiceRechargeResponse(iRechargePrepaid)
    End Function

    Private Shared Function HasTimeoutErrors(Response As String) As Boolean
        If Response.Contains("request channel timed out") Then Return True
        If Response.Contains("task was canceled") Then Return True
        Return False
    End Function

    Private Shared Sub SetRechargePrepaid(ByRef recharge As xRecharge, ByRef rechargePrepaid As xRechargePrepaid, numberOfDays As Integer, Response As PurchaseTokenResponse)
        rechargePrepaid.Narrative = $"{JsonConvert.SerializeObject(Response)}"
        rechargePrepaid.ReturnCode = Response.ReplyCode
        rechargePrepaid.Reference = recharge.ProductCode
        rechargePrepaid.FinalBalance = Response.FinalBalance
        rechargePrepaid.InitialBalance = Response.InitialBalance
        rechargePrepaid.FinalWallet = Response.FinalBalance
        rechargePrepaid.InitialWallet = Response.InitialBalance
        rechargePrepaid.Window = Date.Now
        rechargePrepaid.DebitCredit = True
        rechargePrepaid.RechargeID = recharge.RechargeID
        rechargePrepaid.Window = DateAdd(DateInterval.Day, numberOfDays, Date.Now)
        recharge.State.StateID = IIf(Response.ReplyCode = xState.States.Success, xState.States.Success,
                                         xState.States.Failure)
        If Response.ReplyCode = 312 Then recharge.State.StateID = xState.States.PendingVerification

        recharge.RechargeDate = Now
        rechargePrepaid.Data = 0
    End Sub



    Public Function CustomerInfoRequest(rechargePrepaid As xRechargePrepaid, recharge As xRecharge) As CustomerInfo
        Dim request As New GetCustomerInfoRequest With {
                .Body = New GetCustomerInfoRequestBody With {
                .AppKey = HotAPIKey,
                .MeterNumber = recharge.Mobile
            }}
        Dim netowrkclient = GetNetworkClient()

        Try
            Console.WriteLine("Querying")
            Dim client = netowrkclient.GetNetwork()
            Dim response = client.GetCustomerInfo(request).Body.GetCustomerInfoResult

            xLog_Data.WriteJsonToConsole(response)
            netowrkclient.Close()

            Console.WriteLine("Response from ZESA " & response.ToString)
            rechargePrepaid.ReturnCode = response.ReplyCode
            rechargePrepaid.Reference = GetReference(request.Body.MeterNumber)
            rechargePrepaid.FinalBalance = 0 ' response.MobileBalance
            rechargePrepaid.InitialBalance = 0 ' response.MobileBalance
            rechargePrepaid.Narrative = $"{response.ReplyMessage} - {JsonConvert.SerializeObject(response.CustomerInfo)}"

            rechargePrepaid.DebitCredit = True
            rechargePrepaid.RechargeID = recharge.RechargeID
            recharge.State.StateID = IIf(response.ReplyCode = xState.States.Success, xState.States.Success,
                                             xState.States.Failure)
            recharge.RechargeDate = Now
            Return response.CustomerInfo
        Catch ex As Exception
            netowrkclient.Abort()
            Console.WriteLine("Balance Exception")
            xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
            recharge.State.StateID = xState.States.Failure
            rechargePrepaid.Narrative = "ZESAAPI Error: " & ex.Message

            Dim template = xTemplateAdapter.SelectRow(xTemplate.Templates.NetworkWebserviceUnavailable, SqlConn)
            Dim text = template.TemplateText.Replace("%NETWORK%", Name)
            Throw New HotRequestException(xTemplate.Templates.NetworkWebserviceUnavailable, text)
        End Try
    End Function

    Public Overrides Function GetNetworkClient(Optional timeout As Integer = 30000) As INetworkClient(Of ZESASoap)
        If IsTestMode Then
            Return GetTestClient()
        End If
        Return NetworkClientFactory.Create(Of ZESASoap)(ServiceEndpoint, timeout)
    End Function

    Private Function GetTestClient() As INetworkClient(Of ZESASoap)
        Dim netone As ZESASoap = Substitute.For(Of ZESASoap)
        ' Change Response Code to anything else but 1 to test failure
        Dim response = New PurchaseZESATokenResponse() _
                    With {
                        .Body = New PurchaseZESATokenResponseBody() With {
                             .PurchaseZESATokenResult = New PurchaseTokenResponse() With {
                                .ReplyCode = 2,
                                .ReplyMessage = "Narrative test",
                                .Reference = "Tes Ref",
                                .PurchaseToken = New PurchaseToken() With {
                                .Amount = 1,
                                .Tokens = New List(Of TokenItem)
                                }
                            }
                        }
                    }
        netone.PurchaseZESAToken(Arg.Any(Of PurchaseZESATokenRequest)).Returns(response)
        Dim fakeFactory = Substitute.For(Of INetworkClient(Of ZESASoap))()
        fakeFactory.GetNetwork().Returns(netone)
        Return fakeFactory
    End Function

    Protected Overrides Function Name() As String
        Return "ZESA"
    End Function

End Class

