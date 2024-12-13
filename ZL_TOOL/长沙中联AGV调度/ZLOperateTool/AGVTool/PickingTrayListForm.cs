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

namespace ZLOperateTool.AGVTool
{
    public partial class PickingTrayListForm : Form
    {
        public DataGridViewRow SelectedRow { get; private set; }
        public PickingTrayListForm()
        {
            InitializeComponent();
        }

        private void PickingTrayListForm_Load(object sender, EventArgs e)
        {
            this.pickingGV.Dock = DockStyle.Fill;
            this.pickingGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.pickingGV.MultiSelect = false;
            this.pickingGV.RowHeadersVisible = false;
            this.pickingGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // 列宽度自适应内容


            this.pickingGV.Columns.Add("Column1", "点位");
            this.pickingGV.Columns.Add("Column2", "中文点位");
            this.pickingGV.Columns.Add("Column3", "物料");

            var positionService = new AgvPositionService();
            var result = positionService.GetPickingCompletedTray();
            foreach (var itemPosition in result)
            {
                this.pickingGV.Rows.Add(itemPosition.PositionCode, itemPosition.PositionName, itemPosition.PositionRemark);
            }
            this.pickingGV.DoubleClick += pickingGV_DoubleClick;
            this.pickingGV.AutoResizeColumns();

        }


        private void pickingGV_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.SelectedRows.Count > 0)
            {
                SelectedRow = dgv.SelectedRows[0];
            }
        }
        private void pickingGV_DoubleClick(object sender, EventArgs e)
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
