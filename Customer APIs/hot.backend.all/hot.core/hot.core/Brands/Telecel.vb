Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Core.ServiceWrappers
Imports Hot.Core.TelecelJuice
Imports Hot.Data
Imports NSubstitute

Namespace Brands

    Public Class Telecel
        Inherits NetworkBase(Of TelecelSoap)

        Public Sub New(sqlConn As SqlConnection, applicationName As String, serviceEndpoint As String, isTestMode As Boolean, referencePrefix As String, webServiceOrCorporate As Boolean)
            MyBase.New(sqlConn, applicationName, serviceEndpoint, isTestMode, webServiceOrCorporate, referencePrefix)
        End Sub

        Public Overrides Function GetReference(mobile As String) As String
            Return ReferencePrefix & "+" & mobile & "+" & Format(Now, "dd-MM-yyyy HH:mm:ss.fff").Replace(" ", "+")
        End Function


        Public Overrides Function RechargePrepaid(recharge As xRecharge, Optional numberOfDays As Integer = 90) As ServiceRechargeResponse
            'Change discounts for small amounts
            If recharge.Amount < 1 And recharge.Discount > 5 Then recharge.Discount = 5

            Dim iRechargePrepaid = CreateRechargeObject(recharge)
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)

            Try
                Dim client = GetNetworkClient(60000)
                Dim telecel = client.GetNetwork()

                If recharge.Amount >= 0 Then
                    'Execute Credit
                    Dim juiceResponse As JuiceRechargeResponse = Nothing
                    Dim response As TopupResponse = Nothing
                    Try

                        Dim request = New JuiceRechargeRequest() With
                        {
                        .Body = New JuiceRechargeRequestBody() With {
                                .Amount = recharge.Amount,
                                .AgentCode = "733357030",
                                .MPin = "5394C86F977989489AFC65FEFC525CE1",
                                .TargetMobile = recharge.Mobile,
                                .TransID = iRechargePrepaid.Reference
                            }
                        }
                        juiceResponse = telecel.JuiceRecharge(request)
                        ' Throw New Exception("It failed.")
                        response = juiceResponse.Body.JuiceRechargeResult
                        xLog_Data.WriteJsonToConsole(response)
                    Catch ex As Exception
                        recharge.State.StateID = xState.States.Failure
                        iRechargePrepaid.Narrative = iRechargePrepaid.Narrative & "Exception: TelecelEJuiceWebservice: " & ex.ToString

                        Dim description = "Sending:" &
                            recharge.Amount.ToString & "; " & recharge.Mobile.ToString & "; " & iRechargePrepaid.Reference.ToString & "; " & recharge.Amount.ToString _
                            & vbNewLine & ex.ToString

                        xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, description, SqlConn, "RechargeID", recharge.RechargeID)

                    End Try
                    Try
                        iRechargePrepaid.Narrative = response.responseValue
                        iRechargePrepaid.ReturnCode = response.resultcode
                        iRechargePrepaid.Narrative = response.resultdescription & "|" & response.responseValue _
                        & "|" & response.transid & "|" & response.requestcts & "|" & response.responsects _
                        & "|" & response.walletBalance & "|" & response.preWalletBalance
                        iRechargePrepaid.InitialWallet = response.preWalletBalance
                        iRechargePrepaid.FinalWallet = response.walletBalance

                        If response.resultcode = 0 Then
                            recharge.State.StateID = xState.States.Success
                            Try
                                Dim ResponseString() As String = Split(response.responseValue, "Core,")
                                If ResponseString.Length = 3 Then
                                    iRechargePrepaid.InitialBalance = ResponseString(1).Split(",")(0)
                                    iRechargePrepaid.FinalBalance = ResponseString(2).Split(",")(0)
                                Else
                                    iRechargePrepaid.InitialBalance = 0
                                    iRechargePrepaid.FinalBalance = 0
                                End If
                            Catch ex As Exception
                                iRechargePrepaid.InitialBalance = 0
                                iRechargePrepaid.FinalBalance = 0
                                iRechargePrepaid.Narrative = "Parsing error: " & iRechargePrepaid.Narrative

                                ' RechargeEJuice-Parsing
                                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex.ToString, SqlConn, "RechargeID", recharge.RechargeID)
                            End Try

                        Else
                            recharge.State.StateID = xState.States.Failure
                            iRechargePrepaid.FinalBalance = 0
                            iRechargePrepaid.InitialBalance = 0
                        End If

                    Catch ex As Exception

                        ' "RechargeEJuice Parsing iWebResponse"
                        Dim description As String
                        If Not IsNothing(response) Then
                            description = "; iWebResponse:" & response.agentCode & response.resultdescription & "|" &
                                response.responseValue & "|" & response.transid & vbNewLine & ex.ToString
                        Else
                            description = ex.Message
                        End If

                        xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, description, SqlConn, "RechargeID", recharge.RechargeID)
                    End Try

                Else
                    'Debit not possible on NetOne
                    iRechargePrepaid.Narrative = "Telecel EJuice Debit not possible"
                    iRechargePrepaid.ReturnCode = "099"
                    recharge.State.StateID = xState.States.Failure
                End If
            Catch ex As Exception
                'Just set StateID to a failure
                recharge.State.StateID = xState.States.Failure
                iRechargePrepaid.Narrative = "Exception: " & ex.ToString

            End Try

            iRechargePrepaid.Window = Nothing
            xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)
            recharge.RechargeDate = Now
            xRechargeAdapter.Save(recharge, SqlConn)
            Return New ServiceRechargeResponse(iRechargePrepaid)
        End Function


        Public Overrides Function GetNetworkClient(Optional timeout As Integer = 30000) As INetworkClient(Of TelecelSoap)
            If IsTestMode Then
                Return GetTestClient()
            End If
            Return NetworkClientFactory.Create(Of TelecelSoap)(ServiceEndpoint, timeout)
        End Function

        Private Function GetTestClient() As INetworkClient(Of TelecelSoap)
            Dim telecel As TelecelSoap = Substitute.For(Of TelecelSoap)
            ' Change Response Code to anything else but 1 to test failure
            Dim response = New JuiceRechargeResponse() _
                    With {
                        .Body = New JuiceRechargeResponseBody() With {
                            .JuiceRechargeResult = New TopupResponse() With {
                               .responseValue = "Narrative test Core,",
                               .resultcode = 0
                            }
                        }
                    }
            telecel.JuiceRecharge(Arg.Any(Of JuiceRechargeRequest)).Returns(response)
            Dim fakeFactory = Substitute.For(Of INetworkClient(Of TelecelSoap))()
            fakeFactory.GetNetwork().Returns(telecel)
            Return fakeFactory
        End Function

        Protected Overrides Function Name() As String
            Return "Telecel"
        End Function


    End Class
End Namespace
