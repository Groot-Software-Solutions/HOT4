Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Text


Public Class xLog
    Private _LogID As Long

    Public Property LogID() As Long
        Get
            Return _LogID
        End Get
        Set(ByVal value As Long)
            _LogID = value
        End Set
    End Property

    Private _LogDate As Date

    Public Property LogDate() As Date
        Get
            Return _LogDate
        End Get
        Set(ByVal value As Date)
            _LogDate = value
        End Set
    End Property

    Private _LogModule As String

    Public Property LogModule() As String
        Get
            Return _LogModule
        End Get
        Set(ByVal value As String)
            _LogModule = value
        End Set
    End Property

    Private _LogObject As String

    Public Property LogObject() As String
        Get
            Return _LogObject
        End Get
        Set(ByVal value As String)
            _LogObject = value
        End Set
    End Property

    Private _LogMethod As String

    Public Property LogMethod() As String
        Get
            Return _LogMethod
        End Get
        Set(ByVal value As String)
            _LogMethod = value
        End Set
    End Property

    Private _LogDescription As String

    Public Property LogDescription() As String
        Get
            Return _LogDescription
        End Get
        Set(ByVal value As String)
            _LogDescription = value
        End Set
    End Property

    Private _IDType As String

    Public Property IdType() As String
        Get
            Return _IDType
        End Get
        Set(ByVal value As String)
            _IDType = value
        End Set
    End Property

    Private _IDNumber As Integer

    Public Property IDNumber() As Integer
        Get
            Return _IDNumber
        End Get
        Set(ByVal value As Integer)
            _IDNumber = value
        End Set
    End Property


    Public Sub Fill(ByVal sqlRdr As SqlDataReader)
        _LogID = sqlRdr("LogID")
        _LogDate = sqlRdr("LogDate")
        _LogModule = sqlRdr("LogModule")
        _LogObject = sqlRdr("LogObject")
        _LogMethod = sqlRdr("LogMethod")
        _LogDescription = sqlRdr("LogDescription")
        _IDType = sqlRdr("IDType")
        _IDNumber = sqlRdr("IDNumber")
    End Sub

    Sub New()
    End Sub

    Sub New(ByVal sqlRdr As SqlDataReader)
        Fill(sqlRdr)
    End Sub
End Class

Public Class xLog_Data
    Private Const LogName = ""

    Public Shared Sub Save(iLog As xLog, sqlConn As SqlConnection, sqlTrans As SqlTransaction,
                           Optional logOutput As LogOutput = LogOutput.Sql)
        If logOutput = LogOutput.Sql Then
            Save(iLog.LogModule, iLog.LogObject, iLog.LogMethod, iLog.LogDescription,
                 sqlConn, iLog.IdType, iLog.IDNumber, sqlTrans)
        Else
            SaveEventLog(iLog.LogModule, iLog.LogObject, iLog.LogMethod, iLog.IdType, iLog.IDNumber, iLog.LogDescription)
        End If
    End Sub

    Public Shared Sub Save(logModule As String, logObject As String, logMethod As String,
                           logDescription As String, sqlConnectionString As String, Optional idType As String = Nothing, Optional idNumber As String = Nothing)
        Try
            Using sqlConn As New SqlConnection(sqlConnectionString)
                sqlConn.Open()
                Save(logModule, logObject, logMethod, logDescription, sqlConn, idType, idNumber,)
                sqlConn.Close()
            End Using
        Catch ex As Exception
            SaveEventLog(logModule, logObject, logMethod, idType, idNumber, logDescription)
        End Try
    End Sub

    Public Shared Sub Save(logModule As String, logObject As String, logMethod As String,
                           ex As Exception, sqlConnectionString As String, Optional idType As String = Nothing, Optional idNumber As String = Nothing)

        Dim description As StringBuilder = New StringBuilder(ex.ToString())
        If ex.InnerException IsNot Nothing Then
            description.AppendLine(ex.InnerException.ToString())
        End If
        Console.WriteLine(description)
        Save(logModule, logObject, logMethod, description.ToString(), sqlConnectionString, idType, idNumber)
    End Sub

    Public Shared Sub Save(logModule As String, logObject As String, logMethod As String,
                           logDescription As String, sqlConn As SqlConnection,
                           Optional idType As String = Nothing, Optional idNumber As String = Nothing,
                           Optional sqlTrans As SqlTransaction = Nothing)

        Try
            Using sqlCmd As New SqlCommand("xLog_Insert", sqlConn, sqlTrans)
                sqlCmd.CommandType = CommandType.StoredProcedure
                sqlCmd.Parameters.AddWithValue("LogModule", logModule)
                sqlCmd.Parameters.AddWithValue("LogObject", logObject)
                sqlCmd.Parameters.AddWithValue("LogMethod", logMethod)
                sqlCmd.Parameters.AddWithValue("LogDescription", logDescription)
                sqlCmd.Parameters.AddWithValue("IDType", idType)
                sqlCmd.Parameters.AddWithValue("IDNumber", idNumber)
                sqlCmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Try
                SaveEventLog(logModule, logObject, logMethod, idType, idNumber, logDescription)
            Catch

            End Try

        End Try
    End Sub

    Public Shared Sub SaveEventLog(logModule As String, logObject As String, logMethod As String, idType As String,
                                   idNumber As Integer, logDescription As String)
        ' Admin Priviledges Required to Create or Search for EventLog Source. It is better to just 
        ' assume it exist than to give applications admin priviledges i.e. Webservice. Sources can 
        ' be created by powershell script when new Log module is created - BZ

        'If Not EventLog.SourceExists(logModule) Then
        '    EventLog.CreateEventSource(logModule, LogName)
        '    ' Even though this will mean the first error will not be logged, 
        '    ' it is necessary to return here as an EventLog takes a while to be created.
        '    Return
        'End If

        Dim log As New EventLog(LogName, ".", logModule)
        Dim sb As StringBuilder = FormatLog(logModule, logObject, logMethod, idType, idNumber, logDescription)
        log.WriteEntry(sb.ToString(), EventLogEntryType.Error)
    End Sub

    Public Shared Sub SaveConsoleLog(logModule As String, logObject As String, logMethod As String, idType As String,
                                   idNumber As Integer, logDescription As String)
        Dim sb As StringBuilder = FormatLog(logModule, logObject, logMethod, idType, idNumber, logDescription)
        Console.WriteLine(sb.ToString())
    End Sub

    Private Shared Function FormatLog(logModule As String, logObject As String, logMethod As String, idType As String, idNumber As Integer, logDescription As String) As StringBuilder

        Dim sb As StringBuilder = New StringBuilder()
        sb.AppendFormat("An Error Occurred in: {0}{1}", logModule, vbCrLf)
        sb.AppendFormat("   Object:     {0}{1}", logObject, vbCrLf)
        sb.AppendFormat("   Method:     {0}{1}", logMethod, vbCrLf)
        sb.AppendFormat("   ID Type:    {0}{1}", idType, vbCrLf)
        sb.AppendFormat("   ID Number:  {0}{1}", idNumber, vbCrLf)
        sb.AppendLine()
        sb.AppendLine(logDescription)
        Return sb
    End Function

    Public Shared Sub WriteJsonToConsole(obj As Object)
        If obj Is Nothing Then Return
        Console.WriteLine("=== " & obj.GetType().FullName & " ===")
        ' Console.WriteLine(JsonConvert.SerializeObject(obj))
        Console.WriteLine("===")
        Console.WriteLine("===")
        Console.WriteLine("===")
    End Sub
End Class

Public Enum LogOutput
    Sql = 0
    EventLog = 1
    Console = 2
End Enum