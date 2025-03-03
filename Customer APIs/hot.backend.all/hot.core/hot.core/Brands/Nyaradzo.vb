Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Core.Brands
Imports Hot.Core.ServiceWrappers
Imports Hot.Data
Imports NSubstitute
Imports System.Text
Imports Newtonsoft.Json
Imports Hot.Core.NyaradzoAPI

Public Class Nyaradzo
    Inherits NetworkBase(Of NyaradzoSoap)


    Private Const HotAPIKey As String = "Hot263180"
    Private Const TimeoutPeriod As Integer = 180000

    Public Sub New(sqlConn As SqlConnection, applicationName As String, serviceEndpoint As String, isTestMode As Boolean, referencePrefix As String, webServiceOrCorporate As Boolean)
        MyBase.New(sqlConn, applicationName, serviceEndpoint, isTestMode, webServiceOrCorporate, referencePrefix)
    End Sub


    Private Sub CreditSubscriber(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer)
        Dim creditRequest As ProcessPaymentRequest = CreateCreditRequest(recharge, rechargePrepaid, numberOfDays)
        Dim networkClient = GetNetworkClient(TimeoutPeriod)
        Try
            Console.WriteLine("Crediting")
            Dim client = networkClient.GetNetwork()
            Dim Response = client.ProcessPayment(creditRequest).Body.ProcessPaymentResult
            xLog_Data.WriteJsonToConsole(Response)

            Console.WriteLine("Response from Nyaradzo " & Response.ToString)
            If Not HasTimeoutErrors(Response.ResponseData) Then
                SetRechargePrepaid(recharge, rechargePrepaid, numberOfDays, Response)

            Else
                'QueryRecharge(recharge, rechargePrepaid, numberOfDays, client)hriow 

                Throw New Exception(Response.ResponseData)
            End If

        Catch ex As Exception

            networkClient.Abort()
            Console.WriteLine("Credit Exception")
            xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
            recharge.State.StateID = xState.States.Failure
            rechargePrepaid.Narrative = "NyaradzoAPI Error: " & ex.Message

        Finally
            networkClient.Close()
        End Try
    End Sub


    Private Function CreateCreditRequest(recharge As xRecharge, iRechargePrepaid As xRechargePrepaid,
                                             numberOfDays As Integer) As ProcessPaymentRequest
        Return New ProcessPaymentRequest With {
                .Body = New ProcessPaymentRequestBody With {
                .reference = recharge.RechargeID,
                .amount = recharge.Amount,
                .PolicyNumber = recharge.Mobile,
                .AppKey = HotAPIKey
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

        Return New ServiceRechargeResponse(iRechargePrepaid)
    End Function

    Private Shared Function HasTimeoutErrors(Response As String) As Boolean
        If Response Is Nothing Then Return False
        If Response.Contains("request channel timed out") Then Return True
        If Response.Contains("task was canceled") Then Return True
        Return False
    End Function

    Private Shared Sub SetRechargePrepaid(ByRef recharge As xRecharge, ByRef rechargePrepaid As xRechargePrepaid, numberOfDays As Integer, Response As NyaradzoResultModel)
        rechargePrepaid.Narrative = $"{If(Response.ValidResponse, Response.Result.Message, Response.ErrorData)}|{Response.ResponseData}"
        rechargePrepaid.ReturnCode = If(Response.ValidResponse, "2", "3")
        rechargePrepaid.Reference = recharge.RechargeID.ToString()
        rechargePrepaid.FinalBalance = If(Response.ValidResponse, Response.Item.Balance, 0)
        rechargePrepaid.InitialBalance = If(Response.ValidResponse, Response.Item.Balance, 0)
        rechargePrepaid.FinalWallet = 0 'Response.WalletBalance
        rechargePrepaid.InitialWallet = 0
        rechargePrepaid.Window = Date.Now
        rechargePrepaid.DebitCredit = True
        rechargePrepaid.RechargeID = recharge.RechargeID
        rechargePrepaid.Window = DateAdd(DateInterval.Day, numberOfDays, Date.Now)
        recharge.State.StateID = If(Response.ValidResponse, xState.States.Success, xState.States.Failure)

        recharge.RechargeDate = Now
        rechargePrepaid.Data = 0
    End Sub



    Public Function CustomerInfoRequest(rechargePrepaid As xRechargePrepaid, recharge As xRecharge) As NyaradzoResultModel
        Dim request As New AccountQueryRequest With {
                .Body = New AccountQueryRequestBody With {
                .AppKey = HotAPIKey,
                .PolicyNumber = recharge.Mobile
            }}
        Dim netowrkclient = GetNetworkClient()

        Try
            Console.WriteLine("Querying")
            Dim client = netowrkclient.GetNetwork()
            Dim response = client.AccountQuery(request).Body.AccountQueryResult

            xLog_Data.WriteJsonToConsole(response)
            netowrkclient.Close()

            Console.WriteLine("Response from Nyaradzo " & response.ToString)
            rechargePrepaid.ReturnCode = If(response.ValidResponse, "2", "3")
            rechargePrepaid.Reference = recharge.RechargeID
            rechargePrepaid.FinalBalance = 0 ' response.MobileBalance
            rechargePrepaid.InitialBalance = 0 ' response.MobileBalance
            rechargePrepaid.Narrative = $"{If(response.ValidResponse, response.Result.Message, response.ErrorData)}|{JsonConvert.SerializeObject(Response)}"

        rechargePrepaid.DebitCredit = True
            rechargePrepaid.RechargeID = recharge.RechargeID
            recharge.State.StateID = IIf(response.ValidResponse = xState.States.Success, xState.States.Success,
                                             xState.States.Failure)
            recharge.RechargeDate = Now
            Return response
        Catch ex As Exception
            netowrkclient.Abort()
            Console.WriteLine("Balance Exception")
            xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
            recharge.State.StateID = xState.States.Failure
            rechargePrepaid.Narrative = "NyaradzoAPI Error: " & ex.Message

            Dim template = xTemplateAdapter.SelectRow(xTemplate.Templates.NetworkWebserviceUnavailable, SqlConn)
            Dim text = template.TemplateText.Replace("%NETWORK%", Name)
            Throw New HotRequestException(xTemplate.Templates.NetworkWebserviceUnavailable, text)
        End Try
    End Function

    Public Overrides Function GetNetworkClient(Optional timeout As Integer = 30000) As INetworkClient(Of NyaradzoSoap)
        If IsTestMode Then
            Return GetTestClient()
        End If
        Return NetworkClientFactory.Create(Of NyaradzoSoap)(ServiceEndpoint, timeout)
    End Function

    Private Function GetTestClient() As INetworkClient(Of NyaradzoSoap)
        Dim netone As NyaradzoSoap = Substitute.For(Of NyaradzoSoap)
        '' Change Response Code to anything else but 1 to test failure
        'Dim response = New PurchaseZESATokenResponse() _
        '            With {
        '                .Body = New PurchaseZESATokenResponseBody() With {
        '                     .PurchaseZESATokenResult = New PurchaseTokenResponse() With {
        '                        .ReplyCode = 2,
        '                        .ReplyMessage = "Narrative test",
        '                        .Reference = "Tes Ref",
        '                        .PurchaseToken = New PurchaseToken() With {
        '                        .Amount = 1,
        '                        .Tokens = New List(Of TokenItem)
        '                        }
        '                    }
        '                }
        '            }
        'netone.PurchaseZESAToken(Arg.Any(Of PurchaseZESATokenRequest)).Returns(response)
        Dim fakeFactory = Substitute.For(Of INetworkClient(Of NyaradzoSoap))()
        fakeFactory.GetNetwork().Returns(netone)
        Return fakeFactory
    End Function

    Protected Overrides Function Name() As String
        Return "Nyaradzo"
    End Function

End Class


