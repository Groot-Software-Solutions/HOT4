Namespace Models

    Public Class PayTeloneBillResponse
        Inherits Response

        Public Sub New(agentReference As String)
            Me.AgentReference = agentReference
        End Sub

        'Public Property InitialBalance As Decimal = 0
        'Public Property FinalBalance As Decimal = 0
        Public Property RechargeID As Integer
        Public Property Amount As Decimal = 0
        Public Property Discount As Decimal = 0
        Public Property WalletBalance As Decimal?
    End Class

End Namespace
