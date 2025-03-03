using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hot.BulkRecharge
{
    public partial class FixlineData : Form
    {
        public string Mobile { get; set; } = "";
        public string Amount { get; set; } = "";
        public bool IsData { get; set; } = false;
        public string LineData { get; set; } = ""; 
        public RechargeParsingService ParsingService = new(1000000);
        public FixlineData()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ParsingService.IsValidPhoneNumber(Mobile) && ParsingService.IsValidAmountOrProductCode(Amount))
            {
                DialogResult = DialogResult.OK;
                IsData = ParsingService.IsValidProductCode(Amount);
                this.Close();
            }
            else
            {
                txtError.Text = ParsingService.IsValidPhoneNumber(Mobile) ? "Invalid Number" : "Invalid Amount";
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FixlineData_Load(object sender, EventArgs e)
        {
            textBox3.Text = LineData;
            textBox1.Text = Mobile ?? "";
            textBox2.Text = Amount ?? "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Mobile = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Amount = textBox2.Text;
        }
    }
}
