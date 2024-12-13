namespace ZLOperateTool
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.deviceId = new Sunny.UI.UILabel();
            this.teamOrderBtn = new Sunny.UI.UIButton();
            this.button2 = new Sunny.UI.UIButton();
            this.button8 = new Sunny.UI.UIButton();
            this.button3 = new Sunny.UI.UIButton();
            this.button4 = new Sunny.UI.UIButton();
            this.button5 = new Sunny.UI.UIButton();
            this.button6 = new Sunny.UI.UIButton();
            this.button7 = new Sunny.UI.UIButton();
            this.AddReportBtn = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(59, 83);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(241, 23);
            this.uiLabel1.TabIndex = 9;
            this.uiLabel1.Text = "当前设备编号：";
            // 
            // deviceId
            // 
            this.deviceId.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.deviceId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.deviceId.Location = new System.Drawing.Point(252, 83);
            this.deviceId.Name = "deviceId";
            this.deviceId.Size = new System.Drawing.Size(147, 23);
            this.deviceId.TabIndex = 10;
            this.deviceId.Text = "6";
            // 
            // teamOrderBtn
            // 
            this.teamOrderBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.teamOrderBtn.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.teamOrderBtn.Location = new System.Drawing.Point(50, 143);
            this.teamOrderBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.teamOrderBtn.Name = "teamOrderBtn";
            this.teamOrderBtn.Size = new System.Drawing.Size(194, 61);
            this.teamOrderBtn.TabIndex = 11;
            this.teamOrderBtn.Text = "组队订单";
            this.teamOrderBtn.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.teamOrderBtn.Click += new System.EventHandler(this.teamOrderBtn_Click);
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(476, 143);
            this.button2.MinimumSize = new System.Drawing.Size(1, 1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(172, 61);
            this.button2.TabIndex = 12;
            this.button2.Text = "下料线AGV调度";
            this.button2.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button8
            // 
            this.button8.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button8.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button8.Location = new System.Drawing.Point(476, 352);
            this.button8.MinimumSize = new System.Drawing.Size(1, 1);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(166, 61);
            this.button8.TabIndex = 13;
            this.button8.Text = "托盘出库";
            this.button8.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button3
            // 
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.Location = new System.Drawing.Point(47, 244);
            this.button3.MinimumSize = new System.Drawing.Size(1, 1);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(197, 71);
            this.button3.TabIndex = 14;
            this.button3.Text = "人工拣货AGV调度";
            this.button3.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.Location = new System.Drawing.Point(269, 244);
            this.button4.MinimumSize = new System.Drawing.Size(1, 1);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(188, 71);
            this.button4.TabIndex = 15;
            this.button4.Text = "贴板AGV调度";
            this.button4.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button5.Location = new System.Drawing.Point(476, 244);
            this.button5.MinimumSize = new System.Drawing.Size(1, 1);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(166, 71);
            this.button5.TabIndex = 16;
            this.button5.Text = "组队AGV调度";
            this.button5.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button6.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button6.Location = new System.Drawing.Point(47, 352);
            this.button6.MinimumSize = new System.Drawing.Size(1, 1);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(197, 64);
            this.button6.TabIndex = 17;
            this.button6.Text = "上挂AGV调度";
            this.button6.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // button7
            // 
            this.button7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button7.Location = new System.Drawing.Point(269, 352);
            this.button7.MinimumSize = new System.Drawing.Size(1, 1);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(188, 64);
            this.button7.TabIndex = 18;
            this.button7.Text = "AGV测试";
            this.button7.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // AddReportBtn
            // 
            this.AddReportBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddReportBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AddReportBtn.Location = new System.Drawing.Point(269, 143);
            this.AddReportBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.AddReportBtn.Name = "AddReportBtn";
            this.AddReportBtn.Size = new System.Drawing.Size(188, 61);
            this.AddReportBtn.TabIndex = 19;
            this.AddReportBtn.Text = "补报工";
            this.AddReportBtn.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AddReportBtn.Click += new System.EventHandler(this.AddReportBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(674, 609);
            this.Controls.Add(this.AddReportBtn);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.teamOrderBtn);
            this.Controls.Add(this.deviceId);
            this.Controls.Add(this.uiLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "中联快捷操作工具";
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 659, 612);
            this.ResumeLayout(false);

        }

        #endregion
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel deviceId;
        private Sunny.UI.UIButton teamOrderBtn;
        private Sunny.UI.UIButton button2;
        private Sunny.UI.UIButton button8;
        private Sunny.UI.UIButton button3;
        private Sunny.UI.UIButton button4;
        private Sunny.UI.UIButton button5;
        private Sunny.UI.UIButton button6;
        private Sunny.UI.UIButton button7;
        private Sunny.UI.UIButton AddReportBtn;
    }
}

