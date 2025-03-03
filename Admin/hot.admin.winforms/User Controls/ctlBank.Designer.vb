<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ctlBank
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ctlBank))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.tvw = New System.Windows.Forms.TreeView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.dg = New System.Windows.Forms.DataGridView()
        Me.mnuBank = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuBatchLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEcoCash = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuEcoCashCheck = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuBank.SuspendLayout()
        Me.mnuEcoCash.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.tvw)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.dg)
        Me.SplitContainer1.Size = New System.Drawing.Size(896, 660)
        Me.SplitContainer1.SplitterDistance = 184
        Me.SplitContainer1.TabIndex = 4
        '
        'tvw
        '
        Me.tvw.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvw.ImageIndex = 0
        Me.tvw.ImageList = Me.ImageList1
        Me.tvw.Location = New System.Drawing.Point(0, 0)
        Me.tvw.Name = "tvw"
        Me.tvw.SelectedImageIndex = 0
        Me.tvw.Size = New System.Drawing.Size(184, 660)
        Me.tvw.TabIndex = 0
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Book_angleHS.png")
        Me.ImageList1.Images.SetKeyName(1, "Book_openHS.png")
        Me.ImageList1.Images.SetKeyName(2, "DocumentHS.png")
        Me.ImageList1.Images.SetKeyName(3, "PageUpHS.png")
        '
        'dg
        '
        Me.dg.AllowUserToAddRows = False
        Me.dg.AllowUserToDeleteRows = False
        Me.dg.AllowUserToOrderColumns = True
        Me.dg.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dg.Location = New System.Drawing.Point(0, 0)
        Me.dg.MultiSelect = False
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.RowHeadersVisible = False
        Me.dg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dg.Size = New System.Drawing.Size(708, 660)
        Me.dg.TabIndex = 2
        '
        'mnuBank
        '
        Me.mnuBank.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.mnuBank.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuBatchLoad})
        Me.mnuBank.Name = "mnuType"
        Me.mnuBank.Size = New System.Drawing.Size(150, 42)
        '
        'mnuBatchLoad
        '
        Me.mnuBatchLoad.Image = Global.HOT4.Console.My.Resources.Resources.AddTableHS1
        Me.mnuBatchLoad.Name = "mnuBatchLoad"
        Me.mnuBatchLoad.Size = New System.Drawing.Size(149, 38)
        Me.mnuBatchLoad.Text = "&Load Batch"
        '
        'mnuEcoCash
        '
        Me.mnuEcoCash.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.mnuEcoCash.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEcoCashCheck})
        Me.mnuEcoCash.Name = "mnuEcoCash"
        Me.mnuEcoCash.Size = New System.Drawing.Size(222, 42)
        '
        'mnuEcoCashCheck
        '
        Me.mnuEcoCashCheck.Image = Global.HOT4.Console.My.Resources.Resources.EditTableHS
        Me.mnuEcoCashCheck.Name = "mnuEcoCashCheck"
        Me.mnuEcoCashCheck.Size = New System.Drawing.Size(221, 38)
        Me.mnuEcoCashCheck.Text = "Check Batch for Pending"
        '
        'ctlBank
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "ctlBank"
        Me.Size = New System.Drawing.Size(896, 660)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuBank.ResumeLayout(False)
        Me.mnuEcoCash.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents tvw As System.Windows.Forms.TreeView
    Friend WithEvents dg As System.Windows.Forms.DataGridView
    Friend WithEvents mnuBank As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuBatchLoad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents mnuEcoCash As ContextMenuStrip
    Friend WithEvents mnuEcoCashCheck As ToolStripMenuItem
End Class
