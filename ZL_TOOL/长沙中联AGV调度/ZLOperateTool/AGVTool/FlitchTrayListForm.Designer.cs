namespace ZLOperateTool.AGVTool
{
    partial class FlitchTrayListForm
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
            this.flitchGV = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.flitchGV)).BeginInit();
            this.SuspendLayout();
            // 
            // flitchGV
            // 
            this.flitchGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.flitchGV.Location = new System.Drawing.Point(13, 13);
            this.flitchGV.Name = "flitchGV";
            this.flitchGV.RowHeadersWidth = 51;
            this.flitchGV.RowTemplate.Height = 27;
            this.flitchGV.Size = new System.Drawing.Size(775, 425);
            this.flitchGV.TabIndex = 0;
            // 
            // FlitchTrayListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flitchGV);
            this.Name = "FlitchTrayListForm";
            this.Text = "贴板物料拣选列表";
            this.Load += new System.EventHandler(this.FlitchTrayListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.flitchGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView flitchGV;
    }
}