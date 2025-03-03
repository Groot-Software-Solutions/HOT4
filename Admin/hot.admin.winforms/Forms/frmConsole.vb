Imports Hot.Data

Public Class frmConsole

    Private Sub frmConsole_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "HOT4.Console"
        Try
            Me.Text = Me.Text & " Published version " & System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString
        Catch ex As Exception

        End Try

        Dim f As New frmLogon
        If f.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ShowSystem()
            lblUser.Text = "Logged In: " + HOT4.Console.gUser.UserName
            ' ShowAccountSearch()
        Else
            Me.Close()
        End If

    End Sub

#Region " Account Search "
    Private Sub cmdAccountSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAccountSearch.Click
        ShowAccountSearch()
    End Sub
    Private Sub ShowAccountSearch()
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim tb As TabPage = tcMain.TabPages(TABKEY_ACCOUNTSEARCH)
            If tb Is Nothing Then
                tb = AddTab(TABKEY_ACCOUNTSEARCH, TABKEY_ACCOUNTSEARCH)
                tb.ImageKey = "search4people.ico"
                Dim iCTL As New ctlAccountSearch
                iCTL.Dock = DockStyle.Top
                iCTL.AutoSize = True
                tb.Controls.Add(iCTL)
            End If
            tcMain.SelectedTab = tb
            tb.Controls(0).Focus()
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region

#Region " Pins "
    Private Sub cmdPins_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPins.Click
        ShowPins()
    End Sub
    Private Sub ShowPins()
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim tb As TabPage = tcMain.TabPages(TABKEY_PINS)
            If tb Is Nothing Then
                tb = AddTab(TABKEY_PINS, TABKEY_PINS)
                tb.ImageKey = "BarCodeHS.png"
                Dim iCTL As New ctlPins
                iCTL.Dock = DockStyle.Fill
                tb.Controls.Add(iCTL)
            End If
            tcMain.SelectedTab = tb
            tb.Controls(0).Focus()
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region

#Region " Banking "
    Private Sub cmdBank_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBank.Click
        ShowBanking()
    End Sub
    Private Sub ShowBanking()
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim tb As TabPage = tcMain.TabPages(TABKEY_BANK)
            If tb Is Nothing Then
                tb = AddTab(TABKEY_BANK, TABKEY_BANK)
                tb.ImageKey = "HomeHS.png"
                Dim iCTL As New ctlBank
                iCTL.Dock = DockStyle.Fill
                tb.Controls.Add(iCTL)
            End If
            tcMain.SelectedTab = tb
            tb.Controls(0).Focus()
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region

#Region " SMS and SMPP "
    Private Sub cmdSmppActivity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSmppActivity.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim tb As TabPage = tcMain.TabPages(TABKEY_SMPP)
            If tb Is Nothing Then
                tb = AddTab(TABKEY_SMPP, "Smpp Activity")
                tb.ImageKey = "SearchFolderHS.png"
                Dim iCtl As New ctlSmppActivity
                iCtl.Dock = DockStyle.Fill
                tb.Controls.Add(iCtl)
            End If
            tcMain.SelectedTab = tb
            tb.Controls(0).Focus()
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
        'Dim f As New frmSmppActivity
        'f.ShowDialog()
    End Sub

    Private Sub cmdBulkSMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBulkSMS.Click
        Dim f As New frmBulkSMS
        f.ShowDialog()
    End Sub
#End Region

#Region " System and Errors "

    Private Sub cmdSystem_Click(sender As System.Object, e As System.EventArgs) Handles cmdSystem.Click
        ShowSystem()
    End Sub
    Private Sub ShowSystem()
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim tb As TabPage = tcMain.TabPages(TABKEY_SYSTEM)
            If tb Is Nothing Then
                tb = AddTab(TABKEY_SYSTEM, TABKEY_SYSTEM)
                tb.ImageKey = "OK.png"
                Dim iCTL As New ctlSystem
                iCTL.Dock = DockStyle.Top
                iCTL.AutoSize = True
                ' iCTL.Init()
                tb.Controls.Add(iCTL)
            End If

            tcMain.SelectedTab = tb
            tb.Controls(0).Focus()

            ' Setup Menu
            cmdBulkSMS.Visible = DBRoleAllowsFunction(xHotUIFunction.Send_Bulk_Messages)
            cmdAdmin.Visible = DBRoleAllowsFunction(xHotUIFunction.Admin_Console)
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " Recharges "

    Private Sub cmdRecharges_Click(sender As Object, e As EventArgs) Handles cmdRecharges.Click
        showRecharges()

    End Sub
    Private Sub showRecharges()
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim tb As TabPage = tcMain.TabPages(TABKEY_RECHARGES)
            If tb Is Nothing Then
                tb = AddTab(TABKEY_RECHARGES, TABKEY_RECHARGES)
                tb.ImageKey = "FindHS.png"
                Dim iCtl As New ctlRecharges
                iCtl.Dock = DockStyle.Fill
                tb.Controls.Add(iCtl)

            End If
            tcMain.SelectedTab = tb
            tb.Controls(0).Focus()
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region

#Region " Admin "

    Private Sub cmdAdmin_Click(sender As Object, e As EventArgs) Handles cmdAdmin.Click
        showAdmin()
    End Sub
    Private Sub showAdmin()
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim tb As TabPage = tcMain.TabPages(TABKEY_ADMIN)
            If tb Is Nothing Then
                tb = AddTab(TABKEY_ADMIN, TABKEY_ADMIN)
                tb.ImageKey = "EditTableHS.png"
                Dim iCtl As New ctlAdmin
                iCtl.Dock = DockStyle.Fill
                tb.Controls.Add(iCtl)

            End If
            tcMain.SelectedTab = tb
            tb.Controls(0).Focus()
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region




End Class