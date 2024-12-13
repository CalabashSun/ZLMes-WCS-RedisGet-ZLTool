using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZLOperateTool.AGVTool;
using ZLOperateTool.Services;

namespace ZLOperateTool.BankTool
{
    public partial class BankOutForm : Form
    {

        public BankService BankService = new BankService();
        public BankOutForm()
        {
            InitializeComponent();
        }

        private void BankOutForm_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                CheckDataCount();
            });
        }


        public void CheckDataCount()
        {
            while (true) {
                try {
                    var bankData = BankService.GetBankData();
                    var data1Count = bankData.Where(p => p.DataCode == "1").Sum(p => p.Quantity);
                    var data2Count = bankData.Where(p => p.DataCode == "2").Sum(p => p.Quantity);
                    var data3Count = bankData.Where(p => p.DataCode == "3").Sum(p => p.Quantity);
                    var data4Count = bankData.Where(p => p.DataCode == "4").Sum(p => p.Quantity);

                    Invoke((Action)(() =>
                    {
                        this.code1Num.Text = data1Count.ToString();
                        this.code2Num.Text = data2Count.ToString();
                        this.code3Num.Text = data3Count.ToString();
                        this.code4Num.Text = data4Count.ToString();
                    }));
                    
                } catch { }
                Thread.Sleep(5000);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var bankData = BankService.GetBankData();
            var data1Count = bankData.Where(p => p.DataCode == "1").Sum(p => p.Quantity);
            var data2Count = bankData.Where(p => p.DataCode == "2").Sum(p => p.Quantity);
            var data3Count = bankData.Where(p => p.DataCode == "3").Sum(p => p.Quantity);
            var data4Count = bankData.Where(p => p.DataCode == "4").Sum(p => p.Quantity);

            Invoke((Action)(() =>
            {
                this.code1Num.Text = data1Count.ToString();
                this.code2Num.Text = data2Count.ToString();
                this.code3Num.Text = data3Count.ToString();
                this.code4Num.Text = data4Count.ToString();
            }));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var choosedData = this.choosedDataTxt.Text;
            if (string.IsNullOrEmpty(choosedData))
            {
                MessageBox.Show("请选择需要出库的物料");
                return;
            }
            DialogResult result = MessageBox.Show("确定要对"+ choosedData+",进行出库操作吗", "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var stockNum = choosedData.Split('-')[0];
                var productName = choosedData.Split('-')[1];
                var quantity = choosedData.Split('-')[2];
                var dataId = choosedData.Split('-')[3];

                var lastOrderData = BankService.GetLastestOutData();
                if (lastOrderData != null && Convert.ToInt32(lastOrderData) < 9000)
                {
                    lastOrderData += 10;
                }
                BankService.AddBankOutData(stockNum,quantity,lastOrderData,productName,dataId);
                MessageBox.Show("出库成功");
            }
            else {
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BankDataListForm pickingForm = new BankDataListForm();
            pickingForm.FormClosed += BankDataListForm_FormClosed;
            pickingForm.Show();
        }


        public void BankDataListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            BankDataListForm gridForm = (BankDataListForm)sender;
            if (gridForm.SelectedRow != null)
            {
                // 将选中行的数据设置到上一个界面的文本框
                this.choosedDataTxt.Text = gridForm.SelectedRow.Cells[0].Value.ToString() + "-"
                    + gridForm.SelectedRow.Cells[1].Value.ToString() + "-"
                    + gridForm.SelectedRow.Cells[2].Value.ToString()+"-"
                    + gridForm.SelectedRow.Cells[3].Value.ToString(); // 假设选择的是第一列数据
            }
        }
    }
}
