Imports System.Data.SqlClient

Public Class xAccount

    Private _AccountID As Long
    Public Property AccountID() As Long
        Get
            Return _AccountID
        End Get
        Set(ByVal value As Long)
            _AccountID = value
        End Set
    End Property

    Private _Profile As xProfile
    Public Property Profile() As xProfile
        Get
            Return _Profile
        End Get
        Set(ByVal value As xProfile)
            _Profile = value
        End Set
    End Property

    Private _AccountName As String
    Public Property AccountName() As String
        Get
            Return _AccountName
        End Get
        Set(ByVal value As String)
            _AccountName = value
        End Set
    End Property

    Private _NationalID As String
    Public Property NationalID() As String
        Get
            Return _NationalID
        End Get
        Set(ByVal value As String)
            _NationalID = value
        End Set
    End Property

    Private _Email As String
    Public Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = value
        End Set
    End Property

    Private _ReferredBy As String
    Public Property ReferredBy() As String
        Get
            Return _ReferredBy
        End Get
        Set(ByVal value As String)
            _ReferredBy = value
        End Set
    End Property

    Private _Balance As Decimal
    Public ReadOnly Property Balance() As Decimal
        Get
            Return _Balance
        End Get
    End Property

    Private _SaleValue As Decimal
    Public ReadOnly Property SaleValue() As Decimal
        Get
            Return _SaleValue
        End Get
    End Property

    Private _ZESABalance As Decimal
    Public ReadOnly Property ZESABalance() As Decimal
        Get
            Return _ZESABalance
        End Get
    End Property

    Private _USDBalance As Decimal
    Public ReadOnly Property USDBalance() As Decimal
        Get
            Return _USDBalance
        End Get
    End Property
    Private _USDUtilityBalance As String
    Public Property USDUtilityBalance() As String
        Get
            Return _USDUtilityBalance
        End Get
        Set(ByVal value As String)
            _USDUtilityBalance = value
        End Set
    End Property

    Sub New()
        _Profile = New xProfile
    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _AccountID = sqlRdr("AccountID")
        _Profile = New xProfile(sqlRdr)
        _AccountName = sqlRdr("AccountName")
        _NationalID = sqlRdr("NationalID")
        _Email = sqlRdr("Email")
        _ReferredBy = sqlRdr("ReferredBy")
        _Balance = sqlRdr("Balance")
        _SaleValue = sqlRdr("SaleValue")
        _ZESABalance = sqlRdr("ZESABalance")
        _USDBalance = sqlRdr("USDBalance")
        _USDUtilityBalance = sqlRdr("USDUtilityBalance")
    End Sub
End Class
Public Class xAccountAdapter
    Public Shared Sub Save(ByRef iAccount As xAccount, ByVal sqlConn As SqlConnection, Optional ByVal SqlTrans As SqlTransaction = Nothing)
        Using sqlCmd As New SqlCommand("xAccount_Save", sqlConn, SqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", iAccount.AccountID)
            sqlCmd.Parameters.AddWithValue("ProfileID", iAccount.Profile.ProfileID)
            sqlCmd.Parameters.AddWithValue("AccountName", iAccount.AccountName)
            sqlCmd.Parameters.AddWithValue("NationalID", iAccount.NationalID)
            sqlCmd.Parameters.AddWithValue("Email", iAccount.Email)
            sqlCmd.Parameters.AddWithValue("ReferredBy", iAccount.ReferredBy)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iAccount.AccountID = sqlRdr("AccountID")
                sqlRdr.Close()
            End Using
        End Using
    End Sub
    Public Shared Function SelectRow(ByVal AccountID As Long, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xAccount
        Dim iRow As xAccount = Nothing
        Using sqlCmd As New SqlCommand("xAccount_Select", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iRow = New xAccount(sqlRdr)
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function

    'all under here added by KMR 16 dec 2012 from office web server
    Public Shared Function Search(ByVal Filter As String, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xAccount)
        Dim iList As New List(Of xAccount)
        Using sqlCmd As New SqlCommand("xAccount_Search", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("Filter", Filter)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xAccount(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function RechargeFind(ByVal AccountID As Long, ByVal Mobile As String, ByVal sqlConn As SqlConnection) As List(Of xRecharge)
        Dim iList As New List(Of xRecharge)
        Using sqlCmd As New SqlCommand("xRecharge_Find2", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
            sqlCmd.Parameters.AddWithValue("Mobile", Mobile)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xRecharge(sqlRdr, sqlConn))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function RechargeFindByMobile(ByVal Mobile As String, ByVal sqlConn As SqlConnection) As List(Of xRecharge)
        Dim iList As New List(Of xRecharge)
        Using sqlCmd As New SqlCommand("xRecharge_Find_By_Mobile", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("Mobile", Mobile)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xRecharge(sqlRdr, sqlConn))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function

End Class