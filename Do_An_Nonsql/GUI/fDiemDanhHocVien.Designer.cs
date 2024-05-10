namespace Do_An_Chuyen_Nganh.GUI
{
    partial class fDiemDanhHocVien
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
            this.dataDiemDanh = new System.Windows.Forms.DataGridView();
            this.btnLuudiemdanh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataDiemDanh)).BeginInit();
            this.SuspendLayout();
            // 
            // dataDiemDanh
            // 
            this.dataDiemDanh.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataDiemDanh.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataDiemDanh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataDiemDanh.Location = new System.Drawing.Point(61, 93);
            this.dataDiemDanh.Name = "dataDiemDanh";
            this.dataDiemDanh.Size = new System.Drawing.Size(1011, 375);
            this.dataDiemDanh.TabIndex = 0;
            // 
            // btnLuudiemdanh
            // 
            this.btnLuudiemdanh.Location = new System.Drawing.Point(967, 41);
            this.btnLuudiemdanh.Name = "btnLuudiemdanh";
            this.btnLuudiemdanh.Size = new System.Drawing.Size(68, 33);
            this.btnLuudiemdanh.TabIndex = 1;
            this.btnLuudiemdanh.Text = "Lưu";
            this.btnLuudiemdanh.UseVisualStyleBackColor = true;
            this.btnLuudiemdanh.Click += new System.EventHandler(this.btnLuudiemdanh_Click);
            // 
            // fDiemDanhHocVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 496);
            this.Controls.Add(this.btnLuudiemdanh);
            this.Controls.Add(this.dataDiemDanh);
            this.Name = "fDiemDanhHocVien";
            this.Text = "fDiemDanhHocVien";
            ((System.ComponentModel.ISupportInitialize)(this.dataDiemDanh)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataDiemDanh;
        private System.Windows.Forms.Button btnLuudiemdanh;
    }
}