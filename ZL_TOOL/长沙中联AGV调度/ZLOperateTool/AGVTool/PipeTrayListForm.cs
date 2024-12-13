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
    public partial class PipeTrayListForm : Form
    {
        public DataGridViewRow PipeSelectedRow { get; private set; }

        public PipeTrayListForm()
        {
            InitializeComponent();
        }

        private void PipeTrayListForm_Load(object sender, EventArgs e)
        {
            this.pipeGV.Dock = DockStyle.Fill;
            this.pipeGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.pipeGV.MultiSelect = false;
            this.pipeGV.RowHeadersVisible = false;
            this.pipeGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // 列宽度自适应内容


            this.pipeGV.Columns.Add("Column1", "点位");
            this.pipeGV.Columns.Add("Column2", "中文点位");
            this.pipeGV.Columns.Add("Column3", "物料");

            var positionService = new AgvPositionService();
            var result = positionService.GetPipeCompletedTray();
            foreach (var itemPosition in result)
            {
                this.pipeGV.Rows.Add(itemPosition.PositionCode, itemPosition.PositionName, itemPosition.PositionRemark);
            }
            this.pipeGV.DoubleClick += pipeGV_DoubleClick;
            this.pipeGV.AutoResizeColumns();
        }

        private void pipeGV_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.SelectedRows.Count > 0)
            {
                PipeSelectedRow = dgv.SelectedRows[0];
            }
        }
        private void pipeGV_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.SelectedRows.Count > 0)
            {
                PipeSelectedRow = dgv.SelectedRows[0];
                this.Close(); // 双击选择行后关闭窗体
            }
        }
        private void GridForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
