Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text

Public Class xAccess

    Private _AccessID As Long
    Public Property AccessID() As Long
        Get
            Return _AccessID
        End Get
        Set(ByVal value As Long)
            _AccessID = value
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

    Private _Channel As xChannel
    Public Property Channel() As xChannel
        Get
            Return _Channel
        End Get
        Set(ByVal value As xChannel)
            _Channel = value
        End Set
    End Property

    Private _AccessCode As String
    Public Property AccessCode() As String
        Get
            Return _AccessCode
        End Get
        Set(ByVal value As String)
            _AccessCode = value
        End Set
    End Property

    Private _AccessPassword As String
    Public Property AccessPassword() As String
        Get
            Return _AccessPassword
        End Get
        Set(ByVal value As String)
            _AccessPassword = value
        End Set
    End Property

    Private _Deleted As Boolean

    Public Property Deleted() As Boolean
        Get
            Return _Deleted
        End Get
        Set(ByVal value As Boolean)
            _Deleted = value
        End Set
    End Property

    Public Property PasswordSalt As String
        Get
            Return _passwordSalt
        End Get
        Set(ByVal value As String)
            _passwordSalt = value
        End Set
    End Property

    Private _passwordHash As String
    Private _passwordSalt As String

    Public Property PasswordHash As String
        Get
            Return _passwordHash
        End Get
        Set(ByVal value As String)
            _passwordHash = value
        End Set
    End Property

    Sub New()
        _Channel = New xChannel
    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _AccessID = sqlRdr("AccessID")
        _AccountID = sqlRdr("AccountID")
        _Channel = New xChannel(sqlRdr)
        _AccessCode = sqlRdr("AccessCode")
        _AccessPassword = sqlRdr("AccessPassword")
        _passwordHash = IIf(HasColumn(sqlRdr, "PasswordHash"), sqlRdr("PasswordHash"), "")
        _passwordSalt = IIf(HasColumn(sqlRdr, "PasswordSalt"), sqlRdr("PasswordSalt"), "")
    End Sub

    Private Shared Function HasColumn(dr As SqlDataReader, columnName As String) As Boolean
        For i As Integer = 0 To dr.FieldCount
            If dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase) Then Return True
        Next
        Return False
    End Function


End Class
Public Class xAccessAdapter
    Public Shared Sub Save(ByRef iAccess As xAccess, ByVal sqlConn As SqlConnection, Optional ByVal SqlTrans As SqlTransaction = Nothing)
        Using sqlCmd As New SqlCommand("xAccess_Save2", sqlConn, SqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccessID", iAccess.AccessID)
            sqlCmd.Parameters.AddWithValue("AccountID", iAccess.AccountID)
            sqlCmd.Parameters.AddWithValue("ChannelID", iAccess.Channel.ChannelID)
            sqlCmd.Parameters.AddWithValue("AccessCode", iAccess.AccessCode)
            sqlCmd.Parameters.AddWithValue("AccessPassword", iAccess.AccessPassword)

            Dim salt As String = PasswordHasher.GenerateSalt()
            Dim passwordHash As String = PasswordHasher.GenerateHash(iAccess.AccessPassword, salt)
            sqlCmd.Parameters.AddWithValue("PasswordHash", passwordHash)
            sqlCmd.Parameters.AddWithValue("PasswordSalt", salt)
            'iAccess.AccessPassword = Nothing               

            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iAccess.AccessID = sqlRdr("AccessID")
                sqlRdr.Close()
            End Using
        End Using
    End Sub

    Public Shared Function SelectCode(ByVal AccessCode As String, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xAccess
        Dim iRow As xAccess = Nothing
        Using sqlCmd As New SqlCommand("xAccess_SelectCode", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccessCode", AccessCode)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                If sqlRdr.Read() Then
                    iRow = New xAccess(sqlRdr)
                End If
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function
    Public Shared Function SelectLogin(accessCode As String, accessPassword As String, sqlConn As SqlConnection) As xAccess
        Dim iAccess As xAccess = Nothing
        Using sqlCmd As New SqlCommand("xAccess_SelectLogin", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccessCode", accessCode)
            sqlCmd.Parameters.AddWithValue("AccessPassword", accessPassword)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                If sqlRdr.Read() Then
                    iAccess = New xAccess(sqlRdr)
                End If
                sqlRdr.Close()
            End Using
        End Using
        Return iAccess
    End Function

    ' TODO: Disabled until hashing implemented
    '     Public Shared Function SelectLogin(accessCode As String, accessPassword As String, sqlConn As SqlConnection) As xAccess
    '        Dim iAccess As xAccess = Nothing
    '        Using sqlCmd As New SqlCommand("xAccess_SelectLogin2", sqlConn)
    '            sqlCmd.CommandType = CommandType.StoredProcedure
    '            sqlCmd.Parameters.AddWithValue("AccessCode", AccessCode)
    '            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
    '                If sqlRdr.Read() Then
    '                    iAccess = New xAccess(sqlRdr)
    '                    If Not PasswordHasher.VerifyPassword(AccessPassword, iAccess.PasswordSalt, iAccess.PasswordHash) Then                       
    '                        Return Nothing
    '                    End If
    '                End If
    '                sqlRdr.Close()
    '            End Using
    '        End Using
    '        Return iAccess
    '    End Function

    Public Shared Function SelectRow(ByVal AccessID As Long, ByVal sqlConn As SqlConnection) As xAccess
        Dim iRow As xAccess = Nothing
        Using sqlCmd As New SqlCommand("xAccess_Select", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccessID", AccessID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                If sqlRdr.Read() Then
                    iRow = New xAccess(sqlRdr)
                End If
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function

    'all under here added by KMR 16 dec 2012 from office web server

    Public Shared Sub Delete(ByVal iAccess As xAccess, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
        Using sqlCmd As New SqlCommand("xAccess_Delete", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccessID", iAccess.AccessID)
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Sub UnDelete(ByVal iAccess As xAccess, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
        Using sqlCmd As New SqlCommand("xAccess_UnDelete", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccessID", iAccess.AccessID)
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Shared Sub PasswordChange(ByVal iAccess As xAccess, ByVal NewPassword As String, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
        Using sqlCmd As New SqlCommand("xAccess_PasswordChange2", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccessID", iAccess.AccessID)

            Dim salt As String = PasswordHasher.GenerateSalt()
            Dim passwordHash As String = PasswordHasher.GenerateHash(NewPassword, salt)
            sqlCmd.Parameters.AddWithValue("PasswordHash", passwordHash)
            sqlCmd.Parameters.AddWithValue("PasswordSalt", salt)
            iAccess.AccessPassword = Nothing
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function AdminSelect(ByVal AccountID As Long, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As Long
        Dim iRow As Long = Nothing
        Using sqlCmd As New SqlCommand("xAccess_Admin_Select", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                If sqlRdr.Read() Then
                    iRow = sqlRdr.Item("AdminID")
                End If
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function

    Public Shared Function List(ByVal AccountID As Long, ByVal sqlConn As SqlConnection) As List(Of xAccess)
        Dim iList As New List(Of xAccess)
        Using sqlCmd As New SqlCommand("xAccess_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xAccess(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function

    Public Shared Function ListDeleted(ByVal AccountID As Long, ByVal sqlConn As SqlConnection) As List(Of xAccess)
        Dim iList As New List(Of xAccess)
        Using sqlCmd As New SqlCommand("xAccess_ListDeleted", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xAccess(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function


End Class



Public Class PasswordHasher
    Public Shared Function GenerateHash(password As String, salt As String) As String

        Dim bytes = Encoding.UTF8.GetBytes(salt + password)
        Dim algorithm As MD5 = MD5.Create
        Dim hashBytes = algorithm.ComputeHash(bytes)
        Return GetString(hashBytes)
    End Function

    Public Shared Function VerifyPassword(password As String, salt As String, hash As String) As Boolean
        Return (GenerateHash(password, salt) = hash)
    End Function

    Public Shared Function GenerateSalt() As String
        Dim rng = New RNGCryptoServiceProvider
        Dim buffer = New Byte((10) - 1) {}
        rng.GetBytes(buffer)
        Return GetString(buffer)
    End Function

    Private Shared Function GetString(buffer() As Byte) As String
        Dim sb = New StringBuilder
        For Each b As Byte In buffer
            sb.Append(b.ToString("x2"))
        Next
        Return sb.ToString()
    End Function

End Class
