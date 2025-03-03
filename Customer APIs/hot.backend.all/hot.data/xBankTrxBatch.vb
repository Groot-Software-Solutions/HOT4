Imports System.Data.SqlClient

Public Class xBankTrxBatch

    Private _BankTrxBatchID As Long
    Public Property BankTrxBatchID() As Long
        Get
            Return _BankTrxBatchID
        End Get
        Set(ByVal value As Long)
            _BankTrxBatchID = value
        End Set
    End Property

    Private _BankID As Integer
    Public Property BankID() As Integer
        Get
            Return _BankID
        End Get
        Set(ByVal value As Integer)
            _BankID = value
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

    Private _BatchReference As String
    Public Property BatchReference() As String
        Get
            Return _BatchReference
        End Get
        Set(ByVal value As String)
            _BatchReference = value
        End Set
    End Property

    Private _LastUser As String
    Public Property LastUser() As String
        Get
            Return _LastUser
        End Get
        Set(ByVal value As String)
            _LastUser = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _BankTrxBatchID = sqlRdr("BankTrxBatchID")
        _BankID = sqlRdr("BankID") 'New xBank(sqlRdr)
        _BatchDate = sqlRdr("BatchDate")
        _BatchReference = sqlRdr("BatchReference")
        _LastUser = sqlRdr("LastUser")
    End Sub
End Class
Public Class xBankTrxBatchAdapter
    Public Shared Function List(ByVal BankID As Integer, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xBankTrxBatch)
        Dim iList As New List(Of xBankTrxBatch)
        Using sqlCmd As New SqlCommand("xBankTrxBatch_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("BankID", BankID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xBankTrxBatch(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Sub Insert(ByRef iBankTrxBatch As xBankTrxBatch, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing)
        Using sqlCmd As New SqlCommand("xBankTrxBatch_Insert", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("BankID", iBankTrxBatch.BankID)
            sqlCmd.Parameters.AddWithValue("BatchDate", iBankTrxBatch.BatchDate)
            sqlCmd.Parameters.AddWithValue("BatchReference", iBankTrxBatch.BatchReference)
            sqlCmd.Parameters.AddWithValue("LastUser", iBankTrxBatch.LastUser)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iBankTrxBatch.BankTrxBatchID = sqlRdr("BankTrxBatchID")
                sqlRdr.Close()
            End Using
        End Using
    End Sub
    'added by KMR for vPayments and automated email Payments
    Public Shared Function GetCurrentBatch(ByRef iBankTrxBatch As xBankTrxBatch, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xBankTrxBatch
        Dim iBatch As New xBankTrxBatch
        Using sqlCmd As New SqlCommand("xBankTrxBatch_GetCurrentBatch", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("BankID", iBankTrxBatch.BankID)
            sqlCmd.Parameters.AddWithValue("BatchReference", iBankTrxBatch.BatchReference)
            sqlCmd.Parameters.AddWithValue("LastUser", iBankTrxBatch.LastUser)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iBatch = New xBankTrxBatch(sqlRdr)
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iBatch
    End Function
End Class