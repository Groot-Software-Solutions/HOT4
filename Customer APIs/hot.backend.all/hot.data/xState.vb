Imports System.Data.SqlClient

Public Class xState

    Public Enum States As Integer
        Pending = 0
        Busy = 1
        Success = 2
        Failure = 3
        PendingVerification = 4
    End Enum

    Private _StateID As Integer
    Public Property StateID() As Integer
        Get
            Return _StateID
        End Get
        Set(ByVal value As Integer)
            _StateID = value
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
        _StateID = sqlRdr("StateID")
        _State = sqlRdr("State")
    End Sub

    Public Sub New(id As Integer)
        _StateID = id
        _State = DirectCast(id, States).ToString()
    End Sub

    Public Overrides Function ToString() As String
        Return _State
    End Function
End Class
Public Class xState_Data
    Public Shared Function List(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xState)
        Dim iList As New List(Of xState)
        Using sqlCmd As New SqlCommand("xState_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xState(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class