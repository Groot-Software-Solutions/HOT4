Imports System.Data.SqlClient

Public Class xPinBatchType
    Public Enum PinBatchTypes
        Econet = 1
        NetOne = 2
        Telecel = 3
        eTopUp = 4
        TPS = 5
        Africom = 6
        UMax = 7
        ZESA = 8
        Migrated = 99
        TelOne = 9
    End Enum

    Private _PinBatchTypeID As Integer
    Public Property PinBatchTypeID() As Integer
        Get
            Return _PinBatchTypeID
        End Get
        Set(ByVal value As Integer)
            _PinBatchTypeID = value
        End Set
    End Property

    Private _PinBatchType As String
    Public Property PinBatchType() As String
        Get
            Return _PinBatchType
        End Get
        Set(ByVal value As String)
            _PinBatchType = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _PinBatchTypeID = sqlRdr("PinBatchTypeID")
        _PinBatchType = sqlRdr("PinBatchType")
    End Sub
    Public Overrides Function ToString() As String
        Return _PinBatchType
    End Function
End Class
Public Class xPinBatchTypeAdapter
    Public Shared Function List(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xPinBatchType)
        Dim iList As New List(Of xPinBatchType)
        Using sqlCmd As New SqlCommand("xPinBatchType_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xPinBatchType(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class