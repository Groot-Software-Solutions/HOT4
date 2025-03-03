namespace Hot.BulkRecharge
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPrep = new System.Windows.Forms.TabPage();
            this.lblValidLinesTotal = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkAreUsd = new System.Windows.Forms.CheckBox();
            this.btnExportNetwork = new System.Windows.Forms.Button();
            this.btnRemoveNetwork = new System.Windows.Forms.Button();
            this.txtNetwork = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.lstLoadedRecharges = new System.Windows.Forms.ListView();
            this.Id = new System.Windows.Forms.ColumnHeader();
            this.Mobile = new System.Windows.Forms.ColumnHeader();
            this.Amount = new System.Windows.Forms.ColumnHeader();
            this.Button1 = new System.Windows.Forms.Button();
            this.btnLoadRecharges = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lstLines = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.txtRechargeContent = new System.Windows.Forms.RichTextBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.chkUseDefault = new System.Windows.Forms.CheckBox();
            this.txtCustomMessage = new System.Windows.Forms.RichTextBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.lblUsdBalance = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblValidCreds = new System.Windows.Forms.Label();
            this.lblValid = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.btnCheck = new System.Windows.Forms.Button();
            this.tabRecharges = new System.Windows.Forms.TabPage();
            this.btnExportSuccessful = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnExportFailed = new System.Windows.Forms.Button();
            this.btnExportRetryList = new System.Windows.Forms.Button();
            this.btnRetryFailed = new System.Windows.Forms.Button();
            this.lstRecharges = new System.Windows.Forms.ListView();
            this.ColumnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.tabMain.SuspendLayout();
            this.tabPrep.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.tabRecharges.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabPrep);
            this.tabMain.Controls.Add(this.tabRecharges);
            this.tabMain.Location = new System.Drawing.Point(12, 12);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1263, 568);
            this.tabMain.TabIndex = 0;
            // 
            // tabPrep
            // 
            this.tabPrep.Controls.Add(this.lblValidLinesTotal);
            this.tabPrep.Controls.Add(this.label7);
            this.tabPrep.Controls.Add(this.lblTotal);
            this.tabPrep.Controls.Add(this.btnConfirm);
            this.tabPrep.Controls.Add(this.Label3);
            this.tabPrep.Controls.Add(this.groupBox4);
            this.tabPrep.Controls.Add(this.GroupBox3);
            this.tabPrep.Controls.Add(this.GroupBox2);
            this.tabPrep.Controls.Add(this.GroupBox1);
            this.tabPrep.Location = new System.Drawing.Point(4, 24);
            this.tabPrep.Name = "tabPrep";
            this.tabPrep.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrep.Size = new System.Drawing.Size(1255, 540);
            this.tabPrep.TabIndex = 0;
            this.tabPrep.Text = "Preparation";
            this.tabPrep.UseVisualStyleBackColor = true;
            // 
            // lblValidLinesTotal
            // 
            this.lblValidLinesTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblValidLinesTotal.AutoSize = true;
            this.lblValidLinesTotal.Location = new System.Drawing.Point(472, 507);
            this.lblValidLinesTotal.Name = "lblValidLinesTotal";
            this.lblValidLinesTotal.Size = new System.Drawing.Size(12, 15);
            this.lblValidLinesTotal.TabIndex = 10;
            this.lblValidLinesTotal.Text = "-";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(376, 507);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 15);
            this.label7.TabIndex = 9;
            this.label7.Text = "Recharge Total: ";
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.Location = new System.Drawing.Point(1024, 503);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(96, 23);
            this.lblTotal.TabIndex = 8;
            this.lblTotal.Text = "0.00";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(1130, 503);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(114, 23);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "Confirm && Run";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // Label3
            // 
            this.Label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(945, 507);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(90, 15);
            this.Label3.TabIndex = 6;
            this.Label3.Text = "Recharge Total: ";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.chkAreUsd);
            this.groupBox4.Controls.Add(this.btnExportNetwork);
            this.groupBox4.Controls.Add(this.btnRemoveNetwork);
            this.groupBox4.Controls.Add(this.txtNetwork);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.Label1);
            this.groupBox4.Controls.Add(this.lstLoadedRecharges);
            this.groupBox4.Controls.Add(this.Button1);
            this.groupBox4.Controls.Add(this.btnLoadRecharges);
            this.groupBox4.Controls.Add(this.btnClear);
            this.groupBox4.Controls.Add(this.lstLines);
            this.groupBox4.Controls.Add(this.txtRechargeContent);
            this.groupBox4.Location = new System.Drawing.Point(6, 127);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1243, 366);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Recharges";
            // 
            // chkAreUsd
            // 
            this.chkAreUsd.AutoSize = true;
            this.chkAreUsd.Location = new System.Drawing.Point(168, 24);
            this.chkAreUsd.Name = "chkAreUsd";
            this.chkAreUsd.Size = new System.Drawing.Size(48, 19);
            this.chkAreUsd.TabIndex = 20;
            this.chkAreUsd.Text = "USD";
            this.chkAreUsd.UseVisualStyleBackColor = true;
            // 
            // btnExportNetwork
            // 
            this.btnExportNetwork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportNetwork.Location = new System.Drawing.Point(362, 22);
            this.btnExportNetwork.Name = "btnExportNetwork";
            this.btnExportNetwork.Size = new System.Drawing.Size(105, 23);
            this.btnExportNetwork.TabIndex = 19;
            this.btnExportNetwork.Text = "Export Network";
            this.btnExportNetwork.UseVisualStyleBackColor = true;
            this.btnExportNetwork.Click += new System.EventHandler(this.btnExportNetwork_Click);
            // 
            // btnRemoveNetwork
            // 
            this.btnRemoveNetwork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveNetwork.Location = new System.Drawing.Point(473, 22);
            this.btnRemoveNetwork.Name = "btnRemoveNetwork";
            this.btnRemoveNetwork.Size = new System.Drawing.Size(116, 23);
            this.btnRemoveNetwork.TabIndex = 18;
            this.btnRemoveNetwork.Text = "Remove Network";
            this.btnRemoveNetwork.UseVisualStyleBackColor = true;
            this.btnRemoveNetwork.Click += new System.EventHandler(this.btnRemoveNetwork_Click);
            // 
            // txtNetwork
            // 
            this.txtNetwork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNetwork.FormattingEnabled = true;
            this.txtNetwork.Items.AddRange(new object[] {
            "Econet",
            "Netone",
            "Telecel"});
            this.txtNetwork.Location = new System.Drawing.Point(238, 22);
            this.txtNetwork.Name = "txtNetwork";
            this.txtNetwork.Size = new System.Drawing.Size(118, 23);
            this.txtNetwork.TabIndex = 17;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(595, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Export Failed";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Label1
            // 
            this.Label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(918, 33);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(61, 15);
            this.Label1.TabIndex = 15;
            this.Label1.Text = "Recharges";
            // 
            // lstLoadedRecharges
            // 
            this.lstLoadedRecharges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLoadedRecharges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Id,
            this.Mobile,
            this.Amount});
            this.lstLoadedRecharges.FullRowSelect = true;
            this.lstLoadedRecharges.GridLines = true;
            this.lstLoadedRecharges.Location = new System.Drawing.Point(918, 51);
            this.lstLoadedRecharges.Name = "lstLoadedRecharges";
            this.lstLoadedRecharges.Size = new System.Drawing.Size(320, 309);
            this.lstLoadedRecharges.TabIndex = 14;
            this.lstLoadedRecharges.UseCompatibleStateImageBehavior = false;
            this.lstLoadedRecharges.View = System.Windows.Forms.View.Details;
            // 
            // Id
            // 
            this.Id.Text = "Id";
            this.Id.Width = 40;
            // 
            // Mobile
            // 
            this.Mobile.Text = "Mobile";
            this.Mobile.Width = 130;
            // 
            // Amount
            // 
            this.Amount.Text = "Amount";
            this.Amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Amount.Width = 70;
            // 
            // Button1
            // 
            this.Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button1.Location = new System.Drawing.Point(823, 22);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(88, 23);
            this.Button1.TabIndex = 13;
            this.Button1.Text = "Process Valid";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.ProcessValid_Click);
            // 
            // btnLoadRecharges
            // 
            this.btnLoadRecharges.Location = new System.Drawing.Point(87, 22);
            this.btnLoadRecharges.Name = "btnLoadRecharges";
            this.btnLoadRecharges.Size = new System.Drawing.Size(75, 23);
            this.btnLoadRecharges.TabIndex = 12;
            this.btnLoadRecharges.Text = "Load";
            this.btnLoadRecharges.UseVisualStyleBackColor = true;
            this.btnLoadRecharges.Click += new System.EventHandler(this.btnLoadRecharges_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(6, 22);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lstLines
            // 
            this.lstLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColumnHeader2,
            this.ColumnHeader3,
            this.ColumnHeader4,
            this.ColumnHeader5});
            this.lstLines.FullRowSelect = true;
            this.lstLines.GridLines = true;
            this.lstLines.Location = new System.Drawing.Point(238, 51);
            this.lstLines.Name = "lstLines";
            this.lstLines.Size = new System.Drawing.Size(674, 309);
            this.lstLines.TabIndex = 9;
            this.lstLines.UseCompatibleStateImageBehavior = false;
            this.lstLines.View = System.Windows.Forms.View.Details;
            this.lstLines.DoubleClick += new System.EventHandler(this.lstLines_DoubleClicked);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "Id";
            this.ColumnHeader1.Width = 40;
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "Mobile";
            this.ColumnHeader2.Width = 130;
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "Amount";
            this.ColumnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnHeader3.Width = 70;
            // 
            // ColumnHeader4
            // 
            this.ColumnHeader4.Text = "Status";
            this.ColumnHeader4.Width = 70;
            // 
            // ColumnHeader5
            // 
            this.ColumnHeader5.Text = "LineData";
            this.ColumnHeader5.Width = 360;
            // 
            // txtRechargeContent
            // 
            this.txtRechargeContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRechargeContent.Location = new System.Drawing.Point(6, 51);
            this.txtRechargeContent.Name = "txtRechargeContent";
            this.txtRechargeContent.Size = new System.Drawing.Size(226, 309);
            this.txtRechargeContent.TabIndex = 8;
            this.txtRechargeContent.Text = "";
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.chkUseDefault);
            this.GroupBox3.Controls.Add(this.txtCustomMessage);
            this.GroupBox3.Location = new System.Drawing.Point(478, 6);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(293, 115);
            this.GroupBox3.TabIndex = 4;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "SMS Template Details";
            // 
            // chkUseDefault
            // 
            this.chkUseDefault.AutoSize = true;
            this.chkUseDefault.Checked = true;
            this.chkUseDefault.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseDefault.Location = new System.Drawing.Point(6, 19);
            this.chkUseDefault.Name = "chkUseDefault";
            this.chkUseDefault.Size = new System.Drawing.Size(137, 19);
            this.chkUseDefault.TabIndex = 1;
            this.chkUseDefault.Text = "Use Default Template";
            this.chkUseDefault.UseVisualStyleBackColor = true;
            this.chkUseDefault.CheckedChanged += new System.EventHandler(this.chkUseDefault_CheckedChanged);
            // 
            // txtCustomMessage
            // 
            this.txtCustomMessage.Location = new System.Drawing.Point(6, 44);
            this.txtCustomMessage.Name = "txtCustomMessage";
            this.txtCustomMessage.Size = new System.Drawing.Size(281, 65);
            this.txtCustomMessage.TabIndex = 0;
            this.txtCustomMessage.Text = "";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.txtPassword);
            this.GroupBox2.Controls.Add(this.txtUsername);
            this.GroupBox2.Controls.Add(this.Label10);
            this.GroupBox2.Controls.Add(this.Label11);
            this.GroupBox2.Location = new System.Drawing.Point(6, 6);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(200, 115);
            this.GroupBox2.TabIndex = 2;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Hot Recharge Details";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(6, 77);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(188, 23);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(6, 34);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(188, 23);
            this.txtUsername.TabIndex = 8;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(6, 60);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(57, 15);
            this.Label10.TabIndex = 7;
            this.Label10.Text = "Password";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(6, 16);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(60, 15);
            this.Label11.TabIndex = 6;
            this.Label11.Text = "Username";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.lblUsdBalance);
            this.GroupBox1.Controls.Add(this.label2);
            this.GroupBox1.Controls.Add(this.lblBalance);
            this.GroupBox1.Controls.Add(this.lblValidCreds);
            this.GroupBox1.Controls.Add(this.lblValid);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.btnCheck);
            this.GroupBox1.Location = new System.Drawing.Point(212, 6);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(260, 115);
            this.GroupBox1.TabIndex = 3;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Account Details";
            // 
            // lblUsdBalance
            // 
            this.lblUsdBalance.AutoSize = true;
            this.lblUsdBalance.Location = new System.Drawing.Point(107, 85);
            this.lblUsdBalance.Name = "lblUsdBalance";
            this.lblUsdBalance.Size = new System.Drawing.Size(12, 15);
            this.lblUsdBalance.TabIndex = 8;
            this.lblUsdBalance.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "USD Balance";
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(107, 65);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(12, 15);
            this.lblBalance.TabIndex = 6;
            this.lblBalance.Text = "-";
            // 
            // lblValidCreds
            // 
            this.lblValidCreds.AutoSize = true;
            this.lblValidCreds.Location = new System.Drawing.Point(107, 45);
            this.lblValidCreds.Name = "lblValidCreds";
            this.lblValidCreds.Size = new System.Drawing.Size(12, 15);
            this.lblValidCreds.TabIndex = 5;
            this.lblValidCreds.Text = "-";
            // 
            // lblValid
            // 
            this.lblValid.AutoSize = true;
            this.lblValid.Location = new System.Drawing.Point(107, 25);
            this.lblValid.Name = "lblValid";
            this.lblValid.Size = new System.Drawing.Size(12, 15);
            this.lblValid.TabIndex = 4;
            this.lblValid.Text = "-";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(53, 65);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(48, 15);
            this.Label6.TabIndex = 3;
            this.Label6.Text = "Balance";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(14, 45);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(87, 15);
            this.Label5.TabIndex = 2;
            this.Label5.Text = "Account Name";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(69, 25);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(32, 15);
            this.Label4.TabIndex = 1;
            this.Label4.Text = "Valid";
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(179, 16);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 23);
            this.btnCheck.TabIndex = 0;
            this.btnCheck.Text = "Check";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // tabRecharges
            // 
            this.tabRecharges.Controls.Add(this.btnExportSuccessful);
            this.tabRecharges.Controls.Add(this.btnPause);
            this.tabRecharges.Controls.Add(this.btnExportFailed);
            this.tabRecharges.Controls.Add(this.btnExportRetryList);
            this.tabRecharges.Controls.Add(this.btnRetryFailed);
            this.tabRecharges.Controls.Add(this.lstRecharges);
            this.tabRecharges.Location = new System.Drawing.Point(4, 24);
            this.tabRecharges.Name = "tabRecharges";
            this.tabRecharges.Padding = new System.Windows.Forms.Padding(3);
            this.tabRecharges.Size = new System.Drawing.Size(1255, 540);
            this.tabRecharges.TabIndex = 1;
            this.tabRecharges.Text = "Recharges";
            this.tabRecharges.UseVisualStyleBackColor = true;
            // 
            // btnExportSuccessful
            // 
            this.btnExportSuccessful.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportSuccessful.Location = new System.Drawing.Point(1079, 23);
            this.btnExportSuccessful.Name = "btnExportSuccessful";
            this.btnExportSuccessful.Size = new System.Drawing.Size(81, 41);
            this.btnExportSuccessful.TabIndex = 9;
            this.btnExportSuccessful.Text = "Export Successful";
            this.btnExportSuccessful.UseVisualStyleBackColor = true;
            this.btnExportSuccessful.Click += new System.EventHandler(this.btnExportSuccessful_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(6, 23);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(86, 41);
            this.btnPause.TabIndex = 8;
            this.btnPause.Text = "Pause Recharges";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnExportFailed
            // 
            this.btnExportFailed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportFailed.Location = new System.Drawing.Point(1166, 23);
            this.btnExportFailed.Name = "btnExportFailed";
            this.btnExportFailed.Size = new System.Drawing.Size(75, 41);
            this.btnExportFailed.TabIndex = 7;
            this.btnExportFailed.Text = "Export Failed List";
            this.btnExportFailed.UseVisualStyleBackColor = true;
            this.btnExportFailed.Click += new System.EventHandler(this.btnExportFailed_Click);
            // 
            // btnExportRetryList
            // 
            this.btnExportRetryList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportRetryList.Location = new System.Drawing.Point(949, 23);
            this.btnExportRetryList.Name = "btnExportRetryList";
            this.btnExportRetryList.Size = new System.Drawing.Size(75, 41);
            this.btnExportRetryList.TabIndex = 6;
            this.btnExportRetryList.Text = "Export Retry List";
            this.btnExportRetryList.UseVisualStyleBackColor = true;
            this.btnExportRetryList.Click += new System.EventHandler(this.btnExportRetryList_Click);
            // 
            // btnRetryFailed
            // 
            this.btnRetryFailed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRetryFailed.Location = new System.Drawing.Point(868, 23);
            this.btnRetryFailed.Name = "btnRetryFailed";
            this.btnRetryFailed.Size = new System.Drawing.Size(75, 41);
            this.btnRetryFailed.TabIndex = 5;
            this.btnRetryFailed.Text = "Retry Failed";
            this.btnRetryFailed.UseVisualStyleBackColor = true;
            this.btnRetryFailed.Click += new System.EventHandler(this.btnRetryFailed_Click);
            // 
            // lstRecharges
            // 
            this.lstRecharges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRecharges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader6,
            this.ColumnHeader7,
            this.ColumnHeader8,
            this.ColumnHeader9,
            this.ColumnHeader10});
            this.lstRecharges.FullRowSelect = true;
            this.lstRecharges.GridLines = true;
            this.lstRecharges.Location = new System.Drawing.Point(6, 76);
            this.lstRecharges.Name = "lstRecharges";
            this.lstRecharges.Size = new System.Drawing.Size(1243, 458);
            this.lstRecharges.TabIndex = 4;
            this.lstRecharges.UseCompatibleStateImageBehavior = false;
            this.lstRecharges.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader6
            // 
            this.ColumnHeader6.Text = "Id";
            // 
            // ColumnHeader7
            // 
            this.ColumnHeader7.Text = "Mobile";
            this.ColumnHeader7.Width = 130;
            // 
            // ColumnHeader8
            // 
            this.ColumnHeader8.Text = "Amount";
            this.ColumnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnHeader8.Width = 70;
            // 
            // ColumnHeader9
            // 
            this.ColumnHeader9.Text = "Status";
            this.ColumnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ColumnHeader10
            // 
            this.ColumnHeader10.Text = "Narrative";
            this.ColumnHeader10.Width = 360;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1287, 592);
            this.Controls.Add(this.tabMain);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabMain.ResumeLayout(false);
            this.tabPrep.ResumeLayout(false);
            this.tabPrep.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.tabRecharges.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        

        #endregion

        private TabControl tabMain;
        private TabPage tabPrep;
        private TabPage tabRecharges;
        internal GroupBox GroupBox3;
        internal CheckBox chkUseDefault;
        internal RichTextBox txtCustomMessage;
        internal GroupBox GroupBox2;
        internal TextBox txtPassword;
        internal TextBox txtUsername;
        internal Label Label10;
        internal Label Label11;
        internal GroupBox GroupBox1;
        internal Label lblBalance;
        internal Label lblValidCreds;
        internal Label lblValid;
        internal Label Label6;
        internal Label Label5;
        internal Label Label4;
        internal Button btnCheck;
        private GroupBox groupBox4;
        internal Button Button1;
        internal Button btnLoadRecharges;
        internal Button btnClear;
        internal ListView lstLines;
        internal ColumnHeader ColumnHeader1;
        internal ColumnHeader ColumnHeader2;
        internal ColumnHeader ColumnHeader3;
        internal ColumnHeader ColumnHeader4;
        internal ColumnHeader ColumnHeader5;
        internal RichTextBox txtRechargeContent;
        internal Label Label1;
        internal ListView lstLoadedRecharges;
        internal ColumnHeader Id;
        internal ColumnHeader Mobile;
        internal ColumnHeader Amount;
        internal Label lblTotal;
        internal Button btnConfirm;
        internal Label Label3;
        internal Button btnExportFailed;
        internal Button btnExportRetryList;
        internal Button btnRetryFailed;
        internal ListView lstRecharges;
        internal ColumnHeader ColumnHeader6;
        internal ColumnHeader ColumnHeader7;
        internal ColumnHeader ColumnHeader8;
        internal ColumnHeader ColumnHeader9;
        internal ColumnHeader ColumnHeader10;
        internal Label lblValidLinesTotal;
        internal Label label7;
        private Button btnPause;
        private Button button2;
        private Button btnExportSuccessful;
        private Button btnRemoveNetwork;
        private ComboBox txtNetwork;
        private Button btnExportNetwork;
        private CheckBox chkAreUsd;
        private Label lblUsdBalance;
        private Label label2;
    }
}