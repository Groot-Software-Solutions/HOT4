Imports System.Collections.Concurrent
Imports System.Data.SqlClient

Public Class xHotType

    Public Property HotTypeID As Integer
    Public Property HotTypeName As String
    Public Property SplitCount As Short

    Sub New(ByVal sqlRdr As SqlDataReader)
        HotTypeID = sqlRdr("HotTypeID")
        HotTypeName = sqlRdr("HotType")
        SplitCount = sqlRdr("SplitCount")
    End Sub

    Public Enum HotTypes
        Unknown = 0
        Balance = 1
        Recharge = 2
        Transfer = 3
        Help = 4
        Registration = 5
        Resend = 6
        Zesa = 7
        BulkSMS = 8
        Payment = 9
        Answer = 10
        EcoCash = 11
        PhoneBal = 12
        EcoChargeSelf = 14
        EcoChargeOther = 15
        AffiliateRegistration = 16

    End Enum
End Class
Public Class xHotTypeAdapter

    Public Shared Property HotTypeByCodeAndSize As Dictionary(Of Tuple(Of String, Integer), xHotType.HotTypes) = New Dictionary(Of Tuple(Of String, Integer), xHotType.HotTypes)

    Public Shared Function Identify(ByVal TypeCode As String, ByVal SplitCount As Integer, ByVal sqlConn As SqlConnection) As xHotType.HotTypes
        Dim key = New Tuple(Of String, Integer)(UCase(TypeCode), SplitCount), keyZero = New Tuple(Of String, Integer)(UCase(TypeCode), 0)
        If HotTypeByCodeAndSize.Count = 0 Then List(sqlConn)
        If HotTypeByCodeAndSize.ContainsKey(key) Then Return HotTypeByCodeAndSize(key)
        If HotTypeByCodeAndSize.ContainsKey(keyZero) Then Return HotTypeByCodeAndSize(keyZero)


        Return xHotType.HotTypes.Unknown

    End Function

    Public Shared Function List(sqlConn As SqlConnection) As List(Of xHotType.HotTypes)
        HotTypeByCodeAndSize = New Dictionary(Of Tuple(Of String, Integer), xHotType.HotTypes)
        Dim iList As New List(Of xHotType.HotTypes)
        Using sqlCmd As New SqlCommand("xHotType_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    Dim iHotType As xHotType = New xHotType(sqlRdr)
                    Dim iHottypecode As xHotTypeCode = New xHotTypeCode(sqlRdr)
                    HotTypeByCodeAndSize(New Tuple(Of String, Integer)(UCase(iHottypecode.TypeCode), iHotType.SplitCount)) = iHotType.HotTypeID
                    iList.Add(iHotType.HotTypeID)
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function

    'Public Shared Function Identify(ByVal TypeCode As String, ByVal SplitCount As Integer, ByVal sqlConn As SqlConnection) As xHotType.HotTypes
    '    Dim eHotType As xHotType.HotTypes = xHotType.HotTypes.Unknown
    '    Using sqlCmd As New SqlCommand("xHotType_List", sqlConn)
    '        sqlCmd.CommandType = CommandType.StoredProcedure
    '        sqlCmd.Parameters.AddWithValue("TypeCode", TypeCode)
    '        sqlCmd.Parameters.AddWithValue("SplitCount", SplitCount)
    '        Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
    '            If sqlRdr.Read Then
    '                eHotType = sqlRdr("HotTypeID")
    '            Else
    '                eHotType = xHotType.HotTypes.Unknown
    '            End If
    '            sqlRdr.Close()
    '        End Using
    '    End Using
    '    Return eHotType
    'End Function


End Class