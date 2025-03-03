Namespace Models

    Public Class RechargeTeloneVoipResponse
        Inherits Response

        Public Sub New(agentReference As String)
            Me.AgentReference = agentReference
        End Sub

        'Public Property InitialBalance As Decimal = 0
        'Public Property FinalBalance As Decimal = 0
        Public Property RechargeID As Integer
        Public Property Discount As Decimal
        Public Property Amount As Decimal
        Public Property WalletBalance As Decimal?
    End Class
End Namespace
