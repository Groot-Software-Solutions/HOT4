Imports System.Data.SqlClient

Public Class frmLogon

    Sub New()
        InitializeComponent()
        lblVersion.Text = "Version " & Application.ProductVersion
        txtUserName.Text = My.Settings.Item("LastUserName")
    End Sub

    Private Sub frmLogon_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If txtUserName.Text = "" Then txtUserName.Focus() Else txtPassword.Focus()
    End Sub

    Private Function ValidateForm() As Boolean
        Dim Valid As Boolean = True
        If txtUserName.Text = String.Empty Then
            Err.SetError(txtUserName, "User name may not be blank")
            Valid = False
        End If
        If txtPassword.Text = String.Empty Then
            Err.SetError(txtPassword, "Password may not be blank")
            Valid = False
        End If
        Return Valid
    End Function

    Private Sub cmdLogon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLogon.Click
        Try
            Me.DialogResult = Windows.Forms.DialogResult.None
            If ValidateForm() Then
                gUser = New xUser
                gUser.UserName = txtUserName.Text
                gUser.Password = txtPassword.Text
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    DBRole = xSecurityDBRoleAdapter.GetHighestPriv(sqlConn)
                    sqlConn.Close()
                End Using
                My.Settings.Item("LastUserName") = txtUserName.Text
                My.Settings.Save()
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
        Catch ex As Exception
            Me.txtPassword.Text = ""
            ShowEx(ex)
        End Try
    End Sub

End Class