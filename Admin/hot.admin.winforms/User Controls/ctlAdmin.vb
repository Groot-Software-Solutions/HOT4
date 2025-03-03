Imports System.Data.SqlClient
Imports Hot.Data
Public Class ctlAdmin
    Private _Users As List(Of xUser)
    Private _Roles As List(Of xSecurityDBRole)
    Private _Permisions_All As List(Of xSecurityDBRolePermission)

    Private Sub ctlAdmin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        init()
    End Sub
    Sub init()
        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()
            _Users = xUserAdapter.List(sqlConn)
            _Roles = xSecurityDBRoleAdapter.List_All(sqlConn)
            _Permisions_All = xSecurityDBRolePermissionAdapter.List_All(sqlConn)

            For Each iUser As xUser In _Users
                iUser.Roles = xSecurityDBRoleAdapter.List(iUser.UserName, sqlConn)
            Next
            BindRoles
            BindUsers()
            BindAllPermissions()
            sqlConn.Close()
        End Using


    End Sub

    Sub BindRoles()
        Dim bs As New BindingSource
        bs.DataSource = _roles
        dgRoles.DataSource = bs
        dgRoles.Columns("RoleID").Visible = False
        dgRoles.Columns("RoleName").Visible = False
    End Sub
    Sub BindUsers()
        Dim bs As New BindingSource
        bs.DataSource = _Users
        dgUsers.DataSource = bs
        'dgRoles.Columns("Roles").Visible = False
        dgUsers.Columns("Password").Visible = False

    End Sub
    Sub BindAllPermissions()
        Dim bs As New BindingSource
        bs.DataSource = _Permisions_All
        dgAdvanced.DataSource = bs
        dgAdvanced.Columns("ObjectType").Visible = False
        dgAdvanced.Columns("PermissionName").Visible = False
        dgAdvanced.Columns("StateDescription").Visible = False
        dgAdvanced.Columns("PermissionType").Visible = False
        dgAdvanced.Columns("ObjectName").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
    End Sub
    Sub BindUserRoles(iUsr As xUser)
        Dim bs As New BindingSource
        bs.DataSource = iUsr.Roles
        dgUserRoles.DataSource = bs
        dgUserRoles.Columns("RoleID").Visible = False
        dgUserRoles.Columns("RoleName").Visible = False
    End Sub



    Private Sub btnNewRole_Click(sender As Object, e As EventArgs) Handles btnNewRole.Click
        If Not String.IsNullOrWhiteSpace(txtRoleName.Text) And Not CheckIfRoleExists(txtRoleName.Text) Then
            Dim iDBRole As New xSecurityDBRole
            iDBRole.RoleName = txtRoleName.Text
            iDBRole.Permissions = getRolePermissions()
            Using sqlConn As New SqlConnection(Conn)
                Try
                    sqlConn.Open()

                    xSecurityDBRoleAdapter.Save(iDBRole, sqlConn)
                    sqlConn.Close()
                Catch ex As Exception
                    ShowEx(ex)
                End Try

            End Using
        End If


    End Sub
    Function CheckIfRoleExists(name As String) As Boolean
        For Each role In _Roles
            If role.RoleName = name Then Return True
        Next
        Return False
    End Function

    Function getRolePermissions() As List(Of xSecurityDBRolePermission)
        Dim result As List(Of xSecurityDBRolePermission) = xSecurityDBRolePermissionAdapter.BaseConfiguration
        If tbSimple.Focused Then
            If chkAdmin.Checked Then result.AddRange(xSecurityDBRolePermissionAdapter.AdminConfiguration)
            If chkAddAccess.Checked Then result.AddRange(xSecurityDBRolePermissionAdapter.AddAccessUserConfiguration)
            If chkBulkSend.Checked Then result.AddRange(xSecurityDBRolePermissionAdapter.BulkSendConfiguration)
            If chkLoadPins.Checked Then result.AddRange(xSecurityDBRolePermissionAdapter.LoadPinsConfiguration)
            If chkLoadStatement.Checked Then result.AddRange(xSecurityDBRolePermissionAdapter.LoadStatmentConfiguration)
            If chkModifyAccess.Checked Then result.AddRange(xSecurityDBRolePermissionAdapter.ModifyAccessConfiguration)
            If chkProcessStatement.Checked Then result.AddRange(xSecurityDBRolePermissionAdapter.ProcessStatmentConfiguration)
            If chkUpdateAccount.Checked Then result.AddRange(xSecurityDBRolePermissionAdapter.UpdateAccountConfiguration)
            'If chkViewStatements.Checked Then result.AddRange(xSecurityDBRolePermissionAdapter.)
        Else
            result.Clear()
            For i = 0 To dgAdvanced.Rows.Count - 1
                If CType(dgAdvanced.Rows(i).DataBoundItem, xSecurityDBRolePermission).Active Then result.Add(dgAdvanced.Rows(i).DataBoundItem)
            Next
        End If
        Return result
    End Function


    Private Sub dgUsers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgUsers.CellClick
        BindUserRoles(dgUsers.SelectedRows(0).DataBoundItem)
    End Sub

    Private Sub btnAddUser_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click
        showAddUser
    End Sub
    Sub showAddUser()
        Dim f As New frmUserAdd
        If f.ShowDialog() = DialogResult.OK Then
            Try
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    _Users = xUserAdapter.List(sqlConn)
                    For Each iUser As xUser In _Users
                        iUser.Roles = xSecurityDBRoleAdapter.List(iUser.UserName, sqlConn)
                    Next
                    BindUsers()
                    sqlConn.Close()
                End Using
            Catch ex As Exception
                ShowEx(ex)
            End Try
        End If
    End Sub
End Class
