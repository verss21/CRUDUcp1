namespace CRUDUcp1
{
    partial class menu
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
            this.btnTeknisi = new System.Windows.Forms.Button();
            this.btnKamera = new System.Windows.Forms.Button();
            this.btnRiwayat = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnLogout = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTeknisi
            // 
            this.btnTeknisi.Location = new System.Drawing.Point(83, 89);
            this.btnTeknisi.Name = "btnTeknisi";
            this.btnTeknisi.Size = new System.Drawing.Size(157, 77);
            this.btnTeknisi.TabIndex = 0;
            this.btnTeknisi.Text = "Teknisi";
            this.btnTeknisi.UseVisualStyleBackColor = true;
            this.btnTeknisi.Click += new System.EventHandler(this.btnTeknisi_Click);
            // 
            // btnKamera
            // 
            this.btnKamera.Location = new System.Drawing.Point(83, 188);
            this.btnKamera.Name = "btnKamera";
            this.btnKamera.Size = new System.Drawing.Size(157, 63);
            this.btnKamera.TabIndex = 1;
            this.btnKamera.Text = "Kamera";
            this.btnKamera.UseVisualStyleBackColor = true;
            this.btnKamera.Click += new System.EventHandler(this.btnKamera_Click);
            // 
            // btnRiwayat
            // 
            this.btnRiwayat.Location = new System.Drawing.Point(83, 282);
            this.btnRiwayat.Name = "btnRiwayat";
            this.btnRiwayat.Size = new System.Drawing.Size(157, 76);
            this.btnRiwayat.TabIndex = 2;
            this.btnRiwayat.Text = "Riwayat Maintainence";
            this.btnRiwayat.UseVisualStyleBackColor = true;
            this.btnRiwayat.Click += new System.EventHandler(this.btnRiwayat_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CRUDUcp1.Properties.Resources.vector;
            this.pictureBox1.Location = new System.Drawing.Point(467, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(381, 376);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(298, 373);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(88, 34);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.button1_Click);
            // 
            // menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(996, 450);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnRiwayat);
            this.Controls.Add(this.btnKamera);
            this.Controls.Add(this.btnTeknisi);
            this.Name = "menu";
            this.Text = "menu";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTeknisi;
        private System.Windows.Forms.Button btnKamera;
        private System.Windows.Forms.Button btnRiwayat;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnLogout;
    }
}