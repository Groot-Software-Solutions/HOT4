<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSuspense
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
        Me.txtTransactionID = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAccount = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdFind = New System.Windows.Forms.Button()
        Me.cmdIgnore = New System.Windows.Forms.Button()
        Me.cboBank = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDiscount = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtReference = New System.Windows.Forms.TextBox()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtBank = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtIgnore = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtTransactionID
        '
        Me.txtTransactionID.Location = New System.Drawing.Point(106, 70)
        Me.txtTransactionID.Name = "txtTransactionID"
        Me.txtTransactionID.ReadOnly = True
        Me.txtTransactionID.Size = New System.Drawing.Size(244, 20)
        Me.txtTransactionID.TabIndex = 101
        Me.txtTransactionID.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 73)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 13)
        Me.Label1.TabIndex = 100
        Me.Label1.Text = "Transaction #"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtAccount
        '
        Me.txtAccount.Location = New System.Drawing.Point(106, 16)
        Me.txtAccount.Name = "txtAccount"
        Me.txtAccount.ReadOnly = True
        Me.txtAccount.Size = New System.Drawing.Size(244, 20)
        Me.txtAccount.TabIndex = 103
        Me.txtAccount.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 102
        Me.Label2.Text = "Account Search"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdSave.Image = Global.HOT4.Console.My.Resources.Resources.OK
        Me.cmdSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSave.Location = New System.Drawing.Point(196, 277)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(116, 26)
        Me.cmdSave.TabIndex = 7
        Me.cmdSave.Text = "&Save to Customer"
        Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Image = Global.HOT4.Console.My.Resources.Resources.RecordHS
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(318, 277)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(66, 26)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdFind
        '
        Me.cmdFind.Image = Global.HOT4.Console.My.Resources.Resources.FindHS
        Me.cmdFind.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdFind.Location = New System.Drawing.Point(356, 12)
        Me.cmdFind.Name = "cmdFind"
        Me.cmdFind.Size = New System.Drawing.Size(28, 26)
        Me.cmdFind.TabIndex = 104
        Me.cmdFind.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdFind.UseVisualStyleBackColor = True
        '
        'cmdIgnore
        '
        Me.cmdIgnore.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdIgnore.Image = Global.HOT4.Console.My.Resources.Resources.BreakpointHS
        Me.cmdIgnore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdIgnore.Location = New System.Drawing.Point(285, 240)
        Me.cmdIgnore.Name = "cmdIgnore"
        Me.cmdIgnore.Size = New System.Drawing.Size(65, 26)
        Me.cmdIgnore.TabIndex = 9
        Me.cmdIgnore.Text = "&Ignore"
        Me.cmdIgnore.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdIgnore.UseVisualStyleBackColor = True
        '
        'cboBank
        '
        Me.cboBank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBank.FormattingEnabled = True
        Me.cboBank.Location = New System.Drawing.Point(327, 99)
        Me.cboBank.Name = "cboBank"
        Me.cboBank.Size = New System.Drawing.Size(23, 21)
        Me.cboBank.TabIndex = 105
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(67, 103)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 13)
        Me.Label3.TabIndex = 106
        Me.Label3.Text = "Bank"
        '
        'txtDiscount
        '
        Me.txtDiscount.Location = New System.Drawing.Point(306, 201)
        Me.txtDiscount.Name = "txtDiscount"
        Me.txtDiscount.Size = New System.Drawing.Size(44, 20)
        Me.txtDiscount.TabIndex = 108
        Me.txtDiscount.Text = "0"
        '
        'Label4
        '
        Me.Label4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label4.Location = New System.Drawing.Point(255, 200)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 28)
        Me.Label4.TabIndex = 107
        Me.Label4.Text = "Deposit fee %"
        '
        'txtReference
        '
        Me.txtReference.Location = New System.Drawing.Point(106, 127)
        Me.txtReference.Multiline = True
        Me.txtReference.Name = "txtReference"
        Me.txtReference.Size = New System.Drawing.Size(244, 70)
        Me.txtReference.TabIndex = 109
        '
        'txtAmount
        '
        Me.txtAmount.Location = New System.Drawing.Point(106, 201)
        Me.txtAmount.MaxLength = 50
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(143, 20)
        Me.txtAmount.TabIndex = 110
        Me.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(58, 204)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(43, 13)
        Me.Label5.TabIndex = 111
        Me.Label5.Text = "Amount"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(43, 130)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(57, 13)
        Me.Label6.TabIndex = 112
        Me.Label6.Text = "Reference"
        '
        'txtBank
        '
        Me.txtBank.Location = New System.Drawing.Point(106, 100)
        Me.txtBank.Name = "txtBank"
        Me.txtBank.Size = New System.Drawing.Size(215, 20)
        Me.txtBank.TabIndex = 113
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(15, 41)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(369, 26)
        Me.Label7.TabIndex = 114
        Me.Label7.Text = "Click Save to Customer to reciept this bank transaction as a Payment to the Accou" &
    "nt shown"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Image = Global.HOT4.Console.My.Resources.Resources.EditTableHS
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(75, 277)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(115, 26)
        Me.Button1.TabIndex = 115
        Me.Button1.Text = "&Manual Allocated"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtIgnore
        '
        Me.txtIgnore.FormattingEnabled = True
        Me.txtIgnore.Items.AddRange(New Object() {"", "Ecocash Bank Transfer", "IMTT Charges", "Bank Charges", "Supplier Payment", "Internal Account Transfer", "Reversal Processed"})
        Me.txtIgnore.Location = New System.Drawing.Point(106, 240)
        Me.txtIgnore.Name = "txtIgnore"
        Me.txtIgnore.Size = New System.Drawing.Size(173, 21)
        Me.txtIgnore.TabIndex = 116
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(15, 243)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(88, 13)
        Me.Label8.TabIndex = 117
        Me.Label8.Text = "Reason to ignore"
        '
        'frmSuspense
        '
        Me.AcceptButton = Me.cmdSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(408, 315)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtIgnore)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtBank)
        Me.Controls.Add(Me.txtReference)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmdFind)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.txtTransactionID)
        Me.Controls.Add(Me.cmdIgnore)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cboBank)
        Me.Controls.Add(Me.txtDiscount)
        Me.Controls.Add(Me.txtAccount)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.Label4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSuspense"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Transaction Suspense"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents cmdIgnore As System.Windows.Forms.Button
    Friend WithEvents txtTransactionID As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAccount As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmdFind As System.Windows.Forms.Button
    Friend WithEvents cboBank As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtDiscount As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtReference As System.Windows.Forms.TextBox
    Friend WithEvents txtAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtBank As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Button1 As Button
    Friend WithEvents txtIgnore As ComboBox
    Friend WithEvents Label8 As Label
End Class
