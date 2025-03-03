Imports System.Data.SqlClient
Public Class xvPayment

    Private _BankTrxID As Integer
    Public Property BankTrxID() As Integer
        Get
            Return _BankTrxID
        End Get
        Set(ByVal value As Integer)
            _BankTrxID = value
        End Set
    End Property

    Private _vPaymentID As Guid
    Public Property vPaymentID() As Guid
        Get
            Return _vPaymentID
        End Get
        Set(ByVal value As Guid)
            _vPaymentID = value
        End Set
    End Property

    Private _CheckURL As String
    Public Property CheckURL() As String
        Get
            Return _CheckURL
        End Get
        Set(ByVal value As String)
            _CheckURL = value
        End Set
    End Property

    Private _ProcessURL As String
    Public Property ProcessURL() As String
        Get
            Return _ProcessURL
        End Get
        Set(ByVal value As String)
            _ProcessURL = value
        End Set
    End Property

    Private _ErrorMsg As String
    Public Property ErrorMsg() As String
        Get
            Return _ErrorMsg
        End Get
        Set(ByVal value As String)
            _ErrorMsg = value
        End Set
    End Property

    Private _vPaymentRef As String
    Public Property vPaymentRef() As String
        Get
            Return _vPaymentRef
        End Get
        Set(ByVal value As String)
            _vPaymentRef = value
        End Set
    End Property
    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _BankTrxID = sqlRdr("BankTrxID")
        _vPaymentID = sqlRdr("vPaymentID")
        _CheckURL = sqlRdr("CheckURL")
        _ProcessURL = sqlRdr("ProcessURL")
        _ErrorMsg = sqlRdr("ErrorMsg")
        _vPaymentRef = sqlRdr("vPaymentRef")
    End Sub
End Class

    Public Class xBankvPaymentAdapter
    Public Shared Sub Insert(ByRef iBankvPayment As xvPayment, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing)
        Using sqlCmd As New SqlCommand("xBankvPayment_Insert", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("BanktrxID", iBankvPayment.BankTrxID)
            sqlCmd.Parameters.AddWithValue("vPaymentID", iBankvPayment.vPaymentID)
            sqlCmd.Parameters.AddWithValue("CheckURL", iBankvPayment.CheckURL)
            sqlCmd.Parameters.AddWithValue("ProcessURL", iBankvPayment.ProcessURL)
            sqlCmd.Parameters.AddWithValue("ErrorMsg", iBankvPayment.ErrorMsg)
            sqlCmd.Parameters.AddWithValue("vPaymentRef", iBankvPayment.vPaymentRef)
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Sub UpdateRefAndError(ByRef iBankvPayment As xvPayment, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing)
        Using sqlCmd As New SqlCommand("xBankvPayment_Update", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("vPaymentID", iBankvPayment.vPaymentID)
            sqlCmd.Parameters.AddWithValue("ErrorMsg", iBankvPayment.ErrorMsg)
            sqlCmd.Parameters.AddWithValue("vPaymentRef", iBankvPayment.vPaymentRef)
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Shared Function SelectRow(ByRef iBankvPayment As xvPayment, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xvPayment
        Dim ivPayment As New xvPayment
        Using sqlCmd As New SqlCommand("xBankvPayment_Select", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("vPaymentID", iBankvPayment.vPaymentID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    ivPayment = New xvPayment(sqlRdr)
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return ivPayment
    End Function
End Class






