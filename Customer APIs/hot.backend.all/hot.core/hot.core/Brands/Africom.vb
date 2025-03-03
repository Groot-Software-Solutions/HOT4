Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Core.AfricomService
Imports Hot.Core.ServiceWrappers
Imports Hot.Data
Imports NSubstitute

Namespace Brands
    Public Class Africom
        Inherits NetworkBase(Of ServiceSoap)

        Private const SuccessCode as String = "000"

        Public Sub New(sqlConn As SqlConnection, applicationName As String, serviceEndpoint As String, isTestMode As Boolean, referencePrefix As String,  webServiceOrCorporate As Boolean)
            MyBase.New(sqlConn, applicationName, serviceEndpoint, isTestMode, webServiceOrCorporate,referencePrefix)
        End Sub

        Public Overrides Function RechargePrepaid(recharge As xRecharge, Optional numberOfDays As Integer = 90) As ServiceRechargeResponse
            ApplyDiscountRules(recharge)
            Dim iRechargePrepaid = CreateRechargeObject(recharge)
            Try
                iRechargePrepaid.DebitCredit = recharge.Amount >= 0

                If Not iRechargePrepaid.DebitCredit Then
                    iRechargePrepaid.Narrative = "Africom Debit not possible"
                    iRechargePrepaid.ReturnCode = "099"
                    recharge.State.StateID = xState.States.Failure
                    Return New ServiceRechargeResponse(iRechargePrepaid)
                End If
                xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)

                Dim client = GetNetworkClient(40000)
                Try
                    Try
                        Dim africom = client.GetNetwork()
                        Dim response As Transfer = Nothing
                        Try
                            Dim encryptResponse As String = GetEncryptResponse(recharge, iRechargePrepaid, africom)

                            Dim transferRequest = New AfriTransferRequest() With {
                                    .Body = New AfriTransferRequestBody() With {
                                    .requestParam = "8644003060|" & encryptResponse
                                    }}
                            response = africom.AfriTransfer(transferRequest).Body.AfriTransferResult
                            xLog_Data.WriteJsonToConsole(response)
                        Catch ex As Exception
                            recharge.State.StateID = xState.States.Failure
                            iRechargePrepaid.Narrative = iRechargePrepaid.Narrative & "Exception: AfricomWebservice: " &
                                                         ex.ToString
                            Log(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
                        End Try
                        AssignRechargeFromResponse(recharge, iRechargePrepaid, response)
                    Catch ex As Exception
                        Log(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
                    End Try
                Catch ex As Exception
                    'Just set StateID to a failure
                    recharge.State.StateID = xState.States.Failure
                    iRechargePrepaid.Narrative = "Exception: " & ex.ToString
                    Log(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
                End Try
                recharge.RechargeDate = Now
            Catch ex As Exception
                recharge.State.StateID = xState.States.Failure
                iRechargePrepaid.Narrative = "Exception: " & ex.ToString
                Log(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID)
            End Try

             xRechargePrepaidAdapter.Save(iRechargePrepaid, sqlConn)
             xRechargeAdapter.Save(recharge, sqlConn)
            Return New ServiceRechargeResponse(iRechargePrepaid)
        End Function

        Private Function GetEncryptResponse(recharge As xRecharge, iRechargePrepaid As xRechargePrepaid,
                                            africom As ServiceSoap) As String

            Dim encryptString = "8644003060|cD&7Q3|" & iRechargePrepaid.Reference &
                                "|8644003060|prepaid_credit|" & recharge.Amount & "|" &
                                recharge.Mobile.Substring(1)

            Dim encryptRequest = New AfriEncryptRequest() With {
                    .Body = New AfriEncryptRequestBody() With {
                    .str = encryptString,
                    .secretkey = "629V25eJ"
                    }
                    }
            Return africom.AfriEncrypt(encryptRequest).Body.AfriEncryptResult
        End Function

        Private Sub AssignRechargeFromResponse(recharge As xRecharge, iRechargePrepaid As xRechargePrepaid,
                                               response As Transfer)
            iRechargePrepaid.ReturnCode = response.THeader.Code
            iRechargePrepaid.Narrative = response.THeader.Description

            If response.THeader.Code = SuccessCode Then 'Success
                recharge.State.StateID = xState.States.Success
                iRechargePrepaid.InitialBalance = response.TransferType.TransfereeNoBalanceOld
                iRechargePrepaid.FinalBalance = response.TransferType.TransfereeNoBalanceNew
                iRechargePrepaid.Narrative = "Description:" & response.THeader.Description &
                                             " |System:" & response.THeader.SystemReferenceNo &
                                             " |ProductName:" & response.TransferType.ProductName &
                                             " |Old Wallet:" &
                                             response.TransferType.SubscriberBalanceOld &
                                             " |New Wallet:" &
                                             response.TransferType.SubscriberBalanceNew
                iRechargePrepaid.InitialWallet  =  response.TransferType.SubscriberBalanceOld
                iRechargePrepaid .FinalWallet = response .TransferType .SubscriberBalanceNew 

            Else 'Failure
                recharge.State.StateID = xState.States.Failure
                iRechargePrepaid.FinalBalance = -1
                iRechargePrepaid.InitialBalance = -1
            End If
        End Sub

        Private Sub Log(methodName As String, ex As Exception, idType As String, idNumber As Long)
            xLog_Data.Save(ApplicationName, [GetType]().Name, methodName,
                           ex, SqlConn.ConnectionString, idType, idNumber)

        
        End Sub

        Public Overrides Function GetNetworkClient(Optional timeout As Integer = 30000) _
            As INetworkClient(Of ServiceSoap)
            If IsTestMode Then
                Return GetTestClient()
            End If
            return NetworkClientFactory.Create(Of ServiceSoap)(ServiceEndpoint, timeout)
        End Function

        Private Function GetTestClient() As INetworkClient(Of ServiceSoap)
            Dim africom As ServiceSoap = Substitute.For(Of ServiceSoap)

            Dim encryptResponse = New AfriEncryptResponse() With {
                .Body = New AfriEncryptResponseBody() With {
                .AfriEncryptResult = "dsfsdfj234"
            }}

            africom.AfriEncrypt(Arg.Any(Of AfriEncryptRequest)).Returns(encryptResponse)

            Dim transferResponse = New AfriTransferResponse() With {
                    .Body = New AfriTransferResponseBody() With {
                    .AfriTransferResult = New Transfer() With {
                                              .THeader = New THeader() With {
                                              .Code = SuccessCode,
                                              .Description = "Test Description",
                                              .SystemReferenceNo = "234FDSD"
                                              }, .TransferType = New TransferType() With {
                                              .TransfereeNoBalanceNew = 24.00,
                                              .TransfereeNoBalanceOld = 33.00,
                                              .SubscriberBalanceOld = 65.00,
                                                .SubscriberBalanceNew = 56.00,
                                                .ProductName = "Test product name"
                                                }
                    }}
            }
            africom.AfriTransfer(Arg.Any(Of AfriTransferRequest)).Returns(transferResponse)

            Dim fakeFactory = Substitute.For(Of INetworkClient(Of ServiceSoap))()
            fakeFactory.GetNetwork().Returns(africom)
            Return fakeFactory
        End Function

        Protected Overrides Function Name() As String
            return "Africom"
        End Function
    End Class
End NameSpace