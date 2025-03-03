Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class RechargePinlessRequest
  
         <Required>
        Public Property Amount As Decimal

        '<RegexStringValidator("077xxxxxxx|086XXxxxxxx")>
        <Required>
        Public Property TargetMobile As String

        Public Property BrandID As String

        <MaxLength(300)>
        Public Property CustomerSMS As String

    End Class
End NameSpace