namespace ZLOperateTool.OrderTool
{
    partial class TeamUpOrder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeamUpOrder));
            this.uiDataGridView1 = new Sunny.UI.UIDataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FidCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mesorder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uloc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskCompletedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateDt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskCompletedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uiDataGridViewFooter1 = new Sunny.UI.UIDataGridViewFooter();
            this.uiPagination1 = new Sunny.UI.UIPagination();
            this.orderLable = new Sunny.UI.UILabel();
            this.orderTxt = new Sunny.UI.UITextBox();
            this.productTxt = new Sunny.UI.UITextBox();
            this.productNameLable = new Sunny.UI.UILabel();
            this.completedBtn = new Sunny.UI.UIButton();
            this.unBtn = new Sunny.UI.UIButton();
            this.allBtn = new Sunny.UI.UIButton();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.startOrder = new Sunny.UI.UIButton();
            this.manualBtn = new Sunny.UI.UIButton();
            this.searchBtn = new Sunny.UI.UIButton();
            this.uiStyleManager1 = new Sunny.UI.UIStyleManager(this.components);
            this.startTimePicker = new Sunny.UI.UIDatetimePicker();
            this.endTimePicker = new Sunny.UI.UIDatetimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // uiDataGridView1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.uiDataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.uiDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.uiDataGridView1.ColumnHeadersHeight = 32;
            this.uiDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.uiDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.FidCode,
            this.mesorder,
            this.ProductCode,
            this.ProductName,
            this.uloc,
            this.TaskCount,
            this.TaskCompletedCount,
            this.CreateDt,
            this.TaskCompletedTime});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.uiDataGridView1.EnableHeadersVisualStyles = false;
            this.uiDataGridView1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.Location = new System.Drawing.Point(26, 188);
            this.uiDataGridView1.Name = "uiDataGridView1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.uiDataGridView1.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.uiDataGridView1.RowTemplate.Height = 27;
            this.uiDataGridView1.SelectedIndex = -1;
            this.uiDataGridView1.Size = new System.Drawing.Size(1355, 556);
            this.uiDataGridView1.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.TabIndex = 0;
            this.uiDataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.uiDataGridView1_CellClick);
            this.uiDataGridView1.SelectionChanged += new System.EventHandler(this.DataGridView1_SelectionChanged);
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.MinimumWidth = 6;
            this.Id.Name = "Id";
            this.Id.Visible = false;
            this.Id.Width = 125;
            // 
            // FidCode
            // 
            this.FidCode.HeaderText = "产品型号";
            this.FidCode.MinimumWidth = 6;
            this.FidCode.Name = "FidCode";
            this.FidCode.Visible = false;
            this.FidCode.Width = 125;
            // 
            // mesorder
            // 
            this.mesorder.HeaderText = "工单号";
            this.mesorder.MinimumWidth = 6;
            this.mesorder.Name = "mesorder";
            this.mesorder.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.mesorder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.mesorder.Width = 230;
            // 
            // ProductCode
            // 
            this.ProductCode.HeaderText = "产品编号";
            this.ProductCode.MinimumWidth = 6;
            this.ProductCode.Name = "ProductCode";
            this.ProductCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ProductCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProductCode.Width = 180;
            // 
            // ProductName
            // 
            this.ProductName.HeaderText = "产品名称";
            this.ProductName.MinimumWidth = 6;
            this.ProductName.Name = "ProductName";
            this.ProductName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ProductName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProductName.Width = 300;
            // 
            // uloc
            // 
            this.uloc.HeaderText = "工序";
            this.uloc.MinimumWidth = 6;
            this.uloc.Name = "uloc";
            this.uloc.Width = 125;
            // 
            // TaskCount
            // 
            this.TaskCount.HeaderText = "下发数量";
            this.TaskCount.MinimumWidth = 6;
            this.TaskCount.Name = "TaskCount";
            this.TaskCount.Width = 125;
            // 
            // TaskCompletedCount
            // 
            this.TaskCompletedCount.HeaderText = "已完成数量";
            this.TaskCompletedCount.MinimumWidth = 6;
            this.TaskCompletedCount.Name = "TaskCompletedCount";
            this.TaskCompletedCount.Width = 125;
            // 
            // CreateDt
            // 
            this.CreateDt.HeaderText = "创建时间";
            this.CreateDt.MinimumWidth = 6;
            this.CreateDt.Name = "CreateDt";
            this.CreateDt.Width = 125;
            // 
            // TaskCompletedTime
            // 
            this.TaskCompletedTime.HeaderText = "完成时间";
            this.TaskCompletedTime.MinimumWidth = 6;
            this.TaskCompletedTime.Name = "TaskCompletedTime";
            this.TaskCompletedTime.Width = 125;
            // 
            // uiDataGridViewFooter1
            // 
            this.uiDataGridViewFooter1.DataGridView = null;
            this.uiDataGridViewFooter1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridViewFooter1.Location = new System.Drawing.Point(26, 750);
            this.uiDataGridViewFooter1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiDataGridViewFooter1.Name = "uiDataGridViewFooter1";
            this.uiDataGridViewFooter1.RadiusSides = Sunny.UI.UICornerRadiusSides.None;
            this.uiDataGridViewFooter1.Size = new System.Drawing.Size(1355, 29);
            this.uiDataGridViewFooter1.TabIndex = 1;
            this.uiDataGridViewFooter1.Text = "uiDataGridViewFooter1";
            // 
            // uiPagination1
            // 
            this.uiPagination1.ButtonFillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(204)))));
            this.uiPagination1.ButtonStyleInherited = false;
            this.uiPagination1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPagination1.Location = new System.Drawing.Point(26, 787);
            this.uiPagination1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPagination1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPagination1.Name = "uiPagination1";
            this.uiPagination1.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.uiPagination1.ShowText = false;
            this.uiPagination1.Size = new System.Drawing.Size(1360, 36);
            this.uiPagination1.TabIndex = 2;
            this.uiPagination1.Text = "uiPagination1";
            this.uiPagination1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPagination1.PageChanged += new Sunny.UI.UIPagination.OnPageChangeEventHandler(this.uiPagination1_PageChanged);
            // 
            // orderLable
            // 
            this.orderLable.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.orderLable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.orderLable.Location = new System.Drawing.Point(41, 73);
            this.orderLable.Name = "orderLable";
            this.orderLable.Size = new System.Drawing.Size(100, 23);
            this.orderLable.TabIndex = 3;
            this.orderLable.Text = "工单号：";
            // 
            // orderTxt
            // 
            this.orderTxt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.orderTxt.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.orderTxt.Location = new System.Drawing.Point(135, 73);
            this.orderTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.orderTxt.MinimumSize = new System.Drawing.Size(1, 16);
            this.orderTxt.Name = "orderTxt";
            this.orderTxt.Padding = new System.Windows.Forms.Padding(5);
            this.orderTxt.ShowText = false;
            this.orderTxt.Size = new System.Drawing.Size(202, 29);
            this.orderTxt.TabIndex = 4;
            this.orderTxt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.orderTxt.Watermark = "";
            // 
            // productTxt
            // 
            this.productTxt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.productTxt.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.productTxt.Location = new System.Drawing.Point(512, 73);
            this.productTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.productTxt.MinimumSize = new System.Drawing.Size(1, 16);
            this.productTxt.Name = "productTxt";
            this.productTxt.Padding = new System.Windows.Forms.Padding(5);
            this.productTxt.ShowText = false;
            this.productTxt.Size = new System.Drawing.Size(202, 29);
            this.productTxt.TabIndex = 6;
            this.productTxt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.productTxt.Watermark = "";
            // 
            // productNameLable
            // 
            this.productNameLable.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.productNameLable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.productNameLable.Location = new System.Drawing.Point(395, 73);
            this.productNameLable.Name = "productNameLable";
            this.productNameLable.Size = new System.Drawing.Size(120, 23);
            this.productNameLable.TabIndex = 5;
            this.productNameLable.Text = "产品名称：";
            // 
            // completedBtn
            // 
            this.completedBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.completedBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.completedBtn.Location = new System.Drawing.Point(752, 73);
            this.completedBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.completedBtn.Name = "completedBtn";
            this.completedBtn.Size = new System.Drawing.Size(137, 35);
            this.completedBtn.TabIndex = 7;
            this.completedBtn.Text = "已完成工单";
            this.completedBtn.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.completedBtn.Click += new System.EventHandler(this.completedBtn_Click);
            // 
            // unBtn
            // 
            this.unBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.unBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.unBtn.Location = new System.Drawing.Point(752, 121);
            this.unBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.unBtn.Name = "unBtn";
            this.unBtn.Size = new System.Drawing.Size(137, 35);
            this.unBtn.TabIndex = 8;
            this.unBtn.Text = "未完成工单";
            this.unBtn.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.unBtn.Click += new System.EventHandler(this.unBtn_Click);
            // 
            // allBtn
            // 
            this.allBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.allBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.allBtn.Location = new System.Drawing.Point(923, 73);
            this.allBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.allBtn.Name = "allBtn";
            this.allBtn.Size = new System.Drawing.Size(136, 35);
            this.allBtn.TabIndex = 9;
            this.allBtn.Text = "全部工单";
            this.allBtn.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.allBtn.Click += new System.EventHandler(this.allBtn_Click);
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(22, 133);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(119, 23);
            this.uiLabel1.TabIndex = 10;
            this.uiLabel1.Text = "开始时间：";
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(391, 133);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(147, 23);
            this.uiLabel2.TabIndex = 12;
            this.uiLabel2.Text = "结束时间：";
            // 
            // startOrder
            // 
            this.startOrder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.startOrder.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startOrder.Location = new System.Drawing.Point(1074, 73);
            this.startOrder.MinimumSize = new System.Drawing.Size(1, 1);
            this.startOrder.Name = "startOrder";
            this.startOrder.Size = new System.Drawing.Size(82, 83);
            this.startOrder.TabIndex = 19;
            this.startOrder.Text = "完工";
            this.startOrder.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startOrder.Click += new System.EventHandler(this.startOrder_Click);
            // 
            // manualBtn
            // 
            this.manualBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.manualBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.manualBtn.Location = new System.Drawing.Point(1162, 73);
            this.manualBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.manualBtn.Name = "manualBtn";
            this.manualBtn.Size = new System.Drawing.Size(76, 83);
            this.manualBtn.TabIndex = 20;
            this.manualBtn.Text = "RFID";
            this.manualBtn.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.manualBtn.Click += new System.EventHandler(this.manualBtn_Click);
            // 
            // searchBtn
            // 
            this.searchBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.searchBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.searchBtn.Location = new System.Drawing.Point(923, 121);
            this.searchBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(136, 35);
            this.searchBtn.TabIndex = 21;
            this.searchBtn.Text = "查询";
            this.searchBtn.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // uiStyleManager1
            // 
            this.uiStyleManager1.DPIScale = true;
            this.uiStyleManager1.GlobalFont = true;
            // 
            // startTimePicker
            // 
            this.startTimePicker.FillColor = System.Drawing.Color.White;
            this.startTimePicker.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startTimePicker.Location = new System.Drawing.Point(135, 127);
            this.startTimePicker.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startTimePicker.MaxLength = 19;
            this.startTimePicker.MinimumSize = new System.Drawing.Size(63, 0);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.startTimePicker.Size = new System.Drawing.Size(200, 29);
            this.startTimePicker.SymbolDropDown = 61555;
            this.startTimePicker.SymbolNormal = 61555;
            this.startTimePicker.SymbolSize = 24;
            this.startTimePicker.TabIndex = 22;
            this.startTimePicker.Text = "2024-11-15 16:00:22";
            this.startTimePicker.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.startTimePicker.Value = new System.DateTime(2024, 11, 15, 16, 0, 22, 571);
            this.startTimePicker.Watermark = "";
            // 
            // endTimePicker
            // 
            this.endTimePicker.FillColor = System.Drawing.Color.White;
            this.endTimePicker.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.endTimePicker.Location = new System.Drawing.Point(514, 127);
            this.endTimePicker.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.endTimePicker.MaxLength = 19;
            this.endTimePicker.MinimumSize = new System.Drawing.Size(63, 0);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.endTimePicker.Size = new System.Drawing.Size(200, 35);
            this.endTimePicker.SymbolDropDown = 61555;
            this.endTimePicker.SymbolNormal = 61555;
            this.endTimePicker.SymbolSize = 24;
            this.endTimePicker.TabIndex = 23;
            this.endTimePicker.Text = "2024-11-15 16:00:39";
            this.endTimePicker.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.endTimePicker.Value = new System.DateTime(2024, 11, 15, 16, 0, 39, 406);
            this.endTimePicker.Watermark = "";
            // 
            // TeamUpOrder
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1397, 851);
            this.Controls.Add(this.endTimePicker);
            this.Controls.Add(this.startTimePicker);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.manualBtn);
            this.Controls.Add(this.startOrder);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.uiLabel1);
            this.Controls.Add(this.allBtn);
            this.Controls.Add(this.unBtn);
            this.Controls.Add(this.completedBtn);
            this.Controls.Add(this.productTxt);
            this.Controls.Add(this.productNameLable);
            this.Controls.Add(this.orderTxt);
            this.Controls.Add(this.orderLable);
            this.Controls.Add(this.uiPagination1);
            this.Controls.Add(this.uiDataGridViewFooter1);
            this.Controls.Add(this.uiDataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TeamUpOrder";
            this.Text = "组队工单生产";
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 1769, 927);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ParentForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIDataGridView uiDataGridView1;
        private Sunny.UI.UIDataGridViewFooter uiDataGridViewFooter1;
        private Sunny.UI.UIPagination uiPagination1;
        private Sunny.UI.UILabel orderLable;
        private Sunny.UI.UITextBox orderTxt;
        private Sunny.UI.UITextBox productTxt;
        private Sunny.UI.UILabel productNameLable;
        private Sunny.UI.UIButton completedBtn;
        private Sunny.UI.UIButton unBtn;
        private Sunny.UI.UIButton allBtn;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UIButton startOrder;
        private Sunny.UI.UIButton manualBtn;
        private Sunny.UI.UIButton searchBtn;
        private Sunny.UI.UIStyleManager uiStyleManager1;
        private Sunny.UI.UIDatetimePicker startTimePicker;
        private Sunny.UI.UIDatetimePicker endTimePicker;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn FidCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn mesorder;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn uloc;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskCompletedCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateDt;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskCompletedTime;
    }
}