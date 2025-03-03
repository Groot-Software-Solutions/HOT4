Imports System.Data.SqlClient

Public Class xBankTrx

    Private _BankTrxID As Long
    Public Property BankTrxID() As Long
        Get
            Return _BankTrxID
        End Get
        Set(ByVal value As Long)
            _BankTrxID = value
        End Set
    End Property

    Private _BankTrxBatchID As Long
    Public Property BankTrxBatchID() As Long
        Get
            Return _BankTrxBatchID
        End Get
        Set(ByVal value As Long)
            _BankTrxBatchID = value
        End Set
    End Property

    Private _BankTrxType As xBankTrxType
    Public Property BankTrxType() As xBankTrxType
        Get
            Return _BankTrxType
        End Get
        Set(ByVal value As xBankTrxType)
            _BankTrxType = value
        End Set
    End Property

    Private _BankTrxState As xBankTrxState
    Public Property BankTrxState() As xBankTrxState
        Get
            Return _BankTrxState
        End Get
        Set(ByVal value As xBankTrxState)
            _BankTrxState = value
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

    Private _TrxDate As Date
    Public Property TrxDate() As Date
        Get
            Return _TrxDate
        End Get
        Set(ByVal value As Date)
            _TrxDate = value
        End Set
    End Property

    Private _Identifier As String
    Public Property Identifier() As String
        Get
            Return _Identifier
        End Get
        Set(ByVal value As String)
            _Identifier = value
        End Set
    End Property

    Private _RefName As String
    Public Property RefName() As String
        Get
            Return _RefName
        End Get
        Set(ByVal value As String)
            _RefName = value
        End Set
    End Property

    Private _Branch As String
    Public Property Branch() As String
        Get
            Return _Branch
        End Get
        Set(ByVal value As String)
            _Branch = value
        End Set
    End Property

    Private _BankRef As String
    Public Property BankRef() As String
        Get
            Return _BankRef
        End Get
        Set(ByVal value As String)
            _BankRef = value
        End Set
    End Property

    Private _Balance As Decimal
    Public Property Balance() As Decimal
        Get
            Return _Balance
        End Get
        Set(ByVal value As Decimal)
            _Balance = value
        End Set
    End Property

    Private _PaymentID As Nullable(Of Long)
    Public Property PaymentID() As Nullable(Of Long)
        Get
            Return _PaymentID
        End Get
        Set(ByVal value As Nullable(Of Long))
            _PaymentID = value
        End Set
    End Property

    Sub New()
        _BankTrxState = New xBankTrxState
        _BankTrxType = New xBankTrxType
    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _BankTrxID = sqlRdr("BankTrxID")
        _BankTrxBatchID = sqlRdr("BankTrxBatchID")
        _BankTrxType = New xBankTrxType(sqlRdr)
        _BankTrxState = New xBankTrxState(sqlRdr)
        _Amount = sqlRdr("Amount")
        _TrxDate = sqlRdr("TrxDate")
        _Identifier = sqlRdr("Identifier")
        _RefName = sqlRdr("RefName")
        _Branch = sqlRdr("Branch")
        _BankRef = sqlRdr("BankRef")
        _Balance = sqlRdr("Balance")
        If Not IsDBNull(sqlRdr("PaymentID")) Then _PaymentID = sqlRdr("PaymentID")
    End Sub

End Class
Public Class xBankTrxAdapter
    Public Shared Function List(ByVal BankTrxBatchID As Long, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xBankTrx)
        Dim iList As New List(Of xBankTrx)
        Using sqlCmd As New SqlCommand("xBankTrx_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("BankTrxBatchID", BankTrxBatchID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xBankTrx(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function ListPendingEcoCash(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xBankTrx)
        Dim iList As New List(Of xBankTrx)
        Using sqlCmd As New SqlCommand("xBankTrx_ListPendingEcoCash", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xBankTrx(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function Insert(ByRef iBankTrx As xBankTrx, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing)

        Using sqlCmd As New SqlCommand("xBankTrx_Insert", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("BankTrxBatchID", iBankTrx.BankTrxBatchID)
            sqlCmd.Parameters.AddWithValue("BankTrxTypeID", iBankTrx.BankTrxType.BankTrxTypeID)
            sqlCmd.Parameters.AddWithValue("BankTrxStateID", iBankTrx.BankTrxState.BankTrxStateID)
            sqlCmd.Parameters.AddWithValue("Amount", iBankTrx.Amount)
            sqlCmd.Parameters.AddWithValue("TrxDate", iBankTrx.TrxDate)
            sqlCmd.Parameters.AddWithValue("Identifier", iBankTrx.Identifier)
            sqlCmd.Parameters.AddWithValue("RefName", iBankTrx.RefName)
            sqlCmd.Parameters.AddWithValue("Branch", iBankTrx.Branch)
            sqlCmd.Parameters.AddWithValue("BankRef", iBankTrx.BankRef)
            sqlCmd.Parameters.AddWithValue("Balance", iBankTrx.Balance)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iBankTrx.BankTrxID = sqlRdr("BankTrxID")
                sqlRdr.Close()
            End Using
        End Using
        Return iBankTrx
    End Function
    Public Shared Sub Save(ByRef iBankTrx As xBankTrx, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing)
        Using sqlCmd As New SqlCommand("xBankTrx_Save", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("BankTrxID", iBankTrx.BankTrxID)
            sqlCmd.Parameters.AddWithValue("BankTrxBatchID", iBankTrx.BankTrxBatchID)
            sqlCmd.Parameters.AddWithValue("BankTrxTypeID", iBankTrx.BankTrxType.BankTrxTypeID)
            sqlCmd.Parameters.AddWithValue("BankTrxStateID", iBankTrx.BankTrxState.BankTrxStateID)
            sqlCmd.Parameters.AddWithValue("Amount", iBankTrx.Amount)
            sqlCmd.Parameters.AddWithValue("TrxDate", iBankTrx.TrxDate)
            sqlCmd.Parameters.AddWithValue("Identifier", iBankTrx.Identifier)
            sqlCmd.Parameters.AddWithValue("RefName", iBankTrx.RefName)
            sqlCmd.Parameters.AddWithValue("Branch", iBankTrx.Branch)
            sqlCmd.Parameters.AddWithValue("BankRef", iBankTrx.BankRef)
            sqlCmd.Parameters.AddWithValue("Balance", iBankTrx.Balance)
            sqlCmd.Parameters.AddWithValue("PaymentID", iBankTrx.PaymentID)
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Sub UpdatePaymentID(ByRef iBankTrx As xBankTrx, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing)
        Using sqlCmd As New SqlCommand("xBankTrx_UpdatePaymentID", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("BankTrxID", iBankTrx.BankTrxID)
            sqlCmd.Parameters.AddWithValue("PaymentID", iBankTrx.PaymentID)                        
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Sub UpdateState(ByRef iBankTrx As xBankTrx, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing)
        Using sqlCmd As New SqlCommand("xBankTrx_UpdateState", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("BankTrxID", iBankTrx.BankTrxID)
            sqlCmd.Parameters.AddWithValue("BankTrxStateID", iBankTrx.BankTrxState.BankTrxStateID)
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Shared Function GetFromvPayment(ByVal ivPaymentID As Guid, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xBankTrx
        Dim iBankTrx As xBankTrx = Nothing
        Using sqlCmd As New SqlCommand("xBankTrx_GetFromvPayment", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("vPaymentID", ivPaymentID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iBankTrx = New xBankTrx(sqlRdr)
                sqlRdr.Close()
            End Using
        End Using
        Return iBankTrx
    End Function

    Public Shared Function GetFromBankTrxID(ByVal iBankTrxID As Long, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xBankTrx
        Dim iBankTrx As xBankTrx = Nothing
        Using sqlCmd As New SqlCommand("xBankTrx_GetFromTrxID", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("vPaymentID", iBankTrxID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iBankTrx = New xBankTrx(sqlRdr)
                sqlRdr.Close()
            End Using
        End Using
        Return iBankTrx
    End Function

End Class