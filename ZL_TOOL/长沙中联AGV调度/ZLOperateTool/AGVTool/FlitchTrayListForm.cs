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
    public partial class FlitchTrayListForm : Form
    {
        public DataGridViewRow SelectedRow { get; private set; }
        public FlitchTrayListForm()
        {
            InitializeComponent();
        }

        private void FlitchTrayListForm_Load(object sender, EventArgs e)
        {
            this.flitchGV.Dock = DockStyle.Fill;
            this.flitchGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.flitchGV.MultiSelect = false;
            this.flitchGV.RowHeadersVisible = false;
            this.flitchGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // 列宽度自适应内容


            this.flitchGV.Columns.Add("Column1", "点位");
            this.flitchGV.Columns.Add("Column2", "中文点位");
            this.flitchGV.Columns.Add("Column3", "物料");

            var positionService = new AgvPositionService();
            var result = positionService.GetFlitchCompletedTray();
            foreach (var itemPosition in result)
            {
                this.flitchGV.Rows.Add(itemPosition.PositionCode, itemPosition.PositionName, itemPosition.PositionRemark);
            }
            this.flitchGV.DoubleClick += flitchGV_DoubleClick;
            this.flitchGV.AutoResizeColumns();
        }


        private void flitchGV_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.SelectedRows.Count > 0)
            {
                SelectedRow = dgv.SelectedRows[0];
            }
        }

        /// <summary>
        /// 贴板料双击选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flitchGV_DoubleClick(object sender, EventArgs e)
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
