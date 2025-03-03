Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class RechargeZesaRequest

        <Required>
        <MaxLength(11)>
        <MinLength(11)>
        Public Property MeterNumber() As String

        <Required>
        Public Property TargetNumber() As String

        <Required>
        Public Property Amount As Decimal

        <MaxLength(300)>
        Public Property CustomerSMS As String

    End Class
End Namespace