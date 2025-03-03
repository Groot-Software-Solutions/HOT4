Imports System.Data.SqlClient

Public Class xAccessWeb
    Private _AccessID As Long
    Public Property AccessID() As Long
        Get
            Return _AccessID
        End Get
        Set(ByVal value As Long)
            _AccessID = value
        End Set
    End Property
    Private _AccessName As String
    Public Property AccessName() As String
        Get
            Return _AccessName
        End Get
        Set(ByVal value As String)
            _AccessName = value
        End Set
    End Property
    Private _WebBackground As String
    Public Property WebBackground() As String
        Get
            Return _WebBackground
        End Get
        Set(ByVal value As String)
            _WebBackground = value
        End Set
    End Property
    Private _SalesPassword As Boolean
    Public Property SalesPassword() As Boolean
        Get
            Return _SalesPassword
        End Get
        Set(ByVal value As Boolean)
            _SalesPassword = value
        End Set
    End Property
    Private _ResetToken As String
    Public Property ResetToken() As String
        Get
            Return _ResetToken
        End Get
        Set(ByVal value As String)
            _ResetToken = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _AccessID = sqlRdr("AccessID")
        _AccessName = sqlRdr("AccessName")
        _WebBackground = sqlRdr("WebBackground")
        _SalesPassword = sqlRdr("SalesPassword")
        _ResetToken = sqlRdr("ResetToken")

    End Sub

End Class

Public Class xAccessWebAdapter
    Public Shared Sub Save(ByRef iAccessWeb As xAccessWeb, ByVal sqlConn As SqlConnection, Optional ByVal SqlTrans As SqlTransaction = Nothing)
        Using sqlCmd As New SqlCommand("xAccessWeb_Save", sqlConn, SqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccessID", iAccessWeb.AccessID)
            sqlCmd.Parameters.AddWithValue("AccessName", iAccessWeb.AccessName)
            sqlCmd.Parameters.AddWithValue("WebBackground", iAccessWeb.WebBackground)
            sqlCmd.Parameters.AddWithValue("SalesPassword", iAccessWeb.SalesPassword)
            sqlCmd.Parameters.AddWithValue("ResetToken", iAccessWeb.ResetToken)
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function SelectRow(ByRef AccessID As Long, ByVal sqlConn As SqlConnection, Optional ByVal SqlTrans As SqlTransaction = Nothing) As xAccessWeb
        Dim iRow As xAccessWeb = Nothing
        Using sqlCmd As New SqlCommand("xAccessWeb_Select", sqlConn, SqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccessID", AccessID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                If sqlRdr.Read() Then
                    iRow = New xAccessWeb(sqlRdr)
                End If
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function

End Class

