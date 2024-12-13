namespace ZLOperateTool
{
    partial class FidWrite
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
            this.is128bit = new System.Windows.Forms.CheckBox();
            this.label1 = new Sunny.UI.UILabel();
            this.textBox1 = new Sunny.UI.UITextBox();
            this.fidwritebtn = new Sunny.UI.UIButton();
            this.button1 = new Sunny.UI.UIButton();
            this.label2 = new Sunny.UI.UILabel();
            this.textBox2 = new Sunny.UI.UITextBox();
            this.fidRead = new Sunny.UI.UIButton();
            this.ipaddressLable = new Sunny.UI.UILabel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.connectStateBtn = new Sunny.UI.UISymbolButton();
            this.label4 = new Sunny.UI.UILabel();
            this.SuspendLayout();
            // 
            // is128bit
            // 
            this.is128bit.AutoSize = true;
            this.is128bit.Location = new System.Drawing.Point(1130, 131);
            this.is128bit.Name = "is128bit";
            this.is128bit.Size = new System.Drawing.Size(151, 24);
            this.is128bit.TabIndex = 9;
            this.is128bit.Text = "特定16位数据";
            this.is128bit.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.label1.Location = new System.Drawing.Point(77, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "RFID编码";
            // 
            // textBox1
            // 
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(208, 106);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.MinimumSize = new System.Drawing.Size(1, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Padding = new System.Windows.Forms.Padding(5);
            this.textBox1.ShowText = false;
            this.textBox1.Size = new System.Drawing.Size(584, 54);
            this.textBox1.TabIndex = 11;
            this.textBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.textBox1.Watermark = "";
            // 
            // fidwritebtn
            // 
            this.fidwritebtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fidwritebtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fidwritebtn.Location = new System.Drawing.Point(809, 106);
            this.fidwritebtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.fidwritebtn.Name = "fidwritebtn";
            this.fidwritebtn.Size = new System.Drawing.Size(116, 54);
            this.fidwritebtn.TabIndex = 12;
            this.fidwritebtn.Text = "写入";
            this.fidwritebtn.TipsFont = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fidwritebtn.Click += new System.EventHandler(this.fidwritebtn_Click);
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(964, 106);
            this.button1.MinimumSize = new System.Drawing.Size(1, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 54);
            this.button1.TabIndex = 13;
            this.button1.Text = "断开连接";
            this.button1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.label2.Location = new System.Drawing.Point(77, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 23);
            this.label2.TabIndex = 14;
            this.label2.Text = "读取结果";
            // 
            // textBox2
            // 
            this.textBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(208, 179);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox2.MinimumSize = new System.Drawing.Size(1, 16);
            this.textBox2.Name = "textBox2";
            this.textBox2.Padding = new System.Windows.Forms.Padding(5);
            this.textBox2.ShowText = false;
            this.textBox2.Size = new System.Drawing.Size(584, 51);
            this.textBox2.TabIndex = 15;
            this.textBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.textBox2.Watermark = "";
            // 
            // fidRead
            // 
            this.fidRead.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fidRead.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fidRead.Location = new System.Drawing.Point(809, 179);
            this.fidRead.MinimumSize = new System.Drawing.Size(1, 1);
            this.fidRead.Name = "fidRead";
            this.fidRead.Size = new System.Drawing.Size(116, 51);
            this.fidRead.TabIndex = 16;
            this.fidRead.Text = "读取";
            this.fidRead.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fidRead.Click += new System.EventHandler(this.fidRead_Click);
            // 
            // ipaddressLable
            // 
            this.ipaddressLable.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ipaddressLable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.ipaddressLable.Location = new System.Drawing.Point(77, 60);
            this.ipaddressLable.Name = "ipaddressLable";
            this.ipaddressLable.Size = new System.Drawing.Size(540, 30);
            this.ipaddressLable.TabIndex = 17;
            this.ipaddressLable.Text = "RFID设备IP：";
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(805, 64);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiLabel1.TabIndex = 18;
            this.uiLabel1.Text = "状态：";
            // 
            // connectStateBtn
            // 
            this.connectStateBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.connectStateBtn.FillColor = System.Drawing.Color.Orange;
            this.connectStateBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.connectStateBtn.Location = new System.Drawing.Point(894, 46);
            this.connectStateBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.connectStateBtn.Name = "connectStateBtn";
            this.connectStateBtn.Size = new System.Drawing.Size(44, 44);
            this.connectStateBtn.Symbol = 61761;
            this.connectStateBtn.TabIndex = 20;
            this.connectStateBtn.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.label4.Location = new System.Drawing.Point(960, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 21;
            this.label4.Text = "连接中...";
            // 
            // FidWrite
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1308, 284);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.connectStateBtn);
            this.Controls.Add(this.uiLabel1);
            this.Controls.Add(this.ipaddressLable);
            this.Controls.Add(this.fidRead);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fidwritebtn);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.is128bit);
            this.Name = "FidWrite";
            this.Text = "RFID读取写入";
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 1284, 316);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FidFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox is128bit;
        private Sunny.UI.UILabel label1;
        private Sunny.UI.UITextBox textBox1;
        private Sunny.UI.UIButton fidwritebtn;
        private Sunny.UI.UIButton button1;
        private Sunny.UI.UILabel label2;
        private Sunny.UI.UITextBox textBox2;
        private Sunny.UI.UIButton fidRead;
        private Sunny.UI.UILabel ipaddressLable;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UISymbolButton connectStateBtn;
        private Sunny.UI.UILabel label4;
    }
}