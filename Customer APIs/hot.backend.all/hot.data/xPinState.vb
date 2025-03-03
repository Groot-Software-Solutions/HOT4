Imports System.Data.SqlClient

Public Class xPinState

    Public Enum PinStates
        Available = 0
        SoldHotRecharge = 1
        SoldHotBanking = 2
        SoldFileExport = 3
        DuplicateAvailable = 10
        DuplicateSold = 11
        DoNotSell = 12
    End Enum

    Private _PinStateID As Integer
    Public Property PinStateID() As Integer
        Get
            Return _PinStateID
        End Get
        Set(ByVal value As Integer)
            _PinStateID = value
        End Set
    End Property

    Private _PinState As String
    Public Property PinState() As String
        Get
            Return _PinState
        End Get
        Set(ByVal value As String)
            _PinState = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _PinStateID = sqlRdr("PinStateID")
        _PinState = sqlRdr("PinState")
    End Sub
    Public Overrides Function ToString() As String
        Return _PinState
    End Function

End Class
