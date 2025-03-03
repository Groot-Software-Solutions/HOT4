Imports Hot.Core.NetoneAPI
Imports Hot.Core.TeloneAPI

Namespace Models
    Public Class QueryEVDResponse
        Public Property ReplyCode As Integer = 0
        Public Property ReplyMsg As String = ""
        Public Property InStock As New List(Of PinStockModel)
        Public Property AgentReference As String = ""

        Public Sub New(agentReference As String)
            Me.AgentReference = agentReference
        End Sub

    End Class
End Namespace