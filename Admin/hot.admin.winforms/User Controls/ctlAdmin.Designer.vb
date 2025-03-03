<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlAdmin
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnRemoveRole = New System.Windows.Forms.Button()
        Me.dgRoles = New System.Windows.Forms.DataGridView()
        Me.btnNewRole = New System.Windows.Forms.Button()
        Me.grpUsers = New System.Windows.Forms.GroupBox()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnAddUser = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dgUserRoles = New System.Windows.Forms.DataGridView()
        Me.dgUsers = New System.Windows.Forms.DataGridView()
        Me.tbSimple = New System.Windows.Forms.TabPage()
        Me.chkViewStatements = New System.Windows.Forms.CheckBox()
        Me.chkAddAccess = New System.Windows.Forms.CheckBox()
        Me.chkUpdateAccount = New System.Windows.Forms.CheckBox()
        Me.chkLoadPins = New System.Windows.Forms.CheckBox()
        Me.chkProcessStatement = New System.Windows.Forms.CheckBox()
        Me.chkLoadStatement = New System.Windows.Forms.CheckBox()
        Me.chkBulkSend = New System.Windows.Forms.CheckBox()
        Me.chkAdmin = New System.Windows.Forms.CheckBox()
        Me.chkModifyAccess = New System.Windows.Forms.CheckBox()
        Me.txtRoleName = New System.Windows.Forms.TextBox()
        Me.tbRoleConfig = New System.Windows.Forms.TabControl()
        Me.tbAdvanced = New System.Windows.Forms.TabPage()
        Me.dgAdvanced = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgRoles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpUsers.SuspendLayout()
        CType(Me.dgUserRoles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgUsers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbSimple.SuspendLayout()
        Me.tbRoleConfig.SuspendLayout()
        Me.tbAdvanced.SuspendLayout()
        CType(Me.dgAdvanced, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnRemoveRole)
        Me.GroupBox1.Controls.Add(Me.dgRoles)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(503, 204)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Database Roles"
        '
        'btnRemoveRole
        '
        Me.btnRemoveRole.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveRole.Enabled = False
        Me.btnRemoveRole.Location = New System.Drawing.Point(403, 14)
        Me.btnRemoveRole.Name = "btnRemoveRole"
        Me.btnRemoveRole.Size = New System.Drawing.Size(94, 23)
        Me.btnRemoveRole.TabIndex = 3
        Me.btnRemoveRole.Text = "Remove Role"
        Me.btnRemoveRole.UseVisualStyleBackColor = True
        '
        'dgRoles
        '
        Me.dgRoles.AllowUserToAddRows = False
        Me.dgRoles.AllowUserToDeleteRows = False
        Me.dgRoles.AllowUserToResizeRows = False
        Me.dgRoles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgRoles.Location = New System.Drawing.Point(9, 43)
        Me.dgRoles.Name = "dgRoles"
        Me.dgRoles.ReadOnly = True
        Me.dgRoles.Size = New System.Drawing.Size(488, 155)
        Me.dgRoles.TabIndex = 2
        '
        'btnNewRole
        '
        Me.btnNewRole.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNewRole.Location = New System.Drawing.Point(399, 11)
        Me.btnNewRole.Name = "btnNewRole"
        Me.btnNewRole.Size = New System.Drawing.Size(94, 23)
        Me.btnNewRole.TabIndex = 3
        Me.btnNewRole.Text = "Add Role"
        Me.btnNewRole.UseVisualStyleBackColor = True
        '
        'grpUsers
        '
        Me.grpUsers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpUsers.Controls.Add(Me.btnEdit)
        Me.grpUsers.Controls.Add(Me.btnAddUser)
        Me.grpUsers.Controls.Add(Me.Label2)
        Me.grpUsers.Controls.Add(Me.dgUserRoles)
        Me.grpUsers.Controls.Add(Me.dgUsers)
        Me.grpUsers.Location = New System.Drawing.Point(512, 3)
        Me.grpUsers.Name = "grpUsers"
        Me.grpUsers.Size = New System.Drawing.Size(282, 540)
        Me.grpUsers.TabIndex = 2
        Me.grpUsers.TabStop = False
        Me.grpUsers.Text = "Users"
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(116, 14)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(101, 23)
        Me.btnEdit.TabIndex = 4
        Me.btnEdit.Text = "Edit User"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnAddUser
        '
        Me.btnAddUser.Location = New System.Drawing.Point(9, 14)
        Me.btnAddUser.Name = "btnAddUser"
        Me.btnAddUser.Size = New System.Drawing.Size(101, 23)
        Me.btnAddUser.TabIndex = 3
        Me.btnAddUser.Text = "Add User"
        Me.btnAddUser.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 338)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "User Roles"
        '
        'dgUserRoles
        '
        Me.dgUserRoles.AllowUserToAddRows = False
        Me.dgUserRoles.AllowUserToDeleteRows = False
        Me.dgUserRoles.AllowUserToResizeRows = False
        Me.dgUserRoles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgUserRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgUserRoles.Location = New System.Drawing.Point(6, 354)
        Me.dgUserRoles.Name = "dgUserRoles"
        Me.dgUserRoles.ReadOnly = True
        Me.dgUserRoles.Size = New System.Drawing.Size(270, 180)
        Me.dgUserRoles.TabIndex = 1
        '
        'dgUsers
        '
        Me.dgUsers.AllowUserToAddRows = False
        Me.dgUsers.AllowUserToDeleteRows = False
        Me.dgUsers.AllowUserToResizeRows = False
        Me.dgUsers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgUsers.Location = New System.Drawing.Point(6, 43)
        Me.dgUsers.Name = "dgUsers"
        Me.dgUsers.ReadOnly = True
        Me.dgUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgUsers.Size = New System.Drawing.Size(270, 292)
        Me.dgUsers.TabIndex = 0
        '
        'tbSimple
        '
        Me.tbSimple.Controls.Add(Me.chkViewStatements)
        Me.tbSimple.Controls.Add(Me.chkAddAccess)
        Me.tbSimple.Controls.Add(Me.chkUpdateAccount)
        Me.tbSimple.Controls.Add(Me.chkLoadPins)
        Me.tbSimple.Controls.Add(Me.chkProcessStatement)
        Me.tbSimple.Controls.Add(Me.chkLoadStatement)
        Me.tbSimple.Controls.Add(Me.chkBulkSend)
        Me.tbSimple.Controls.Add(Me.chkAdmin)
        Me.tbSimple.Controls.Add(Me.chkModifyAccess)
        Me.tbSimple.Location = New System.Drawing.Point(4, 22)
        Me.tbSimple.Name = "tbSimple"
        Me.tbSimple.Padding = New System.Windows.Forms.Padding(3)
        Me.tbSimple.Size = New System.Drawing.Size(484, 233)
        Me.tbSimple.TabIndex = 0
        Me.tbSimple.Text = "Simple"
        Me.tbSimple.UseVisualStyleBackColor = True
        '
        'chkViewStatements
        '
        Me.chkViewStatements.AutoSize = True
        Me.chkViewStatements.Location = New System.Drawing.Point(297, 45)
        Me.chkViewStatements.Name = "chkViewStatements"
        Me.chkViewStatements.Size = New System.Drawing.Size(133, 17)
        Me.chkViewStatements.TabIndex = 0
        Me.chkViewStatements.Text = "View Bank Statements"
        Me.chkViewStatements.UseVisualStyleBackColor = True
        '
        'chkAddAccess
        '
        Me.chkAddAccess.AutoSize = True
        Me.chkAddAccess.Location = New System.Drawing.Point(6, 77)
        Me.chkAddAccess.Name = "chkAddAccess"
        Me.chkAddAccess.Size = New System.Drawing.Size(113, 17)
        Me.chkAddAccess.TabIndex = 0
        Me.chkAddAccess.Text = "Add Access Users"
        Me.chkAddAccess.UseVisualStyleBackColor = True
        '
        'chkUpdateAccount
        '
        Me.chkUpdateAccount.AutoSize = True
        Me.chkUpdateAccount.Location = New System.Drawing.Point(297, 77)
        Me.chkUpdateAccount.Name = "chkUpdateAccount"
        Me.chkUpdateAccount.Size = New System.Drawing.Size(139, 17)
        Me.chkUpdateAccount.TabIndex = 0
        Me.chkUpdateAccount.Text = "Update Account Details"
        Me.chkUpdateAccount.UseVisualStyleBackColor = True
        '
        'chkLoadPins
        '
        Me.chkLoadPins.AutoSize = True
        Me.chkLoadPins.Location = New System.Drawing.Point(297, 15)
        Me.chkLoadPins.Name = "chkLoadPins"
        Me.chkLoadPins.Size = New System.Drawing.Size(73, 17)
        Me.chkLoadPins.TabIndex = 0
        Me.chkLoadPins.Text = "Load Pins"
        Me.chkLoadPins.UseVisualStyleBackColor = True
        '
        'chkProcessStatement
        '
        Me.chkProcessStatement.AutoSize = True
        Me.chkProcessStatement.Location = New System.Drawing.Point(146, 45)
        Me.chkProcessStatement.Name = "chkProcessStatement"
        Me.chkProcessStatement.Size = New System.Drawing.Size(148, 17)
        Me.chkProcessStatement.TabIndex = 0
        Me.chkProcessStatement.Text = "Process Bank Statements"
        Me.chkProcessStatement.UseVisualStyleBackColor = True
        '
        'chkLoadStatement
        '
        Me.chkLoadStatement.AutoSize = True
        Me.chkLoadStatement.Location = New System.Drawing.Point(6, 45)
        Me.chkLoadStatement.Name = "chkLoadStatement"
        Me.chkLoadStatement.Size = New System.Drawing.Size(134, 17)
        Me.chkLoadStatement.TabIndex = 0
        Me.chkLoadStatement.Text = "Load Bank Statements"
        Me.chkLoadStatement.UseVisualStyleBackColor = True
        '
        'chkBulkSend
        '
        Me.chkBulkSend.AutoSize = True
        Me.chkBulkSend.Location = New System.Drawing.Point(146, 15)
        Me.chkBulkSend.Name = "chkBulkSend"
        Me.chkBulkSend.Size = New System.Drawing.Size(126, 17)
        Me.chkBulkSend.TabIndex = 0
        Me.chkBulkSend.Text = "Send Bulk Messages"
        Me.chkBulkSend.UseVisualStyleBackColor = True
        '
        'chkAdmin
        '
        Me.chkAdmin.AutoSize = True
        Me.chkAdmin.Location = New System.Drawing.Point(6, 15)
        Me.chkAdmin.Name = "chkAdmin"
        Me.chkAdmin.Size = New System.Drawing.Size(91, 17)
        Me.chkAdmin.TabIndex = 0
        Me.chkAdmin.Text = "Administration"
        Me.chkAdmin.UseVisualStyleBackColor = True
        '
        'chkModifyAccess
        '
        Me.chkModifyAccess.AutoSize = True
        Me.chkModifyAccess.Location = New System.Drawing.Point(146, 77)
        Me.chkModifyAccess.Name = "chkModifyAccess"
        Me.chkModifyAccess.Size = New System.Drawing.Size(153, 17)
        Me.chkModifyAccess.TabIndex = 0
        Me.chkModifyAccess.Text = "Modify/Reset Access User"
        Me.chkModifyAccess.UseVisualStyleBackColor = True
        '
        'txtRoleName
        '
        Me.txtRoleName.Location = New System.Drawing.Point(9, 32)
        Me.txtRoleName.Name = "txtRoleName"
        Me.txtRoleName.Size = New System.Drawing.Size(263, 20)
        Me.txtRoleName.TabIndex = 11
        '
        'tbRoleConfig
        '
        Me.tbRoleConfig.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbRoleConfig.Controls.Add(Me.tbSimple)
        Me.tbRoleConfig.Controls.Add(Me.tbAdvanced)
        Me.tbRoleConfig.Location = New System.Drawing.Point(5, 71)
        Me.tbRoleConfig.Name = "tbRoleConfig"
        Me.tbRoleConfig.SelectedIndex = 0
        Me.tbRoleConfig.Size = New System.Drawing.Size(492, 259)
        Me.tbRoleConfig.TabIndex = 10
        '
        'tbAdvanced
        '
        Me.tbAdvanced.Controls.Add(Me.dgAdvanced)
        Me.tbAdvanced.Location = New System.Drawing.Point(4, 22)
        Me.tbAdvanced.Name = "tbAdvanced"
        Me.tbAdvanced.Padding = New System.Windows.Forms.Padding(3)
        Me.tbAdvanced.Size = New System.Drawing.Size(484, 233)
        Me.tbAdvanced.TabIndex = 1
        Me.tbAdvanced.Text = "Advanced"
        Me.tbAdvanced.UseVisualStyleBackColor = True
        '
        'dgAdvanced
        '
        Me.dgAdvanced.AllowUserToAddRows = False
        Me.dgAdvanced.AllowUserToDeleteRows = False
        Me.dgAdvanced.AllowUserToResizeColumns = False
        Me.dgAdvanced.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgAdvanced.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgAdvanced.Location = New System.Drawing.Point(3, 3)
        Me.dgAdvanced.Name = "dgAdvanced"
        Me.dgAdvanced.Size = New System.Drawing.Size(478, 227)
        Me.dgAdvanced.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Role Name"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(153, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Database Roles Configurations"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.btnNewRole)
        Me.GroupBox2.Controls.Add(Me.txtRoleName)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.tbRoleConfig)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 213)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(503, 336)
        Me.GroupBox2.TabIndex = 12
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Add Role"
        '
        'ctlAdmin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.grpUsers)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "ctlAdmin"
        Me.Size = New System.Drawing.Size(797, 552)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.dgRoles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpUsers.ResumeLayout(False)
        Me.grpUsers.PerformLayout()
        CType(Me.dgUserRoles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgUsers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbSimple.ResumeLayout(False)
        Me.tbSimple.PerformLayout()
        Me.tbRoleConfig.ResumeLayout(False)
        Me.tbAdvanced.ResumeLayout(False)
        CType(Me.dgAdvanced, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnRemoveRole As Button
    Friend WithEvents btnNewRole As Button
    Friend WithEvents dgRoles As DataGridView
    Friend WithEvents grpUsers As GroupBox
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnAddUser As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents dgUserRoles As DataGridView
    Friend WithEvents dgUsers As DataGridView
    Friend WithEvents tbSimple As TabPage
    Friend WithEvents txtRoleName As TextBox
    Friend WithEvents tbRoleConfig As TabControl
    Friend WithEvents tbAdvanced As TabPage
    Friend WithEvents dgAdvanced As DataGridView
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents chkAdmin As CheckBox
    Friend WithEvents chkViewStatements As CheckBox
    Friend WithEvents chkAddAccess As CheckBox
    Friend WithEvents chkUpdateAccount As CheckBox
    Friend WithEvents chkLoadPins As CheckBox
    Friend WithEvents chkProcessStatement As CheckBox
    Friend WithEvents chkLoadStatement As CheckBox
    Friend WithEvents chkBulkSend As CheckBox
    Friend WithEvents chkModifyAccess As CheckBox
End Class
