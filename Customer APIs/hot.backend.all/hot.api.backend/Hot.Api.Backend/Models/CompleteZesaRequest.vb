Imports System.Collections.ObjectModel

Public Class CompleteZesaRequest

    Public Property rechargeId As Integer
    Public Property purchaseToken As Purchasetoken
End Class

Public Class Purchasetoken
    Public Property reference As String
    Public Property _date As Date
    Public Property amount As Double
    Public Property meterNumber As String
    Public Property rawResponse As String
    Public Property responseCode As String
    Public Property narrative As String
    Public Property vendorReference As String
    Public Property tokens As List(Of Token)
End Class

Public Class Token
    Public Property zesaReference As String
    Public Property token As String
    Public Property units As Double
    Public Property netAmount As Double
    Public Property arrears As Integer
    Public Property levy As Double
    Public Property taxAmount As Double
End Class

