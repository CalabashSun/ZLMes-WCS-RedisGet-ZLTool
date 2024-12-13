namespace ZLOperateTool.BankTool
{
    partial class BankDataListForm
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
            this.bankDataGv = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.bankDataGv)).BeginInit();
            this.SuspendLayout();
            // 
            // bankDataGv
            // 
            this.bankDataGv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bankDataGv.Location = new System.Drawing.Point(13, 13);
            this.bankDataGv.Name = "bankDataGv";
            this.bankDataGv.RowHeadersWidth = 51;
            this.bankDataGv.RowTemplate.Height = 27;
            this.bankDataGv.Size = new System.Drawing.Size(766, 425);
            this.bankDataGv.TabIndex = 0;
            this.bankDataGv.SelectionChanged += new System.EventHandler(this.pipeGV_SelectionChanged);
            // 
            // BankDataListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bankDataGv);
            this.Name = "BankDataListForm";
            this.Text = "立库物料列表";
            this.Load += new System.EventHandler(this.BankDataListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bankDataGv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView bankDataGv;
    }
}