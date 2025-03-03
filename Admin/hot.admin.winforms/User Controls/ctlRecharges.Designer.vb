<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlRecharges
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
        Me.components = New System.ComponentModel.Container()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtDebit = New System.Windows.Forms.TextBox()
        Me.btnDebit = New System.Windows.Forms.Button()
        Me.btnPhoneBal = New System.Windows.Forms.Button()
        Me.cmdReversal = New System.Windows.Forms.Button()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgRecharge = New System.Windows.Forms.DataGridView()
        Me.Err = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.dgRechargeDetail = New System.Windows.Forms.DataGridView()
        Me.Panel1.SuspendLayout()
        CType(Me.dgRecharge, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Err, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgRechargeDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.txtDebit)
        Me.Panel1.Controls.Add(Me.btnDebit)
        Me.Panel1.Controls.Add(Me.btnPhoneBal)
        Me.Panel1.Controls.Add(Me.cmdReversal)
        Me.Panel1.Controls.Add(Me.lblSearch)
        Me.Panel1.Controls.Add(Me.cmdSearch)
        Me.Panel1.Controls.Add(Me.txtFilter)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(896, 59)
        Me.Panel1.TabIndex = 1
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Location = New System.Drawing.Point(640, 22)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(113, 13)
        Me.Label11.TabIndex = 23
        Me.Label11.Text = "Amount to be Debited "
        '
        'txtDebit
        '
        Me.txtDebit.Location = New System.Drawing.Point(759, 18)
        Me.txtDebit.Name = "txtDebit"
        Me.txtDebit.Size = New System.Drawing.Size(36, 20)
        Me.txtDebit.TabIndex = 22
        Me.txtDebit.Text = "0"
        Me.txtDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnDebit
        '
        Me.btnDebit.Location = New System.Drawing.Point(801, 16)
        Me.btnDebit.Name = "btnDebit"
        Me.btnDebit.Size = New System.Drawing.Size(75, 25)
        Me.btnDebit.TabIndex = 21
        Me.btnDebit.Text = "Debit"
        Me.btnDebit.UseVisualStyleBackColor = True
        '
        'btnPhoneBal
        '
        Me.btnPhoneBal.Image = Global.HOT4.Console.My.Resources.Resources.FindHS
        Me.btnPhoneBal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPhoneBal.Location = New System.Drawing.Point(527, 15)
        Me.btnPhoneBal.Name = "btnPhoneBal"
        Me.btnPhoneBal.Size = New System.Drawing.Size(106, 26)
        Me.btnPhoneBal.TabIndex = 20
        Me.btnPhoneBal.Text = "&Phone Balance"
        Me.btnPhoneBal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPhoneBal.UseVisualStyleBackColor = True
        '
        'cmdReversal
        '
        Me.cmdReversal.Image = Global.HOT4.Console.My.Resources.Resources.DoubleLeftArrowHS
        Me.cmdReversal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdReversal.Location = New System.Drawing.Point(446, 15)
        Me.cmdReversal.Name = "cmdReversal"
        Me.cmdReversal.Size = New System.Drawing.Size(75, 26)
        Me.cmdReversal.TabIndex = 19
        Me.cmdReversal.Text = "&Reversal"
        Me.cmdReversal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdReversal.UseVisualStyleBackColor = True
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.Location = New System.Drawing.Point(335, 18)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(105, 20)
        Me.lblSearch.TabIndex = 2
        Me.lblSearch.Text = "Searching......"
        Me.lblSearch.Visible = False
        '
        'cmdSearch
        '
        Me.cmdSearch.Image = Global.HOT4.Console.My.Resources.Resources.FindHS
        Me.cmdSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSearch.Location = New System.Drawing.Point(254, 15)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(75, 26)
        Me.cmdSearch.TabIndex = 1
        Me.cmdSearch.Text = "&Search"
        Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSearch.UseVisualStyleBackColor = True
        '
        'txtFilter
        '
        Me.txtFilter.Location = New System.Drawing.Point(51, 19)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(181, 20)
        Me.txtFilter.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Mobile"
        '
        'dgRecharge
        '
        Me.dgRecharge.AllowUserToAddRows = False
        Me.dgRecharge.AllowUserToDeleteRows = False
        Me.dgRecharge.AllowUserToOrderColumns = True
        Me.dgRecharge.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgRecharge.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgRecharge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgRecharge.Location = New System.Drawing.Point(0, 59)
        Me.dgRecharge.MultiSelect = False
        Me.dgRecharge.Name = "dgRecharge"
        Me.dgRecharge.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgRecharge.Size = New System.Drawing.Size(896, 451)
        Me.dgRecharge.TabIndex = 2
        '
        'Err
        '
        Me.Err.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.Err.ContainerControl = Me
        '
        'dgRechargeDetail
        '
        Me.dgRechargeDetail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgRechargeDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgRechargeDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgRechargeDetail.Location = New System.Drawing.Point(0, 516)
        Me.dgRechargeDetail.Name = "dgRechargeDetail"
        Me.dgRechargeDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgRechargeDetail.Size = New System.Drawing.Size(896, 144)
        Me.dgRechargeDetail.TabIndex = 8
        '
        'ctlRecharges
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dgRechargeDetail)
        Me.Controls.Add(Me.dgRecharge)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "ctlRecharges"
        Me.Size = New System.Drawing.Size(896, 660)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgRecharge, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Err, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgRechargeDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblSearch As Label
    Friend WithEvents cmdSearch As Button
    Friend WithEvents txtFilter As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents dgRecharge As DataGridView
    Friend WithEvents Err As ErrorProvider
    Friend WithEvents Label11 As Label
    Friend WithEvents txtDebit As TextBox
    Friend WithEvents btnDebit As Button
    Friend WithEvents btnPhoneBal As Button
    Friend WithEvents cmdReversal As Button
    Friend WithEvents dgRechargeDetail As DataGridView
End Class
