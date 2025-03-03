Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class RechargeDataRequest

        <Required>
        Public Property ProductCode As String

        '<RegexStringValidator("077xxxxxxx|086XXxxxxxx")>
        <Required>
        Public Property TargetMobile As String

        Public Property Amount As Decimal

        <MaxLength(300)>
        Public Property CustomerSMS As String

    End Class
End Namespace