namespace ZLOperateTool.AGVTool
{
    partial class CollectionAgvForm
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
            this.callEmpty = new System.Windows.Forms.Panel();
            this.collectionCallEmptyBtn = new System.Windows.Forms.Button();
            this.collectionEmptyCmb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.collectionCallEndBtn = new System.Windows.Forms.Button();
            this.collectionMC = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.collectionMN = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.collectionEndCmb = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.waitPolishCMX = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.callEmpty.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // callEmpty
            // 
            this.callEmpty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.callEmpty.Controls.Add(this.collectionCallEmptyBtn);
            this.callEmpty.Controls.Add(this.collectionEmptyCmb);
            this.callEmpty.Controls.Add(this.label2);
            this.callEmpty.Controls.Add(this.label1);
            this.callEmpty.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.callEmpty.Location = new System.Drawing.Point(4, 57);
            this.callEmpty.Name = "callEmpty";
            this.callEmpty.Size = new System.Drawing.Size(455, 336);
            this.callEmpty.TabIndex = 0;
            // 
            // collectionCallEmptyBtn
            // 
            this.collectionCallEmptyBtn.Location = new System.Drawing.Point(130, 152);
            this.collectionCallEmptyBtn.Name = "collectionCallEmptyBtn";
            this.collectionCallEmptyBtn.Size = new System.Drawing.Size(153, 45);
            this.collectionCallEmptyBtn.TabIndex = 3;
            this.collectionCallEmptyBtn.Text = "空盘呼叫";
            this.collectionCallEmptyBtn.UseVisualStyleBackColor = true;
            this.collectionCallEmptyBtn.Click += new System.EventHandler(this.collectionCallEmptyBtn_Click);
            // 
            // collectionEmptyCmb
            // 
            this.collectionEmptyCmb.FormattingEnabled = true;
            this.collectionEmptyCmb.Location = new System.Drawing.Point(103, 76);
            this.collectionEmptyCmb.Name = "collectionEmptyCmb";
            this.collectionEmptyCmb.Size = new System.Drawing.Size(344, 33);
            this.collectionEmptyCmb.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "点位：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(125, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "呼叫空托盘";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.collectionCallEndBtn);
            this.panel1.Controls.Add(this.collectionMC);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.collectionMN);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.collectionEndCmb);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(465, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(511, 336);
            this.panel1.TabIndex = 1;
            // 
            // collectionCallEndBtn
            // 
            this.collectionCallEndBtn.Location = new System.Drawing.Point(149, 279);
            this.collectionCallEndBtn.Name = "collectionCallEndBtn";
            this.collectionCallEndBtn.Size = new System.Drawing.Size(194, 42);
            this.collectionCallEndBtn.TabIndex = 7;
            this.collectionCallEndBtn.Text = "下料呼叫";
            this.collectionCallEndBtn.UseVisualStyleBackColor = true;
            this.collectionCallEndBtn.Click += new System.EventHandler(this.collectionCallEndBtn_Click);
            // 
            // collectionMC
            // 
            this.collectionMC.Location = new System.Drawing.Point(114, 196);
            this.collectionMC.Name = "collectionMC";
            this.collectionMC.Size = new System.Drawing.Size(376, 36);
            this.collectionMC.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 207);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 25);
            this.label6.TabIndex = 5;
            this.label6.Text = "数量：";
            // 
            // collectionMN
            // 
            this.collectionMN.Location = new System.Drawing.Point(114, 131);
            this.collectionMN.Name = "collectionMN";
            this.collectionMN.Size = new System.Drawing.Size(376, 36);
            this.collectionMN.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 25);
            this.label5.TabIndex = 3;
            this.label5.Text = "物料：";
            // 
            // collectionEndCmb
            // 
            this.collectionEndCmb.FormattingEnabled = true;
            this.collectionEndCmb.Location = new System.Drawing.Point(114, 71);
            this.collectionEndCmb.Name = "collectionEndCmb";
            this.collectionEndCmb.Size = new System.Drawing.Size(376, 33);
            this.collectionEndCmb.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "下料点：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(158, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "下料线下料";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 39);
            this.button1.TabIndex = 2;
            this.button1.Text = "打开点位图";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.waitPolishCMX);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Location = new System.Drawing.Point(4, 400);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(970, 206);
            this.panel2.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(278, 122);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(194, 42);
            this.button2.TabIndex = 3;
            this.button2.Text = "完工";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // waitPolishCMX
            // 
            this.waitPolishCMX.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.waitPolishCMX.FormattingEnabled = true;
            this.waitPolishCMX.Location = new System.Drawing.Point(156, 71);
            this.waitPolishCMX.Name = "waitPolishCMX";
            this.waitPolishCMX.Size = new System.Drawing.Size(795, 33);
            this.waitPolishCMX.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(14, 79);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(137, 25);
            this.label8.TabIndex = 1;
            this.label8.Text = "完工点位：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(283, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 25);
            this.label7.TabIndex = 0;
            this.label7.Text = "打磨完工";
            // 
            // CollectionAgvForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 618);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.callEmpty);
            this.Name = "CollectionAgvForm";
            this.Text = "下料线AGV调度";
            this.Load += new System.EventHandler(this.CollectionAgvForm_Load);
            this.callEmpty.ResumeLayout(false);
            this.callEmpty.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel callEmpty;
        private System.Windows.Forms.Button collectionCallEmptyBtn;
        private System.Windows.Forms.ComboBox collectionEmptyCmb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button collectionCallEndBtn;
        private System.Windows.Forms.TextBox collectionMC;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox collectionMN;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox collectionEndCmb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox waitPolishCMX;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}