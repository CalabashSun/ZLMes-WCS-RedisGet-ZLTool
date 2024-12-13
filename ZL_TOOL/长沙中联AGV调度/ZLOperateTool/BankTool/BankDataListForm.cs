using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZLOperateTool.Services;

namespace ZLOperateTool.BankTool
{
    public partial class BankDataListForm : Form
    {

        public DataGridViewRow SelectedRow { get; private set; }
        public BankDataListForm()
        {
            InitializeComponent();
        }

        private void BankDataListForm_Load(object sender, EventArgs e)
        {
            this.bankDataGv.Dock = DockStyle.Fill;
            this.bankDataGv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.bankDataGv.MultiSelect = false;
            this.bankDataGv.RowHeadersVisible = false;
            this.bankDataGv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // 列宽度自适应内容


            this.bankDataGv.Columns.Add("Column1", "库位");
            this.bankDataGv.Columns.Add("Column2", "物料");
            this.bankDataGv.Columns.Add("Column3", "数量");
            this.bankDataGv.Columns.Add("Column3", "物料code码");
            var bankService = new BankService();
            var result = bankService.GetBankData();
            foreach (var itemPosition in result)
            {
                this.bankDataGv.Rows.Add(itemPosition.StockNum, itemPosition.ProductName, itemPosition.Quantity,itemPosition.DataCode);
            }
            this.bankDataGv.DoubleClick += bankDataGv_DoubleClick;
            this.bankDataGv.AutoResizeColumns();
        }

        private void pipeGV_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.SelectedRows.Count > 0)
            {
                SelectedRow = dgv.SelectedRows[0];
            }
        }


        private void bankDataGv_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.SelectedRows.Count > 0)
            {
                SelectedRow = dgv.SelectedRows[0];
                this.Close(); // 双击选择行后关闭窗体
            }
        }

        private void GridForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
