Namespace Models

    Public Class BulkADSLResponse
        Inherits Response

        Public Sub New(agentReference As String)
            Me.AgentReference = agentReference
        End Sub

        Public Property WalletBalance As Decimal = 0
        Public Property Amount As Decimal = 0
        Public Property Discount As Decimal = 0

        'Public Property Pins As New List(Of Hot.Lib.Models.PinRechargeModel)
        Public Property Vouchers As New List(Of Hot.Core.TelOneAPI.Voucher)

        Public Property RechargeID As Integer
    End Class
End Namespace



