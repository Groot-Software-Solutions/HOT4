

Imports HOT44.Checker.Service.Commons
Public Class Service1
    Dim Checker As xHotChecker

    Protected Overrides Sub OnStart(ByVal args() As String)

        Try
            'Set Config
            Checker = New xHotChecker("Econet")
            Checker.InitiateChecker()

        Catch ex As Exception
            '# Catastrophic Failure
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        Checker.StopChecker()
    End Sub
End Class
