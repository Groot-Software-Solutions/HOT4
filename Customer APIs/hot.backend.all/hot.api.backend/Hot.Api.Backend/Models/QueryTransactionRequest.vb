Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class QueryTransactionRequest
        <Required>
        <StringLength(50)>
        Public Property AgentReference As String
    End Class
End NameSpace