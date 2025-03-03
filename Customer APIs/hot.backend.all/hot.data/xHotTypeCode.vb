Imports System.Data.SqlClient

Public Class xHotTypeCode
    Public Property HotTypeCodeID As Integer
    Public Property HotTypeID As Integer
    Public Property TypeCode As String

    Sub New(ByVal sqlRdr As SqlDataReader)
        HotTypeCodeID = sqlRdr("HotTypeCodeID")
        HotTypeID = sqlRdr("HotTypeID")
        TypeCode = sqlRdr("TypeCode")
    End Sub
End Class
