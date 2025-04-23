using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDUcp1
{
    public partial class Mengelola_Data_Transaksi : Form
    {
        private string connectionString = "Data Source=LAPTOP-DBS9EP5T\\RAEHANARJUN;Initial Catalog=RentalKamera;Integrated Security=True";
        public Mengelola_Data_Transaksi()
        {
            InitializeComponent();
        }

        private void Mengelola_Data_Transaksi_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Riwayat_Maintainence WHERE ID_Riwayat = @ID_Riwayat";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Riwayat", txtIDRiwayat.Text);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Riwayat_Maintainence";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Riwayat_Maintainence (ID_Kamera, ID_Teknisi, Tanggal_Maintenance, Keterangan) " +
                               "VALUES (@ID_Kamera, @ID_Teknisi, @Tanggal_Maintenance, @Keterangan)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Kamera", txtIDKamera.Text);
                    command.Parameters.AddWithValue("@ID_Teknisi", txtIDTeknisi.Text);
                    command.Parameters.AddWithValue("@Tanggal_Maintenance", dtpTanggal.Value);
                    command.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            MessageBox.Show("Data transaksi berhasil ditambahkan.");
            RefreshDataGrid();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Riwayat_Maintainence " +
                               "SET ID_Kamera = @ID_Kamera, ID_Teknisi = @ID_Teknisi, Tanggal_Maintainence = @Tanggal_Maintenance, Keterangan = @Keterangan " +
                               "WHERE ID_Riwayat = @ID_Riwayat";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Riwayat", txtIDRiwayat.Text);
                    command.Parameters.AddWithValue("@ID_Kamera", txtIDKamera.Text);
                    command.Parameters.AddWithValue("@ID_Teknisi", txtIDTeknisi.Text);
                    command.Parameters.AddWithValue("@Tanggal_Maintenance", dtpTanggal.Value);
                    command.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            MessageBox.Show("Data transaksi berhasil diperbarui.");
            RefreshDataGrid();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtIDRiwayat.Clear();
            txtIDKamera.Clear();
            txtIDTeknisi.Clear();
            txtKeterangan.Clear();
            dtpTanggal.Value = DateTime.Now;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtIDKamera_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

