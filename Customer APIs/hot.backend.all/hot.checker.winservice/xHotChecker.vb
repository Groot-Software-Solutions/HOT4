Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Data.SqlClient
Imports System.Timers
Imports Hot.Data
Imports HOT44.Checker.Service.Commons
Imports NetOneRechargeClass
Imports NetOneRechargeClass.xDongleMessage

Public Class Commons
    Public Shared SQLConStr As String = "Data Source=HOT5-SVR;Initial Catalog=HOT4;Persist Security Info=True;User ID=Hotadmin;Password=GfFybB4l6"
    Public Shared LogInterval As Long = 60

    Public Shared Sub Log(ByVal LogModule As String, ByVal LogObject As String, ByVal LogMethod As String, ByVal LogDescription As String)
        Try
            Using sqlConn As New SqlConnection(SQLConStr)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    Dim iLog As New xLog
                    iLog.LogModule = LogModule
                    iLog.LogObject = LogObject
                    iLog.LogMethod = LogMethod
                    iLog.LogDescription = LogDescription
                    xLog_Data.Save(iLog, sqlConn, sqlTrans)
                    sqlTrans.Commit()
                Catch ex As Exception
                    sqlTrans.Rollback()
                    Throw ex
                Finally
                    sqlConn.Close()
                End Try
            End Using
        Catch ex As Exception
            '#Catasrophic Failure
        End Try
    End Sub
End Class

Public Class xHotCheckerCheckResult

    Public AlertLevel As AlertLevels = AlertLevels.Information
    Public ResultInformation As String = ""
    Public PingData As New xHotCheckerPingData
    Public WebData As New xHotCheckerWebConnectData
    Public Check As xHotCheckerCheck

    Public AlertTotal As Integer = 0
    Public ResultCollection As New List(Of xHotCheckerCheckResult)

    Enum AlertLevels As Integer
        Information = 1
        LowPriority = 2
        AttentionRequired = 3
        HighPriority = 4
        SeriousProblem = 5
    End Enum

    Sub New()

    End Sub
    Sub New(_check As xHotCheckerCheck)
        Check = _check
    End Sub
End Class

Public Class xHotCheckerCheck
    Enum TestType As Integer
        Unknown = 0
        Ping = 1
        Web = 2
        Dongle = 3
        Balance = 4

    End Enum

    ' Check Config 
    Public ConfigName As String = ""
    Public IPAddress As String = ""
    Public URL As String = ""
    Public ExpectedContent = ""
    Public ExpectedLatency = 300
    Public ErrorLogCheckID As Long = 0
    Public Network As Integer = 0
    Public TestTypeID As TestType = TestType.Unknown
    Public CountThreshold As Integer = 2
    Public ErrorInterval As Integer = 0
    Public CheckInterval As Integer = 0
    Public CheckEnabled As Boolean = False
    Public Port As String = ""
    Public Server As String = ""

    ' Specific to Running Check 
    Public CheckPing As Boolean = False
    Public CheckLatency As Boolean = False
    Public CheckURL As Boolean = False
    Public CheckContent As Boolean = False
    Public CheckDongle As Boolean = False
    Public WithEvents IntervalTimer As New Timer
    Private WithEvents BackgroundProcess As New System.ComponentModel.BackgroundWorker

    ' Check Result "
    Dim ErrorCount As Long = 0
    Dim SuccessCount As Long = 0
    Dim PersistantErrorCount As Long = 0
    Dim PersistantSuccessCount As Long = 0
    Dim CheckResults As New List(Of xHotCheckerCheckResult)

    ' Status Query
    Public ReadOnly Property Status As xHotCheckerCheckResult.AlertLevels
        Get
            Return StatusData.AlertLevel
        End Get
    End Property
    Public ReadOnly Property StatusString As String
        Get
            Return StatusData.ResultInformation
        End Get
    End Property
    ReadOnly Property StatusData() As xHotCheckerCheckResult
        Get
            Dim result As New xHotCheckerCheckResult

            Dim lastresult As xHotCheckerCheckResult.AlertLevels = xHotCheckerCheckResult.AlertLevels.Information
            If CheckResults.Count() > 0 Then lastresult = CheckResults(CheckResults.Count() - 1).AlertLevel
            Dim nearhistory = TakeLast(CheckResults, 5)
            Dim ErrorsInNearHistory = FilterList(nearhistory, xHotCheckerCheckResult.AlertLevels.LowPriority)
            Dim WarningsInNearHistory = FilterList(nearhistory, xHotCheckerCheckResult.AlertLevels.LowPriority, True)

            Dim Errors As Integer = ErrorsInNearHistory.Count()
            Dim HighestErrorLevel As xHotCheckerCheckResult.AlertLevels = xHotCheckerCheckResult.AlertLevels.Information
            If Errors > 0 Then HighestErrorLevel = MostSeriousError(ErrorsInNearHistory).AlertLevel

            Dim ErrorMessages As String = ""
            For Each b As xHotCheckerCheckResult In ErrorsInNearHistory
                If Not ErrorMessages.Contains(b.ResultInformation) Then ErrorMessages += b.ResultInformation
            Next

            Dim Warnings As Integer = WarningsInNearHistory.Count()
            Dim WarningMessages As String = ""
            For Each b As xHotCheckerCheckResult In WarningsInNearHistory
                If Not WarningMessages.Contains(b.ResultInformation) Then WarningMessages += b.ResultInformation
            Next

            If Errors > 0 Then
                result.AlertLevel = HighestErrorLevel
                result.ResultInformation = ErrorMessages
            ElseIf Warnings > 0 Then
                result.AlertLevel = xHotCheckerCheckResult.AlertLevels.LowPriority
                result.ResultInformation = WarningMessages
            Else
                result.AlertLevel = xHotCheckerCheckResult.AlertLevels.Information

            End If
            Return result

        End Get
    End Property
    Private Function TakeLast(ByVal AllResults As List(Of xHotCheckerCheckResult), ByVal Number As Integer) As List(Of xHotCheckerCheckResult)
        Dim result As New List(Of xHotCheckerCheckResult)
        Dim x As Integer
        For x = 1 To Number Step 1
            If ((AllResults.Count - x) < 0) Then Exit For
            result.Add(AllResults(AllResults.Count - x))
        Next
        Return result
    End Function
    Private Function FilterList(ByVal List As List(Of xHotCheckerCheckResult), ByVal alertlevel As xHotCheckerCheckResult.AlertLevels, Optional ByVal equal As Boolean = False) As List(Of xHotCheckerCheckResult)
        Dim result As New List(Of xHotCheckerCheckResult)
        For Each item As xHotCheckerCheckResult In List
            If equal And item.AlertLevel = alertlevel Then result.Add(item)
            If Not equal And item.AlertLevel > alertlevel Then result.Add(item)
        Next
        Return result
    End Function
    Private Function MostSeriousError(ByVal List As List(Of xHotCheckerCheckResult)) As xHotCheckerCheckResult
        Dim result As New xHotCheckerCheckResult
        For Each item As xHotCheckerCheckResult In List
            If item.AlertLevel >= result.AlertLevel Then result = item
        Next
        Return result
    End Function


    Public ReadOnly Property LogItem As xHotCheckerLogItem
        Get  'KMR changed to PersistantErrorCount
            Return New xHotCheckerLogItem(Me.ErrorLogCheckID, PersistantErrorCount, StatusString)
        End Get
    End Property


    Sub New(Optional ByVal _configName As String = "New Config", Optional ByVal _CheckPing As Boolean = False, Optional ByVal _IPAdd As String = "127.0.0.1", _
    Optional ByVal _CheckLatency As Boolean = False, Optional ByVal _Latency As Long = 500, Optional ByVal _CheckURL As Boolean = False, _
    Optional ByVal _URL As String = "", Optional ByVal _CheckContent As Boolean = False, Optional ByVal _ExpectedContent As String = "")
        ConfigName = _configName
        CheckPing = _CheckPing
        CheckLatency = _CheckLatency
        CheckURL = _CheckURL
        CheckContent = _CheckContent
        IPAddress = _IPAdd
        URL = _URL
        ExpectedContent = _ExpectedContent
        ExpectedLatency = _Latency

    End Sub
    Sub New(ByVal spRead As SqlDataReader)
        ReadCheckConfig(spRead)
    End Sub
    Sub ReadCheckConfig(ByVal spRead As SqlDataReader)
        ConfigName = IIf(IsDBNull(spRead.Item("Name")), "", spRead.Item("Name"))
        ErrorLogCheckID = IIf(IsDBNull(spRead.Item("ErrorLogCheckID")), 0, spRead.Item("ErrorLogCheckID"))
        Network = IIf(IsDBNull(spRead.Item("Network")), 0, spRead.Item("Network"))
        TestTypeID = IIf(IsDBNull(spRead.Item("TestType")), 0, spRead.Item("TestType"))
        CountThreshold = IIf(IsDBNull(spRead.Item("CountThreshold")), 0, spRead.Item("CountThreshold"))
        ErrorInterval = IIf(IsDBNull(spRead.Item("ErrorInterval")), 0, (spRead.Item("ErrorInterval") * 1000))
        CheckInterval = IIf(IsDBNull(spRead.Item("CheckInterval")), 30000, (spRead.Item("CheckInterval") * 1000))
        CheckEnabled = IIf(IsDBNull(spRead.Item("Enabled")), False, spRead.Item("Enabled"))
        URL = IIf(IsDBNull(spRead.Item("Url")), "", spRead.Item("Url"))
        IPAddress = IIf(IsDBNull(spRead.Item("HostAddress")), "", spRead.Item("HostAddress"))
        ExpectedContent = IIf(IsDBNull(spRead.Item("ExpectedContent")), "", spRead.Item("ExpectedContent"))
        ExpectedLatency = IIf(IsDBNull(spRead.Item("Latency")), 500, spRead.Item("Latency"))
        Port = IIf(IsDBNull(spRead.Item("Port")), "", spRead.Item("Port"))
        Server = IIf(IsDBNull(spRead.Item("Server")), "", spRead.Item("Server"))

        If CheckEnabled Then
            Select Case TestTypeID
                Case TestType.Ping
                    CheckPing = True
                    If ExpectedLatency <> 500 Then CheckLatency = True
                Case TestType.Web
                    CheckURL = True
                    If ExpectedContent <> "" Then CheckContent = True
                Case Else

            End Select
        End If
        IntervalTimer.Interval = CheckInterval
    End Sub
    Public Sub ClearStats()
        ErrorCount = 0
        SuccessCount = 0
        CheckResults = New List(Of xHotCheckerCheckResult)
    End Sub

    Public Function StartCheck() As Boolean
        If CheckEnabled Then IntervalTimer.Start()
        Return True
    End Function
    Public Function StopCheck() As Boolean
        IntervalTimer.Stop()
        Try
            BackgroundProcess.CancelAsync()
        Catch ex As Exception

        End Try

        Return True
    End Function

    Function RunThisCheck() As Boolean
        Dim result As xHotCheckerCheckResult = xHotChecker.RunCheck(Me)
        CheckResults.Add(result)
        If result.AlertLevel > xHotCheckerCheckResult.AlertLevels.LowPriority Then
            If ErrorCount >= CountThreshold Then
                SuccessCount = 0
                IntervalTimer.Interval = ErrorInterval
            End If
            ErrorCount += 1 : PersistantErrorCount += 1
        Else

            If SuccessCount > 2 Then
                ErrorCount = 0 'This is reset also each Log interval
                PersistantErrorCount = 0 'This is reset only when there is a successful result
                IntervalTimer.Interval = CheckInterval
            End If
            SuccessCount += 1 : PersistantSuccessCount += 1
        End If
        Return True
    End Function
    Private Sub IntervalTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles IntervalTimer.Elapsed
        If Not BackgroundProcess.IsBusy Then BackgroundProcess.RunWorkerAsync()
    End Sub
    Private Sub BackgroundProcess_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundProcess.DoWork
        Me.RunThisCheck()
    End Sub

End Class

Public Class xHotChecker
    Public WithEvents LogTimer As New Timer
    Private WithEvents BackgroundProcess As New System.ComponentModel.BackgroundWorker
    Public ServerChecks As List(Of xHotCheckerCheck)
    Public CheckerRunning As Boolean = False

    Sub New(Optional Server As String = "")
        Try

            If Not Server = "" Then ServerChecks = xHotChecker.LoadCheckConfigs(Server)
            If ServerChecks.Count > 0 Then LogTimer.Interval = (LogInterval * 1000)
        Catch ex As Exception
            Log("Checker Service", "xHotChecker", "New (Load List of Checks)", ex.ToString)
        End Try

    End Sub
    Sub InitiateChecker()
        Try
            xHotChecker.InitChecks(ServerChecks)
        Catch ex As Exception
            Log("Checker Service", "xHotChecker", "InitiateChecker", ex.ToString)
        End Try
        LogTimer.Start()
        CheckerRunning = True
    End Sub
    Sub StopChecker()
        xHotChecker.StopChecks(ServerChecks)
        LogTimer.Stop()
        CheckerRunning = False
    End Sub
    Private Sub LogTimer_Tick(sender As Object, e As EventArgs) Handles LogTimer.Elapsed
        Try
            If Not BackgroundProcess.isBusy Then BackgroundProcess.RunWorkerAsync()
        Catch ex As Exception
            Log("Checker Service", "xHotChecker", "LogTimer_Tick", ex.ToString)
        End Try

    End Sub
    Private Sub BackgroundProcess_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundProcess.DoWork
        LogChecks(ServerChecks)
    End Sub


    Public Shared Function Ping(Address As String) As xHotCheckerPingData
        Dim pingsender As New System.Net.NetworkInformation.Ping
        Dim options As New System.Net.NetworkInformation.PingOptions
        Dim data As String = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" '32 bytes of dataData
        Dim buffer = Encoding.ASCII.GetBytes(data)
        Dim timeout As Integer = 250
        Dim result As New xHotCheckerPingData
        Dim reply As System.Net.NetworkInformation.PingReply

        Dim x As Integer
        For x = 1 To 5
            Try
                reply = pingsender.Send(Address, timeout, buffer, options)
                If (reply.Status = System.Net.NetworkInformation.IPStatus.Success) Then
                    result.AddSuccess(reply.RoundtripTime)
                Else
                    result.AddFailure()
                End If
                Threading.Thread.Sleep(150)
            Catch ex As Exception
                result.AddFailure()
            End Try

        Next
        Return result
    End Function
    Public Shared Function WebConnectivity(URL As String) As xHotCheckerWebConnectData
        Dim request As HttpWebRequest, response As HttpWebResponse = Nothing
        Dim reader As StreamReader, address As Uri, result As New xHotCheckerWebConnectData
        If Not (LCase(URL).StartsWith("http://") Or LCase(URL).StartsWith("https://")) Then URL = "http://" + URL
        Try
            address = New Uri(URL)
        Catch ex As Exception
            Return result
        End Try
        'ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
        request = DirectCast(WebRequest.Create(address), HttpWebRequest)
        'request.KeepAlive = False

        Try
            response = DirectCast(request.GetResponse(), HttpWebResponse)
            reader = New StreamReader(response.GetResponseStream())
            result.ResponseCode = response.StatusCode
            result.ResponseContent = reader.ReadToEnd()

        Catch wex As WebException
            If Not wex.Response Is Nothing Then
                Dim errorResponse As HttpWebResponse = Nothing
                Try
                    errorResponse = DirectCast(wex.Response, HttpWebResponse)
                    result.ResponseCode = errorResponse.StatusCode
                    result.ResponseContent = ""
                    result.ErrorMessage = errorResponse.StatusDescription
                Finally
                    If Not errorResponse Is Nothing Then errorResponse.Close()
                End Try
            Else
                result.ResponseCode = wex.Status
                result.ResponseContent = wex.Message
                result.ErrorMessage = wex.Message
            End If
        Finally
            If Not response Is Nothing Then response.Close()
        End Try

        Return result
    End Function
    Public Shared Function WebQualityCheck(response As xHotCheckerWebConnectData, expected As String) As Boolean
        Return (LCase(response.ResponseContent).StartsWith(LCase(expected)) Or LCase(response.ResponseContent).Contains(LCase(expected)))
    End Function
    Public Shared Function CheckDongle(COMPortSetting As String) As xDongleMessage
        Dim xListener As New xDongleListener, result As New xDongleMessage
        Dim BalanceStart As String = "The dealer balance for account 0716275592 is "
        Dim messages As Integer = 0, timeoutcount = 0, timeout = 10
        Try
            xListener.Start(COMPortSetting)
            Try
                xListener.Transmitter("AT+CUSD = 1, """ + xPDU.Encode("*33404#") + """" + vbNewLine)
                While xListener.COMPort.IsOpen
                    Threading.Thread.Sleep(100)
                    For Each Message As xDongleMessage In xListener.Messages
                        If Message.MessageType = xDongleMessageType.USSDReply And Message.Message.StartsWith(BalanceStart) Then result = Message : Exit While
                        If Message.MessageType = xDongleMessageType.USSDError Then result = Message : Exit While
                        If Message.MessageType = xDongleMessageType.SignalError Then Message.Message = "No Network Signal" : result = Message : Exit While
                    Next
                    If messages = xListener.Messages.Count Then
                        timeoutcount += 1
                        If timeoutcount > (timeout * 10) Then Return New xDongleMessage("Connection Timed Out Error:", xDongleMessageType.TimeOut)
                    Else
                        messages = xListener.Messages.Count
                        timeoutcount = 0
                    End If
                End While
                Return result
            Catch ex As Exception
                Return New xDongleMessage("Transmit Error: " + ex.Message, xDongleMessageType.DongleError)
            Finally
                xListener.ClosePort()
            End Try
        Catch ex As Exception
            Return New xDongleMessage("Connection Error: " + ex.Message, xDongleMessageType.DongleError)
        End Try

    End Function

    Public Shared Function RunCheck(check As xHotCheckerCheck) As xHotCheckerCheckResult
        Dim checkresult As New xHotCheckerCheckResult(check)

        If check.CheckPing Then
            checkresult.PingData = Ping(check.IPAddress)
            If Not checkresult.PingData.Result Then
                checkresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.AttentionRequired
                checkresult.ResultInformation += "Ping Failed; "
                GoTo CheckWeb
            End If
            If check.CheckLatency Then
                If check.ExpectedLatency < checkresult.PingData.SuccessLatencyAverage Then
                    checkresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.LowPriority
                    checkresult.ResultInformation += "Slow Ping; "
                End If
            End If
        End If
CheckWeb:
        If check.CheckURL Then
            checkresult.WebData = xHotChecker.WebConnectivity(check.URL)
            If Not (checkresult.WebData.ResponseCode > 100 And checkresult.WebData.ResponseCode < 250) Then
                checkresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.HighPriority
                checkresult.ResultInformation += "Web Error: " + checkresult.WebData.ErrorMessage + "; "
                GoTo ReturnResult
            End If
            If check.CheckContent Then
                If Not xHotChecker.WebQualityCheck(checkresult.WebData, check.ExpectedContent) Then
                    checkresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.HighPriority
                    checkresult.ResultInformation += "WebService unexpected data mismatch; "
                End If
            End If
        End If
        If check.CheckDongle Then
            Dim dongleresult As xDongleMessage = xHotChecker.CheckDongle(check.Port)
            If Not dongleresult.MessageType = xDongleMessageType.USSDReply Then
                Select Case dongleresult.MessageType
                    Case xDongleMessageType.DongleError
                        checkresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.SeriousProblem
                    Case xDongleMessageType.SignalError
                        checkresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.HighPriority
                    Case xDongleMessageType.USSDError
                        checkresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.AttentionRequired
                    Case xDongleMessageType.TimeOut
                        checkresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.LowPriority
                    Case Else
                        checkresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.Information
                End Select
                checkresult.ResultInformation += "Dongle Error:" + dongleresult.Message + "; "
            End If
        End If
ReturnResult:
        Return checkresult
    End Function
    Public Shared Function RunChecks(checks As List(Of xHotCheckerCheck)) As xHotCheckerCheckResult
        Dim finalresult As New xHotCheckerCheckResult()

        For Each check As xHotCheckerCheck In checks
            Dim running As xHotCheckerCheckResult = RunCheck(check)
            Select Case running.AlertLevel
                Case xHotCheckerCheckResult.AlertLevels.Information
                    finalresult.ResultInformation += "Check for " + check.ConfigName + " successful. "
                Case xHotCheckerCheckResult.AlertLevels.LowPriority
                    finalresult.ResultInformation += check.ConfigName + ": " + running.ResultInformation
                    finalresult.AlertTotal += 1
                Case xHotCheckerCheckResult.AlertLevels.AttentionRequired
                    finalresult.ResultInformation += check.ConfigName + ": " + running.ResultInformation
                    finalresult.AlertTotal += 3
                Case xHotCheckerCheckResult.AlertLevels.HighPriority, xHotCheckerCheckResult.AlertLevels.SeriousProblem
                    finalresult.ResultInformation += check.ConfigName + ": " + running.ResultInformation
                    finalresult.AlertTotal += 5
            End Select
            finalresult.ResultCollection.Add(running)

        Next
        Select Case finalresult.AlertTotal
            Case 2
                finalresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.LowPriority
            Case 3, 4
                finalresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.AttentionRequired
            Case 5 To 7
                finalresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.HighPriority
            Case Is > 7
                finalresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.SeriousProblem
            Case Else
                finalresult.AlertLevel = xHotCheckerCheckResult.AlertLevels.Information
        End Select
        Return finalresult
    End Function

    Public Shared Function LoadCheckConfigs(Server As String) As List(Of xHotCheckerCheck)
        Dim configs As New List(Of xHotCheckerCheck)
        Using sqlcon As New SqlConnection(SQLConStr)
            Dim command As New SqlCommand("xErrorChecks", sqlcon)
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.AddWithValue("@ServerName", Server)
            Try
                sqlcon.Open()
                Dim spRead As SqlDataReader = command.ExecuteReader
                Try
                    While spRead.Read
                        configs.Add(New xHotCheckerCheck(spRead))
                    End While
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                Finally
                    If Not spRead.IsClosed Then spRead.Close()
                End Try
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If sqlcon.State = ConnectionState.Open Then sqlcon.Close()
            End Try
        End Using
        Return configs
    End Function

    Public Shared Function InitChecks(checks As List(Of xHotCheckerCheck)) As Boolean
        Dim check As xHotCheckerCheck
        For Each check In checks
            check.StartCheck()
        Next
        Return True
    End Function
    Public Shared Function StopChecks(checks As List(Of xHotCheckerCheck)) As Boolean
        Dim check As xHotCheckerCheck
        For Each check In checks
            check.StopCheck()
        Next
        Return True
    End Function

    Public Shared Function GetChecksLogItem(checks As List(Of xHotCheckerCheck)) As List(Of xHotCheckerLogItem)
        Dim logItems As New List(Of xHotCheckerLogItem), allfine As Boolean = True
        Dim check As xHotCheckerCheck
        For Each check In checks
            If Not check.Status = xHotCheckerCheckResult.AlertLevels.Information Then logItems.Add(check.LogItem) : allfine = False
        Next
        If allfine Then
            logItems.Add(New xHotCheckerLogItem(0, 0, "All Checks Passed"))
        End If
        Return logItems
    End Function
    Public Shared Sub LogChecks(checks As List(Of xHotCheckerCheck))
        Using sqlcon As New SqlConnection(SQLConStr)
            Dim item As xHotCheckerLogItem
            Dim check As xHotCheckerCheck
            Try
                sqlcon.Open()
                For Each item In GetChecksLogItem(checks)
                    item.Save(sqlcon)
                Next
                For Each check In checks
                    check.ClearStats()
                Next
            Finally
                If sqlcon.State = ConnectionState.Open Then sqlcon.Close()
            End Try
        End Using
    End Sub


End Class

Public Class xHotCheckerLogItem
    Public ErrorID As Long = 0
    Public LogDate As DateTime = Date.Now
    Public CheckID As Integer = 0
    Public ErrorCount As Integer = 0
    Public ErrorData As String = 0

    Sub New(_checkID As Long, _ErrorCount As Long, _ErrorData As String)
        CheckID = _checkID
        ErrorCount = _ErrorCount
        ErrorData = _ErrorData
    End Sub

    Public Function Save(sqlcon As SqlConnection) As Boolean
        Using command As New SqlCommand("xErrorLog_Save", sqlcon)
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.AddWithValue("@checkID", CheckID)
            command.Parameters.AddWithValue("@count", ErrorCount)
            command.Parameters.AddWithValue("@data", ErrorData)
            Try
                command.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception(ex.Message)
                Return False
            End Try
        End Using
        Return True
    End Function

End Class

Public Class xHotCheckerPingData
    Public Failures As Integer = 0
    Public Successes As Integer = 0
    Public LatencyTotal As Long = 0
    Public ReadOnly Property SuccessLatencyAverage As Long
        Get
            Try
                Return LatencyTotal / Successes
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property
    Public ReadOnly Property Result As Boolean
        Get
            Return (Successes > 1)
        End Get
    End Property
    Public Sub AddSuccess(latency As Long)
        Successes += 1
        LatencyTotal += latency
    End Sub
    Public Sub AddFailure()
        Failures += 1
    End Sub
End Class

Public Class xHotCheckerWebConnectData

    Public ResponseCode As Integer = 0
    Public ErrorMessage As String = ""
    Public ResponseContent As String = ""
    Public ReadOnly Property Result As Boolean
        Get
            Return (ResponseCode < 250)
        End Get
    End Property

End Class

