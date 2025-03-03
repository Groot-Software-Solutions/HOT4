Imports Hot.Api.Backend.Models
Imports Hot.Core.ZesaAPI
Namespace Models
    Public Class ZesaCustomerResponseModel

        Public Property ReplyCode As Integer
        Public Property ReplyMsg As String
        Public Property Meter As String
        Public Property CustomerInfo As CustomerInfo
        Public Property Currency As String = "ZiG"
        Public Property AgentReference As String
    End Class
End Namespace

