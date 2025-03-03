Imports System.ComponentModel.DataAnnotations
Namespace Models
    Public Class SelfTopUpRequest
        <Required>
        <MaxLength(20)>
        Public Property TargetMobile As String
        <Required>
        <MaxLength(20)>
        Public Property BillerMobile As String
        <Required>
        Public Property Amount As Decimal

    End Class

    Public Class SelfTopUpDataRequest
        <Required>
        <MaxLength(20)>
        Public Property TargetMobile As String
        <Required>
        <MaxLength(20)>
        Public Property BillerMobile As String
        <Required>
        Public Property Amount As Decimal
        <Required>
        Public Property BrandId As Integer

    End Class
End Namespace
