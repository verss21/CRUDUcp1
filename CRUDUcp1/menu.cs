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
            Teknisi t = new Teknisi();
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
    }
}
