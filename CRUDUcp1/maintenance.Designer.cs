namespace CRUDUcp1
{
    partial class Maintenance
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTambah = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.dgvMaintenance = new System.Windows.Forms.DataGridView();
            this.dtpMaintenance = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmbKamera = new System.Windows.Forms.ComboBox();
            this.cmbTeknisi = new System.Windows.Forms.ComboBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnAnalisis = new System.Windows.Forms.Button();
            this.lblMessages = new System.Windows.Forms.Label();
            this.btnLaporan = new System.Windows.Forms.Button();
            this.cmbKeterangan = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaintenance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tanggal Maintenance";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Keterangan";
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(113, 201);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(88, 35);
            this.btnTambah.TabIndex = 6;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(225, 199);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(88, 34);
            this.btnHapus.TabIndex = 7;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = true;
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(337, 199);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(88, 32);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // dgvMaintenance
            // 
            this.dgvMaintenance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMaintenance.Location = new System.Drawing.Point(50, 254);
            this.dgvMaintenance.Name = "dgvMaintenance";
            this.dgvMaintenance.RowHeadersWidth = 51;
            this.dgvMaintenance.RowTemplate.Height = 24;
            this.dgvMaintenance.Size = new System.Drawing.Size(792, 150);
            this.dgvMaintenance.TabIndex = 9;
            this.dgvMaintenance.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMaintenance_CellContentClick);
            // 
            // dtpMaintenance
            // 
            this.dtpMaintenance.Location = new System.Drawing.Point(278, 107);
            this.dtpMaintenance.Name = "dtpMaintenance";
            this.dtpMaintenance.Size = new System.Drawing.Size(200, 22);
            this.dtpMaintenance.TabIndex = 10;
            this.dtpMaintenance.ValueChanged += new System.EventHandler(this.dtpMaintenance_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Nama Kamera";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "Nama Teknisi";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CRUDUcp1.Properties.Resources.login;
            this.pictureBox1.Location = new System.Drawing.Point(609, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(224, 202);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // cmbKamera
            // 
            this.cmbKamera.FormattingEnabled = true;
            this.cmbKamera.Location = new System.Drawing.Point(278, 41);
            this.cmbKamera.Name = "cmbKamera";
            this.cmbKamera.Size = new System.Drawing.Size(200, 24);
            this.cmbKamera.TabIndex = 16;
            // 
            // cmbTeknisi
            // 
            this.cmbTeknisi.FormattingEnabled = true;
            this.cmbTeknisi.Location = new System.Drawing.Point(278, 77);
            this.cmbTeknisi.Name = "cmbTeknisi";
            this.cmbTeknisi.Size = new System.Drawing.Size(200, 24);
            this.cmbTeknisi.TabIndex = 17;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(908, 44);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 18;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnAnalisis
            // 
            this.btnAnalisis.Location = new System.Drawing.Point(908, 96);
            this.btnAnalisis.Name = "btnAnalisis";
            this.btnAnalisis.Size = new System.Drawing.Size(75, 23);
            this.btnAnalisis.TabIndex = 19;
            this.btnAnalisis.Text = "Analisis";
            this.btnAnalisis.UseVisualStyleBackColor = true;
            this.btnAnalisis.Click += new System.EventHandler(this.btnAnalisis_Click);
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(912, 159);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(71, 16);
            this.lblMessages.TabIndex = 20;
            this.lblMessages.Text = "Messages";
            // 
            // btnLaporan
            // 
            this.btnLaporan.Location = new System.Drawing.Point(503, 201);
            this.btnLaporan.Name = "btnLaporan";
            this.btnLaporan.Size = new System.Drawing.Size(75, 23);
            this.btnLaporan.TabIndex = 21;
            this.btnLaporan.Text = "Laporan";
            this.btnLaporan.UseVisualStyleBackColor = true;
            this.btnLaporan.Click += new System.EventHandler(this.btnLaporan_Click);
            // 
            // cmbKeterangan
            // 
            this.cmbKeterangan.FormattingEnabled = true;
            this.cmbKeterangan.Items.AddRange(new object[] {
            "Sedang Dikerjakan",
            "Pending",
            "Selesai"});
            this.cmbKeterangan.Location = new System.Drawing.Point(278, 140);
            this.cmbKeterangan.Name = "cmbKeterangan";
            this.cmbKeterangan.Size = new System.Drawing.Size(200, 24);
            this.cmbKeterangan.TabIndex = 22;
            // 
            // Maintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1266, 450);
            this.Controls.Add(this.cmbKeterangan);
            this.Controls.Add(this.btnLaporan);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.btnAnalisis);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.cmbTeknisi);
            this.Controls.Add(this.cmbKamera);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpMaintenance);
            this.Controls.Add(this.dgvMaintenance);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "Maintenance";
            this.Text = "maintenance";
            this.Load += new System.EventHandler(this.Maintenance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaintenance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.DataGridView dgvMaintenance;
        private System.Windows.Forms.DateTimePicker dtpMaintenance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox cmbKamera;
        private System.Windows.Forms.ComboBox cmbTeknisi;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnAnalisis;
        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.Button btnLaporan;
        private System.Windows.Forms.ComboBox cmbKeterangan;
    }
}