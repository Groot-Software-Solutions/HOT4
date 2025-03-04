Imports System.Configuration

Public Class SmsHandler
    Public x As New xProcess
    Protected Overrides Sub OnStart(ByVal args() As String)
        x.StartProcess(ConfigurationManager.AppSettings("SQLConn"))

    End Sub

    Protected Overrides Sub OnStop()
        x.StopProcess()
    End Sub

End Class
