<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConsole
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConsole))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.lblUser = New System.Windows.Forms.ToolStripLabel()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdSystem = New System.Windows.Forms.ToolStripButton()
        Me.cmdAccountSearch = New System.Windows.Forms.ToolStripButton()
        Me.cmdPins = New System.Windows.Forms.ToolStripButton()
        Me.cmdBank = New System.Windows.Forms.ToolStripButton()
        Me.cmdBulkSMS = New System.Windows.Forms.ToolStripButton()
        Me.cmdSmppActivity = New System.Windows.Forms.ToolStripButton()
        Me.cmdRecharges = New System.Windows.Forms.ToolStripButton()
        Me.cmdAdmin = New System.Windows.Forms.ToolStripButton()
        Me.tcMain = New HOT4.Console.TabControlEx()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblUser, Me.cmdSystem, Me.cmdAccountSearch, Me.cmdPins, Me.cmdBank, Me.cmdBulkSMS, Me.cmdSmppActivity, Me.cmdRecharges, Me.cmdAdmin})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(870, 39)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'lblUser
        '
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Size = New System.Drawing.Size(113, 36)
        Me.lblUser.Text = "You are not logged on"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 386)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(870, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "search4people.ico")
        Me.ImageList1.Images.SetKeyName(1, "user.ico")
        Me.ImageList1.Images.SetKeyName(2, "BarCodeHS.png")
        Me.ImageList1.Images.SetKeyName(3, "HomeHS.png")
        Me.ImageList1.Images.SetKeyName(4, "FindHS.png")
        Me.ImageList1.Images.SetKeyName(5, "SearchFolderHS.png")
        Me.ImageList1.Images.SetKeyName(6, "EditTableHS.png")
        '
        'cmdSystem
        '
        Me.cmdSystem.Image = Global.HOT4.Console.My.Resources.Resources.HomeHS
        Me.cmdSystem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdSystem.Name = "cmdSystem"
        Me.cmdSystem.Size = New System.Drawing.Size(78, 36)
        Me.cmdSystem.Text = "System"
        '
        'cmdAccountSearch
        '
        Me.cmdAccountSearch.Image = CType(resources.GetObject("cmdAccountSearch.Image"), System.Drawing.Image)
        Me.cmdAccountSearch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdAccountSearch.Name = "cmdAccountSearch"
        Me.cmdAccountSearch.Size = New System.Drawing.Size(118, 36)
        Me.cmdAccountSearch.Text = "Account Search"
        '
        'cmdPins
        '
        Me.cmdPins.Image = Global.HOT4.Console.My.Resources.Resources.BarCodeHS
        Me.cmdPins.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdPins.Name = "cmdPins"
        Me.cmdPins.Size = New System.Drawing.Size(62, 36)
        Me.cmdPins.Text = "Pins"
        '
        'cmdBank
        '
        Me.cmdBank.Image = Global.HOT4.Console.My.Resources.Resources.HomeHS
        Me.cmdBank.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdBank.Name = "cmdBank"
        Me.cmdBank.Size = New System.Drawing.Size(80, 36)
        Me.cmdBank.Text = "Banking"
        '
        'cmdBulkSMS
        '
        Me.cmdBulkSMS.Image = Global.HOT4.Console.My.Resources.Resources.EnvelopeHS
        Me.cmdBulkSMS.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdBulkSMS.Name = "cmdBulkSMS"
        Me.cmdBulkSMS.Size = New System.Drawing.Size(85, 36)
        Me.cmdBulkSMS.Text = "Bulk SMS"
        '
        'cmdSmppActivity
        '
        Me.cmdSmppActivity.Image = Global.HOT4.Console.My.Resources.Resources.SearchFolderHS
        Me.cmdSmppActivity.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdSmppActivity.Name = "cmdSmppActivity"
        Me.cmdSmppActivity.Size = New System.Drawing.Size(108, 36)
        Me.cmdSmppActivity.Text = "Smpp Activity"
        '
        'cmdRecharges
        '
        Me.cmdRecharges.Image = Global.HOT4.Console.My.Resources.Resources.FindHS
        Me.cmdRecharges.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdRecharges.Name = "cmdRecharges"
        Me.cmdRecharges.Size = New System.Drawing.Size(94, 36)
        Me.cmdRecharges.Text = "Recharges"
        '
        'cmdAdmin
        '
        Me.cmdAdmin.Image = Global.HOT4.Console.My.Resources.Resources.EditTableHS
        Me.cmdAdmin.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdAdmin.Name = "cmdAdmin"
        Me.cmdAdmin.Size = New System.Drawing.Size(72, 36)
        Me.cmdAdmin.Text = "Admin"
        Me.cmdAdmin.Visible = False
        '
        'tcMain
        '
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.ImageList = Me.ImageList1
        Me.tcMain.Location = New System.Drawing.Point(0, 39)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(870, 347)
        Me.tcMain.TabIndex = 1
        '
        'frmConsole
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Green
        Me.ClientSize = New System.Drawing.Size(870, 408)
        Me.Controls.Add(Me.tcMain)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Name = "frmConsole"
        Me.Text = "HOT4.Console"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents cmdAccountSearch As System.Windows.Forms.ToolStripButton
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents cmdPins As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdBank As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdSmppActivity As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdBulkSMS As System.Windows.Forms.ToolStripButton
    Friend WithEvents tcMain As HOT4.Console.TabControlEx
    Friend WithEvents cmdSystem As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblUser As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cmdRecharges As ToolStripButton
    Friend WithEvents cmdAdmin As ToolStripButton
End Class
