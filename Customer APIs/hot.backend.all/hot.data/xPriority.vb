Imports System.Data.SqlClient

Public Class xPriority

    Public Enum Priorities
        Low = 0
        Normal = 1
        High = 2
    End Enum

    Private _PriorityID As Integer
    Public Property PriorityID() As Integer
        Get
            Return _PriorityID
        End Get
        Set(ByVal value As Integer)
            _PriorityID = value
        End Set
    End Property

    Private _Priority As String
    Public Property Priority() As String
        Get
            Return _Priority
        End Get
        Set(ByVal value As String)
            _Priority = value
        End Set
    End Property

    Sub New()

    End Sub    
    Sub New(ByVal sqlRdr As SqlDataReader)
        _PriorityID = sqlRdr("PriorityID")
        _Priority = sqlRdr("Priority")
    End Sub
    Public Overrides Function ToString() As String
        Return _Priority
    End Function
End Class
Public Class xPriority_Data
    Public Shared Function List(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xPriority)
        Dim iList As New List(Of xPriority)
        Using sqlCmd As New SqlCommand("xPriority_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xPriority(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class