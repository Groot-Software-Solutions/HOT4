Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class NyaradzoPaymentRequest
        Public Property Amount As Decimal
        Public Property PolicyNumber As String
        Public Property MobileNumber As String
        <MaxLength(300)>
        Public Property CustomerSMS As String

    End Class
End Namespace
