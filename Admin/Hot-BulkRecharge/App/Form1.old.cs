using Hot.API.Client;
using Hot.API.Client.Models;
using OneOf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class Form1 : Form
    {
        private readonly int ConcurrentTransactions = 3;
        private readonly HotRechargeService client;
        private readonly RechargeParsingService parser;
        private readonly decimal maxRecharge = 30000;
        private List<BulkRecharge> Recharges { get; set; } = new List<BulkRecharge>();
        private List<BulkRecharge> Rechargescompleted { get; set; } = new List<BulkRecharge>();

        public Form1(HotRechargeService client)
        {
            InitializeComponent();
            parser = new RechargeParsingService(maxRecharge);
            this.client = client;
        }


        private async void BtnCheck_ClickAsync(object sender, EventArgs e)
        {
            await CheckBalanceAsync();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Recharges.Clear();
            RefreshLoadedRecharges();

        }

        private void ChkUseDefault_CheckedChanged(object sender, EventArgs e)
        {
            txtCustomMessage.Enabled = !chkUseDefault.Checked;
        }

        private async void BtnConfirm_Click(object sender, EventArgs e)
        {
            await ProcessTransactions();

        }

        private void BtnLoadRecharges_Click(object sender, EventArgs e)
        {
            LoadRecharges(txtRechargeContent.Text);
        }

        private void TxtUsername_TextChanged(object sender, EventArgs e)
        {
            client.AccessCode = txtUsername.Text;
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            client.AccessPassword = txtPassword.Text;
        }

        private void BtnExportFailed_Click(object sender, EventArgs e)
        {
            ExportFailedList(); 
        }
         
        private void BtnRetryFailed_Click(object sender, EventArgs e)
        {
            RetryFailedTransactions(); 
        }
         
        private void BtnExportRetryList_Click(object sender, EventArgs e)
        {
            ExportRetryFailedList();
        }



        private async Task CheckBalanceAsync()
        {
            WalletBalanceResponse result = await client.CheckHotWalletBalanceAsync();
            lblBalance.Text = result.ReplyCode == (int)ReplyCode.Success ? result.WalletBalance.ToString("#,##0.00") : "0.00";
            lblValidCreds.Text = result.ReplyCode == (int)ReplyCode.Success ? "Valid" : "Invalid Credentials";
        }
         
        private async Task ProcessTransactions()
        {
            Rechargescompleted.Clear();
            RefreshRecharges();
            if (chkThreaded.Checked)
            {
                await ProcessRechargesThreadedAsync(Recharges);
            }
            else
            {
                await ProcessRechargesSingularAsync(Recharges);
            }
        }
       
        private void LoadRecharges(string text)
        {
            Recharges = parser.ParseRecharges(text);
            RefreshLoadedRecharges(); 
        }

        private void RefreshLoadedRecharges()
        {
            lstLoadedRecharges.Items.Clear();
            foreach (var item in Recharges)
            {
                item.Id = lstLoadedRecharges.Items.Count + 1;
                var listitem = new ListViewItem((item.Id).ToString());
                listitem.SubItems.Add(item.Mobile);
                listitem.SubItems.Add(item.IsData ? item.ProductCode : item.Amount.ToString("#,##0.00"));
                lstLoadedRecharges.Items.Add(listitem);
            }
            lblTotal.Text = Recharges.Sum(r => r.Amount).ToString("#,##0.00");
            tabMain.SelectedTab = tabPrep;
        }

        private void RefreshRecharges()
        {
            lstRecharges.Items.Clear();
            foreach (var item in Recharges)
            {
                var listitem = new ListViewItem(item.Id.ToString());
                listitem.SubItems.Add(item.Mobile);
                listitem.SubItems.Add(item.IsData ? item.ProductCode : item.Amount.ToString("#,##0.00"));
                listitem.SubItems.Add(item.Status);
                listitem.SubItems.Add(item.Narrative);
                lstRecharges.Items.Add(listitem);
            }
            lblTotal.Text = Recharges.Sum(r => r.Amount).ToString("#,##0.00");
            tabMain.SelectedTab = tabRecharges;
        }

        private void RetryFailedTransactions()
        {
            var failedRecharges = Recharges.Where(r => r.ReplyCode != (int)ReplyCode.Success).ToList();

            var retryRechargesContent = string.Join('\n',
                failedRecharges
                    .Select(r => $"{r.Mobile},{(r.IsData ? r.ProductCode : r.Amount.ToString())}"));
            LoadRecharges(retryRechargesContent);
        }

        private void ExportFailedList()
        {
            var failedRecharges = Recharges.Where(r => r.ReplyCode != (int)ReplyCode.Success).ToList();
            var frmSaveFile = new SaveFileDialog();
            if (frmSaveFile.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(
                               frmSaveFile.FileName,
                               string.Join('\n', failedRecharges.Select(r => SerializedObjectsFileFormat(r)))
                               );
                MessageBox.Show("Exported");
            }
        }

        private void ExportRetryFailedList()
        {
            var failedRecharges = Recharges.Where(r => r.ReplyCode != (int)ReplyCode.Success).ToList();
            var frmSaveFile = new SaveFileDialog();
            if (frmSaveFile.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(
                               frmSaveFile.FileName,
                               string.Join('\n', failedRecharges.Select(r => SerializedRetryFileFormat(r)))
                               );
                MessageBox.Show("Exported");
            }
        }

      

        private async Task ProcessRechargesSingularAsync(List<BulkRecharge> recharges)
        {
            foreach(var item in recharges)
            {
                await HandleRecharge(item);
            }
        }

        private async Task ProcessRechargesThreadedAsync(List<BulkRecharge> recharges)
        {
           
            foreach (var list in recharges.TakeListOf(ConcurrentTransactions))
            {
                var tasks = new List<Task>();
                list.ToList().ForEach(item => tasks.Add(HandleRecharge(item)));
                await Task.WhenAll(tasks);
            }
        }

        private async Task HandleRecharge(BulkRecharge item)
        {
            if (!chkUseDefault.Checked) item.CustomSMS = txtCustomMessage.Text;
            var result = (item.IsData) ? await client.RechargeDataBundle(item) : await client.RechargeAirtime(item);
            item.SetResult(result.ReplyCode, string.IsNullOrEmpty(result.ReplyMsg) ? result.ReplyMessage : result.ReplyMsg);
            Rechargescompleted.Add(item);
            UpdateResultInWindow(item);
        }

        private void UpdateResultInWindow(BulkRecharge item)
        {
            var listitem = lstRecharges.FindItemWithText(item.Id.ToString());
            var index = lstRecharges.Items.IndexOf(listitem);
            lstRecharges.Items[index].SubItems[3].Text = item.Status;
            lstRecharges.Items[index].SubItems[4].Text = item.Narrative;

        }
         
        private string SerializedObjectsFileFormat(BulkRecharge r)
        {
            return $"{r.Id},\"{r.Mobile}\",{(r.IsData ? $"\"{r.ProductCode}\"" : r.Amount.ToString())},\"{r.Narrative}\",{r.ReplyCode},\"{r.Reference}\",\"{r.Status}\"";
            //return JsonSerializer.Serialize(r);
        }

        private string SerializedRetryFileFormat(BulkRecharge r)
        {
            return $"{r.Mobile},{(r.IsData ? r.ProductCode : r.Amount.ToString("###0.00"))}"; 
        }

    }

}
