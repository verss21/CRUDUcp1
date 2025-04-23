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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CRUDUcp1
{
    public partial class Teknisi : Form
    {
        private string connectionString = "Data Source=LAPTOP-DBS9EP5T\\RAEHANARJUN;Initial Catalog=RentalKamera;Integrated Security=True";

        public Teknisi()
        {
            InitializeComponent();
            DataGridViewTeknisi.CellClick += DataGridViewTeknisi_CellClick;
            RefreshDataGrid();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Teknisi (Nama_Teknisi, No_Telepon, Email) " +
                               "VALUES (@Nama_Teknisi, @No_Telepon, @Email)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nama_Teknisi", txtNamaTeknisi.Text);
                    command.Parameters.AddWithValue("@No_Telepon", txtNoTelp.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            ClearFields();
            MessageBox.Show("Data transaksi berhasil ditambahkan.");
            RefreshDataGrid();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (DataGridViewTeknisi.CurrentRow != null)
            {
                int id = Convert.ToInt32(DataGridViewTeknisi.CurrentRow.Cells["ID_Teknisi"].Value);
                string nama = DataGridViewTeknisi.CurrentRow.Cells["Nama_Teknisi"].Value.ToString();

                var confirm = MessageBox.Show($"Yakin ingin menghapus teknisi '{nama}' dengan ID {id}?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string query = "DELETE FROM Teknisi WHERE ID_Teknisi = @ID_Teknisi";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@ID_Teknisi", id);

                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Data teknisi berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    RefreshDataGrid();
                                }
                                else
                                {
                                    MessageBox.Show("Data teknisi tidak ditemukan.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        ClearFields();
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih baris teknisi yang ingin dihapus.", "Tidak Ada Baris Terpilih", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearFields()
        {
            txtNamaTeknisi.Clear();
            txtNoTelp.Clear();
            txtEmail.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (DataGridViewTeknisi.CurrentRow != null)
            {
                int id = Convert.ToInt32(DataGridViewTeknisi.CurrentRow.Cells["ID_Teknisi"].Value);
                string namaTeknisi = txtNamaTeknisi.Text;
                string noTelp = txtNoTelp.Text;
                string email = txtEmail.Text;

                var confirm = MessageBox.Show("Yakin ingin mengupdate data teknisi ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "UPDATE Teknisi SET Nama_Teknisi = @Nama_Teknisi, No_Telepon = @No_Telepon, Email = @Email WHERE ID_Teknisi = @ID_Teknisi";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ID_Teknisi", id);
                            command.Parameters.AddWithValue("@Nama_Teknisi", namaTeknisi);
                            command.Parameters.AddWithValue("@No_Telepon", noTelp);
                            command.Parameters.AddWithValue("@Email", email);

                            try
                            {
                                connection.Open();
                                command.ExecuteNonQuery();
                                MessageBox.Show("Data teknisi berhasil diperbarui.");
                                RefreshDataGrid();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                            ClearFields();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih baris teknisi yang ingin diperbarui terlebih dahulu.");
            }
        }



        private void btnClear_Clic(object sender, EventArgs e)
        {
            txtNamaTeknisi.Clear();
            txtNoTelp.Clear();
            txtEmail.Clear();
        }

        private void RefreshDataGrid()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Teknisi";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataGridViewTeknisi.DataSource = dt;
            }
        }

        private void DataGridViewTeknisi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridViewTeknisi.Rows[e.RowIndex];

                txtNamaTeknisi.Text = row.Cells["Nama_Teknisi"].Value?.ToString();
                txtNoTelp.Text = row.Cells["No_Telepon"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();

            }
        }
    }
}
        



