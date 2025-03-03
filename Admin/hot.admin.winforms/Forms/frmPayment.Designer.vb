<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPayment
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
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtPaymentID = New System.Windows.Forms.TextBox()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.cboSource = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtDate = New System.Windows.Forms.DateTimePicker()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtReference = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtUser = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.Err = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.txtTime = New System.Windows.Forms.MaskedTextBox()
        Me.txtDiscount = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtAccount = New System.Windows.Forms.TextBox()
        Me.txtProfileID = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        CType(Me.Err, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(40, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Payment #"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtPaymentID
        '
        Me.txtPaymentID.Location = New System.Drawing.Point(104, 12)
        Me.txtPaymentID.Name = "txtPaymentID"
        Me.txtPaymentID.ReadOnly = True
        Me.txtPaymentID.Size = New System.Drawing.Size(206, 20)
        Me.txtPaymentID.TabIndex = 99
        Me.txtPaymentID.TabStop = False
        '
        'cboType
        '
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.FormattingEnabled = True
        Me.cboType.Location = New System.Drawing.Point(104, 119)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(206, 21)
        Me.cboType.TabIndex = 0
        '
        'cboSource
        '
        Me.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSource.FormattingEnabled = True
        Me.cboSource.Location = New System.Drawing.Point(104, 146)
        Me.cboSource.Name = "cboSource"
        Me.cboSource.Size = New System.Drawing.Size(206, 21)
        Me.cboSource.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(57, 149)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Source"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(67, 122)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Type"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtAmount
        '
        Me.txtAmount.Location = New System.Drawing.Point(104, 173)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(206, 20)
        Me.txtAmount.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(55, 176)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(43, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Amount"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'dtDate
        '
        Me.dtDate.Enabled = False
        Me.dtDate.Location = New System.Drawing.Point(104, 199)
        Me.dtDate.Name = "dtDate"
        Me.dtDate.Size = New System.Drawing.Size(142, 20)
        Me.dtDate.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(68, 205)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(30, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Date"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtReference
        '
        Me.txtReference.Location = New System.Drawing.Point(104, 225)
        Me.txtReference.Name = "txtReference"
        Me.txtReference.Size = New System.Drawing.Size(206, 20)
        Me.txtReference.TabIndex = 4
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(41, 228)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(57, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Reference"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtUser
        '
        Me.txtUser.Location = New System.Drawing.Point(104, 251)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.ReadOnly = True
        Me.txtUser.Size = New System.Drawing.Size(206, 20)
        Me.txtUser.TabIndex = 99
        Me.txtUser.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(69, 254)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(29, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "User"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdSave
        '
        Me.cmdSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdSave.Image = Global.HOT4.Console.My.Resources.Resources.OK
        Me.cmdSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSave.Location = New System.Drawing.Point(103, 284)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(126, 26)
        Me.cmdSave.TabIndex = 5
        Me.cmdSave.Text = "&Receive Payment"
        Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Image = Global.HOT4.Console.My.Resources.Resources.RecordHS
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(235, 284)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 26)
        Me.cmdCancel.TabIndex = 6
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'Err
        '
        Me.Err.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.Err.ContainerControl = Me
        '
        'txtTime
        '
        Me.txtTime.Enabled = False
        Me.txtTime.Location = New System.Drawing.Point(252, 199)
        Me.txtTime.Mask = "90:00"
        Me.txtTime.Name = "txtTime"
        Me.txtTime.Size = New System.Drawing.Size(57, 20)
        Me.txtTime.TabIndex = 100
        Me.txtTime.ValidatingType = GetType(Date)
        '
        'txtDiscount
        '
        Me.txtDiscount.Location = New System.Drawing.Point(263, 93)
        Me.txtDiscount.Name = "txtDiscount"
        Me.txtDiscount.Size = New System.Drawing.Size(47, 20)
        Me.txtDiscount.TabIndex = 110
        Me.txtDiscount.Text = "0"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(185, 96)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(72, 13)
        Me.Label9.TabIndex = 109
        Me.Label9.Text = "Deposit fee %"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(51, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Account"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtAccount
        '
        Me.txtAccount.Location = New System.Drawing.Point(104, 38)
        Me.txtAccount.Name = "txtAccount"
        Me.txtAccount.ReadOnly = True
        Me.txtAccount.Size = New System.Drawing.Size(206, 20)
        Me.txtAccount.TabIndex = 99
        Me.txtAccount.TabStop = False
        '
        'txtProfileID
        '
        Me.txtProfileID.Location = New System.Drawing.Point(103, 64)
        Me.txtProfileID.Name = "txtProfileID"
        Me.txtProfileID.ReadOnly = True
        Me.txtProfileID.Size = New System.Drawing.Size(206, 20)
        Me.txtProfileID.TabIndex = 112
        Me.txtProfileID.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(50, 67)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(36, 13)
        Me.Label10.TabIndex = 111
        Me.Label10.Text = "Profile"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmPayment
        '
        Me.AcceptButton = Me.cmdSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(345, 324)
        Me.Controls.Add(Me.txtProfileID)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtDiscount)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtTime)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.txtUser)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtReference)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.dtDate)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cboSource)
        Me.Controls.Add(Me.txtAccount)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cboType)
        Me.Controls.Add(Me.txtPaymentID)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPayment"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Payment"
        CType(Me.Err, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPaymentID As System.Windows.Forms.TextBox
    Friend WithEvents cboType As System.Windows.Forms.ComboBox
    Friend WithEvents cboSource As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents dtDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtReference As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtUser As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents Err As System.Windows.Forms.ErrorProvider
    Friend WithEvents txtTime As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtDiscount As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtProfileID As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtAccount As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
