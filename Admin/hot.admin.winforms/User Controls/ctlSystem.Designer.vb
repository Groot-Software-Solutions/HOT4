<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlSystem
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ChkBxNet1 = New System.Windows.Forms.CheckBox()
        Me.txtNetOne = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.dgError = New System.Windows.Forms.DataGridView()
        Me.txtTimerTick = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtTelecel = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblWorking = New System.Windows.Forms.Label()
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtNetoneUSD = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtTelecelUSDBalance = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtEconet = New System.Windows.Forms.TextBox()
        Me.txtEconetUsd = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtTeloneUSD = New System.Windows.Forms.TextBox()
        Me.txtTelone = New System.Windows.Forms.TextBox()
        Me.txtZESAUSD = New System.Windows.Forms.TextBox()
        Me.txtZesa = New System.Windows.Forms.TextBox()
        CType(Me.dgError, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ChkBxNet1
        '
        Me.ChkBxNet1.AutoSize = True
        Me.ChkBxNet1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ChkBxNet1.Location = New System.Drawing.Point(27, 6)
        Me.ChkBxNet1.Name = "ChkBxNet1"
        Me.ChkBxNet1.Size = New System.Drawing.Size(75, 17)
        Me.ChkBxNet1.TabIndex = 13
        Me.ChkBxNet1.Text = "Run Timer"
        Me.ChkBxNet1.UseVisualStyleBackColor = True
        '
        'txtNetOne
        '
        Me.txtNetOne.Location = New System.Drawing.Point(241, 34)
        Me.txtNetOne.Name = "txtNetOne"
        Me.txtNetOne.ReadOnly = True
        Me.txtNetOne.Size = New System.Drawing.Size(217, 20)
        Me.txtNetOne.TabIndex = 12
        '
        'Timer1
        '
        Me.Timer1.Interval = 60000
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(14, 9)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 15
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'dgError
        '
        Me.dgError.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgError.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgError.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgError.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgError.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgError.Location = New System.Drawing.Point(0, 57)
        Me.dgError.Name = "dgError"
        Me.dgError.Size = New System.Drawing.Size(852, 243)
        Me.dgError.TabIndex = 17
        '
        'txtTimerTick
        '
        Me.txtTimerTick.Location = New System.Drawing.Point(119, 4)
        Me.txtTimerTick.Name = "txtTimerTick"
        Me.txtTimerTick.Size = New System.Drawing.Size(35, 20)
        Me.txtTimerTick.TabIndex = 18
        Me.txtTimerTick.Text = "1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(160, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(103, 13)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Timer Tick (Minutes)"
        '
        'txtTelecel
        '
        Me.txtTelecel.BackColor = System.Drawing.SystemColors.Control
        Me.txtTelecel.Location = New System.Drawing.Point(241, 58)
        Me.txtTelecel.Name = "txtTelecel"
        Me.txtTelecel.Size = New System.Drawing.Size(217, 20)
        Me.txtTelecel.TabIndex = 20
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(149, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 13)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "NetOne Balance"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(151, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 13)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "Telecel Balance"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(26, 32)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(333, 13)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "System Error Status Checker - Does not guarantee all Systems are ok"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label5.Location = New System.Drawing.Point(0, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(93, 13)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "Recharge Failures"
        '
        'lblWorking
        '
        Me.lblWorking.AutoSize = True
        Me.lblWorking.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWorking.Location = New System.Drawing.Point(392, 19)
        Me.lblWorking.Name = "lblWorking"
        Me.lblWorking.Size = New System.Drawing.Size(89, 20)
        Me.lblWorking.TabIndex = 25
        Me.lblWorking.Text = "Working..."
        Me.lblWorking.Visible = False
        '
        'ToolStripContainer1
        '
        Me.ToolStripContainer1.BottomToolStripPanelVisible = False
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.BackColor = System.Drawing.Color.MistyRose
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.dgError)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.lblWorking)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.ChkBxNet1)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.txtTimerTick)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.Label4)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.Label1)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(852, 300)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Top
        Me.ToolStripContainer1.LeftToolStripPanelVisible = False
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.RightToolStripPanelVisible = False
        Me.ToolStripContainer1.Size = New System.Drawing.Size(852, 300)
        Me.ToolStripContainer1.TabIndex = 26
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        Me.ToolStripContainer1.TopToolStripPanelVisible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtNetoneUSD)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.txtTelecelUSDBalance)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.txtEconet)
        Me.Panel1.Controls.Add(Me.txtEconetUsd)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.txtTeloneUSD)
        Me.Panel1.Controls.Add(Me.txtTelone)
        Me.Panel1.Controls.Add(Me.btnRefresh)
        Me.Panel1.Controls.Add(Me.txtZESAUSD)
        Me.Panel1.Controls.Add(Me.txtZesa)
        Me.Panel1.Controls.Add(Me.txtTelecel)
        Me.Panel1.Controls.Add(Me.txtNetOne)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 332)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(852, 166)
        Me.Panel1.TabIndex = 27
        '
        'txtNetoneUSD
        '
        Me.txtNetoneUSD.Location = New System.Drawing.Point(614, 34)
        Me.txtNetoneUSD.Margin = New System.Windows.Forms.Padding(2)
        Me.txtNetoneUSD.Multiline = True
        Me.txtNetoneUSD.Name = "txtNetoneUSD"
        Me.txtNetoneUSD.ReadOnly = True
        Me.txtNetoneUSD.Size = New System.Drawing.Size(217, 19)
        Me.txtNetoneUSD.TabIndex = 30
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(502, 37)
        Me.Label11.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(110, 13)
        Me.Label11.TabIndex = 29
        Me.Label11.Text = "Netone USD Balance"
        '
        'txtTelecelUSDBalance
        '
        Me.txtTelecelUSDBalance.Location = New System.Drawing.Point(614, 57)
        Me.txtTelecelUSDBalance.Margin = New System.Windows.Forms.Padding(2)
        Me.txtTelecelUSDBalance.Multiline = True
        Me.txtTelecelUSDBalance.Name = "txtTelecelUSDBalance"
        Me.txtTelecelUSDBalance.ReadOnly = True
        Me.txtTelecelUSDBalance.Size = New System.Drawing.Size(217, 19)
        Me.txtTelecelUSDBalance.TabIndex = 28
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(501, 60)
        Me.Label10.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(110, 13)
        Me.Label10.TabIndex = 27
        Me.Label10.Text = "Telecel USD Balance"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(116, 14)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(119, 13)
        Me.Label7.TabIndex = 26
        Me.Label7.Text = "Econet Bundle Balance"
        '
        'txtEconet
        '
        Me.txtEconet.Location = New System.Drawing.Point(241, 10)
        Me.txtEconet.Name = "txtEconet"
        Me.txtEconet.ReadOnly = True
        Me.txtEconet.Size = New System.Drawing.Size(217, 20)
        Me.txtEconet.TabIndex = 25
        '
        'txtEconetUsd
        '
        Me.txtEconetUsd.Location = New System.Drawing.Point(614, 11)
        Me.txtEconetUsd.Margin = New System.Windows.Forms.Padding(2)
        Me.txtEconetUsd.Multiline = True
        Me.txtEconetUsd.Name = "txtEconetUsd"
        Me.txtEconetUsd.ReadOnly = True
        Me.txtEconetUsd.Size = New System.Drawing.Size(217, 19)
        Me.txtEconetUsd.TabIndex = 24
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(502, 14)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(109, 13)
        Me.Label6.TabIndex = 23
        Me.Label6.Text = "Econet USD Balance"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(502, 106)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(110, 13)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "TelOne USD Balance"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(153, 109)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(84, 13)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "TelOne Balance"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(512, 83)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(99, 13)
        Me.Label13.TabIndex = 21
        Me.Label13.Text = "Zesa USD Balance"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(162, 85)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(73, 13)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Zesa Balance"
        '
        'txtTeloneUSD
        '
        Me.txtTeloneUSD.BackColor = System.Drawing.SystemColors.Control
        Me.txtTeloneUSD.Location = New System.Drawing.Point(614, 104)
        Me.txtTeloneUSD.Name = "txtTeloneUSD"
        Me.txtTeloneUSD.Size = New System.Drawing.Size(217, 20)
        Me.txtTeloneUSD.TabIndex = 20
        '
        'txtTelone
        '
        Me.txtTelone.BackColor = System.Drawing.SystemColors.Control
        Me.txtTelone.Location = New System.Drawing.Point(241, 105)
        Me.txtTelone.Name = "txtTelone"
        Me.txtTelone.Size = New System.Drawing.Size(217, 20)
        Me.txtTelone.TabIndex = 20
        '
        'txtZESAUSD
        '
        Me.txtZESAUSD.Location = New System.Drawing.Point(614, 80)
        Me.txtZESAUSD.Name = "txtZESAUSD"
        Me.txtZESAUSD.ReadOnly = True
        Me.txtZESAUSD.Size = New System.Drawing.Size(217, 20)
        Me.txtZESAUSD.TabIndex = 12
        '
        'txtZesa
        '
        Me.txtZesa.Location = New System.Drawing.Point(241, 81)
        Me.txtZesa.Name = "txtZesa"
        Me.txtZesa.ReadOnly = True
        Me.txtZesa.Size = New System.Drawing.Size(217, 20)
        Me.txtZesa.TabIndex = 12
        '
        'ctlSystem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.MistyRose
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Controls.Add(Me.Label5)
        Me.Name = "ctlSystem"
        Me.Size = New System.Drawing.Size(852, 498)
        CType(Me.dgError, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.ContentPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ChkBxNet1 As System.Windows.Forms.CheckBox
    Friend WithEvents txtNetOne As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents dgError As System.Windows.Forms.DataGridView
    Friend WithEvents txtTimerTick As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtTelecel As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblWorking As System.Windows.Forms.Label
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtEconetUsd As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As Label
    Friend WithEvents txtEconet As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents txtTelone As TextBox
    Friend WithEvents txtZesa As TextBox
    Friend WithEvents txtTelecelUSDBalance As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtNetoneUSD As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents txtTeloneUSD As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtZESAUSD As TextBox
End Class
