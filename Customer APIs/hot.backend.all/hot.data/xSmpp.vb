Imports System.Data.SqlClient

Public Class xSmpp

    Private _SmppID As Integer
    Public Property SmppID() As Integer
        Get
            Return _SmppID
        End Get
        Set(ByVal value As Integer)
            _SmppID = value
        End Set
    End Property

    Private _SmppName As String
    Public Property SmppName() As String
        Get
            Return _SmppName
        End Get
        Set(ByVal value As String)
            _SmppName = value
        End Set
    End Property

    Private _AllowSend As Boolean
    Public Property AllowSend() As Boolean
        Get
            Return _AllowSend
        End Get
        Set(ByVal value As Boolean)
            _AllowSend = value
        End Set
    End Property

    Private _AllowReceive As Boolean
    Public Property AllowReceive() As Boolean
        Get
            Return _AllowReceive
        End Get
        Set(ByVal value As Boolean)
            _AllowReceive = value
        End Set
    End Property

    Private _SmppEnabled As Boolean
    Public Property SmppEnabled() As Boolean
        Get
            Return _SmppEnabled
        End Get
        Set(ByVal value As Boolean)
            _SmppEnabled = value
        End Set
    End Property

    Private _DestinationAddressNpi As Integer
    Public Property DestinationAddressNpi() As Integer
        Get
            Return _DestinationAddressNpi
        End Get
        Set(ByVal value As Integer)
            _DestinationAddressNpi = value
        End Set
    End Property

    Private _DestinationAddressTon As Integer
    Public Property DestinationAddressTon() As Integer
        Get
            Return _DestinationAddressTon
        End Get
        Set(ByVal value As Integer)
            _DestinationAddressTon = value
        End Set
    End Property

    Private _SourceAddress As String
    Public Property SourceAddress() As String
        Get
            Return _SourceAddress
        End Get
        Set(ByVal value As String)
            _SourceAddress = value
        End Set
    End Property

    Private _SourceAddressNpi As Integer
    Public Property SourceAddressNpi() As Integer
        Get
            Return _SourceAddressNpi
        End Get
        Set(ByVal value As Integer)
            _SourceAddressNpi = value
        End Set
    End Property

    Private _SourceAddressTon As Integer
    Public Property SourceAddressTon() As Integer
        Get
            Return _SourceAddressTon
        End Get
        Set(ByVal value As Integer)
            _SourceAddressTon = value
        End Set
    End Property

    Private _SmppTimeout As Integer
    Public Property SmppTimeout() As Integer
        Get
            Return _SmppTimeout
        End Get
        Set(ByVal value As Integer)
            _SmppTimeout = value
        End Set
    End Property

    Private _RemoteHost As String
    Public Property RemoteHost() As String
        Get
            Return _RemoteHost
        End Get
        Set(ByVal value As String)
            _RemoteHost = value
        End Set
    End Property

    Private _RemotePort As Integer
    Public Property RemotePort() As Integer
        Get
            Return _RemotePort
        End Get
        Set(ByVal value As Integer)
            _RemotePort = value
        End Set
    End Property

    Private _SystemID As String
    Public Property SystemID() As String
        Get
            Return _SystemID
        End Get
        Set(ByVal value As String)
            _SystemID = value
        End Set
    End Property

    Private _SmppPassword As String
    Public Property SmppPassword() As String
        Get
            Return _SmppPassword
        End Get
        Set(ByVal value As String)
            _SmppPassword = value
        End Set
    End Property

    Private _AddressRange As String
    Public Property AddressRange() As String
        Get
            Return _AddressRange
        End Get
        Set(ByVal value As String)
            _AddressRange = value
        End Set
    End Property

    Private _InterfaceVersion As Integer
    Public Property InterfaceVersion() As Integer
        Get
            Return _InterfaceVersion
        End Get
        Set(ByVal value As Integer)
            _InterfaceVersion = value
        End Set
    End Property

    Private _SystemType As String
    Public Property SystemType() As String
        Get
            Return _SystemType
        End Get
        Set(ByVal value As String)
            _SystemType = value
        End Set
    End Property

    'Private _TimerInterval As Integer
    'Public Property TimerInterval() As Integer
    '    Get
    '        Return _TimerInterval
    '    End Get
    '    Set(ByVal value As Integer)
    '        _TimerInterval = value
    '    End Set
    'End Property

    'Private _HOTRecharge As Boolean
    'Public Property HOTRecharge() As Boolean
    '    Get
    '        Return _HOTRecharge
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _HOTRecharge = value
    '    End Set
    'End Property

    Private _EconetPrefix As String
    Public Property EconetPrefix() As String
        Get
            Return _EconetPrefix
        End Get
        Set(ByVal value As String)
            _EconetPrefix = value
        End Set
    End Property

    Private _NetOnePrefix As String
    Public Property NetOnePrefix() As String
        Get
            Return _NetOnePrefix
        End Get
        Set(ByVal value As String)
            _NetOnePrefix = value
        End Set
    End Property

    Private _TelecelPrefix As String
    Public Property TelecelPrefix() As String
        Get
            Return _TelecelPrefix
        End Get
        Set(ByVal value As String)
            _TelecelPrefix = value
        End Set
    End Property


    Private Sub Fill(ByVal sqlRdr As SqlDataReader)
        _SmppID = sqlRdr("SmppID")
        _SmppName = sqlRdr("SmppName")
        _AllowSend = sqlRdr("AllowSend")
        _AllowReceive = sqlRdr("AllowReceive")
        _SmppEnabled = sqlRdr("SmppEnabled")
        _DestinationAddressNpi = sqlRdr("DestinationAddressNpi")
        _DestinationAddressTon = sqlRdr("DestinationAddressTon")
        _SourceAddress = sqlRdr("SourceAddress")
        _SourceAddressNpi = sqlRdr("SourceAddressNpi")
        _SourceAddressTon = sqlRdr("SourceAddressTon")
        _SmppTimeout = sqlRdr("SmppTimeout")
        _RemoteHost = sqlRdr("RemoteHost")
        _RemotePort = sqlRdr("RemotePort")
        _SystemID = sqlRdr("SystemID")
        _SmppPassword = sqlRdr("SmppPassword")
        _AddressRange = sqlRdr("AddressRange")
        _InterfaceVersion = sqlRdr("InterfaceVersion")
        _SystemType = sqlRdr("SystemType")
        '_TimerInterval = sqlRdr("TimerInterval")
        '_HOTRecharge = sqlRdr("HOTRecharge")
        _EconetPrefix = sqlRdr("EconetPrefix")
        _NetOnePrefix = sqlRdr("NetOnePrefix")
        _TelecelPrefix = sqlRdr("TelecelPrefix")
    End Sub
    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        Fill(sqlRdr)
    End Sub
    Public Overrides Function ToString() As String
        Return _SmppName
    End Function
End Class
Public Class xSmpp_Data
    'Public Shared Sub Save(ByVal iSmpp As xSmpp, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
    '    Using sqlCmd As New SqlCommand("xSmpp_Save", sqlConn, sqlTrans)
    '        sqlCmd.CommandType = CommandType.StoredProcedure
    '        sqlCmd.Parameters.AddWithValue("SmppID", iSmpp.SmppID)
    '        sqlCmd.Parameters.AddWithValue("SmppName", iSmpp.SmppName)
    '        sqlCmd.Parameters.AddWithValue("AllowSend", iSmpp.AllowSend)
    '        sqlCmd.Parameters.AddWithValue("AllowReceive", iSmpp.AllowReceive)
    '        sqlCmd.Parameters.AddWithValue("SmppEnabled", iSmpp.SmppEnabled)
    '        sqlCmd.Parameters.AddWithValue("DestinationAddressNpi", iSmpp.DestinationAddressNpi)
    '        sqlCmd.Parameters.AddWithValue("DestinationAddressTon", iSmpp.DestinationAddressTon)
    '        sqlCmd.Parameters.AddWithValue("SourceAddress", iSmpp.SourceAddress)
    '        sqlCmd.Parameters.AddWithValue("SourceAddressNpi", iSmpp.SourceAddressNpi)
    '        sqlCmd.Parameters.AddWithValue("SourceAddressTon", iSmpp.SourceAddressTon)
    '        sqlCmd.Parameters.AddWithValue("SmppTimeout", iSmpp.SmppTimeout)
    '        sqlCmd.Parameters.AddWithValue("RemoteHost", iSmpp.RemoteHost)
    '        sqlCmd.Parameters.AddWithValue("RemotePort", iSmpp.RemotePort)
    '        sqlCmd.Parameters.AddWithValue("SystemID", iSmpp.SystemID)
    '        sqlCmd.Parameters.AddWithValue("SmppPassword", iSmpp.SmppPassword)
    '        sqlCmd.Parameters.AddWithValue("AddressRange", iSmpp.AddressRange)
    '        sqlCmd.Parameters.AddWithValue("InterfaceVersion", iSmpp.InterfaceVersion)
    '        sqlCmd.Parameters.AddWithValue("SystemType", iSmpp.SystemType)
    '        sqlCmd.Parameters.AddWithValue("TimerInterval", iSmpp.TimerInterval)
    '        sqlCmd.Parameters.AddWithValue("HOTRecharge", iSmpp.HOTRecharge)
    '        sqlCmd.Parameters.AddWithValue("EconetPrefix", iSmpp.EconetPrefix)
    '        sqlCmd.Parameters.AddWithValue("NetOnePrefix", iSmpp.NetOnePrefix)
    '        sqlCmd.Parameters.AddWithValue("TelecelPrefix", iSmpp.TelecelPrefix)
    '        sqlCmd.ExecuteNonQuery()
    '    End Using
    'End Sub
    'Public Shared Sub Delete(ByVal iSmpp As xSmpp, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
    '    Using sqlCmd As New SqlCommand("xSmpp_Delete", sqlConn, sqlTrans)
    '        sqlCmd.CommandType = CommandType.StoredProcedure
    '        sqlCmd.Parameters.AddWithValue("SmppID", iSmpp.SmppID)
    '        sqlCmd.ExecuteNonQuery()
    '    End Using
    'End Sub
    Public Shared Function List(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xSmpp)
        Dim iList As New List(Of xSmpp)
        Using sqlCmd As New SqlCommand("xSmpp_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xSmpp(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    'Public Shared Function SelectRow(ByVal SmppID As Guid, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xSmpp
    '    Dim iSmpp As xSmpp = Nothing
    '    Using sqlCmd As New SqlCommand("xSmpp_Select", sqlConn, sqlTrans)
    '        sqlCmd.CommandType = CommandType.StoredProcedure
    '        sqlCmd.Parameters.AddWithValue("SmppID", SmppID)
    '        Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
    '            sqlRdr.Read()
    '            iSmpp = New xSmpp(sqlRdr)
    '            sqlRdr.Close()
    '        End Using
    '    End Using
    '    Return iSmpp
    'End Function
End Class