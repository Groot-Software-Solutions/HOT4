Imports Hot.EconetBundle.Models
Namespace Models
    Public Class GetBundleResponse
        Public Property ReplyCode As Integer = 0
        Public Property Bundles As List(Of BundleProduct)
        Public Property AgentReference As String = ""
    End Class
End Namespace
