
namespace App
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblValidCreds = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtRechargeContent = new System.Windows.Forms.TextBox();
            this.chkThreaded = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnLoadRecharges = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbnConfirm = new System.Windows.Forms.Button();
            this.lstLoadedRecharges = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCustomMessage = new System.Windows.Forms.TextBox();
            this.chkUseDefault = new System.Windows.Forms.CheckBox();
            this.grpcreds = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabRecharges = new System.Windows.Forms.TabPage();
            this.btnExportFailed = new System.Windows.Forms.Button();
            this.btnRetryFailed = new System.Windows.Forms.Button();
            this.lstRecharges = new System.Windows.Forms.ListView();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.btnExportRetryList = new System.Windows.Forms.Button();
            this.tabMain.SuspendLayout();
            this.tabPrep.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpcreds.SuspendLayout();
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
            this.tabMain.Size = new System.Drawing.Size(967, 485);
            this.tabMain.TabIndex = 0; 
            // 
            // tabPrep
            // 
            this.tabPrep.Controls.Add(this.groupBox3);
            this.tabPrep.Controls.Add(this.groupBox2);
            this.tabPrep.Controls.Add(this.groupBox1);
            this.tabPrep.Controls.Add(this.grpcreds);
            this.tabPrep.Location = new System.Drawing.Point(4, 24);
            this.tabPrep.Name = "tabPrep";
            this.tabPrep.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrep.Size = new System.Drawing.Size(959, 457);
            this.tabPrep.TabIndex = 0;
            this.tabPrep.Text = "Preparation";
            this.tabPrep.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnCheck);
            this.groupBox3.Controls.Add(this.lblBalance);
            this.groupBox3.Controls.Add(this.lblValidCreds);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(248, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(321, 120);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Account Details";
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(231, 18);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(84, 25);
            this.btnCheck.TabIndex = 2;
            this.btnCheck.Text = "Check";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.BtnCheck_ClickAsync);
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(106, 79);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(48, 15);
            this.lblBalance.TabIndex = 0;
            this.lblBalance.Text = "Balance";
            // 
            // lblValidCreds
            // 
            this.lblValidCreds.AutoSize = true;
            this.lblValidCreds.Location = new System.Drawing.Point(106, 54);
            this.lblValidCreds.Name = "lblValidCreds";
            this.lblValidCreds.Size = new System.Drawing.Size(84, 15);
            this.lblValidCreds.TabIndex = 0;
            this.lblValidCreds.Text = "AccountName";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(52, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Balance:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Valid Credentials:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtRechargeContent);
            this.groupBox2.Controls.Add(this.chkThreaded);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lblTotal);
            this.groupBox2.Controls.Add(this.btnLoadRecharges);
            this.groupBox2.Controls.Add(this.btnClear);
            this.groupBox2.Controls.Add(this.tbnConfirm);
            this.groupBox2.Controls.Add(this.lstLoadedRecharges);
            this.groupBox2.Location = new System.Drawing.Point(3, 129);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(953, 325);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Load Recharges";
            // 
            // txtRechargeContent
            // 
            this.txtRechargeContent.AcceptsReturn = true;
            this.txtRechargeContent.AcceptsTab = true;
            this.txtRechargeContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRechargeContent.Location = new System.Drawing.Point(6, 50);
            this.txtRechargeContent.MaxLength = 1132767;
            this.txtRechargeContent.Multiline = true;
            this.txtRechargeContent.Name = "txtRechargeContent";
            this.txtRechargeContent.Size = new System.Drawing.Size(276, 240);
            this.txtRechargeContent.TabIndex = 6;
            // 
            // chkThreaded
            // 
            this.chkThreaded.AutoSize = true;
            this.chkThreaded.Location = new System.Drawing.Point(733, 299);
            this.chkThreaded.Name = "chkThreaded";
            this.chkThreaded.Size = new System.Drawing.Size(99, 19);
            this.chkThreaded.TabIndex = 5;
            this.chkThreaded.Text = "Run Threaded";
            this.chkThreaded.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(529, 301);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Recharge Total";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Recharge List Data";
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.Location = new System.Drawing.Point(621, 301);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(104, 15);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "0.00";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnLoadRecharges
            // 
            this.btnLoadRecharges.Location = new System.Drawing.Point(207, 21);
            this.btnLoadRecharges.Name = "btnLoadRecharges";
            this.btnLoadRecharges.Size = new System.Drawing.Size(75, 23);
            this.btnLoadRecharges.TabIndex = 1;
            this.btnLoadRecharges.Text = "Load";
            this.btnLoadRecharges.UseVisualStyleBackColor = true;
            this.btnLoadRecharges.Click += new System.EventHandler(this.BtnLoadRecharges_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(126, 21);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // tbnConfirm
            // 
            this.tbnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbnConfirm.Location = new System.Drawing.Point(840, 297);
            this.tbnConfirm.Name = "tbnConfirm";
            this.tbnConfirm.Size = new System.Drawing.Size(113, 23);
            this.tbnConfirm.TabIndex = 4;
            this.tbnConfirm.Text = "Confirm && Run";
            this.tbnConfirm.UseVisualStyleBackColor = true;
            this.tbnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // lstLoadedRecharges
            // 
            this.lstLoadedRecharges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLoadedRecharges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstLoadedRecharges.FullRowSelect = true;
            this.lstLoadedRecharges.GridLines = true;
            this.lstLoadedRecharges.HideSelection = false;
            this.lstLoadedRecharges.Location = new System.Drawing.Point(288, 21);
            this.lstLoadedRecharges.Name = "lstLoadedRecharges";
            this.lstLoadedRecharges.Size = new System.Drawing.Size(659, 269);
            this.lstLoadedRecharges.TabIndex = 3;
            this.lstLoadedRecharges.UseCompatibleStateImageBehavior = false;
            this.lstLoadedRecharges.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Mobile";
            this.columnHeader2.Width = 260;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Amount";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 160;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCustomMessage);
            this.groupBox1.Controls.Add(this.chkUseDefault);
            this.groupBox1.Location = new System.Drawing.Point(575, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 120);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SMS Template";
            // 
            // txtCustomMessage
            // 
            this.txtCustomMessage.AcceptsReturn = true;
            this.txtCustomMessage.AcceptsTab = true;
            this.txtCustomMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomMessage.Location = new System.Drawing.Point(6, 40);
            this.txtCustomMessage.Multiline = true;
            this.txtCustomMessage.Name = "txtCustomMessage";
            this.txtCustomMessage.Size = new System.Drawing.Size(363, 74);
            this.txtCustomMessage.TabIndex = 1;
            // 
            // chkUseDefault
            // 
            this.chkUseDefault.AutoSize = true;
            this.chkUseDefault.Checked = true;
            this.chkUseDefault.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseDefault.Location = new System.Drawing.Point(6, 18);
            this.chkUseDefault.Name = "chkUseDefault";
            this.chkUseDefault.Size = new System.Drawing.Size(137, 19);
            this.chkUseDefault.TabIndex = 0;
            this.chkUseDefault.Text = "Use Default Template";
            this.chkUseDefault.UseVisualStyleBackColor = true;
            this.chkUseDefault.CheckedChanged += new System.EventHandler(this.ChkUseDefault_CheckedChanged);
            // 
            // grpcreds
            // 
            this.grpcreds.Controls.Add(this.txtPassword);
            this.grpcreds.Controls.Add(this.label2);
            this.grpcreds.Controls.Add(this.txtUsername);
            this.grpcreds.Controls.Add(this.label1);
            this.grpcreds.Location = new System.Drawing.Point(3, 3);
            this.grpcreds.Name = "grpcreds";
            this.grpcreds.Size = new System.Drawing.Size(239, 120);
            this.grpcreds.TabIndex = 0;
            this.grpcreds.TabStop = false;
            this.grpcreds.Text = "Hot Recharge Credentials";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(6, 87);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(224, 23);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.TxtPassword_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(6, 36);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(224, 23);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.TextChanged += new System.EventHandler(this.TxtUsername_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // tabRecharges
            // 
            this.tabRecharges.Controls.Add(this.btnExportRetryList);
            this.tabRecharges.Controls.Add(this.btnExportFailed);
            this.tabRecharges.Controls.Add(this.btnRetryFailed);
            this.tabRecharges.Controls.Add(this.lstRecharges);
            this.tabRecharges.Location = new System.Drawing.Point(4, 24);
            this.tabRecharges.Name = "tabRecharges";
            this.tabRecharges.Padding = new System.Windows.Forms.Padding(3);
            this.tabRecharges.Size = new System.Drawing.Size(959, 457);
            this.tabRecharges.TabIndex = 1;
            this.tabRecharges.Text = "Recharges";
            this.tabRecharges.UseVisualStyleBackColor = true;
            // 
            // btnExportFailed
            // 
            this.btnExportFailed.Location = new System.Drawing.Point(884, 3);
            this.btnExportFailed.Name = "btnExportFailed";
            this.btnExportFailed.Size = new System.Drawing.Size(72, 46);
            this.btnExportFailed.TabIndex = 5;
            this.btnExportFailed.Text = "Export Failed List";
            this.btnExportFailed.UseVisualStyleBackColor = true;
            this.btnExportFailed.Click += new System.EventHandler(this.BtnExportFailed_Click);
            // 
            // btnRetryFailed
            // 
            this.btnRetryFailed.Location = new System.Drawing.Point(3, 3);
            this.btnRetryFailed.Name = "btnRetryFailed";
            this.btnRetryFailed.Size = new System.Drawing.Size(72, 46);
            this.btnRetryFailed.TabIndex = 5;
            this.btnRetryFailed.Text = "Retry Failed";
            this.btnRetryFailed.UseVisualStyleBackColor = true;
            this.btnRetryFailed.Click += new System.EventHandler(this.BtnRetryFailed_Click);
            // 
            // lstRecharges
            // 
            this.lstRecharges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRecharges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.lstRecharges.FullRowSelect = true;
            this.lstRecharges.GridLines = true;
            this.lstRecharges.HideSelection = false;
            this.lstRecharges.Location = new System.Drawing.Point(3, 55);
            this.lstRecharges.Name = "lstRecharges";
            this.lstRecharges.Size = new System.Drawing.Size(953, 399);
            this.lstRecharges.TabIndex = 4;
            this.lstRecharges.UseCompatibleStateImageBehavior = false;
            this.lstRecharges.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Id";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Mobile";
            this.columnHeader5.Width = 160;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Amount";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader6.Width = 80;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Status";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader7.Width = 120;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Narrative";
            this.columnHeader8.Width = 460;
            // 
            // btnExportRetryList
            // 
            this.btnExportRetryList.Location = new System.Drawing.Point(803, 3);
            this.btnExportRetryList.Name = "btnExportRetryList";
            this.btnExportRetryList.Size = new System.Drawing.Size(75, 46);
            this.btnExportRetryList.TabIndex = 6;
            this.btnExportRetryList.Text = "Export Retry List";
            this.btnExportRetryList.UseVisualStyleBackColor = true;
            this.btnExportRetryList.Click += new System.EventHandler(this.BtnExportRetryList_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 509);
            this.Controls.Add(this.tabMain);
            this.Name = "Form1";
            this.Text = "Bulk Recharger";
            this.tabMain.ResumeLayout(false);
            this.tabPrep.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpcreds.ResumeLayout(false);
            this.grpcreds.PerformLayout();
            this.tabRecharges.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPrep;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLoadRecharges;
        private System.Windows.Forms.GroupBox grpcreds;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabRecharges;
        private System.Windows.Forms.ListView lstLoadedRecharges;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button tbnConfirm;
        private System.Windows.Forms.ListView lstRecharges;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button btnExportFailed;
        private System.Windows.Forms.Button btnRetryFailed;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblValidCreds;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkUseDefault;
        private System.Windows.Forms.TextBox txtRechargeContent;
        private System.Windows.Forms.CheckBox chkThreaded;
        private System.Windows.Forms.TextBox txtCustomMessage;
        private System.Windows.Forms.Button btnExportRetryList;
    }
}

