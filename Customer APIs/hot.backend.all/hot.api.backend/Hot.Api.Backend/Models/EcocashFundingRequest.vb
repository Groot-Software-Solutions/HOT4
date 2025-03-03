Imports System.ComponentModel.DataAnnotations
Public Class EcocashFundingRequest
    <Required>
    Public Property Amount As Decimal
    <Required>
    <StringLength(12)>
    Public Property TargetMobile As String

    Public Property OnBehalfOf As String = "Comm Shop"

    Public Property Account As Integer = 1

End Class
