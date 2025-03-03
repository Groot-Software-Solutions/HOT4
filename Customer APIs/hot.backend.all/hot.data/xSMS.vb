Imports System.Data.SqlClient

Public Class xSMS

    Private _SMSID As Long
    Public Property SMSID() As Long
        Get
            Return _SMSID
        End Get
        Set(ByVal value As Long)
            _SMSID = value
        End Set
    End Property

    Private _SmppID As Nullable(Of Integer)
    Public Property SmppID() As Nullable(Of Integer)
        Get
            Return _SmppID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _SmppID = value
        End Set
    End Property

    Private _State As xState
    Public Property State() As xState
        Get
            Return _State
        End Get
        Set(ByVal value As xState)
            _State = value
        End Set
    End Property

    Private _Priority As xPriority
    Public Property Priority() As xPriority
        Get
            Return _Priority
        End Get
        Set(ByVal value As xPriority)
            _Priority = value
        End Set
    End Property

    Private _Direction As Boolean
    Public Property Direction() As Boolean
        Get
            Return _Direction
        End Get
        Set(ByVal value As Boolean)
            _Direction = value
        End Set
    End Property

    Private _Mobile As String
    Public Property Mobile() As String
        Get
            Return _Mobile
        End Get
        Set(ByVal value As String)
            _Mobile = value
        End Set
    End Property

    Private _SMSText As String
    Public Property SMSText() As String
        Get
            Return _SMSText
        End Get
        Set(ByVal value As String)
            _SMSText = value
        End Set
    End Property

    Private _SMSDate As Date
    Public Property SMSDate() As Date
        Get
            Return _SMSDate
        End Get
        Set(ByVal value As Date)
            _SMSDate = value
        End Set
    End Property

    Private _SMSID_In As Nullable(Of Long)
    Public Property SMSID_In() As Nullable(Of Long)
        Get
            Return _SMSID_In
        End Get
        Set(ByVal value As Nullable(Of Long))
            _SMSID_In = value
        End Set
    End Property

    Public Property InsertDate As Date


    Sub New()
        _Priority = New xPriority
        _State = New xState
    End Sub    
    Sub New(ByVal sqlRdr As SqlDataReader)
        _SMSID = sqlRdr("SMSID")
        If Not IsDBNull(sqlRdr("SmppID")) Then _SmppID = CInt(sqlRdr("SmppID"))
        _State = New xState(sqlRdr)
        _Priority = New xPriority(sqlRdr)
        _Direction = sqlRdr("Direction")
        _Mobile = sqlRdr("Mobile")
        _SMSText = sqlRdr("SMSText")
        _SMSDate = sqlRdr("SMSDate")
        InsertDate = sqlRdr("InsertDate")
        If Not IsDBNull(sqlRdr("SMSID_In")) Then _SMSID_In = sqlRdr("SMSID_In")
    End Sub
End Class
Public Class xSMSAdapter
    Public Shared Sub Save(ByVal iSMS As xSMS, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing)
        Using sqlCmd As New SqlCommand("xSMS_Save", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("SMSID", iSMS.SMSID)
            sqlCmd.Parameters.AddWithValue("SmppID", iSMS.SmppID)
            sqlCmd.Parameters.AddWithValue("StateID", iSMS.State.StateID)
            sqlCmd.Parameters.AddWithValue("PriorityID", iSMS.Priority.PriorityID)
            sqlCmd.Parameters.AddWithValue("Direction", iSMS.Direction)
            sqlCmd.Parameters.AddWithValue("Mobile", iSMS.Mobile)
            sqlCmd.Parameters.AddWithValue("SMSText", iSMS.SMSText)
            sqlCmd.Parameters.AddWithValue("SMSID_In", iSMS.SMSID_In)
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Sub Resend(ByVal Mobile As String, ByVal RechargeMobile As String, ByVal sqlConn As SqlConnection)
        Using sqlCmd As New SqlCommand("xResend", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("Mobile", Mobile)
            sqlCmd.Parameters.AddWithValue("RechargeMobile", RechargeMobile)            
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Shared Sub BulkSend(ByVal MessageText As String, ByVal sqlConn As SqlConnection)
        Using sqlCmd As New SqlCommand("xSMS_BulkSend", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("MessageText", MessageText)
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function Duplicate(ByVal iSMS As xSMS, ByVal sqlConn As SqlConnection) As xSMS
        Dim iRow As xSMS = Nothing
        Using sqlCmd As New SqlCommand("xDuplicateRecharge", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("SMSID", iSMS.SMSID)
            sqlCmd.Parameters.AddWithValue("SMSText", iSMS.SMSText)
            sqlCmd.Parameters.AddWithValue("Mobile", iSMS.Mobile)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                If sqlRdr.Read() Then iRow = New xSMS(sqlRdr)
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function    
    'Public Shared Function Resend(ByVal iSMS As xSMS, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction) As Integer
    '    Dim Count As Integer = 0
    '    Using sqlCmd As New SqlCommand("xResend", sqlConn, sqlTrans)
    '        sqlCmd.CommandType = CommandType.StoredProcedure
    '        sqlCmd.Parameters.AddWithValue("Mobile", iSMS.Mobile)
    '        sqlCmd.Parameters.AddWithValue("Resend", iSMS.SMSText.ToUpper.Replace("RESEND ", ""))
    '        Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
    '            Count = sqlRdr.RecordsAffected
    '            sqlRdr.Close()
    '        End Using
    '    End Using
    '    Return Count
    'End Function
    Public Shared Function Outbox(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xSMS)
        Dim iList As New List(Of xSMS)
        Using sqlCmd As New SqlCommand("xSMS_Outbox", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xSMS(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function Inbox(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xSMS)
        Dim iList As New List(Of xSMS)
        Using sqlCmd As New SqlCommand("xSMS_Inbox", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xSMS(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function SMSListIn(ByVal AccountID As Long, ByVal SMSDate As Date, ByVal sqlConn As SqlConnection) As List(Of xSMS)
        Dim iList As New List(Of xSMS)
        Using sqlCmd As New SqlCommand("xSMS_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
            sqlCmd.Parameters.AddWithValue("SMSDate", SMSDate)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xSMS(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function SMSListOut(ByVal SMSID As Long, ByVal sqlConn As SqlConnection) As List(Of xSMS)
        Dim iList As New List(Of xSMS)
        Using sqlCmd As New SqlCommand("xSMS_ListOut", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("SMSID", SMSID)            
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xSMS(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function Search(ByVal StartDate As Date, ByVal EndDate As Date, ByVal Mobile As String, ByVal MessageText As String, _
            ByVal StateID As Integer, ByVal SmppID As Integer, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xSMS)
        Dim iList As New List(Of xSMS)
        Using sqlCmd As New SqlCommand("xSMS_Search", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("StartDate", StartDate)
            sqlCmd.Parameters.AddWithValue("EndDate", EndDate)
            sqlCmd.Parameters.AddWithValue("Mobile", Mobile)
            sqlCmd.Parameters.AddWithValue("MessageText", MessageText)
            sqlCmd.Parameters.AddWithValue("SmppID", SmppID)
            sqlCmd.Parameters.AddWithValue("StateID", StateID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xSMS(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    'Public Shared Function SelectRow(ByVal SMSID As Guid, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xSMS
    '    Dim iSMS As xSMS = Nothing
    '    Using sqlCmd As New SqlCommand("xSMS_Select", sqlConn, sqlTrans)
    '        sqlCmd.CommandType = CommandType.StoredProcedure
    '        sqlCmd.Parameters.AddWithValue("SMSID", SMSID)
    '        Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
    '            sqlRdr.Read()
    '            iSMS = New xSMS(sqlRdr)
    '            sqlRdr.Close()
    '        End Using
    '    End Using
    '    Return iSMS
    'End Function
End Class