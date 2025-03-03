Imports System.Configuration
Imports Hot.Recharge.Winservice.My
Public Class WinService

    ReadOnly _process As New xProcess
    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        _process.StartProcess((ConfigurationManager.AppSettings("ConnectionString")))
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        _process.StopProcess()
    End Sub

End Class
