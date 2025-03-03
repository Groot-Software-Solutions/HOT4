Imports System.Data.SqlClient

Public Class xTransaction

    Private _TransactionDate As Date
    Public Property TransactionDate() As Date
        Get
            Return _TransactionDate
        End Get
        Set(ByVal value As Date)
            _TransactionDate = value
        End Set
    End Property

    Private _Reference As String
    Public Property Reference() As String
        Get
            Return _Reference
        End Get
        Set(ByVal value As String)
            _Reference = value
        End Set
    End Property

    Private _Amount As Decimal
    Public Property Amount() As Decimal
        Get
            Return _Amount
        End Get
        Set(ByVal value As Decimal)
            _Amount = value
        End Set
    End Property

    Private _State As String
    Public Property State() As String
        Get
            Return _State
        End Get
        Set(ByVal value As String)
            _State = value
        End Set
    End Property
    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _TransactionDate = sqlRdr("TransactionDate")
        _Reference = sqlRdr("Reference")
        _Amount = sqlRdr("Amount")
        _State = sqlRdr("State")
    End Sub

End Class

Public Class xTransaction_Data
    Public Shared Function ListDate(ByVal AccountID As Long, ByVal StartDate As Date, ByVal EndDate As Date, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xTransaction)
        Dim iList As New List(Of xTransaction)
        Using sqlCmd As New SqlCommand("xTransactions_Date", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
            sqlCmd.Parameters.AddWithValue("StartDate", StartDate)
            sqlCmd.Parameters.AddWithValue("EndDate", EndDate)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xTransaction(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function ListSubscriber(ByVal AccountID As Long, ByVal SubscriberID As Long, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xTransaction)
        Dim iList As New List(Of xTransaction)
        Using sqlCmd As New SqlCommand("xTransactions_Subscriber", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
            sqlCmd.Parameters.AddWithValue("SubscriberID", SubscriberID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xTransaction(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class