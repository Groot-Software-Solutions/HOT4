<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdjustLimit
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.txtCurrentMonthly = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtCurrentRemaining = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCurrentDaily = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtNewDaily = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNewMonthly = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtNewRemaining = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtDailyAmountToAdd = New System.Windows.Forms.TextBox()
        Me.txtMonthlyAmountToAdd = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Image = Global.HOT4.Console.My.Resources.Resources.RecordHS
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(198, 299)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 26)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdSave
        '
        Me.cmdSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdSave.Image = Global.HOT4.Console.My.Resources.Resources.OK
        Me.cmdSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSave.Location = New System.Drawing.Point(96, 299)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(96, 26)
        Me.cmdSave.TabIndex = 7
        Me.cmdSave.Text = "&Update Limit"
        Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'txtCurrentMonthly
        '
        Me.txtCurrentMonthly.Location = New System.Drawing.Point(67, 17)
        Me.txtCurrentMonthly.Name = "txtCurrentMonthly"
        Me.txtCurrentMonthly.ReadOnly = True
        Me.txtCurrentMonthly.Size = New System.Drawing.Size(184, 20)
        Me.txtCurrentMonthly.TabIndex = 118
        Me.txtCurrentMonthly.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 72)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(57, 13)
        Me.Label10.TabIndex = 117
        Me.Label10.Text = "Remaining"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtCurrentRemaining
        '
        Me.txtCurrentRemaining.Location = New System.Drawing.Point(67, 69)
        Me.txtCurrentRemaining.Name = "txtCurrentRemaining"
        Me.txtCurrentRemaining.ReadOnly = True
        Me.txtCurrentRemaining.Size = New System.Drawing.Size(184, 20)
        Me.txtCurrentRemaining.TabIndex = 115
        Me.txtCurrentRemaining.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(33, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 13)
        Me.Label2.TabIndex = 114
        Me.Label2.Text = "Daily"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtCurrentDaily
        '
        Me.txtCurrentDaily.Location = New System.Drawing.Point(67, 43)
        Me.txtCurrentDaily.Name = "txtCurrentDaily"
        Me.txtCurrentDaily.ReadOnly = True
        Me.txtCurrentDaily.Size = New System.Drawing.Size(184, 20)
        Me.txtCurrentDaily.TabIndex = 116
        Me.txtCurrentDaily.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 113
        Me.Label1.Text = "Monthly"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtCurrentRemaining)
        Me.GroupBox1.Controls.Add(Me.txtCurrentMonthly)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtCurrentDaily)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(261, 100)
        Me.GroupBox1.TabIndex = 119
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Current Limits"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtNewRemaining)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.txtNewDaily)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.txtNewMonthly)
        Me.GroupBox2.Location = New System.Drawing.Point(13, 195)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(260, 98)
        Me.GroupBox2.TabIndex = 120
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "New Limits"
        '
        'txtNewDaily
        '
        Me.txtNewDaily.Location = New System.Drawing.Point(66, 40)
        Me.txtNewDaily.Name = "txtNewDaily"
        Me.txtNewDaily.ReadOnly = True
        Me.txtNewDaily.Size = New System.Drawing.Size(184, 20)
        Me.txtNewDaily.TabIndex = 121
        Me.txtNewDaily.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 13)
        Me.Label3.TabIndex = 119
        Me.Label3.Text = "Monthly"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtNewMonthly
        '
        Me.txtNewMonthly.Location = New System.Drawing.Point(66, 14)
        Me.txtNewMonthly.Name = "txtNewMonthly"
        Me.txtNewMonthly.ReadOnly = True
        Me.txtNewMonthly.Size = New System.Drawing.Size(184, 20)
        Me.txtNewMonthly.TabIndex = 122
        Me.txtNewMonthly.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(32, 43)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 13)
        Me.Label4.TabIndex = 120
        Me.Label4.Text = "Daily"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtNewRemaining
        '
        Me.txtNewRemaining.Location = New System.Drawing.Point(66, 66)
        Me.txtNewRemaining.Name = "txtNewRemaining"
        Me.txtNewRemaining.ReadOnly = True
        Me.txtNewRemaining.Size = New System.Drawing.Size(184, 20)
        Me.txtNewRemaining.TabIndex = 124
        Me.txtNewRemaining.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(5, 69)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(57, 13)
        Me.Label5.TabIndex = 123
        Me.Label5.Text = "Remaining"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.txtDailyAmountToAdd)
        Me.GroupBox3.Controls.Add(Me.txtMonthlyAmountToAdd)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 114)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(261, 75)
        Me.GroupBox3.TabIndex = 121
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Amounts to Add to Limit"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(26, 47)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(33, 13)
        Me.Label7.TabIndex = 122
        Me.Label7.Text = " Daily"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(15, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(44, 13)
        Me.Label6.TabIndex = 123
        Me.Label6.Text = "Monthly"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtDailyAmountToAdd
        '
        Me.txtDailyAmountToAdd.Location = New System.Drawing.Point(67, 44)
        Me.txtDailyAmountToAdd.Name = "txtDailyAmountToAdd"
        Me.txtDailyAmountToAdd.Size = New System.Drawing.Size(184, 20)
        Me.txtDailyAmountToAdd.TabIndex = 124
        '
        'txtMonthlyAmountToAdd
        '
        Me.txtMonthlyAmountToAdd.Location = New System.Drawing.Point(67, 18)
        Me.txtMonthlyAmountToAdd.Name = "txtMonthlyAmountToAdd"
        Me.txtMonthlyAmountToAdd.Size = New System.Drawing.Size(184, 20)
        Me.txtMonthlyAmountToAdd.TabIndex = 125
        '
        'frmAdjustLimit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 338)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmAdjustLimit"
        Me.Text = "Adjust Limit"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdSave As Button
    Friend WithEvents txtCurrentMonthly As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtCurrentRemaining As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtCurrentDaily As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents txtNewRemaining As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtNewDaily As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtNewMonthly As TextBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents txtDailyAmountToAdd As TextBox
    Friend WithEvents txtMonthlyAmountToAdd As TextBox
End Class
