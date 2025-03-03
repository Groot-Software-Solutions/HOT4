Imports System.Data.SqlClient

Public Class xNetwork

    Public Enum Networks As Integer
        Econet = 1
        NetOne = 2
        Telecel = 3
        Africom = 4
        Umax = 5
        Powertel = 6
        Econet078 = 7
    End Enum
    Private _NetworkID As Integer
    Public Property NetworkID() As Integer
        Get
            Return _NetworkID
        End Get
        Set(ByVal value As Integer)
            _NetworkID = value
        End Set
    End Property

    Private _Network As String
    Public Property Network() As String
        Get
            Return _Network
        End Get
        Set(ByVal value As String)
            _Network = value
        End Set
    End Property

    Private _Prefix As String
    Public Property Prefix() As String
        Get
            Return _Prefix
        End Get
        Set(ByVal value As String)
            _Prefix = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _NetworkID = sqlRdr("NetworkID")
        _Network = sqlRdr("Network")
        _Prefix = sqlRdr("Prefix")
    End Sub
    Public Overrides Function ToString() As String
        Return _Network
    End Function
End Class

Public Class xNetwork_Data
    Public Shared Function Identify(ByVal Mobile As String, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xNetwork
        Dim iNetwork As xNetwork = Nothing
        Using sqlCmd As New SqlCommand("xNetwork_Identify", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("Mobile", Mobile)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                If sqlRdr.Read() Then
                    iNetwork = New xNetwork(sqlRdr)
                End If
                sqlRdr.Close()
            End Using
        End Using
        Return iNetwork
    End Function
End Class