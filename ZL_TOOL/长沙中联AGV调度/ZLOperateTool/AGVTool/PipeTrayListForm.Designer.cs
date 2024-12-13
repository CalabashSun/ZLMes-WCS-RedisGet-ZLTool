namespace ZLOperateTool.AGVTool
{
    partial class PipeTrayListForm
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
            this.pipeGV = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pipeGV)).BeginInit();
            this.SuspendLayout();
            // 
            // pipeGV
            // 
            this.pipeGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pipeGV.Location = new System.Drawing.Point(3, 12);
            this.pipeGV.Name = "pipeGV";
            this.pipeGV.RowHeadersWidth = 51;
            this.pipeGV.RowTemplate.Height = 27;
            this.pipeGV.Size = new System.Drawing.Size(785, 426);
            this.pipeGV.TabIndex = 0;
            this.pipeGV.SelectionChanged += new System.EventHandler(this.pipeGV_SelectionChanged);
            // 
            // PipeTrayListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pipeGV);
            this.Name = "PipeTrayListForm";
            this.Text = "矩管料托盘列表";
            this.Load += new System.EventHandler(this.PipeTrayListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pipeGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView pipeGV;
    }
}