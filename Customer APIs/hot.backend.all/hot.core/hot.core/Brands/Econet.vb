Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Core.EconetEPayAPI
Imports Hot.Core.ServiceWrappers
Imports Hot.Data
Imports NSubstitute

Namespace Brands
    Public Class Econet
        Inherits NetworkBase(Of EconetSoap)

        Private Const SuccessCode As String = "000"

        Public Sub New(sqlConn As SqlConnection, applicationName As String, serviceEndpoint As String,
                       isTestMode As Boolean, referencePrefix As String, webServiceOrCorporate As Boolean)
            MyBase.New(sqlConn, applicationName, serviceEndpoint, isTestMode, webServiceOrCorporate, referencePrefix:=referencePrefix)
        End Sub

        Public Overrides Function RechargePrepaid(recharge As xRecharge, Optional numberOfDays As Integer = 90) As ServiceRechargeResponse
            ApplyDiscountRules(recharge)
            Dim iRechargePrepaid As xRechargePrepaid = CreateRechargeObject(recharge)
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)

            If iRechargePrepaid.DebitCredit Then
                CreditSubscriber(recharge, iRechargePrepaid, numberOfDays)
            Else
                DebitSubscriber(recharge, iRechargePrepaid)
            End If
            Console.WriteLine("Saving prepaid")
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)
            Console.WriteLine("Saving Recharge")
            xRechargeAdapter.Save(recharge, SqlConn)
            Return New ServiceRechargeResponse(iRechargePrepaid)
        End Function

        Public Sub DebitSubscriber(recharge As xRecharge, iRechargePrepaid As xRechargePrepaid)
            'Dim debitRequest As debitSubscriberRequest = CreateDebitRequest(recharge, iRechargePrepaid)
            Dim channelFactory = GetNetworkClient()
            Dim client = channelFactory.GetNetwork()
            Try
                Dim iWebResponse As DebitResponse = client.DebitMobile(recharge.Mobile, recharge.Amount, iRechargePrepaid.Reference)
                iRechargePrepaid.FinalBalance = iWebResponse.finalBalance
                iRechargePrepaid.InitialBalance = iWebResponse.initialBalance
                iRechargePrepaid.Narrative = iWebResponse.narrative
                iRechargePrepaid.ReturnCode = iWebResponse.responseCode
                recharge.State.StateID = IIf(iWebResponse.responseCode = SuccessCode, xState.States.Success,
                                             xState.States.Failure)
            Catch ex As Exception
                channelFactory.Abort()
                recharge.State.StateID = xState.States.Failure
                iRechargePrepaid.Narrative = "Exception: " & ex.ToString
            End Try
        End Sub

        Private Sub CreditSubscriber(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer)
            'Dim creditRequest As CreditRequest = CreateCreditRequest(recharge, rechargePrepaid, numberOfDays)
            Dim client = GetNetworkClient()
            Try
                Console.WriteLine("Crediting")
                Dim econet = client.GetNetwork()
                Dim response = econet.RechargeMobile(recharge.Mobile, recharge.Amount, rechargePrepaid.RechargeID)

                xLog_Data.WriteJsonToConsole(response)

                client.Close()
                Console.WriteLine("Response from Econet" & response.narrative)
                rechargePrepaid.FinalBalance = response.finalBalance
                rechargePrepaid.InitialBalance = response.initialBalance
                rechargePrepaid.Reference = $"POSTPAID-{ rechargePrepaid.RechargeID}"
                If (response.narrative.ToLower().Contains("prepaid#")) Then
                    rechargePrepaid.Reference = $"PREPAID-{ rechargePrepaid.RechargeID}"
                    Dim data = response.narrative.Split("#")
                    response.narrative = data(0)
                    If data.Length > 2 Then
                        Dim balances = data(2).Split(",")
                        If balances.Length > 1 Then
                            Decimal.TryParse(balances(0), rechargePrepaid.FinalWallet)
                            Decimal.TryParse(balances(1), rechargePrepaid.InitialWallet)
                        End If
                    End If
                End If

                rechargePrepaid.Narrative = response.narrative
                rechargePrepaid.ReturnCode = response.responseCode
                rechargePrepaid.DebitCredit = True
                rechargePrepaid.RechargeID = recharge.RechargeID
                rechargePrepaid.Window = response.expiryDate
                recharge.State.StateID = IIf(response.responseCode = SuccessCode, xState.States.Success,
                                             xState.States.Failure)

                'TEST: Throw New Exception("Test Exception.")
                recharge.RechargeDate = Now
            Catch ex As Exception
                client.Abort()
                Console.WriteLine("Credit Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
                recharge.State.StateID = xState.States.Failure
                rechargePrepaid.Narrative = "Reversal Pending: " & "Credit Exception: "
                recharge.ErrorCode = ErrorCodes.MobileNetworkError
                Console.WriteLine("Reversing credit")
                ReverseCredit(rechargePrepaid, recharge)
            End Try
        End Sub

        Public Sub ReverseCredit(rechargePrepaid As xRechargePrepaid, recharge As xRecharge)
            Dim client = GetNetworkClient(40000)
            Dim econet = client.GetNetwork()
            Try
                'Dim request = New creditReversalRequest With {
                '    .originalCredit = iTransaction
                '}
                Dim iWebResponse2 = econet.RechargeReversal(recharge.Mobile, recharge.Amount, rechargePrepaid.Reference)
                rechargePrepaid.FinalBalance = iWebResponse2.finalBalance
                rechargePrepaid.InitialBalance = iWebResponse2.initialBalance
                rechargePrepaid.ReturnCode = iWebResponse2.responseCode
                rechargePrepaid.Narrative =
                    IIf(iWebResponse2.responseCode = SuccessCode, "Reversal Succeeded: ", "Reversal Failed: ") &
                    iWebResponse2.narrative


                ' TEST:  Throw New Exception("Reversal Exception")
            Catch ex As Exception
                client.Abort()
                If InStr(ex.ToString, "The Operation has timed out") Then
                    rechargePrepaid.Narrative = "Reversal Failed: Econet ePay Gateway Timeout"
                ElseIf InStr(ex.ToString, "Unable to connect") Then
                    rechargePrepaid.Narrative = "Reversal Failed: Econet ePay Gateway not reachable"
                ElseIf InStr(ex.ToString, "Internal Error") Then
                    rechargePrepaid.Narrative = "Reversal Failed: Internal error contacting ePay"
                Else
                    rechargePrepaid.Narrative = "Reversal Failed: Econet ePay Gateway Other Fault"
                End If
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", rechargePrepaid.RechargeID)
            End Try
        End Sub

        Public Function BalanceRequest(rechargePrepaid As xRechargePrepaid, recharge As xRecharge) As Balance
            'Dim request As New BalanceRequest With {
            '    .reference = rechargePrepaid.Reference,
            '    .serviceId = "43",
            '    .mobileNumber = recharge.Mobile,
            '    .serviceProviderId = "Mobile-Connectivity"
            '}
            'Dim r = New balanceEnquiryRequest() With {.BalanceRequest = request}

            Dim client = GetNetworkClient(40000)
            Dim econet = client.GetNetwork()


            Dim res = econet.EndUserBalance(recharge.Mobile, rechargePrepaid.Reference)
            If res Is Nothing Then
                Dim template = xTemplateAdapter.SelectRow(xTemplate.Templates.NetworkWebserviceUnavailable, SqlConn)
                Dim text = template.TemplateText.Replace("%NETWORK%", Name)
                Throw New HotRequestException(xTemplate.Templates.NetworkWebserviceUnavailable, text)
            End If
            Dim response = res

            rechargePrepaid.Reference = GetReference(recharge.Mobile)
            rechargePrepaid.FinalBalance = response.currentBalance
            rechargePrepaid.InitialBalance = response.currentBalance
            'to add expiry date and class of service, etc
            rechargePrepaid.Narrative = response.narrative & ", Expiry: " & response.expiryDate _
                        & ", SubscriberType:" & response.suscriberType & ", ClassOfService:" & response.classOfService
            rechargePrepaid.ReturnCode = response.responseCode

            Return New Balance() With {
                .IsSuccessful = response.responseCode = "000",
                .CurrentBalance = response.currentBalance,
                .ExpiryDate = response.expiryDate
            }
        End Function

        Public Overrides Function GetNetworkClient(Optional timeout As Integer = 30000) As INetworkClient(Of EconetSoap)
            'If IsTestMode Then
            '    Return GetTestClient()
            'End If
            Return NetworkClientFactory.Create(Of EconetSoap)(ServiceEndpoint, timeout)
        End Function

        'Private Function GetTestClient() As INetworkClient(Of EconetSoap)
        '    Dim econet As EconetSoap = Substitute.For(Of EconetSoap)

        '    ' Change Response Code to anything else to test failure
        '    Dim response = New CreditResponse() _
        '            With {.creditSubcriberResponse = New CreditResponse() With {
        '            .responseCode = SuccessCode,
        '            .narrative = "Test Narrative",
        '            .initialBalance = 55.55,
        '            .finalBalance = 77.77,
        '            .expiryDate = Now()
        '            }}
        '    econet.creditSubscriber(Arg.Any(Of creditSubscriberRequest)).Returns(response)

        '    Dim reverse = New creditReversalResponse() _
        '        With {.reversalResult = New CreditReversalResult() With {
        '            .responseCode = "000"
        '        }}

        '    econet.creditReversal(Arg.Any(Of creditReversalRequest)).Returns(reverse)

        '    Dim fakeFactory = Substitute.For(Of INetworkClient(Of EconetSoap))()
        '    fakeFactory.GetNetwork().Returns(econet)
        '    Return fakeFactory
        'End Function

        Protected Overrides Function Name() As String
            Return "Econet"
        End Function
    End Class

End Namespace

