using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDUcp1
{
    public partial class Maintenance : Form
    {
        private string connectionString = "Data Source=LAPTOP-DBS9EP5T\\RAEHANARJUN;Initial Catalog=RentalKamera;Integrated Security=True";

        public Maintenance()
        {
            InitializeComponent();
            dgvMaintenance.CellClick += dgvMaintenance_CellClick;
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
                dgvMaintenance.DataSource = dt;
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKeterangan.Text) || string.IsNullOrWhiteSpace(txtIDKamera.Text) || string.IsNullOrWhiteSpace(txtIDTeknisi.Text))
            {
                MessageBox.Show("Semua kolom (ID Kamera, ID Teknisi, Keterangan) harus diisi.");
                return;
            }

            DateTime tanggal = dtpMaintenance.Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Riwayat_Maintainence (ID_Kamera, ID_Teknisi, Tanggal_Maintenance, Keterangan) VALUES (@ID_Kamera, @ID_Teknisi, @Tanggal_Maintenance, @Keterangan)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID_Kamera", txtIDKamera.Text);
                    cmd.Parameters.AddWithValue("@ID_Teknisi", txtIDTeknisi.Text);
                    cmd.Parameters.AddWithValue("@Tanggal_Maintenance", tanggal);
                    cmd.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data berhasil ditambahkan!");
                        RefreshDataGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKeterangan.Text) || string.IsNullOrWhiteSpace(txtIDKamera.Text) || string.IsNullOrWhiteSpace(txtIDTeknisi.Text))
            {
                MessageBox.Show("Semua kolom (ID Kamera, ID Teknisi, Keterangan) harus diisi.");
                return;
            }

            if (dgvMaintenance.CurrentRow != null && dgvMaintenance.CurrentRow.Cells["ID_Riwayat"].Value != null && int.TryParse(dgvMaintenance.CurrentRow.Cells["ID_Riwayat"].Value.ToString(), out int idRiwayat))
            {
                DateTime tanggal = dtpMaintenance.Value;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Riwayat_Maintainence SET ID_Kamera=@ID_Kamera, ID_Teknisi=@ID_Teknisi, Tanggal_Maintenance=@Tanggal_Maintenance, Keterangan=@Keterangan WHERE ID_Riwayat=@ID_Riwayat";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_Riwayat", idRiwayat);
                        cmd.Parameters.AddWithValue("@ID_Kamera", txtIDKamera.Text);
                        cmd.Parameters.AddWithValue("@ID_Teknisi", txtIDTeknisi.Text);
                        cmd.Parameters.AddWithValue("@Tanggal_Maintenance", tanggal);
                        cmd.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);

                        try
                        {
                            conn.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Data berhasil diupdate.");
                                RefreshDataGrid();
                            }
                            else
                            {
                                MessageBox.Show("Gagal mengupdate data. Pastikan ID Riwayat sesuai dengan data yang ingin diubah.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih data yang ingin diupdate.");
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKeterangan.Text) || string.IsNullOrWhiteSpace(txtIDKamera.Text) || string.IsNullOrWhiteSpace(txtIDTeknisi.Text))
            {
                MessageBox.Show("Semua kolom (ID Kamera, ID Teknisi, Keterangan) harus diisi.");
                return;
            }

            if (dgvMaintenance.CurrentRow != null && dgvMaintenance.CurrentRow.Cells["ID_Riwayat"].Value != null && int.TryParse(dgvMaintenance.CurrentRow.Cells["ID_Riwayat"].Value.ToString(), out int idRiwayat))
            {
                DialogResult result = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Riwayat_Maintainence WHERE ID_Riwayat=@ID_Riwayat";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ID_Riwayat", idRiwayat);
                            //Anda masih bisa menambahkan parameter lain jika diperlukan untuk logging atau validasi
                             cmd.Parameters.AddWithValue("@ID_Kamera", txtIDKamera.Text);
                            cmd.Parameters.AddWithValue("@ID_Teknisi", txtIDTeknisi.Text);
                            cmd.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);

                            try
                            {
                                conn.Open();
                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Data berhasil dihapus.");
                                    RefreshDataGrid();
                                }
                                else
                                {
                                    MessageBox.Show("Gagal menghapus data. Pastikan ID Riwayat sesuai dengan data yang ingin dihapus.");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih data yang ingin dihapus.");
            }
        }

        private void dgvMaintenance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMaintenance.CurrentRow != null)
            {
                DataGridViewRow row = dgvMaintenance.CurrentRow;

                // Menetapkan nilai ke kontrol di form berdasarkan kolom yang dipilih
                txtKeterangan.Text = row.Cells["Keterangan"].Value?.ToString();

                // Mengonversi nilai tanggal menjadi DateTime dan menetapkannya ke DateTimePicker
                if (DateTime.TryParse(row.Cells["Tanggal_Maintenance"].Value?.ToString(), out DateTime tanggal))
                {
                    dtpMaintenance.Value = tanggal;
                }
                else
                {
                    MessageBox.Show("Format tanggal tidak valid.");
                }

                // Menetapkan nilai untuk kontrol lainnya
                txtIDKamera.Text = row.Cells["ID_Kamera"].Value?.ToString();
                txtIDTeknisi.Text = row.Cells["ID_Teknisi"].Value?.ToString();
            }
            // Jika tidak ada baris yang dipilih (dgvMaintenance.CurrentRow == null),
            // tidak ada tindakan yang akan diambil. Anda mungkin ingin menambahkan
            // logika lain di sini jika diperlukan, misalnya membersihkan kontrol input.
        }

        private void dgvMaintenance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvMaintenance.Rows[e.RowIndex];
                string cellValue = row.Cells[e.ColumnIndex].Value?.ToString();
                MessageBox.Show("Anda klik: " + cellValue);
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void txtIDKamera_TextChanged(object sender, EventArgs e) { }
    }
}