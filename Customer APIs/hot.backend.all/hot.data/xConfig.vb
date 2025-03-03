Imports System.Data.SqlClient

Public Class xConfig

    Private _ConfigID As Integer
    Public Property ConfigID() As Integer
        Get
            Return _ConfigID
        End Get
        Set(ByVal value As Integer)
            _ConfigID = value
        End Set
    End Property

    Private _ProfileID_NewSMSDealer As Integer
    Public Property ProfileID_NewSMSDealer() As Integer
        Get
            Return _ProfileID_NewSMSDealer
        End Get
        Set(ByVal value As Integer)
            _ProfileID_NewSMSDealer = value
        End Set
    End Property

    Private _ProfileID_NewWebDealer As Integer
    Public Property ProfileID_NewWebDealer() As Integer
        Get
            Return _ProfileID_NewWebDealer
        End Get
        Set(ByVal value As Integer)
            _ProfileID_NewWebDealer = value
        End Set
    End Property

    Private _MinRecharge As Decimal
    Public Property MinRecharge() As Decimal
        Get
            Return _MinRecharge
        End Get
        Set(ByVal value As Decimal)
            _MinRecharge = value
        End Set
    End Property

    Private _MaxRecharge As Decimal
    Public Property MaxRecharge() As Decimal
        Get
            Return _MaxRecharge
        End Get
        Set(ByVal value As Decimal)
            _MaxRecharge = value
        End Set
    End Property

    Private _PrepaidEnabled As Boolean
    Public Property PrepaidEnabled() As Boolean
        Get
            Return _PrepaidEnabled
        End Get
        Set(ByVal value As Boolean)
            _PrepaidEnabled = value
        End Set
    End Property

    Private _MinTransfer As Decimal
    Public Property MinTransfer() As Decimal
        Get
            Return _MinTransfer
        End Get
        Set(ByVal value As Decimal)
            _MinTransfer = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _ConfigID = sqlRdr("ConfigID")
        _ProfileID_NewSMSDealer = sqlRdr("ProfileID_NewSMSDealer")
        _ProfileID_NewWebDealer = sqlRdr("ProfileID_NewWebDealer")
        _MinRecharge = sqlRdr("MinRecharge")
        _MaxRecharge = sqlRdr("MaxRecharge")
        _PrepaidEnabled = sqlRdr("PrepaidEnabled")
        _MinTransfer = sqlRdr("MinTransfer")        
    End Sub
End Class
Public Class xConfigAdapter
    Public Shared Function Config(ByVal sqlConn As SqlConnection) As xConfig
        Dim iConfig As xConfig = Nothing
        Using sqlCmd As New SqlCommand("xConfig_Select", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iConfig = New xConfig(sqlRdr)
                sqlRdr.Close()
            End Using
        End Using
        Return iConfig
    End Function
End Class