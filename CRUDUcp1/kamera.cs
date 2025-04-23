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
    public partial class kamera : Form
    {
        private string connectionString = "Data Source=LAPTOP-DBS9EP5T\\RAEHANARJUN;Initial Catalog=RentalKamera;Integrated Security=True";

        public kamera()
        {
            InitializeComponent();
        }

        private void kamera_Load(object sender, EventArgs e)
        {
            RefreshDataGrid();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Kamera (Merk_Kamera, Model, Status, Lokasi) " +
                               "VALUES (@Merk_Kamera, @Model, @Status, @Lokasi)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Merk_Kamera", txtMerkKamera.Text);
                    command.Parameters.AddWithValue("@Model", txtModel.Text);
                    command.Parameters.AddWithValue("@Status", cmbStatus.Text);
                    command.Parameters.AddWithValue("@Lokasi", txtLokasi.Text);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            MessageBox.Show("Data kamera berhasil ditambahkan.");
            RefreshDataGrid();
            ClearFields();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvKamera.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvKamera.CurrentRow.Cells["ID_Kamera"].Value);


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Kamera SET Merk_Kamera = @Merk_Kamera, Model = @Model, Status = @Status, Lokasi = @Lokasi " +
                                   "WHERE ID_Kamera = @ID_Kamera";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID_Kamera", id);
                        command.Parameters.AddWithValue("@Merk_Kamera", txtMerkKamera.Text);
                        command.Parameters.AddWithValue("@Model", txtModel.Text);
                        command.Parameters.AddWithValue("@Status", cmbStatus.Text);
                        command.Parameters.AddWithValue("@Lokasi", txtLokasi.Text);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                MessageBox.Show("Data kamera berhasil diperbarui.");
                RefreshDataGrid();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Silakan pilih baris yang ingin diperbarui terlebih dahulu.");
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (dgvKamera.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvKamera.CurrentRow.Cells["ID_Kamera"].Value);
                string merkKamera = dgvKamera.CurrentRow.Cells["Merk_Kamera"].Value?.ToString();
                string model = dgvKamera.CurrentRow.Cells["Model"].Value?.ToString();
                string status = dgvKamera.CurrentRow.Cells["Status"].Value?.ToString();
                string lokasi = dgvKamera.CurrentRow.Cells["Lokasi"].Value?.ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Kamera SET Merk_Kamera = @Merk_Kamera, Model = @Model, Status = @Status, Lokasi = @Lokasi " +
                                   "WHERE ID_Kamera = @ID_Kamera";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID_Kamera", id);
                        command.Parameters.AddWithValue("@Merk_Kamera", txtMerkKamera.Text);
                        command.Parameters.AddWithValue("@Model", txtModel.Text);
                        command.Parameters.AddWithValue("@Status", cmbStatus.Text);
                        command.Parameters.AddWithValue("@Lokasi", txtLokasi.Text);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                MessageBox.Show("Data kamera berhasil diperbarui.");
                RefreshDataGrid();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Silakan pilih baris yang ingin diperbarui terlebih dahulu.");
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvKamera.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvKamera.CurrentRow.Cells["ID_Kamera"].Value);

                var confirm = MessageBox.Show("Yakin ingin menghapus data kamera ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Kamera WHERE ID_Kamera = @ID_Kamera";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ID_Kamera", id);

                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }

                    MessageBox.Show("Data kamera berhasil dihapus.");
                    RefreshDataGrid();
                    ClearFields();
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih baris yang ingin dihapus terlebih dahulu.");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtMerkKamera.Clear();
            txtModel.Clear();
            cmbStatus.SelectedIndex = -1;
            txtLokasi.Clear();
        }

        private void RefreshDataGrid()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Kamera";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvKamera.DataSource = dt;
            }
        }

        private void dataGridViewKamera_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKamera.Rows[e.RowIndex];

                txtMerkKamera.Text = row.Cells["Merk_Kamera"].Value.ToString();
                txtModel.Text = row.Cells["Model"].Value.ToString();
                cmbStatus.Text = row.Cells["Status"].Value.ToString();
                txtLokasi.Text = row.Cells["Lokasi"].Value.ToString();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // kosongkan saja atau isi sesuai kebutuhan
        }
    }
}
