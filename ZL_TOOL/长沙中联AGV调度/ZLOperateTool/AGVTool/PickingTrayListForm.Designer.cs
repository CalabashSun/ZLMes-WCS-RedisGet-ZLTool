namespace ZLOperateTool.AGVTool
{
    partial class PickingTrayListForm
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
            this.pickingGV = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pickingGV)).BeginInit();
            this.SuspendLayout();
            // 
            // pickingGV
            // 
            this.pickingGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pickingGV.Location = new System.Drawing.Point(13, 13);
            this.pickingGV.Name = "pickingGV";
            this.pickingGV.RowHeadersWidth = 51;
            this.pickingGV.RowTemplate.Height = 27;
            this.pickingGV.Size = new System.Drawing.Size(775, 425);
            this.pickingGV.TabIndex = 0;
            this.pickingGV.SelectionChanged += new System.EventHandler(this.pickingGV_SelectionChanged);
            // 
            // PickingTrayListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pickingGV);
            this.Name = "PickingTrayListForm";
            this.Text = "拣选料托盘列表";
            this.Load += new System.EventHandler(this.PickingTrayListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pickingGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView pickingGV;
    }
}