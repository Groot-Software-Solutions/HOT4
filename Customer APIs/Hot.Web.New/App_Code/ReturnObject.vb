Imports Microsoft.VisualBasic

Public Class ReturnObject
    Public Enum Returncodes
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

    Private _ReturnCode As Integer
    Public Property ReturnCode() As Integer
        Get
            Return _ReturnCode
        End Get
        Set(ByVal value As Integer)
            _ReturnCode = value
        End Set
    End Property

    Private _ReturnMsg As String
    Public Property ReturnMsg() As String
        Get
            Return _ReturnMsg
        End Get
        Set(ByVal value As String)
            _ReturnMsg = value
        End Set
    End Property


    Private _ReturnValue As Decimal
    Public Property ReturnValue() As Decimal
        Get
            Return _ReturnValue
        End Get
        Set(ByVal value As Decimal)
            _ReturnValue = value
        End Set
    End Property

    Sub New()

    End Sub

End Class