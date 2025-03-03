Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Core.ServiceWrappers
Imports Hot.Core.TelOneAPI
Imports Hot.Data
Imports Newtonsoft.Json
Imports NSubstitute

Namespace Brands
    Public Class Telone
        Inherits NetworkBase(Of TeloneSoap)

        Private Const HotAPIKey As String = "Hot263180"
        Private Const TimeoutPeriod As Integer = 180000

        Public Sub New(sqlConn As SqlConnection, applicationName As String, serviceEndpoint As String, isTestMode As Boolean, referencePrefix As String, webServiceOrCorporate As Boolean)
            MyBase.New(sqlConn, applicationName, serviceEndpoint, isTestMode, webServiceOrCorporate, referencePrefix)
        End Sub

        Public Overrides Function RechargePrepaid(recharge As xRecharge, Optional numberOfDays As Integer = 90) As ServiceRechargeResponse
            ApplyDiscountRules(recharge)
            Dim iRechargePrepaid As xRechargePrepaid = CreateRechargeObject(recharge)
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)

            If iRechargePrepaid.DebitCredit Then
                CreditSubscriber(recharge, iRechargePrepaid, numberOfDays)
            Else
                'Debit not possible on Telone 
                iRechargePrepaid.Narrative = "Bundle Debit not possible"
                iRechargePrepaid.ReturnCode = "099"
                recharge.State.StateID = xState.States.Failure
            End If
            Console.WriteLine("Saving prepaid")
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)
            Console.WriteLine("Saving Recharge")
            xRechargeAdapter.Save(recharge, SqlConn)
            Return New ServiceRechargeResponse(iRechargePrepaid)
        End Function

        Public Function BulkRechargePrepaid(recharge As xRecharge, Optional numberOfDays As Integer = 90) As ServiceRechargeResponse
            ApplyDiscountRules(recharge)
            Dim iRechargePrepaid As xRechargePrepaid = CreateRechargeObject(recharge)
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)

            If iRechargePrepaid.DebitCredit Then
                BulkAdslProductRecharge(recharge, iRechargePrepaid, numberOfDays)
            Else
                'Debit not possible on Telone 
                iRechargePrepaid.Narrative = "Bundle Debit not possible"
                iRechargePrepaid.ReturnCode = "099"
                recharge.State.StateID = xState.States.Failure
            End If
            Console.WriteLine("Saving prepaid")
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)
            Console.WriteLine("Saving Recharge")
            xRechargeAdapter.Save(recharge, SqlConn)
            Return New ServiceRechargeResponse(iRechargePrepaid)
        End Function


        Private Sub CreditSubscriber(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer)
            Dim client = GetNetworkClient(TimeoutPeriod)
            Try
                Console.WriteLine("Crediting")
                Dim serviceClient = client.GetNetwork()
                Dim Response As RechargeAccountResponse
                If recharge.Brand.BrandID = 43 Then
                    Response = serviceClient.RechargeAccountAdslUSD(HotAPIKey, recharge.Mobile, recharge.Denomination, recharge.RechargeID)
                Else
                    Response = serviceClient.RechargeAccountAdsl(HotAPIKey, recharge.Mobile, recharge.Denomination, recharge.RechargeID)
                End If

                xLog_Data.WriteJsonToConsole(Response)

                Console.WriteLine("Response from Telone " & Response.ToString)
                If Not HasTimeoutErrors(Response.ReplyMessage) Then
                    SetRechargePrepaid(recharge, rechargePrepaid, numberOfDays, Response)

                Else
                    'QueryRecharge(recharge, rechargePrepaid, numberOfDays, client)hriow  
                    Throw New Exception(Response.ReplyMessage)
                End If
                If recharge.IsSuccessFul() Then
                    Dim list = New List(Of Voucher)
                    list.AddRange(Response.Result.Voucher)
                    SavePins(list, recharge, SqlConn.ConnectionString)
                End If
            Catch ex As Exception

                client.Abort()
                Console.WriteLine("Credit Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                                   SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
                recharge.State.StateID = xState.States.Failure
                rechargePrepaid.Narrative = "TelOneAPI Error: " & ex.Message

            Finally
                client.Close()
            End Try
        End Sub

        Private Sub BulkAdslProductRecharge(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer)
            'Dim creditRequest = CreateBulkBroadbandRequest(recharge, rechargePrepaid, numberOfDays)
            Dim client = GetNetworkClient(TimeoutPeriod)
            Try
                Console.WriteLine("Crediting")
                Dim serviceClient = client.GetNetwork()
                Dim Response = serviceClient.BulkAdslPurchaseBroadband(HotAPIKey, recharge.Denomination, recharge.Quantity, recharge.RechargeID)
                xLog_Data.WriteJsonToConsole(Response)

                Console.WriteLine("Response from Telone " & Response.ToString)
                If Not HasTimeoutErrors(Response.ReplyMessage) Then
                    SetRechargePrepaid(recharge, rechargePrepaid, numberOfDays, Response)

                Else
                    'QueryRecharge(recharge, rechargePrepaid, numberOfDays, client)hriow  
                    Throw New Exception(Response.ReplyMessage)
                End If
                If recharge.IsSuccessFul() Then
                    SavePins(Response.Result.Vouchers.ToList(), recharge, SqlConn.ConnectionString)
                End If
            Catch ex As Exception

                client.Abort()
                Console.WriteLine("Credit Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                                   SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
                recharge.State.StateID = xState.States.Failure
                rechargePrepaid.Narrative = "TelOneAPI Error: " & ex.Message

            Finally
                client.Close()
            End Try
        End Sub


        Private Shared Function HasTimeoutErrors(Response As String) As Boolean
            If Response.Contains("request channel timed out") Then Return True
            If Response.Contains("task was canceled") Then Return True
            Return False
        End Function

        Private Shared Sub SetRechargePrepaid(ByRef recharge As xRecharge, ByRef rechargePrepaid As xRechargePrepaid,
                                              numberOfDays As Integer, Response As RechargeAccountResponse)
            rechargePrepaid.Narrative = Response.ReplyMessage + " Raw:" + JsonConvert.SerializeObject(Response.Result)
            rechargePrepaid.ReturnCode = Response.ReplyCode
            'rechargePrepaid.Reference = response.AgentReference
            rechargePrepaid.FinalBalance = 0
            rechargePrepaid.InitialBalance = 0
            rechargePrepaid.FinalWallet = Response.Result.Balance ' Response.WalletBalance

            rechargePrepaid.Window = Date.Now
            rechargePrepaid.DebitCredit = True
            rechargePrepaid.RechargeID = recharge.RechargeID
            rechargePrepaid.Window = DateAdd(DateInterval.Day, numberOfDays, Date.Now)
            recharge.State.StateID = IIf(Response.ReplyCode = xState.States.Success, xState.States.Success,
                                         xState.States.Failure)
            rechargePrepaid.InitialWallet = Response.Result.Balance + IIf(recharge.IsSuccessFul, recharge.Amount, 0) ' Response.WalletBalance

            recharge.RechargeDate = Now
            rechargePrepaid.Data = 0
        End Sub

        Private Shared Sub SetRechargePrepaid(ByRef recharge As xRecharge, ByRef rechargePrepaid As xRechargePrepaid,
                                              numberOfDays As Integer, Response As BulkPurchaseBroadbandResponse)
            rechargePrepaid.Narrative = Response.ReplyMessage + " Raw:" + JsonConvert.SerializeObject(Response.Result)
            rechargePrepaid.ReturnCode = Response.ReplyCode
            'rechargePrepaid.Reference = response.AgentReference
            rechargePrepaid.FinalBalance = 0
            rechargePrepaid.InitialBalance = 0
            rechargePrepaid.FinalWallet = Response.Result.Balance ' Response.WalletBalance

            rechargePrepaid.Window = Date.Now
            rechargePrepaid.DebitCredit = True
            rechargePrepaid.RechargeID = recharge.RechargeID
            rechargePrepaid.Window = DateAdd(DateInterval.Day, numberOfDays, Date.Now)
            recharge.State.StateID = IIf(Response.ReplyCode = xState.States.Success, xState.States.Success,
                                         xState.States.Failure)
            recharge.RechargeDate = Now
            rechargePrepaid.Data = 0
        End Sub


        'Private Function CreateCreditRequest(recharge As xRecharge, iRechargePrepaid As xRechargePrepaid,
        '                                     numberOfDays As Integer) As RechargeAccountAdslRequest
        '    Return New RechargeAccountAdslRequest With {
        '        .Body = New RechargeAccountAdslRequestBody With {
        '        .RechargeId = recharge.RechargeID,
        '        .AccountId = recharge.Mobile,
        '        .productId = recharge.Denomination,
        '        .AppKey = HotAPIKey
        '    }}
        'End Function

        'Private Function CreateBulkBroadbandRequest(recharge As xRecharge, iRechargePrepaid As xRechargePrepaid,
        '                                     numberOfDays As Integer) As BulkAdslPurchaseBroadbandRequest
        '    Return New BulkAdslPurchaseBroadbandRequest With {
        '        .Body = New BulkAdslPurchaseBroadbandRequestBody With {
        '        .RechargeId = recharge.RechargeID,
        '        .Quantity = recharge.Quantity,
        '        .ProductId = recharge.Denomination,
        '        .AppKey = HotAPIKey
        '    }}
        'End Function

        Public Function VerifyAccount(AccountId As String) As CustomerAccountResponse

            'Dim request = New VerifiyUserAccountRequest(New VerifiyUserAccountRequestBody(HotAPIKey, AccountId))
            Dim client = GetNetworkClient(TimeoutPeriod)
            Try
                Dim serviceClient = client.GetNetwork()
                Dim Response = serviceClient.VerifiyUserAccount(HotAPIKey, AccountId)

                If Response.ReplyCode = xState.States.Success Then Return Response.Result
                Return New CustomerAccountResponse() With {
                    .AccountNumber = AccountId,
                    .ResponseCode = Response.ReplyCode.ToString(),
                    .AccountName = "Not Found"
                    }
            Catch ex As Exception

                client.Abort()
                Console.WriteLine("Credit Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", 0)
                Return Nothing
            Finally
                client.Close()
            End Try
        End Function

        Public Function GetEndUserBalance(AccountId As String) As Decimal

            'Dim request = New VerifiyUserAccountRequest(New VerifiyUserAccountRequestBody(HotAPIKey, AccountId))
            Dim client = GetNetworkClient(TimeoutPeriod)
            Try
                Dim serviceClient = client.GetNetwork()
                Dim Response = serviceClient.EndUserVoiceBalance(HotAPIKey, AccountId)

                If Response.ReplyCode = xState.States.Success Then Return Response.Balance
                Return -1

            Catch ex As Exception

                client.Abort()
                Console.WriteLine("Credit Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", 0)
                Return Nothing
            Finally
                client.Close()
            End Try
        End Function

        Public Function QueryEVDStock(Optional IsUsd As Boolean = False) As List(Of BroadbandProductItem)
            'Dim request = New GetAvailableBundlesRequest()
            Dim client = GetNetworkClient(TimeoutPeriod)
            Try
                Dim serviceClient = client.GetNetwork()

                Dim Response = If(IsUsd, serviceClient.GetAvailableBundlesUSD(HotAPIKey), serviceClient.GetAvailableBundles(HotAPIKey))
                Dim items = New List(Of BroadbandProductItem)

                If Response.ReplyCode = xState.States.Success Then items = Response.List.ToList()

                Return items

            Catch ex As Exception

                client.Abort()
                Console.WriteLine("Credit Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", 0)
                Return Nothing
            Finally
                client.Close()
            End Try
        End Function


        Public Function PayAccountBill(recharge As xRecharge, Optional numberOfDays As Integer = 90) As ServiceRechargeResponse
            ApplyDiscountRules(recharge)
            Dim iRechargePrepaid As xRechargePrepaid = CreateRechargeObject(recharge)
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)

            If iRechargePrepaid.DebitCredit Then
                PayBill(recharge, iRechargePrepaid, numberOfDays)
            Else
                'Debit not possible on Telone 
                iRechargePrepaid.Narrative = "Bundle Debit not possible"
                iRechargePrepaid.ReturnCode = "099"
                recharge.State.StateID = xState.States.Failure
            End If
            Console.WriteLine("Saving prepaid")
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)
            Console.WriteLine("Saving Recharge")
            xRechargeAdapter.Save(recharge, SqlConn)
            Return New ServiceRechargeResponse(iRechargePrepaid)
        End Function

        Private Function PayBill(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer) As ServiceRechargeResponse
            'Dim creditRequest = CreateBulkBroadbandRequest(recharge, rechargePrepaid, numberOfDays)
            Dim client = GetNetworkClient(TimeoutPeriod)
            Try
                Console.WriteLine("Crediting")
                Dim serviceClient = client.GetNetwork()
                Dim Response = serviceClient.PayBill(HotAPIKey, recharge.Mobile, recharge.Amount, recharge.RechargeID)
                xLog_Data.WriteJsonToConsole(Response)

                Console.WriteLine("Response from Telone " & Response.ToString)
                If Not HasTimeoutErrors(Response.ReplyMessage) Then
                    SetRechargePrepaid(recharge, rechargePrepaid, numberOfDays, Response)

                Else
                    'QueryRecharge(recharge, rechargePrepaid, numberOfDays, client)hriow  
                    Throw New Exception(Response.ReplyMessage)
                End If

            Catch ex As Exception

                client.Abort()
                Console.WriteLine("Credit Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                                   SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
                recharge.State.StateID = xState.States.Failure
                rechargePrepaid.Narrative = "TelOneAPI Error: " & ex.Message

            Finally
                client.Close()
            End Try
        End Function

        Private Sub SetRechargePrepaid(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer, response As PayAccountBillResponse)
            rechargePrepaid.Narrative = response.ReplyMessage + " Raw:" + JsonConvert.SerializeObject(response.Result)
            rechargePrepaid.ReturnCode = response.ReplyCode
            'rechargePrepaid.Reference = response.AgentReference
            rechargePrepaid.FinalBalance = 0
            rechargePrepaid.InitialBalance = 0
            rechargePrepaid.FinalWallet = response.Result.Balance ' Response.WalletBalance

            rechargePrepaid.Window = Date.Now
            rechargePrepaid.DebitCredit = True
            rechargePrepaid.RechargeID = recharge.RechargeID
            rechargePrepaid.Window = DateAdd(DateInterval.Day, numberOfDays, Date.Now)
            recharge.State.StateID = IIf(response.ReplyCode = xState.States.Success, xState.States.Success,
                                         xState.States.Failure)
            recharge.RechargeDate = Now
            rechargePrepaid.Data = 0
        End Sub


        Public Function RechargeVoipAccount(recharge As xRecharge, Optional numberOfDays As Integer = 90) As ServiceRechargeResponse
            ApplyDiscountRules(recharge)
            Dim iRechargePrepaid As xRechargePrepaid = CreateRechargeObject(recharge)
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)

            If iRechargePrepaid.DebitCredit Then
                RechargeVoip(recharge, iRechargePrepaid, numberOfDays)
            Else
                'Debit not possible on Telone 
                iRechargePrepaid.Narrative = "Bundle Debit not possible"
                iRechargePrepaid.ReturnCode = "099"
                recharge.State.StateID = xState.States.Failure
            End If
            Console.WriteLine("Saving prepaid")
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)
            Console.WriteLine("Saving Recharge")
            xRechargeAdapter.Save(recharge, SqlConn)
            Return New ServiceRechargeResponse(iRechargePrepaid)
        End Function

        Private Function RechargeVoip(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer) As ServiceRechargeResponse
            'Dim creditRequest = CreateBulkBroadbandRequest(recharge, rechargePrepaid, numberOfDays)
            Dim client = GetNetworkClient(TimeoutPeriod)
            Try
                Console.WriteLine("Crediting")
                Dim serviceClient = client.GetNetwork()
                Dim Response = serviceClient.RechargeAccountVoip(HotAPIKey, recharge.Mobile, recharge.Amount, recharge.RechargeID)
                xLog_Data.WriteJsonToConsole(Response)

                Console.WriteLine("Response from Telone " & Response.ToString)
                If Not HasTimeoutErrors(Response.ReplyMessage) Then
                    SetRechargePrepaid(recharge, rechargePrepaid, numberOfDays, Response)
                Else
                    'QueryRecharge(recharge, rechargePrepaid, numberOfDays, client)hriow  
                    Throw New Exception(Response.ReplyMessage)
                End If

            Catch ex As Exception

                client.Abort()
                Console.WriteLine("Credit Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                                   SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
                recharge.State.StateID = xState.States.Failure
                rechargePrepaid.Narrative = "TelOneAPI Error: " & ex.Message

            Finally
                client.Close()
            End Try
        End Function

        Private Sub SetRechargePrepaid(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer, response As RechargeVoipResponse)
            rechargePrepaid.Narrative = response.ReplyMessage + " Raw:" + JsonConvert.SerializeObject(response.Result)
            rechargePrepaid.ReturnCode = response.ReplyCode
            'rechargePrepaid.Reference = response.AgentReference
            rechargePrepaid.FinalBalance = 0
            rechargePrepaid.InitialBalance = 0
            rechargePrepaid.FinalWallet = response.Result.Balance ' Response.WalletBalance

            rechargePrepaid.Window = Date.Now
            rechargePrepaid.DebitCredit = True
            rechargePrepaid.RechargeID = recharge.RechargeID
            rechargePrepaid.Window = DateAdd(DateInterval.Day, numberOfDays, Date.Now)
            recharge.State.StateID = IIf(response.ReplyCode = xState.States.Success, xState.States.Success,
                                         xState.States.Failure)
            recharge.RechargeDate = Now
            rechargePrepaid.Data = 0
        End Sub




        Private Sub SavePins(pins As List(Of Voucher), recharge As xRecharge, connectionString As String)
            Using sqlConn As New SqlConnection(connectionString)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    'Batch
                    Dim iBatch As New xPinBatch With {
                        .PinBatch = $"TelOneAPI-PinBatch-{Date.Now:dd-MMM-yy}",
                        .PinBatchType = New xPinBatchType With {
                        .PinBatchTypeID = xPinBatchType.PinBatchTypes.TelOne
                    }
                    }
                    xPinBatchAdapter.Insert(iBatch, sqlConn, sqlTrans)
                    Dim iPins = New List(Of xPin)
                    For Each pin In pins
                        Dim iPin As xPin = GetPin(pin, recharge, iBatch)
                        If Not String.IsNullOrEmpty(iPin.Pin) Then
                            xPinAdapter.Insert(iPin, sqlConn, sqlTrans)
                            iPins.Add(iPin)
                        End If
                    Next
                    Dim RP = New DataTable()
                    RP.Columns.Add("RechargeID")
                    RP.Columns.Add("PinID")
                    For Each ipin In iPins
                        Dim dr = RP.NewRow()
                        dr("PinID") = ipin.PinID
                        dr("RechargeID") = recharge.RechargeID
                        RP.Rows.Add(dr)
                    Next

                    Dim objbulk = New SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, sqlTrans)
                    objbulk.DestinationTableName = "tblRechargePin"
                    objbulk.ColumnMappings.Add("RechargeID", "RechargeID")
                    objbulk.ColumnMappings.Add("PinID", "PinID")
                    objbulk.WriteToServer(RP)
                    sqlTrans.Commit()
                Catch ex As Exception
                    sqlTrans.Rollback()
                    'Throw ex
                Finally
                    sqlConn.Close()
                End Try
            End Using
        End Sub

        Private Function GetPin(body As Voucher, recharge As xRecharge, iBatch As xPinBatch) As xPin
            Dim iPin As New xPin
            Try
                ' Sample format - "0030855480903599,000309176729|58,1.00,3/5/2022"
                iPin.Brand = New xBrand
                iPin.Brand = recharge.Brand

                iPin.Pin = body.Pin
                iPin.PinRef = $"{body.CardNumber}|{body.SerialNumber}|{body.BatchNumber}"
                iPin.PinBatch = iBatch
                iPin.PinExpiry = Date.Now.AddYears(1)
                iPin.PinState = New xPinState
                iPin.PinState.PinStateID = xPinState.PinStates.SoldFileExport
                iPin.PinValue = recharge.Amount
            Catch ex As Exception
                Return New xPin()
            End Try 'Brand

            Return iPin
        End Function



        Public Overrides Function GetNetworkClient(Optional timeout As Integer = 30000) As INetworkClient(Of TeloneSoap)
            If IsTestMode Then
                Return GetTestClient()
            End If
            Return NetworkClientFactory.Create(Of TeloneSoap)(ServiceEndpoint, timeout)
        End Function

        Private Function GetTestClient() As INetworkClient(Of TeloneSoap)
            Dim client As TeloneSoap = Substitute.For(Of TeloneSoap)
            ' Change Response Code to anything else but 1 to test failure
            Dim response = New TelOneAPI.RechargeAccountResponse() _
                    With {
                                .ReplyCode = 2,
                                .ReplyMessage = "Narrative test",
                                .Reference = "hjkk"
                        }
            client.RechargeAccountAdsl(Arg.Any(Of String), Arg.Any(Of String), Arg.Any(Of Integer), Arg.Any(Of Integer)).Returns(response)
            Dim fakeFactory = Substitute.For(Of INetworkClient(Of TeloneSoap))()
            fakeFactory.GetNetwork().Returns(client)
            Return fakeFactory
        End Function

        Protected Overrides Function Name() As String
            Return "TelOne"
        End Function
    End Class

End Namespace

