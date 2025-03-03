<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmBankLoader
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
        Me.components = New System.ComponentModel.Container()
        Me.txtDiscount = New System.Windows.Forms.TextBox()
        Me.Err = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmdImportTransactions = New System.Windows.Forms.Button()
        Me.dgTransactions = New System.Windows.Forms.DataGridView()
        Me.pb = New System.Windows.Forms.ProgressBar()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdLoad = New System.Windows.Forms.Button()
        Me.cmdBrowse = New System.Windows.Forms.Button()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.cboBank = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnEcoCash = New System.Windows.Forms.Button()
        Me.btnUpdateTrx = New System.Windows.Forms.Button()
        CType(Me.Err, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgTransactions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtDiscount
        '
        Me.txtDiscount.Location = New System.Drawing.Point(736, 444)
        Me.txtDiscount.Name = "txtDiscount"
        Me.txtDiscount.Size = New System.Drawing.Size(47, 20)
        Me.txtDiscount.TabIndex = 34
        Me.txtDiscount.Text = "0"
        '
        'Err
        '
        Me.Err.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.Err.ContainerControl = Me
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(598, 447)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(128, 13)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "CABS only Deposit Fee %"
        '
        'cmdImportTransactions
        '
        Me.cmdImportTransactions.Location = New System.Drawing.Point(789, 440)
        Me.cmdImportTransactions.Name = "cmdImportTransactions"
        Me.cmdImportTransactions.Size = New System.Drawing.Size(118, 26)
        Me.cmdImportTransactions.TabIndex = 32
        Me.cmdImportTransactions.Text = "Import Transactions"
        Me.cmdImportTransactions.UseVisualStyleBackColor = True
        '
        'dgTransactions
        '
        Me.dgTransactions.AllowUserToAddRows = False
        Me.dgTransactions.AllowUserToDeleteRows = False
        Me.dgTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgTransactions.Location = New System.Drawing.Point(12, 65)
        Me.dgTransactions.Name = "dgTransactions"
        Me.dgTransactions.ReadOnly = True
        Me.dgTransactions.Size = New System.Drawing.Size(895, 369)
        Me.dgTransactions.TabIndex = 31
        '
        'pb
        '
        Me.pb.Location = New System.Drawing.Point(83, 36)
        Me.pb.Name = "pb"
        Me.pb.Size = New System.Drawing.Size(657, 23)
        Me.pb.TabIndex = 30
        '
        'cmdCancel
        '
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Image = Global.HOT4.Console.My.Resources.Resources.DeleteHS
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(827, 4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 26)
        Me.cmdCancel.TabIndex = 26
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdLoad
        '
        Me.cmdLoad.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdLoad.Image = Global.HOT4.Console.My.Resources.Resources.RecordHS
        Me.cmdLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdLoad.Location = New System.Drawing.Point(746, 4)
        Me.cmdLoad.Name = "cmdLoad"
        Me.cmdLoad.Size = New System.Drawing.Size(75, 26)
        Me.cmdLoad.TabIndex = 25
        Me.cmdLoad.Text = "&Load"
        Me.cmdLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdLoad.UseVisualStyleBackColor = True
        '
        'cmdBrowse
        '
        Me.cmdBrowse.Image = Global.HOT4.Console.My.Resources.Resources.SearchFolderHS
        Me.cmdBrowse.Location = New System.Drawing.Point(708, 4)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.Size = New System.Drawing.Size(32, 26)
        Me.cmdBrowse.TabIndex = 24
        Me.cmdBrowse.UseVisualStyleBackColor = True
        '
        'txtFileName
        '
        Me.txtFileName.Location = New System.Drawing.Point(369, 8)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(333, 20)
        Me.txtFileName.TabIndex = 23
        '
        'cboBank
        '
        Me.cboBank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBank.FormattingEnabled = True
        Me.cboBank.Location = New System.Drawing.Point(83, 8)
        Me.cboBank.Name = "cboBank"
        Me.cboBank.Size = New System.Drawing.Size(211, 21)
        Me.cboBank.TabIndex = 22
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(309, 11)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 13)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "File Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(29, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 28
        Me.Label2.Text = "Progress"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(45, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Bank"
        '
        'btnEcoCash
        '
        Me.btnEcoCash.Image = Global.HOT4.Console.My.Resources.Resources.EditTableHS
        Me.btnEcoCash.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEcoCash.Location = New System.Drawing.Point(746, 34)
        Me.btnEcoCash.Name = "btnEcoCash"
        Me.btnEcoCash.Size = New System.Drawing.Size(156, 26)
        Me.btnEcoCash.TabIndex = 35
        Me.btnEcoCash.Text = "&View Ecocash Recon"
        Me.btnEcoCash.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEcoCash.UseVisualStyleBackColor = True
        Me.btnEcoCash.Visible = False
        '
        'btnUpdateTrx
        '
        Me.btnUpdateTrx.Location = New System.Drawing.Point(789, 440)
        Me.btnUpdateTrx.Name = "btnUpdateTrx"
        Me.btnUpdateTrx.Size = New System.Drawing.Size(118, 26)
        Me.btnUpdateTrx.TabIndex = 36
        Me.btnUpdateTrx.Text = "Update Transactions"
        Me.btnUpdateTrx.UseVisualStyleBackColor = True
        Me.btnUpdateTrx.Visible = False
        '
        'frmBankLoader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(919, 470)
        Me.Controls.Add(Me.btnEcoCash)
        Me.Controls.Add(Me.txtDiscount)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmdImportTransactions)
        Me.Controls.Add(Me.dgTransactions)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdLoad)
        Me.Controls.Add(Me.cmdBrowse)
        Me.Controls.Add(Me.txtFileName)
        Me.Controls.Add(Me.cboBank)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pb)
        Me.Controls.Add(Me.btnUpdateTrx)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBankLoader"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bank Loader"
        CType(Me.Err, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgTransactions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtDiscount As System.Windows.Forms.TextBox
    Friend WithEvents Err As System.Windows.Forms.ErrorProvider
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmdImportTransactions As System.Windows.Forms.Button
    Friend WithEvents dgTransactions As System.Windows.Forms.DataGridView
    Friend WithEvents pb As System.Windows.Forms.ProgressBar
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdLoad As System.Windows.Forms.Button
    Friend WithEvents cmdBrowse As System.Windows.Forms.Button
    Friend WithEvents txtFileName As System.Windows.Forms.TextBox
    Friend WithEvents cboBank As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnEcoCash As Button
    Friend WithEvents btnUpdateTrx As Button
End Class
