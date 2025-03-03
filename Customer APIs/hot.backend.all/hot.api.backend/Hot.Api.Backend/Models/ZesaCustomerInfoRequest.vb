Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class ZesaCustomerInfoRequest
        <Required>
        <MaxLength(11)>
        <MinLength(11)>
        Public Property MeterNumber() As String

    End Class
End Namespace
