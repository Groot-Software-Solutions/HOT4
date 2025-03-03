Imports System.Data.SqlClient

Public Class xBankTrxState

    Public Enum BankTrxStates
        Pending = 0
        Success = 1
        Suspended = 2
        Ignore = 3
        Expired = 4
        Failed = 5
        BusyConfirming = 6
        Confirmed = 7
        Rejected = 8
        ToBeAllocated = 9
    End Enum

    Private _BankTrxStateID As Integer
    Public Property BankTrxStateID() As Integer
        Get
            Return _BankTrxStateID
        End Get
        Set(ByVal value As Integer)
            _BankTrxStateID = value
        End Set
    End Property

    Private _BankTrxState As String
    Public Property BankTrxState() As String
        Get
            Return _BankTrxState
        End Get
        Set(ByVal value As String)
            _BankTrxState = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _BankTrxStateID = sqlRdr("BankTrxStateID")
        _BankTrxState = sqlRdr("BankTrxState")
    End Sub
    Public Overrides Function ToString() As String
        Return _BankTrxState
    End Function

End Class
Public Class xBankTrxStateAdapter
    Public Shared Function List(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xBankTrxState)
        Dim iList As New List(Of xBankTrxState)
        Using sqlCmd As New SqlCommand("xBankTrxState_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xBankTrxState(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class