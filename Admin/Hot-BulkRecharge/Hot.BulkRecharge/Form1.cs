using Hot.API.Client.Models;
using Hot.API.Client;
using System.Configuration.Internal;
using Microsoft.Extensions.Configuration;

namespace Hot.BulkRecharge
{
    public partial class Form1 : Form
    {
        private readonly int ConcurrentTransactions = 3;
        private readonly HotRechargeService client;
        private readonly RechargeParsingService parser;
        private readonly decimal maxRecharge;
        private bool RechargesPaused { get; set; } = false;
        private List<BulkRecharge> RechargeLines { get; set; } = new List<BulkRecharge>();
        private List<BulkRecharge> Recharges { get; set; } = new List<BulkRecharge>();
        private List<BulkRecharge> Rechargescompleted { get; set; } = new List<BulkRecharge>();

        public Form1(HotRechargeService hotRechargeService, IConfiguration configuration)
        {
            InitializeComponent();
            var dataRegexPattern = configuration.GetValue<string>("DataRegex");
            var mobileRegexPattern = configuration.GetValue<string>("MobileRegex");
            maxRecharge = configuration.GetValue<decimal>("MaxRecharge",200000);

            parser = new RechargeParsingService(maxRecharge, dataRegexPattern, mobileRegexPattern);
            client = hotRechargeService;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnCheck_Click(object sender, EventArgs e)
        {
            await CheckBalanceAsync();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Recharges.Clear();
            RefreshLoadedRecharges();
            txtRechargeContent.Clear();
        }

        private void btnLoadRecharges_Click(object sender, EventArgs e)
        {
            LoadRecharges(txtRechargeContent.Text, chkAreUsd.Checked);
        }

        private void chkUseDefault_CheckedChanged(object sender, EventArgs e)
        {
            txtCustomMessage.Enabled = !chkUseDefault.Checked;
        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            await ProcessTransactions();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            client.AccessCode = txtUsername.Text;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            client.AccessPassword = txtPassword.Text;
        }

        private void btnExportFailed_Click(object sender, EventArgs e)
        {
            ExportFailedList();
        }

        private void btnRetryFailed_Click(object sender, EventArgs e)
        {
            RetryFailedTransactions();
        }

        private void btnExportRetryList_Click(object sender, EventArgs e)
        {
            ExportRetryFailedList();
        }

        private void lstLines_DoubleClicked(object sender, EventArgs e)
        {
            var selectedItem = lstLines.SelectedItems[0];
            if (selectedItem is null) return;
            int linePosition = lstLines.Items.IndexOf(selectedItem);
            var item = RechargeLines[linePosition];
            if (item is null) return;

            var dialog = new FixlineData();
            dialog.LineData = item.LineData ?? "";
            dialog.Mobile = item.Mobile;
            dialog.Amount = item.Amount.ToString();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                RechargeLines[linePosition].Mobile = dialog.Mobile;
                if (!dialog.IsData)
                {
                    RechargeLines[linePosition].Amount = Convert.ToDecimal(dialog.Amount);
                }
                else
                {
                    RechargeLines[linePosition].ProductCode = dialog.Amount;
                }
                RechargeLines[linePosition].isValid = true;
            }
            RefreshLoadedRechargeLines();
        }

        private void ProcessValid_Click(object sender, EventArgs e)
        {
            Recharges = RechargeLines.Where(r => r.isValid).ToList();
            RefreshLoadedRecharges();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            RechargesPaused = !RechargesPaused;
            btnPause.Text = RechargesPaused ? "Resume Recharges" : "Pause Recharges";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExportFailedLineData();
        }

        private void btnExportSuccessful_Click(object sender, EventArgs e)
        {
            ExportSuccessfulData();

        }

        private void ExportSuccessfulData()
        {
            var successRecharges = Recharges.Where(r => r.ReplyCode == (int)ReplyCode.Success).ToList();
            var frmSaveFile = new SaveFileDialog();
            frmSaveFile.DefaultExt = "csv";
            if (frmSaveFile.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(
                               frmSaveFile.FileName,
                               string.Join('\n', successRecharges.Select(r => SerializedSuccessFileFormat(r)))
                               );
                MessageBox.Show("Exported");
            }
        }

        private void ExportFailedLineData()
        {
            var failedLines = RechargeLines.Where(r => !r.isValid).ToList();
            var frmSaveFile = new SaveFileDialog();
            frmSaveFile.DefaultExt = "csv";
            if (frmSaveFile.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(
                               frmSaveFile.FileName,
                               string.Join('\n', failedLines.Select(r => SerializedLineFileFormat(r)))
                               );
                MessageBox.Show("Exported");
            }
        }

        private async Task CheckBalanceAsync()
        {
            WalletBalanceResponse result = await client.CheckHotWalletBalanceAsync();
            WalletBalanceResponse resultUsd = await client.CheckHotWalletBalanceUSDAsync();
            lblBalance.Text = result.ReplyCode == (int)ReplyCode.Success ? result.WalletBalance.ToString("#,##0.00") : "0.00";
            lblValid.Text = result.ReplyCode == (int)ReplyCode.Success ? "Valid" : "Invalid Credentials";
            lblUsdBalance.Text = resultUsd.ReplyCode == (int)ReplyCode.Success ? resultUsd.WalletBalance.ToString("#,##0.00") : "0.00";

        }

        private async Task ProcessTransactions()
        {
            Rechargescompleted.Clear();
            RefreshRecharges();
            //if (chkThreaded.Checked)
            //{
            //    await ProcessRechargesThreadedAsync(Recharges);
            //}
            //else
            //{
            await ProcessRechargesSingularAsync(Recharges);
            //}
        }

        private void LoadRecharges(string text, bool USDRecharges)
        {
            RechargeLines = parser.ParseRecharges(text, USDRecharges)
                .OrderBy(b => b.isValid)
                .ToList();
            RefreshLoadedRechargeLines();
        }
        private void RefreshLoadedRechargeLines()
        {
            lstLines.Items.Clear();
            foreach (var item in RechargeLines)
            {
                item.Id = lstLines.Items.Count + 1;
                var listitem = new ListViewItem((item.Id).ToString());
                listitem.SubItems.Add(item.Mobile);
                listitem.SubItems.Add(item.IsData ? item.ProductCode : item.Amount.ToString("#,##0.00"));
                listitem.SubItems.Add(item.isValid ? "Valid" : "Invalid");
                listitem.SubItems.Add(item.LineData);
                listitem.BackColor = item.isValid ? listitem.BackColor : Color.Orange;
                lstLines.Items.Add(listitem);
            }
            lblValidLinesTotal.Text = RechargeLines.Where(l => l.isValid).Sum(r => r.Amount).ToString("#,##0.00");
            tabMain.SelectedTab = tabPrep;

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
            LoadRecharges(retryRechargesContent, chkAreUsd.Checked);
        }

        private void ExportFailedList()
        {
            var failedRecharges = Recharges.Where(r => r.ReplyCode != (int)ReplyCode.Success).ToList();
            var frmSaveFile = new SaveFileDialog();
            frmSaveFile.DefaultExt = "csv";
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
            frmSaveFile.DefaultExt = "csv";
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
            foreach (var item in recharges)
            {
                while (RechargesPaused)
                {
                    await Task.Delay(400);
                }
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
            RechargeResponse result;
            if (item.IsData)
            {
                result = await client.RechargeDataBundle(item);
            }
            else
            {
                result = await client.RechargeAirtime(item);
            }
            item.SetResult(result);
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

        private string SerializedLineFileFormat(BulkRecharge r)
        {

            return $"{r.LineData}";
        }
        private string SerializedSuccessFileFormat(BulkRecharge r)
        {
            return $"{r.Id},\"{r.Mobile}\",{(r.IsData ? $"\"{r.ProductCode}\"" : r.Amount.ToString())},\"{r.Narrative}\",{r.ReplyCode},\"{r.Reference}\",\"{r.Status}\",{r.Response?.InitialBalance:#,##0.00},{r.Response?.FinalBalance:#,##0.00}";
        }

        private void btnRemoveNetwork_Click(object sender, EventArgs e)
        {
            RemoveSelectedNetwork(txtNetwork.Text);
        }

        private void RemoveSelectedNetwork(string text)
        {
            string prefix = GetPrefix(text);

            RechargeLines = RechargeLines
                 .Where(r => (!(r.Mobile.StartsWith(prefix) || (r.Mobile.StartsWith("078") && prefix == "077"))))
                 .ToList();

            RefreshLoadedRechargeLines();

        }

        private static string GetPrefix(string text)
        {
            return text switch
            {
                "Econet" => "077",
                "Netone" => "071",
                "Telecel" => "073",
                _ => "078",
            };
        }

        private void btnExportNetwork_Click(object sender, EventArgs e)
        {
            var prefix = GetPrefix(txtNetwork.Text);
            var toExport = RechargeLines = RechargeLines
                 .Where(r => (r.Mobile.StartsWith(prefix) || (r.Mobile.StartsWith("078") && prefix == "077")))
                 .ToList();

            var frmSaveFile = new SaveFileDialog();
            frmSaveFile.DefaultExt = "csv";
            if (frmSaveFile.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(
                               frmSaveFile.FileName,
                               string.Join('\n', toExport.Select(r => SerializedRetryFileFormat(r)))
                               );
                MessageBox.Show("Exported");
            }
        }
    }
}