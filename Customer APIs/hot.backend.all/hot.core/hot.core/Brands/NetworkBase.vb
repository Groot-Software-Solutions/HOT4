Imports System.Data.SqlClient
Imports Hot.Core.ServiceWrappers
Imports Hot.Data

Namespace Brands
    Public MustInherit Class NetworkBase(Of T)

        Protected ApplicationName As String
        Protected SqlConn As SqlConnection
        Protected ServiceEndpoint As String
        Protected MustOverride Function Name As String
        Public Property IsTestMode As Boolean
        Public Property WebServiceOrCorporate As Boolean
        Public Property ReferencePrefix As String
        
        Public Sub New(sqlConn As SqlConnection, applicationName As String, serviceEndpoint As String, isTestMode As Boolean, webServiceOrCorporate As Boolean, Optional referencePrefix As String = "999")
            Me.ApplicationName = applicationName
            Me.SqlConn = sqlConn
            Me.ServiceEndpoint = serviceEndpoint
            Me.IsTestMode = isTestMode
            Me.WebServiceOrCorporate = webServiceOrCorporate
            Me.ReferencePrefix = referencePrefix
        End Sub

        Public Overridable Function GetReference(mobile As String) As String
            Return ReferencePrefix & "+" & mobile & "+" & Format(Now, "dd-MM-yyyy HH:mm:ss.fff")
        End Function

        Protected Sub ApplyDiscountRules(recharge As xRecharge)
            If Not WebServiceOrCorporate then
                Dim limit = new LimitDiscountTo5Percent()
                limit.Apply(recharge)
            End If
        End Sub

        Public Function CreateRechargeObject(recharge As xRecharge) As xRechargePrepaid
            Dim iRechargePrepaid As New xRechargePrepaid
            iRechargePrepaid.RechargeID = recharge.RechargeID
            iRechargePrepaid.DebitCredit = recharge.Amount >= 0
            iRechargePrepaid.Reference = GetReference(recharge.Mobile)
            iRechargePrepaid.FinalBalance = - 1
            iRechargePrepaid.InitialBalance = - 1
            iRechargePrepaid.Narrative = IIf(iRechargePrepaid.DebitCredit, "Pending", "Debit Pending")
            iRechargePrepaid.ReturnCode = - 1
            Return iRechargePrepaid
        End Function

        Public MustOverride Function RechargePrepaid(recharge As xRecharge, Optional numberOfDays As Integer = 90) As ServiceRechargeResponse

        Public MustOverride Function GetNetworkClient(Optional timeout As Integer = 30000) As INetworkClient(Of T)
    End Class
End Namespace