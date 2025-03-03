Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Core.NetoneAPI
Imports Hot.Core.ServiceWrappers
Imports Hot.Data
Imports Newtonsoft.Json
Imports NSubstitute

Namespace Brands

    Public Class NetOne
        Inherits NetworkBase(Of NetoneSoap)

        Private Const HotAPIKey As String = "Hot263180"
        Private Const TimeoutPeriod As Integer = 180000

        Public Sub New(sqlConn As SqlConnection, applicationName As String, serviceEndpoint As String, isTestMode As Boolean, referencePrefix As String, webServiceOrCorporate As Boolean)
            MyBase.New(sqlConn, applicationName, serviceEndpoint, isTestMode, webServiceOrCorporate, referencePrefix)
        End Sub

        'Public Overrides Function RechargePrepaid(recharge As xRecharge, Optional numberOfDays As Integer = 90) As ServiceRechargeResponse
        '    ApplyDiscountRules(recharge)
        '    'Change discounts for small amounts

        '    Dim iRechargePrepaid = CreateRechargeObject(recharge)
        '        xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)
        '        Try
        '            Dim client = GetNetworkClient(3500)
        '            Dim netone = client.GetNetwork()
        '            If recharge.Amount >= 0 Then
        '                'Execute Credit
        '                Try
        '                    Dim request = new RechargeRequest()
        '                    request.Body = New RechargeRequestBody()
        '                    request.Body.TargetMobile = recharge.Mobile
        '                    request.Body.Amount = recharge.Amount
        '                    request.Body.AppKey = "Hot263180"

        '                    Dim response As ReturnObject = netone.Recharge(request).Body.RechargeResult
        '                    ' Throw New Exception ("It failed.")
        '                    xLog_Data.WriteJsonToConsole(response)
        '                    iRechargePrepaid.FinalBalance = 0
        '                    iRechargePrepaid.InitialBalance = 0

        '                    iRechargePrepaid.Narrative = response.ReturnMsg
        '                    iRechargePrepaid.ReturnCode = response.ReturnCode
        '                    If iRechargePrepaid.ReturnCode = 1 Then
        '                        recharge.State.StateID = xState.States.Success
        '                    Else
        '                        recharge.State.StateID = xState.States.Failure
        '                    End If
        '                Catch ex As Exception
        '                    recharge.State.StateID = xState.States.Failure
        '                    iRechargePrepaid.Narrative = "Exception: NetOneDongleWebservice: " & ex.ToString
        '                End Try
        '            Else
        '                'Debit not possible on NetOne
        '                iRechargePrepaid.Narrative = "NetOne Debit not possible"
        '                iRechargePrepaid.ReturnCode = "099"
        '                recharge.State.StateID = xState.States.Failure
        '            End If
        '        Catch ex As Exception
        '            'Just set StateID to a failure
        '            recharge.State.StateID = xState.States.Failure
        '            iRechargePrepaid.Narrative = "Exception: " & ex.ToString
        '        End Try

        '        xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)
        '        recharge.RechargeDate = Now
        '        xRechargeAdapter.Save(recharge, SqlConn)
        '       Return New ServiceRechargeResponse(iRechargePrepaid)
        'End Function


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
            Return New ServiceRechargeResponse(iRechargePrepaid)
        End Function

        Private Function CreateCreditRequest(recharge As xRecharge, iRechargePrepaid As xRechargePrepaid,
                                             numberOfDays As Integer) As RechargeMobileRequest
            Return New RechargeMobileRequest With {
                .Body = New RechargeMobileRequestBody With {
                .RechargeID = recharge.RechargeID,
                .Amount = recharge.Amount,
                .TargetMobile = recharge.Mobile,
                .AppKey = HotAPIKey
            }}
        End Function
        Private Function CreateCreditRequestUSD(recharge As xRecharge, iRechargePrepaid As xRechargePrepaid,
                                             numberOfDays As Integer) As RechargeMobileUSDRequest
            Return New RechargeMobileUSDRequest With {
                .Body = New RechargeMobileUSDRequestBody With {
                .RechargeID = recharge.RechargeID,
                .Amount = recharge.Amount,
                .TargetMobile = recharge.Mobile,
                .AppKey = HotAPIKey
            }}
        End Function

        Private Sub CreditSubscriber(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer)

            Dim client = GetNetworkClient(TimeoutPeriod)
            Try
                Console.WriteLine("Crediting")
                Dim netoneClient = client.GetNetwork()
                Dim Response As New NetoneAPI.RechargeResponse
                Dim BundledReponse As New NetoneAPI.RechargeBundledResponse
                If recharge.Brand.WalletTypeId = 3 Then
                    Dim Result = netoneClient.RechargeMobileUSD(CreateCreditRequestUSD(recharge, rechargePrepaid, numberOfDays)).Body.RechargeMobileUSDResult
                    Response = New NetoneAPI.RechargeResponse() With {
                        .AgentReference = Result.AgentReference,
                        .Amount = Result.Amount,
                        .Data = Result.Data,
                        .FinalBalance = Result.FinalBalance,
                        .InitialBalance = Result.InitialBalance,
                        .RechargeID = Result.RechargeID,
                        .ReplyCode = Result.ReplyCode,
                        .ReplyMessage = Result.ReplyMessage,
                        .ReplyMsg = Result.ReplyMsg,
                        .SMS = Result.SMS,
                        .WalletBalance = Result.WalletBalance,
                        .Window = Result.Window
                        }
                    BundledReponse = Result
                Else
                    Response = netoneClient.RechargeMobile(CreateCreditRequest(recharge, rechargePrepaid, numberOfDays)).Body.RechargeMobileResult
                End If
                xLog_Data.WriteJsonToConsole(Response)

                Console.WriteLine("Response from Netone " & Response.ToString)
                If Not HasTimeoutErrors(Response.ReplyMessage + Response.ReplyMsg) Then
                    SetRechargePrepaid(recharge, rechargePrepaid, numberOfDays, Response, BundledReponse)
                Else
                    QueryRecharge(recharge, rechargePrepaid, numberOfDays, client)
                End If

            Catch ex As Exception
                If HasTimeoutErrors(ex.Message) Then
                    QueryRecharge(recharge, rechargePrepaid, numberOfDays, client)
                Else
                    client.Abort()
                    Console.WriteLine("Credit Exception")
                    xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                                   SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
                    recharge.State.StateID = xState.States.Failure
                    rechargePrepaid.Narrative = "NetoneAPI Error: " & ex.Message
                End If
            Finally
                client.Close()
            End Try
        End Sub

        Private Shared Function HasTimeoutErrors(Response As String) As Boolean
            If Response.Contains("request channel timed out") Then Return True
            If Response.Contains("task was canceled") Then Return True
            Return False
        End Function

        Private Shared Sub QueryRecharge(ByRef recharge As xRecharge, ByRef rechargePrepaid As xRechargePrepaid, numberOfDays As Integer, client As INetworkClient(Of NetoneSoap))
            Dim queryClient = client.GetNetwork()
            Dim Response = queryClient.QueryRecharge(New QueryRechargeRequest() With {
                                                          .Body = New QueryRechargeRequestBody() With {
                                                          .AgentReference = "Hot-" + recharge.RechargeID,
                                                          .AppKey = HotAPIKey
                                                          }
                                                     }).Body.QueryRechargeResult
            SetRechargePrepaid(recharge, rechargePrepaid, numberOfDays, Response)
        End Sub

        Private Shared Sub SetRechargePrepaid(ByRef recharge As xRecharge, ByRef rechargePrepaid As xRechargePrepaid, numberOfDays As Integer, Response As NetoneAPI.RechargeResponse, Optional BundledResponse As RechargeBundledResponse = Nothing)
            rechargePrepaid.Narrative = Response.ReplyMessage + Response.ReplyMsg
            rechargePrepaid.ReturnCode = Response.ReplyCode
            rechargePrepaid.Reference = Response.AgentReference
            'rechargePrepaid.Data = JsonConvert.SerializeObject(BundledResponse)
            rechargePrepaid.FinalBalance = Response.FinalBalance
            rechargePrepaid.InitialBalance = Response.InitialBalance
            rechargePrepaid.FinalWallet = Response.WalletBalance

            rechargePrepaid.Window = Response.Window
            rechargePrepaid.DebitCredit = True
            rechargePrepaid.RechargeID = recharge.RechargeID
            rechargePrepaid.Window = DateAdd(DateInterval.Day, numberOfDays, Date.Now)
            recharge.State.StateID = IIf(Response.ReplyCode = xState.States.Success, xState.States.Success,
                                         xState.States.Failure)
            recharge.RechargeDate = Now
        End Sub

        Public Function BalanceRequest(rechargePrepaid As xRechargePrepaid, recharge As xRecharge) As Balance
            Dim request As New UserBalanceRequest With {
                .Body = New UserBalanceRequestBody With {
                .AppKey = HotAPIKey,
                .TargetMobile = recharge.Mobile
            }}
            Dim client = GetNetworkClient()

            Try
                Console.WriteLine("Querying")
                Dim netoneClient = client.GetNetwork()
                Dim response = netoneClient.UserBalance(request).Body.UserBalanceResult

                xLog_Data.WriteJsonToConsole(response)
                client.Close()


                Console.WriteLine("Response from Netone " & response.ToString)
                rechargePrepaid.ReturnCode = response.ReplyCode
                rechargePrepaid.Reference = GetReference(request.Body.TargetMobile)
                rechargePrepaid.FinalBalance = response.MobileBalance
                rechargePrepaid.InitialBalance = response.MobileBalance
                rechargePrepaid.Narrative = response.ReplyMsg & ", Expiry: " & response.WindowPeriod

                rechargePrepaid.DebitCredit = True
                rechargePrepaid.RechargeID = recharge.RechargeID
                recharge.State.StateID = IIf(response.ReplyCode = xState.States.Success, xState.States.Success,
                                             xState.States.Failure)
                recharge.RechargeDate = Now
                Return New Balance() With {
                    .IsSuccessful = response.ReplyCode = xState.States.Success,
                    .CurrentBalance = response.MobileBalance,
                    .ExpiryDate = response.WindowPeriod
                }
            Catch ex As Exception
                client.Abort()
                Console.WriteLine("Balance Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
                recharge.State.StateID = xState.States.Failure
                rechargePrepaid.Narrative = "NetoneAPI Error: " & ex.Message

                Dim template = xTemplateAdapter.SelectRow(xTemplate.Templates.NetworkWebserviceUnavailable, SqlConn)
                Dim text = template.TemplateText.Replace("%NETWORK%", Name)
                Throw New HotRequestException(xTemplate.Templates.NetworkWebserviceUnavailable, text)
            End Try
        End Function

        Public Overrides Function GetNetworkClient(Optional timeout As Integer = 30000) As INetworkClient(Of NetoneSoap)
            If IsTestMode Then
                Return GetTestClient()
            End If
            Return NetworkClientFactory.Create(Of NetoneSoap)(ServiceEndpoint, timeout)
        End Function

        Private Function GetTestClient() As INetworkClient(Of NetoneSoap)
            Dim netone As NetoneSoap = Substitute.For(Of NetoneSoap)
            ' Change Response Code to anything else but 1 to test failure
            Dim response = New NetoneAPI.RechargeMobileResponse() _
                    With {
                        .Body = New RechargeMobileResponseBody() With {
                            .RechargeMobileResult = New NetoneAPI.RechargeResponse() With {
                                .ReplyCode = 2,
                                .ReplyMessage = "Narrative test",
                                .InitialBalance = 1,
                                .FinalBalance = 1,
                                .Amount = 1,
                                .RechargeID = 1000000,
                                .ReplyMsg = "Test",
                                .AgentReference = "hjkk"
                            }
                        }
                    }
            netone.RechargeMobile(Arg.Any(Of RechargeMobileRequest)).Returns(response)
            Dim fakeFactory = Substitute.For(Of INetworkClient(Of NetoneSoap))()
            fakeFactory.GetNetwork().Returns(netone)
            Return fakeFactory
        End Function

        Protected Overrides Function Name() As String
            Return "NetOne"
        End Function
    End Class
End Namespace