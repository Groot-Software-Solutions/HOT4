Imports System.Data.SqlClient

Public Class xPinBatch

    Private _PinBatchID As Long
    Public Property PinBatchID() As Long
        Get
            Return _PinBatchID
        End Get
        Set(ByVal value As Long)
            _PinBatchID = value
        End Set
    End Property

    Private _PinBatch As String
    Public Property PinBatch() As String
        Get
            Return _PinBatch
        End Get
        Set(ByVal value As String)
            _PinBatch = value
        End Set
    End Property

    Private _BatchDate As Date
    Public Property BatchDate() As Date
        Get
            Return _BatchDate
        End Get
        Set(ByVal value As Date)
            _BatchDate = value
        End Set
    End Property
    Private _PinBatchType As xPinBatchType
    Public Property PinBatchType() As xPinBatchType
        Get
            Return _PinBatchType
        End Get
        Set(ByVal value As xPinBatchType)
            _PinBatchType = value
        End Set
    End Property
    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _PinBatchID = sqlRdr("PinBatchID")
        _PinBatch = sqlRdr("PinBatch")
        _BatchDate = sqlRdr("BatchDate")
        _PinBatchType = New xPinBatchType(sqlRdr)
    End Sub
    Public Overrides Function ToString() As String
        Return _PinBatch
    End Function
End Class
Public Class xPinBatchAdapter
    'all under here added by KMR 16 dec 2012 from office web server

    Public Shared Function List(ByVal PinBatchTypeID As Integer, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xPinBatch)
        Dim iList As New List(Of xPinBatch)
        Using sqlCmd As New SqlCommand("xPinBatch_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("PinBatchTypeID", PinBatchTypeID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xPinBatch(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Sub Insert(ByRef iPinBatch As xPinBatch, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
        Using sqlCmd As New SqlCommand("xPinBatch_Insert", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("PinBatch", iPinBatch.PinBatch)
            sqlCmd.Parameters.AddWithValue("PinBatchTypeID", iPinBatch.PinBatchType.PinBatchTypeID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iPinBatch.PinBatchID = sqlRdr("PinBatchID")
                iPinBatch.BatchDate = sqlRdr("BatchDate")
                sqlRdr.Close()
            End Using
        End Using
    End Sub
End Class