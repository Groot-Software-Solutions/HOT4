Imports System.Data.SqlClient
Imports HOT.Data

Public Class frmAccountSearch

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
            Me.Cursor = Cursors.WaitCursor
            SearchLabel.Visible = True
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
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
            SearchLabel.Visible = False
        End Try
    End Sub

    Private _Account As xAccount
    Public ReadOnly Property Account() As xAccount
        Get
            Return _Account
        End Get        
    End Property

    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Try
            Me.Cursor = Cursors.WaitCursor
            If dg.SelectedRows.Count = 1 Then
                _Account = dg.SelectedRows(0).DataBoundItem
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    
 
End Class