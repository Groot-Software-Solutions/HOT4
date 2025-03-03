<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBulkSMS
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
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCounter = New System.Windows.Forms.TextBox()
        Me.txtCounter2 = New System.Windows.Forms.TextBox()
        Me.txtBody = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSubject = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.lblAgg = New System.Windows.Forms.Label()
        Me.lblSMS = New System.Windows.Forms.Label()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.btnFormat = New System.Windows.Forms.Button()
        Me.cmdSendToAggregators = New System.Windows.Forms.Button()
        Me.cmdSendToCorporates = New System.Windows.Forms.Button()
        Me.cmdSend = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtMessage
        '
        Me.txtMessage.AcceptsReturn = True
        Me.txtMessage.AcceptsTab = True
        Me.txtMessage.AllowDrop = True
        Me.txtMessage.HideSelection = False
        Me.txtMessage.Location = New System.Drawing.Point(48, 115)
        Me.txtMessage.Margin = New System.Windows.Forms.Padding(6)
        Me.txtMessage.MaxLength = 160
        Me.txtMessage.Multiline = True
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(337, 454)
        Me.txtMessage.TabIndex = 0
        Me.txtMessage.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "HOT Recharge"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(48, 25)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 25)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Message"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtCounter
        '
        Me.txtCounter.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCounter.Location = New System.Drawing.Point(295, 45)
        Me.txtCounter.Margin = New System.Windows.Forms.Padding(6)
        Me.txtCounter.MaxLength = 4
        Me.txtCounter.Name = "txtCounter"
        Me.txtCounter.ReadOnly = True
        Me.txtCounter.ShortcutsEnabled = False
        Me.txtCounter.Size = New System.Drawing.Size(90, 55)
        Me.txtCounter.TabIndex = 10
        Me.txtCounter.TabStop = False
        '
        'txtCounter2
        '
        Me.txtCounter2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCounter2.Location = New System.Drawing.Point(1102, 45)
        Me.txtCounter2.Margin = New System.Windows.Forms.Padding(6)
        Me.txtCounter2.MaxLength = 4
        Me.txtCounter2.Name = "txtCounter2"
        Me.txtCounter2.ReadOnly = True
        Me.txtCounter2.ShortcutsEnabled = False
        Me.txtCounter2.Size = New System.Drawing.Size(90, 55)
        Me.txtCounter2.TabIndex = 20
        Me.txtCounter2.TabStop = False
        '
        'txtBody
        '
        Me.txtBody.AcceptsReturn = True
        Me.txtBody.AcceptsTab = True
        Me.txtBody.AllowDrop = True
        Me.txtBody.HideSelection = False
        Me.txtBody.Location = New System.Drawing.Point(528, 115)
        Me.txtBody.Margin = New System.Windows.Forms.Padding(6)
        Me.txtBody.MaxLength = 1000
        Me.txtBody.Multiline = True
        Me.txtBody.Name = "txtBody"
        Me.txtBody.Size = New System.Drawing.Size(664, 454)
        Me.txtBody.TabIndex = 17
        Me.txtBody.Text = "" & Global.Microsoft.VisualBasic.ChrW(13)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(416, 67)
        Me.Label2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 25)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Subject"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtSubject
        '
        Me.txtSubject.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubject.Location = New System.Drawing.Point(528, 56)
        Me.txtSubject.Margin = New System.Windows.Forms.Padding(6)
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.Size = New System.Drawing.Size(561, 37)
        Me.txtSubject.TabIndex = 22
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(422, 25)
        Me.Label3.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(246, 25)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "Email - <br> for New line"
        '
        'lblEmail
        '
        Me.lblEmail.Location = New System.Drawing.Point(523, 629)
        Me.lblEmail.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblEmail.MaximumSize = New System.Drawing.Size(360, 60)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(331, 60)
        Me.lblEmail.TabIndex = 25
        Me.lblEmail.Text = "Emails to Corporate Customers"
        '
        'lblAgg
        '
        Me.lblAgg.Location = New System.Drawing.Point(866, 629)
        Me.lblAgg.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblAgg.Name = "lblAgg"
        Me.lblAgg.Size = New System.Drawing.Size(326, 60)
        Me.lblAgg.TabIndex = 26
        Me.lblAgg.Text = "Emails to Aggregators"
        '
        'lblSMS
        '
        Me.lblSMS.Location = New System.Drawing.Point(48, 629)
        Me.lblSMS.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblSMS.Name = "lblSMS"
        Me.lblSMS.Size = New System.Drawing.Size(337, 25)
        Me.lblSMS.TabIndex = 27
        Me.lblSMS.Text = "SMS"
        '
        'btnCopy
        '
        Me.btnCopy.Location = New System.Drawing.Point(403, 178)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(106, 44)
        Me.btnCopy.TabIndex = 28
        Me.btnCopy.Text = "Copy >"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'btnFormat
        '
        Me.btnFormat.Location = New System.Drawing.Point(403, 284)
        Me.btnFormat.Name = "btnFormat"
        Me.btnFormat.Size = New System.Drawing.Size(106, 114)
        Me.btnFormat.TabIndex = 29
        Me.btnFormat.Text = "Format email as HTML "
        Me.btnFormat.UseVisualStyleBackColor = True
        '
        'cmdSendToAggregators
        '
        Me.cmdSendToAggregators.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSendToAggregators.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdSendToAggregators.Image = Global.HOT4.Console.My.Resources.Resources.OK
        Me.cmdSendToAggregators.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSendToAggregators.Location = New System.Drawing.Point(866, 573)
        Me.cmdSendToAggregators.Margin = New System.Windows.Forms.Padding(6)
        Me.cmdSendToAggregators.Name = "cmdSendToAggregators"
        Me.cmdSendToAggregators.Size = New System.Drawing.Size(326, 50)
        Me.cmdSendToAggregators.TabIndex = 24
        Me.cmdSendToAggregators.Text = "&Send Email to Aggregators"
        Me.cmdSendToAggregators.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSendToAggregators.UseVisualStyleBackColor = True
        '
        'cmdSendToCorporates
        '
        Me.cmdSendToCorporates.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSendToCorporates.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdSendToCorporates.Image = Global.HOT4.Console.My.Resources.Resources.OK
        Me.cmdSendToCorporates.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSendToCorporates.Location = New System.Drawing.Point(528, 573)
        Me.cmdSendToCorporates.Margin = New System.Windows.Forms.Padding(6)
        Me.cmdSendToCorporates.Name = "cmdSendToCorporates"
        Me.cmdSendToCorporates.Size = New System.Drawing.Size(326, 50)
        Me.cmdSendToCorporates.TabIndex = 18
        Me.cmdSendToCorporates.Text = "&Send Email to Corporates"
        Me.cmdSendToCorporates.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSendToCorporates.UseVisualStyleBackColor = True
        '
        'cmdSend
        '
        Me.cmdSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSend.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdSend.Image = Global.HOT4.Console.My.Resources.Resources.OK
        Me.cmdSend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSend.Location = New System.Drawing.Point(47, 573)
        Me.cmdSend.Margin = New System.Windows.Forms.Padding(6)
        Me.cmdSend.Name = "cmdSend"
        Me.cmdSend.Size = New System.Drawing.Size(338, 50)
        Me.cmdSend.TabIndex = 2
        Me.cmdSend.Text = "&Send Bulk SMS to Dealers"
        Me.cmdSend.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSend.UseVisualStyleBackColor = True
        '
        'frmBulkSMS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(1215, 729)
        Me.Controls.Add(Me.btnFormat)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.lblSMS)
        Me.Controls.Add(Me.lblAgg)
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.cmdSendToAggregators)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtSubject)
        Me.Controls.Add(Me.txtCounter2)
        Me.Controls.Add(Me.cmdSendToCorporates)
        Me.Controls.Add(Me.txtBody)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtCounter)
        Me.Controls.Add(Me.cmdSend)
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1500, 1000)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(1200, 800)
        Me.Name = "frmBulkSMS"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bulk SMS"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdSend As System.Windows.Forms.Button
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCounter As System.Windows.Forms.TextBox
    Friend WithEvents txtCounter2 As System.Windows.Forms.TextBox
    Friend WithEvents cmdSendToCorporates As System.Windows.Forms.Button
    Friend WithEvents txtBody As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtSubject As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmdSendToAggregators As System.Windows.Forms.Button
    Friend WithEvents lblEmail As System.Windows.Forms.Label
    Friend WithEvents lblAgg As System.Windows.Forms.Label
    Friend WithEvents lblSMS As System.Windows.Forms.Label
    Friend WithEvents btnCopy As System.Windows.Forms.Button
    Friend WithEvents btnFormat As System.Windows.Forms.Button
End Class
