Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Core.NetoneAPI
Imports Hot.Core.ServiceWrappers
Imports Hot.Data
Imports NSubstitute

Namespace Brands
    Public Class NetoneData
        Inherits NetworkBase(Of NetoneSoap)


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
                'Debit not possible on Ecionet Bundles
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
                                         numberOfDays As Integer) As RechargeBundleRequest
            Return New RechargeBundleRequest With {
            .Body = New RechargeBundleRequestBody With {
            .RechargeID = recharge.RechargeID,
            .ProductCode = recharge.Brand.BrandSuffix,
            .TargetMobile = recharge.Mobile,
            .Amount = recharge.Amount,
            .AppKey = "Hot263180"
            }
        }

        End Function

        Private Sub CreditSubscriber(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer)
            Dim creditRequest As RechargeBundleRequest = CreateCreditRequest(recharge, rechargePrepaid, numberOfDays)
            Dim client = GetNetworkClient()
            Try
                Console.WriteLine("Crediting")
                Dim bundleClient = client.GetNetwork()
                Dim response = bundleClient.RechargeBundle(creditRequest).Body.RechargeBundleResult

                xLog_Data.WriteJsonToConsole(response)

                client.Close()
                Console.WriteLine($"Response from Netone {response.ReplyMessage}")
                rechargePrepaid.Narrative = response.ReplyMessage + response.ReplyMsg
                rechargePrepaid.ReturnCode = response.ReplyCode
                'rechargePrepaid.Reference = response.AgentReference
                rechargePrepaid.FinalBalance = response.FinalBalance
                rechargePrepaid.InitialBalance = response.InitialBalance
                rechargePrepaid.FinalWallet = response.WalletBalance

                rechargePrepaid.Window = response.Window
                rechargePrepaid.DebitCredit = True
                rechargePrepaid.RechargeID = recharge.RechargeID
                rechargePrepaid.Window = DateAdd(DateInterval.Day, numberOfDays, Date.Now)
                recharge.State.StateID = IIf(response.ReplyCode = xState.States.Success, xState.States.Success,
                                         xState.States.Failure)
                recharge.RechargeDate = Now
            Catch ex As Exception
                client.Abort()
                Console.WriteLine("Credit Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                           SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
                recharge.State.StateID = xState.States.Failure
                rechargePrepaid.Narrative = "EconetData Error: " & ex.Message

            End Try
        End Sub

        Public Overrides Function GetNetworkClient(Optional timeout As Integer = 30000) As INetworkClient(Of NetoneSoap)

            Return NetworkClientFactory.Create(Of NetoneSoap)(ServiceEndpoint, timeout)
        End Function


        Protected Overrides Function Name() As String
            Return "NetoneData"
        End Function

    End Class
End Namespace