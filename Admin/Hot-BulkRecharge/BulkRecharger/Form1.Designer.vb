<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.btnConfirm = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.btnLoadRecharges = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lstLines = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader()
        Me.txtRechargeContent = New System.Windows.Forms.RichTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lstLoadedRecharges = New System.Windows.Forms.ListView()
        Me.Id = New System.Windows.Forms.ColumnHeader()
        Me.Mobile = New System.Windows.Forms.ColumnHeader()
        Me.Amount = New System.Windows.Forms.ColumnHeader()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.chkUseDefault = New System.Windows.Forms.CheckBox()
        Me.txtCustomMessage = New System.Windows.Forms.RichTextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblBalance = New System.Windows.Forms.Label()
        Me.lblValidCreds = New System.Windows.Forms.Label()
        Me.lblValid = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnCheck = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.lstRecharges = New System.Windows.Forms.ListView()
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader()
        Me.btnRetryFailed = New System.Windows.Forms.Button()
        Me.btnExportRetryList = New System.Windows.Forms.Button()
        Me.btnExportFailed = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1250, 535)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lblTotal)
        Me.TabPage1.Controls.Add(Me.btnConfirm)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.GroupBox4)
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1242, 507)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Preparation"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.Location = New System.Drawing.Point(1020, 477)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(96, 23)
        Me.lblTotal.TabIndex = 5
        Me.lblTotal.Text = "0.00"
        Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnConfirm
        '
        Me.btnConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnConfirm.Location = New System.Drawing.Point(1126, 477)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(114, 23)
        Me.btnConfirm.TabIndex = 4
        Me.btnConfirm.Text = "Confirm && Run"
        Me.btnConfirm.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(941, 481)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(90, 15)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Recharge Total: "
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.Button1)
        Me.GroupBox4.Controls.Add(Me.btnLoadRecharges)
        Me.GroupBox4.Controls.Add(Me.btnClear)
        Me.GroupBox4.Controls.Add(Me.Label2)
        Me.GroupBox4.Controls.Add(Me.lstLines)
        Me.GroupBox4.Controls.Add(Me.txtRechargeContent)
        Me.GroupBox4.Controls.Add(Me.Label1)
        Me.GroupBox4.Controls.Add(Me.lstLoadedRecharges)
        Me.GroupBox4.Location = New System.Drawing.Point(6, 127)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(1230, 345)
        Me.GroupBox4.TabIndex = 2
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Load Recharges"
        '
        'btnLoadRecharges
        '
        Me.btnLoadRecharges.Location = New System.Drawing.Point(96, 22)
        Me.btnLoadRecharges.Name = "btnLoadRecharges"
        Me.btnLoadRecharges.Size = New System.Drawing.Size(75, 23)
        Me.btnLoadRecharges.TabIndex = 6
        Me.btnLoadRecharges.Text = "Load"
        Me.btnLoadRecharges.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(15, 22)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 5
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(249, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 15)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Loaded Lines"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lstLines
        '
        Me.lstLines.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstLines.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.lstLines.GridLines = True
        Me.lstLines.Location = New System.Drawing.Point(247, 51)
        Me.lstLines.Name = "lstLines"
        Me.lstLines.Size = New System.Drawing.Size(682, 288)
        Me.lstLines.TabIndex = 3
        Me.lstLines.UseCompatibleStateImageBehavior = False
        Me.lstLines.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Id"
        Me.ColumnHeader1.Width = 40
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Mobile"
        Me.ColumnHeader2.Width = 130
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Amount"
        Me.ColumnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader3.Width = 70
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Status"
        Me.ColumnHeader4.Width = 70
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "LineData"
        Me.ColumnHeader5.Width = 360
        '
        'txtRechargeContent
        '
        Me.txtRechargeContent.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtRechargeContent.Location = New System.Drawing.Point(6, 51)
        Me.txtRechargeContent.Name = "txtRechargeContent"
        Me.txtRechargeContent.Size = New System.Drawing.Size(235, 288)
        Me.txtRechargeContent.TabIndex = 2
        Me.txtRechargeContent.Text = ""
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(935, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Recharges"
        '
        'lstLoadedRecharges
        '
        Me.lstLoadedRecharges.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstLoadedRecharges.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Id, Me.Mobile, Me.Amount})
        Me.lstLoadedRecharges.GridLines = True
        Me.lstLoadedRecharges.Location = New System.Drawing.Point(935, 51)
        Me.lstLoadedRecharges.Name = "lstLoadedRecharges"
        Me.lstLoadedRecharges.Size = New System.Drawing.Size(289, 288)
        Me.lstLoadedRecharges.TabIndex = 0
        Me.lstLoadedRecharges.UseCompatibleStateImageBehavior = False
        Me.lstLoadedRecharges.View = System.Windows.Forms.View.Details
        '
        'Id
        '
        Me.Id.Text = "Id"
        Me.Id.Width = 40
        '
        'Mobile
        '
        Me.Mobile.Text = "Mobile"
        Me.Mobile.Width = 130
        '
        'Amount
        '
        Me.Amount.Text = "Amount"
        Me.Amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Amount.Width = 70
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.chkUseDefault)
        Me.GroupBox3.Controls.Add(Me.txtCustomMessage)
        Me.GroupBox3.Location = New System.Drawing.Point(478, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(293, 115)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "SMS Template Details"
        '
        'chkUseDefault
        '
        Me.chkUseDefault.AutoSize = True
        Me.chkUseDefault.Checked = True
        Me.chkUseDefault.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUseDefault.Location = New System.Drawing.Point(6, 19)
        Me.chkUseDefault.Name = "chkUseDefault"
        Me.chkUseDefault.Size = New System.Drawing.Size(137, 19)
        Me.chkUseDefault.TabIndex = 1
        Me.chkUseDefault.Text = "Use Default Template"
        Me.chkUseDefault.UseVisualStyleBackColor = True
        '
        'txtCustomMessage
        '
        Me.txtCustomMessage.Location = New System.Drawing.Point(6, 44)
        Me.txtCustomMessage.Name = "txtCustomMessage"
        Me.txtCustomMessage.Size = New System.Drawing.Size(281, 65)
        Me.txtCustomMessage.TabIndex = 0
        Me.txtCustomMessage.Text = ""
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtPassword)
        Me.GroupBox2.Controls.Add(Me.txtUsername)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(200, 115)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Hot Recharge Details"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(6, 77)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(188, 23)
        Me.txtPassword.TabIndex = 9
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(6, 34)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(188, 23)
        Me.txtUsername.TabIndex = 8
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 60)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(57, 15)
        Me.Label10.TabIndex = 7
        Me.Label10.Text = "Password"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(60, 15)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Username"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblBalance)
        Me.GroupBox1.Controls.Add(Me.lblValidCreds)
        Me.GroupBox1.Controls.Add(Me.lblValid)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.btnCheck)
        Me.GroupBox1.Location = New System.Drawing.Point(212, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(260, 115)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Account Details"
        '
        'lblBalance
        '
        Me.lblBalance.AutoSize = True
        Me.lblBalance.Location = New System.Drawing.Point(107, 79)
        Me.lblBalance.Name = "lblBalance"
        Me.lblBalance.Size = New System.Drawing.Size(12, 15)
        Me.lblBalance.TabIndex = 6
        Me.lblBalance.Text = "-"
        '
        'lblValidCreds
        '
        Me.lblValidCreds.AutoSize = True
        Me.lblValidCreds.Location = New System.Drawing.Point(107, 52)
        Me.lblValidCreds.Name = "lblValidCreds"
        Me.lblValidCreds.Size = New System.Drawing.Size(12, 15)
        Me.lblValidCreds.TabIndex = 5
        Me.lblValidCreds.Text = "-"
        '
        'lblValid
        '
        Me.lblValid.AutoSize = True
        Me.lblValid.Location = New System.Drawing.Point(107, 25)
        Me.lblValid.Name = "lblValid"
        Me.lblValid.Size = New System.Drawing.Size(12, 15)
        Me.lblValid.TabIndex = 4
        Me.lblValid.Text = "-"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(53, 79)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(48, 15)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Balance"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 52)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(87, 15)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Account Name"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(69, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(32, 15)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Valid"
        '
        'btnCheck
        '
        Me.btnCheck.Location = New System.Drawing.Point(179, 16)
        Me.btnCheck.Name = "btnCheck"
        Me.btnCheck.Size = New System.Drawing.Size(75, 23)
        Me.btnCheck.TabIndex = 0
        Me.btnCheck.Text = "Check"
        Me.btnCheck.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.btnExportFailed)
        Me.TabPage2.Controls.Add(Me.btnExportRetryList)
        Me.TabPage2.Controls.Add(Me.btnRetryFailed)
        Me.TabPage2.Controls.Add(Me.lstRecharges)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1242, 507)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Recharges"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(1242, 507)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Other"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'lstRecharges
        '
        Me.lstRecharges.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstRecharges.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader10})
        Me.lstRecharges.GridLines = True
        Me.lstRecharges.Location = New System.Drawing.Point(6, 53)
        Me.lstRecharges.Name = "lstRecharges"
        Me.lstRecharges.Size = New System.Drawing.Size(1230, 448)
        Me.lstRecharges.TabIndex = 0
        Me.lstRecharges.UseCompatibleStateImageBehavior = False
        Me.lstRecharges.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Id"
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Mobile"
        Me.ColumnHeader7.Width = 130
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Amount"
        Me.ColumnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader8.Width = 70
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Status"
        Me.ColumnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Narrative"
        Me.ColumnHeader10.Width = 360
        '
        'btnRetryFailed
        '
        Me.btnRetryFailed.Location = New System.Drawing.Point(6, 6)
        Me.btnRetryFailed.Name = "btnRetryFailed"
        Me.btnRetryFailed.Size = New System.Drawing.Size(75, 41)
        Me.btnRetryFailed.TabIndex = 1
        Me.btnRetryFailed.Text = "Retry Failed"
        Me.btnRetryFailed.UseVisualStyleBackColor = True
        '
        'btnExportRetryList
        '
        Me.btnExportRetryList.Location = New System.Drawing.Point(1043, 6)
        Me.btnExportRetryList.Name = "btnExportRetryList"
        Me.btnExportRetryList.Size = New System.Drawing.Size(75, 41)
        Me.btnExportRetryList.TabIndex = 2
        Me.btnExportRetryList.Text = "Export Retry List"
        Me.btnExportRetryList.UseVisualStyleBackColor = True
        '
        'btnExportFailed
        '
        Me.btnExportFailed.Location = New System.Drawing.Point(1142, 6)
        Me.btnExportFailed.Name = "btnExportFailed"
        Me.btnExportFailed.Size = New System.Drawing.Size(75, 41)
        Me.btnExportFailed.TabIndex = 3
        Me.btnExportFailed.Text = "Export Failed List"
        Me.btnExportFailed.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(824, 25)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(105, 23)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "Add Valid Lines"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1274, 559)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents lblTotal As Label
    Friend WithEvents btnConfirm As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents btnLoadRecharges As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents lstLines As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ColumnHeader5 As ColumnHeader
    Friend WithEvents txtRechargeContent As RichTextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lstLoadedRecharges As ListView
    Friend WithEvents Id As ColumnHeader
    Friend WithEvents Mobile As ColumnHeader
    Friend WithEvents Amount As ColumnHeader
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents chkUseDefault As CheckBox
    Friend WithEvents txtCustomMessage As RichTextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents txtUsername As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblBalance As Label
    Friend WithEvents lblValidCreds As Label
    Friend WithEvents lblValid As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents btnCheck As Button
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents btnExportFailed As Button
    Friend WithEvents btnExportRetryList As Button
    Friend WithEvents btnRetryFailed As Button
    Friend WithEvents lstRecharges As ListView
    Friend WithEvents ColumnHeader6 As ColumnHeader
    Friend WithEvents ColumnHeader7 As ColumnHeader
    Friend WithEvents ColumnHeader8 As ColumnHeader
    Friend WithEvents ColumnHeader9 As ColumnHeader
    Friend WithEvents ColumnHeader10 As ColumnHeader
    Friend WithEvents Button1 As Button

    Private Sub lstLines_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstLines.SelectedIndexChanged

    End Sub
End Class
