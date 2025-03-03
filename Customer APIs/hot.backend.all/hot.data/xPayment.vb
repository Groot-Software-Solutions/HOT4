Imports System.Data.SqlClient

Public Class xPayment

    Private _PaymentID As Long
    Public Property PaymentID() As Long
        Get
            Return _PaymentID
        End Get
        Set(ByVal value As Long)
            _PaymentID = value
        End Set
    End Property

    Private _AccountID As Long
    Public Property AccountID() As Long
        Get
            Return _AccountID
        End Get
        Set(ByVal value As Long)
            _AccountID = value
        End Set
    End Property

    Private _PaymentType As xPaymentType
    Public Property PaymentType() As xPaymentType
        Get
            Return _PaymentType
        End Get
        Set(ByVal value As xPaymentType)
            _PaymentType = value
        End Set
    End Property

    Private _PaymentSource As xPaymentSource
    Public Property PaymentSource() As xPaymentSource
        Get
            Return _PaymentSource
        End Get
        Set(ByVal value As xPaymentSource)
            _PaymentSource = value
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

    Private _PaymentDate As Date
    Public Property PaymentDate() As Date
        Get
            Return _PaymentDate
        End Get
        Set(ByVal value As Date)
            _PaymentDate = value
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
        _PaymentType = New xPaymentType
        _PaymentSource = New xPaymentSource
    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _PaymentID = sqlRdr("PaymentID")
        _AccountID = sqlRdr("AccountID")
        _PaymentType = New xPaymentType(sqlRdr)
        _PaymentSource = New xPaymentSource(sqlRdr)
        _Amount = sqlRdr("Amount")
        _PaymentDate = sqlRdr("PaymentDate")
        _Reference = sqlRdr("Reference")
        _LastUser = sqlRdr("LastUser")
    End Sub
End Class

Public Class xPaymentAdapter
    Public Shared Sub Save(ByRef iPayment As xPayment, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing)
        Using sqlCmd As New SqlCommand("xPayment_Save", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("PaymentID", iPayment.PaymentID)
            sqlCmd.Parameters.AddWithValue("AccountID", iPayment.AccountID)
            sqlCmd.Parameters.AddWithValue("PaymentTypeID", iPayment.PaymentType.PaymentTypeID)
            sqlCmd.Parameters.AddWithValue("PaymentSourceID", iPayment.PaymentSource.PaymentSourceID)
            sqlCmd.Parameters.AddWithValue("Amount", iPayment.Amount)
            sqlCmd.Parameters.AddWithValue("PaymentDate", iPayment.PaymentDate)
            sqlCmd.Parameters.AddWithValue("Reference", iPayment.Reference)
            sqlCmd.Parameters.AddWithValue("LastUser", iPayment.LastUser)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iPayment.PaymentID = sqlRdr("PaymentID")
                sqlRdr.Close()
            End Using
        End Using
    End Sub

    'all under here added by KMR 16 dec 2012 from office web server

    Public Shared Function List(ByVal AccountID As Long, ByVal sqlConn As SqlConnection) As List(Of xPayment)
        Dim iList As New List(Of xPayment)
        Using sqlCmd As New SqlCommand("xPayment_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xPayment(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function

End Class