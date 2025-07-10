using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDUcp1
{
    public partial class menu : Form
    {
        public menu()
        {
            InitializeComponent();
        }

        private void btnRiwayat_Click(object sender, EventArgs e)
        {
            Maintenance m = new Maintenance(); // Use 'm' as the variable name for the new Maintenance instance
            m.Show(); // Show the Maintenance form
        }


        private void btnTeknisi_Click(object sender, EventArgs e)
        {
            this.Hide(); // Sembunyikan menu
            Teknisi t = new Teknisi();

            // Saat form Teknisi ditutup, tampilkan kembali menu
            t.FormClosed += (s, args) => this.Show();

            t.Show();
        }

        private void btnKamera_Click(object sender, EventArgs e)
        {
            kamera k = new kamera();
            k.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda yakin ingin logout?", "Konfirmasi Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide(); // Sembunyikan form menu
                Login login = new Login(); // Buat instance baru form login
                login.Show(); // Tampilkan form login
            }
            // Jika pilih "No", tidak melakukan apa-apa
        }

        private void menu_Load(object sender, EventArgs e)
        {

        }
    }
}
