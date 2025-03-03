Imports System.Data.SqlClient

Public Class xChannel
    Public Enum Channels As Integer
        SMS = 1
        Web = 2
        Email = 3
        ServiceWithNoTag = 4
    End Enum

    Private _ChannelID As Integer
    Public Property ChannelID() As Integer
        Get
            Return _ChannelID
        End Get
        Set(ByVal value As Integer)
            _ChannelID = value
        End Set
    End Property

    Private _Channel As String
    Public Property Channel() As String
        Get
            Return _Channel
        End Get
        Set(ByVal value As String)
            _Channel = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _ChannelID = sqlRdr("ChannelID")
        _Channel = sqlRdr("Channel")
    End Sub
    Public Overrides Function ToString() As String
        Return _Channel
    End Function
End Class

Public Class xChannelAdapter
    Public Shared Function List(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xChannel)
        Dim iList As New List(Of xChannel)
        Using sqlCmd As New SqlCommand("xChannel_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xChannel(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class

