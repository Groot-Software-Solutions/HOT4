Imports Hot.Checker
Public Class Form1
    Dim Checker As xHotChecker

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Checker = New xHotChecker("Home")
        Catch ex As Exception
            MsgBox("Error Loading Checks: " + ex.Message)
        End Try
    End Sub

    Sub display()
        Dim allfine As Boolean = True
        For Each check In Checker.ServerChecks
            If Not check.Status = xHotCheckerCheckResult.AlertLevels.Information Then addresult(check) : allfine = False
        Next
        If allfine Then
            lstList.Items.Add(New ListViewItem("System Fine"))
        End If
    End Sub

    Sub addresult(x As xHotCheckerCheck)
        Dim y As New ListViewItem
        y.Text = x.ConfigName
        y.SubItems.Add([Enum].GetName(GetType(xHotCheckerCheck.TestType), x.TestTypeID))
        y.SubItems.Add(x.LogItem.ErrorCount)
        y.SubItems.Add([Enum].GetName(GetType(xHotCheckerCheckResult.AlertLevels), x.Status))
        y.SubItems.Add(x.StatusString)
        Select Case x.Status
            Case xHotCheckerCheckResult.AlertLevels.LowPriority
                y.BackColor = Color.LightYellow
            Case xHotCheckerCheckResult.AlertLevels.AttentionRequired
                y.BackColor = Color.Orange
            Case xHotCheckerCheckResult.AlertLevels.HighPriority
                y.BackColor = Color.OrangeRed
            Case xHotCheckerCheckResult.AlertLevels.SeriousProblem
                y.BackColor = Color.Red

        End Select
        lstList.Items.Add(y)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Checker.InitiateChecker()
        Timer1.Start()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Checker.StopChecker()
        Timer1.Stop()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        display()
    End Sub
End Class
