Imports System.Data.SqlClient

Public Class frmUserAdd
    Private _Users As List(Of xUser)
    Private Sub frmUserAdd_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()
            cboRole.DisplayMember = "Name"
            cboRole.ValueMember = "RoleName"
            cboRole.DataSource = xSecurityDBRoleAdapter.List_All(sqlConn)
            cboRole.SelectedIndex = 1

            _Users = xUserAdapter.List(sqlConn)
            sqlConn.Close()
        End Using

    End Sub
    Sub AddUser()
        Try
            If Not UserExists(txtUserName.Text) Then
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    Dim newUser As xUser = getUserItem()
                    If xUserAdapter.Save(newUser, sqlConn) Then
                        Me.DialogResult = DialogResult.OK
                        Me.Close()
                    End If
                    sqlConn.Close()
                End Using
            Else
                Throw New Exception("User Already Exists")
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try

    End Sub

    Function getUserItem() As xUser
        Return New xUser With {
            .UserName = txtUserName.Text,
            .Password = txtPassword.Text,
            .Roles = New List(Of xSecurityDBRole) From {
                cboRole.SelectedItem}
        }
    End Function
    Function UserExists(_Name As String) As Boolean
        Return ((From u As xUser In _Users Where u.UserName.ToLower() = _Name.ToLower() Select u).Count() > 0)
    End Function
    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        AddUser()
    End Sub

    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cboRole_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRole.SelectedIndexChanged

    End Sub
End Class