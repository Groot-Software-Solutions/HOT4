Namespace Models
    Public Class BulkEvdResponse
        Public Sub New(agentReference As String)
            Me.AgentReference = agentReference
        End Sub

        Public Property ReplyCode As Integer = 0
        Public Property ReplyMsg As String = ""
        Public Property WalletBalance As Decimal = 0
        Public Property Amount As Decimal = 0
        Public Property Discount As Decimal = 0

        'Public Property Pins As New List(Of Hot.Lib.Models.PinRechargeModel)
        Public Property Pins As New List(Of String)

        Public Property AgentReference As String = ""
        Public Property RechargeID As Integer
    End Class
End Namespace
