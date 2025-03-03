Imports System.Runtime.Serialization

Namespace Models
    Public Class EndUserBalanceResponse

        Public Sub New (agentReference As String)
            Me.AgentReference = agentReference
        End Sub
        Public Property ReplyCode As Integer = 0
        Public Property ReplyMsg As String = ""
        Public Property MobileBalance As Decimal = 0
        Public Property WindowPeriod As String = ""
        Public Property AgentReference As String = ""

        <IgnoreDataMember>
        Public Property RechargeId() As Integer
    End Class
End NameSpace