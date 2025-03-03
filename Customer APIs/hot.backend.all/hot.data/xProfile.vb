Imports System.Data.SqlClient

Public Class xProfile

    Private _ProfileID As Integer
    Public Property ProfileID() As Integer
        Get
            Return _ProfileID
        End Get
        Set(ByVal value As Integer)
            _ProfileID = value
        End Set
    End Property

    Private _ProfileName As String
    Public Property ProfileName() As String
        Get
            Return _ProfileName
        End Get
        Set(ByVal value As String)
            _ProfileName = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _ProfileID = sqlRdr("ProfileID")
        _ProfileName = sqlRdr("ProfileName")
    End Sub
    Public Overrides Function ToString() As String
        Return _ProfileName
    End Function
End Class

Public Class xProfileAdapter
    Public Shared Function Discounts(ByVal ProfileID As Integer, ByVal sqlConn As SqlConnection) As List(Of xProfileDiscount)
        Dim iList As New List(Of xProfileDiscount)
        Using sqlCmd As New SqlCommand("xProfileDiscount_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("ProfileID", ProfileID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xProfileDiscount(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    'all under here added by KMR 16 dec 2012 from office web server

    Public Shared Function List(ByVal sqlConn As SqlConnection) As List(Of xProfile)
        Dim iList As New List(Of xProfile)
        Using sqlCmd As New SqlCommand("xProfile_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xProfile(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class