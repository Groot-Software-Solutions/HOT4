Imports Hot.Core.TeloneAPI

Namespace Models
    Public Class QueryADSLResponse
        Inherits Response


        Public Sub New(agentReference As String)
            Me.AgentReference = agentReference
        End Sub

        Public Property Products As List(Of BroadbandProductItem)
    End Class
End Namespace


