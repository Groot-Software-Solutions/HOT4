Imports System.Data.SqlClient
Imports Hot.Data

Public Class ctlAccountSearch

    Private Sub txtFilter_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFilter.KeyUp
        If e.KeyCode = Keys.Enter Then
            Search()
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Search()
    End Sub

    Private Sub Search()
        Try
            Label2.Visible = True
            Label2.Update()
            If txtFilter.Text.Length < 3 Then
                Err.SetError(txtFilter, "Filter must be at least 3 characters")
                txtFilter.Focus()
                Exit Sub
            End If
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim bs As New BindingSource
                bs.DataSource = xAccountAdapter.Search(txtFilter.Text, sqlConn)
                dg.DataSource = bs
                sqlConn.Close()
            End Using
            If dg.RowCount = 1 Then
                OpenSingleRow()
            End If
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Label2.Visible = False
            Label2.Update()
        End Try
    End Sub
    Private Sub OpenSingleRow()
        Try
            Me.Cursor = Cursors.WaitCursor
            If dg.SelectedRows.Count = 1 Then
                Dim iAccount As xAccount = dg.SelectedRows(0).DataBoundItem
                Dim tb As TabPage = frmConsole.tcMain.TabPages(TABKEY_ACCOUNT & iAccount.AccountID)
                If tb Is Nothing Then
                    tb = AddTab(TABKEY_ACCOUNT & iAccount.AccountID, iAccount.AccountName)
                    tb.ImageKey = "user.ico"
                    Dim iCtl As New ctlAccount
                    iCtl.Init(iAccount)
                    iCtl.Dock = DockStyle.Fill
                    tb.Controls.Add(iCtl)
                End If
                frmConsole.tcMain.SelectedTab = tb
                tb.Controls(0).Focus()
            End If
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Try
            Me.Cursor = Cursors.WaitCursor
            If dg.SelectedRows.Count = 1 Then
                Dim iAccount As xAccount = dg.SelectedRows(0).DataBoundItem
                Dim tb As TabPage = frmConsole.tcMain.TabPages(TABKEY_ACCOUNT & iAccount.AccountID)
                If tb Is Nothing Then
                    tb = AddTab(TABKEY_ACCOUNT & iAccount.AccountID, iAccount.AccountName)
                    tb.ImageKey = "user.ico"
                    Dim iCtl As New ctlAccount
                    iCtl.Init(iAccount)
                    iCtl.Dock = DockStyle.Fill
                    tb.Controls.Add(iCtl)
                End If
                frmConsole.tcMain.SelectedTab = tb
                tb.Controls(0).Focus()
            End If
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

End Class
