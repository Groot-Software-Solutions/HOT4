Imports System.Data.SqlClient
Imports Hot.Data

Public Class frmHOTAccess

    Private _Access As xAccess
    Private _AccountID As Long
    Private _AccessWeb As xAccessWeb
    Sub New(ByVal iAccess As xAccess, ByVal iAccountID As Long)
        InitializeComponent()
        _Access = iAccess
        _AccountID = iAccountID

        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()

            cboChannel.DisplayMember = "Channel"
            cboChannel.ValueMember = "ChannelID"
            cboChannel.DataSource = xChannelAdapter.List(sqlConn)
            _AccessWeb = xAccessWebAdapter.SelectRow(_Access.AccessID, sqlConn)
            If _AccessWeb Is Nothing Then _AccessWeb = New xAccessWeb
            sqlConn.Close()
        End Using
        If _Access.AccessID = 0 Then
            '_Access.AccessID = Guid.NewGuid
            _Access.AccountID = _AccountID
        Else

            cboChannel.SelectedValue = _Access.Channel.ChannelID
            txtCode.Text = _Access.AccessCode
            txtPassword.Text = _Access.AccessPassword
            txtAccessName.Text = _AccessWeb.AccessName
        End If
        txtID.Text = _Access.AccessID.ToString
        txtAccountID.Text = _AccountID.ToString
    End Sub
    Public Function GetAccess() As xAccess
        _Access.Channel = cboChannel.SelectedItem
        _Access.AccessCode = txtCode.Text
        _Access.AccessPassword = txtPassword.Text
        _Access.Deleted = False
        Return _Access
    End Function

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Me.DialogResult = Windows.Forms.DialogResult.None
            If txtCode.Text = String.Empty Then
                Throw New Exception("Invalid Access Code")
            End If
            If txtPassword.TextLength < 4 Then
                Throw New Exception("Invalid Access Password, must be at least 4 characters or digits")
            End If

            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                _Access.AccountID = _AccountID
                _Access.Channel = cboChannel.SelectedItem
                _Access.AccessCode = txtCode.Text
                _Access.AccessPassword = txtPassword.Text
                _Access.Deleted = False
                xAccessAdapter.Save(_Access, sqlConn)

                _AccessWeb.AccessName = txtAccessName.Text
                xAccessWebAdapter.Save(_AccessWeb, sqlConn)
                sqlConn.Close()
            End Using

            Me.DialogResult = Windows.Forms.DialogResult.OK

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    'Private Sub cmdAccessDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAccessDelete.Click
    '    Try
    '        Me.DialogResult = Windows.Forms.DialogResult.None
    '        If MessageBox.Show("Are you sure you want to deleted this record permanently?", "PERMANENT DELETE", MessageBoxButtons.OKCancel) = vbOK Then
    '            Using sqlConn As New SqlConnection(Conn)
    '                sqlConn.Open()
    '                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
    '                xAccessAdapter.Delet(_Access, sqlConn, sqlTrans)
    '                sqlTrans.Commit()
    '                sqlConn.Close()
    '            End Using
    '        End If
    '        Me.DialogResult = Windows.Forms.DialogResult.OK
    '    Catch ex As Exception


    '        MsgBox(ex.Message, MsgBoxStyle.Exclamation)



    '    End Try
    'End Sub

    
End Class