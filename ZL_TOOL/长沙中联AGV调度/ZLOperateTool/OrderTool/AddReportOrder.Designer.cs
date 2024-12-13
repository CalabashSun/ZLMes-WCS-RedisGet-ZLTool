namespace ZLOperateTool.OrderTool
{
    partial class AddReportOrder
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
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.mesorderTxt = new Sunny.UI.UITextBox();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.rfidTxt = new Sunny.UI.UITextBox();
            this.teamupBtn = new Sunny.UI.UIButton();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(84, 112);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.Text = "MesOrder:";
            // 
            // mesorderTxt
            // 
            this.mesorderTxt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mesorderTxt.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mesorderTxt.Location = new System.Drawing.Point(202, 106);
            this.mesorderTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mesorderTxt.MinimumSize = new System.Drawing.Size(1, 16);
            this.mesorderTxt.Name = "mesorderTxt";
            this.mesorderTxt.Padding = new System.Windows.Forms.Padding(5);
            this.mesorderTxt.ShowText = false;
            this.mesorderTxt.Size = new System.Drawing.Size(457, 29);
            this.mesorderTxt.TabIndex = 1;
            this.mesorderTxt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.mesorderTxt.Watermark = "";
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(119, 162);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(100, 23);
            this.uiLabel2.TabIndex = 2;
            this.uiLabel2.Text = "RFID:";
            // 
            // rfidTxt
            // 
            this.rfidTxt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rfidTxt.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rfidTxt.Location = new System.Drawing.Point(202, 156);
            this.rfidTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rfidTxt.MinimumSize = new System.Drawing.Size(1, 16);
            this.rfidTxt.Name = "rfidTxt";
            this.rfidTxt.Padding = new System.Windows.Forms.Padding(5);
            this.rfidTxt.ShowText = false;
            this.rfidTxt.Size = new System.Drawing.Size(457, 29);
            this.rfidTxt.TabIndex = 3;
            this.rfidTxt.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.rfidTxt.Watermark = "";
            // 
            // teamupBtn
            // 
            this.teamupBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.teamupBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.teamupBtn.Location = new System.Drawing.Point(123, 223);
            this.teamupBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.teamupBtn.Name = "teamupBtn";
            this.teamupBtn.Size = new System.Drawing.Size(207, 42);
            this.teamupBtn.TabIndex = 4;
            this.teamupBtn.Text = "组队工单补推送";
            this.teamupBtn.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.teamupBtn.Click += new System.EventHandler(this.teamupBtn_Click);
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Location = new System.Drawing.Point(369, 223);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(169, 42);
            this.uiButton1.TabIndex = 5;
            this.uiButton1.Text = "自动焊工单补推送";
            this.uiButton1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // AddReportOrder
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(723, 309);
            this.Controls.Add(this.uiButton1);
            this.Controls.Add(this.teamupBtn);
            this.Controls.Add(this.rfidTxt);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.mesorderTxt);
            this.Controls.Add(this.uiLabel1);
            this.Name = "AddReportOrder";
            this.Text = "补报工";
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 723, 309);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UITextBox mesorderTxt;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UITextBox rfidTxt;
        private Sunny.UI.UIButton teamupBtn;
        private Sunny.UI.UIButton uiButton1;
    }
}