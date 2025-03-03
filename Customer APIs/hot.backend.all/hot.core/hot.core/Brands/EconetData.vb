Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Core.EconetBundle
Imports Hot.Core.ServiceWrappers
Imports Hot.Data
Imports NSubstitute

Namespace Brands
    Public Class EconetData
        Inherits NetworkBase(Of EconetBundleSoap)


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
                                             numberOfDays As Integer) As RechargeRequest
            Return New RechargeRequest With {
                .Body = New RechargeRequestBody With {
                .RechargeId = recharge.RechargeID,
                .BundleCode = recharge.Brand.BrandSuffix,
                .TargetMobile = recharge.Mobile,
                .AppKey = "Hot263180",
                .Currency = If(recharge.Brand.WalletTypeId = 3, "1", "")
            }
            }

        End Function

        Private Sub CreditSubscriber(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer)
            Dim creditRequest As RechargeRequest = CreateCreditRequest(recharge, rechargePrepaid, numberOfDays)
            Dim client = GetNetworkClient()
            Try
                Console.WriteLine("Crediting")
                Dim econetbundle = client.GetNetwork()
                Dim response = econetbundle.Recharge(creditRequest).Body.RechargeResult

                xLog_Data.WriteJsonToConsole(response)

                client.Close()
                Console.WriteLine($"Response from Econet {response.Status}")
                rechargePrepaid.Narrative = $"{response.Description}; Serial:{response.Serial}; Product Code:{recharge.Brand.BrandSuffix}" +
                $" Raw Request:{{RechargeID:{creditRequest.Body.RechargeId},ProductCode:{creditRequest.Body.BundleCode},TargetMobile:{creditRequest.Body.TargetMobile}}}" +
                $" Raw Response:{{Description:{response.Description},StatusCode:{response.StatusCode},Status:{response.Status},Serial:{response.Serial},ProviderReference:{response.ProviderReference}}}"
                rechargePrepaid.ReturnCode = response.StatusCode
                rechargePrepaid.DebitCredit = True
                rechargePrepaid.RechargeID = recharge.RechargeID
                rechargePrepaid.Window = DateAdd(DateInterval.Day, numberOfDays, Date.Now)
                recharge.State.StateID = IIf(response.StatusCode = EconetBundleStatusCodes.Success, xState.States.Success, xState.States.Failure)
                recharge.RechargeDate = Now
                If (recharge.State.StateID = xState.States.Success) Then
                    rechargePrepaid.InitialWallet = response.InitialWalletBalance
                    rechargePrepaid.FinalWallet = response.FinalWalletBalance
                End If

            Catch ex As Exception
                client.Abort()
                Console.WriteLine("Credit Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
                recharge.State.StateID = xState.States.Failure
                rechargePrepaid.Narrative = "EconetData Error: " & ex.Message

            End Try
        End Sub

        Public Overrides Function GetNetworkClient(Optional timeout As Integer = 30000) As INetworkClient(Of EconetBundleSoap)
            If IsTestMode Then
                Return GetTestClient()
            End If
            Return NetworkClientFactory.Create(Of EconetBundleSoap)(ServiceEndpoint, timeout)
        End Function

        Private Function GetTestClient() As INetworkClient(Of EconetBundleSoap)
            Dim netone As EconetBundleSoap = Substitute.For(Of EconetBundleSoap)
            ' Change Response Code to anything else but 1 to test failure
            Dim response = New EconetBundle.RechargeResponse() _
                    With {
                        .Body = New RechargeResponseBody() With {
                            .RechargeResult = New LoadDataResponse() With {
                                .ProviderReference = "Mobile",
                                .RawResponseData = "Rawdata text",
                                .StatusCode = EconetBundleStatusCodes.Success,
                                .Description = "Narrative test",
                                .Serial = "Test Serial",
                                .Status = EconetBundleStatusCodes.Success
                            }
                        }
                    }
            netone.Recharge(Arg.Any(Of RechargeRequest)).Returns(response)
            Dim fakeFactory = Substitute.For(Of INetworkClient(Of EconetBundleSoap))()
            fakeFactory.GetNetwork().Returns(netone)
            Return fakeFactory
        End Function

        Protected Overrides Function Name() As String
            Return "EconetData"
        End Function

    End Class

    Public Enum EconetBundleStatusCodes

        Success = 0
        Invalid_Account = 901
        Invalid_Provider = 902
        Invalid_Product = 903
        Invalid_Bundle_Quantity = 904
        Insufficient_Credit = 905
        Limits_Exceeded = 906
        Invalid_Amount = 907
        Invalid_Voucher = 911
        Invalid_Batch = 921
        Invalid_Specification = 922
        Download_Encryption_Error = 923
        Invalid_Batch_State = 924
        Invalid_User = 970
        Invalid_PIN_or_Password = 971
        Username_already_exists = 972
        Request_Timeout = 990
        General_Error = 999
    End Enum


End Namespace
