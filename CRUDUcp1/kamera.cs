using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Windows.Forms;

namespace CRUDUcp1
{
    public partial class kamera : Form
    {
        private string connectionString = "Data Source=LAPTOP-DBS9EP5T\\RAEHANARJUN;Initial Catalog=RentalKamera;Integrated Security=True";

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        private const string CacheKey = "KameraData";

        public kamera()
        {
            InitializeComponent();
        }

        private void kamera_Load(object sender, EventArgs e)
        {
            EnsureIndexes();
            LoadData();
        }

        private void EnsureIndexes()
        {
            string indexScript = @"
            IF OBJECT_ID('dbo.Kamera','U') IS NOT NULL
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Kamera_Merk')
                    CREATE NONCLUSTERED INDEX IX_Kamera_Merk ON Kamera(Merk_Kamera);
                
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Kamera_Model')
                    CREATE NONCLUSTERED INDEX IX_Kamera_Model ON Kamera(Model);
                
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Kamera_Status')
                    CREATE NONCLUSTERED INDEX IX_Kamera_Status ON Kamera(Status);
                
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Kamera_Lokasi')
                    CREATE NONCLUSTERED INDEX IX_Kamera_Lokasi ON Kamera(Lokasi);
            END";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(indexScript, conn))
            {
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating indexes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void LoadData()
        {
            DataTable dt;
            if (_cache.Contains(CacheKey))
            {
                dt = _cache.Get(CacheKey) as DataTable;
                lblMessages.Text = "Data loaded from cache";
            }
            else
            {
                var stopwatch = Stopwatch.StartNew();

                dt = new DataTable();
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT ID_Kamera, Merk_Kamera, Model, Status, Lokasi FROM Kamera", conn))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }

                _cache.Add(CacheKey, dt, _policy);
                stopwatch.Stop();
                lblMessages.Text = $"Data loaded from database in {stopwatch.ElapsedMilliseconds} ms";
            }

            dgvKamera.AutoGenerateColumns = true;
            dgvKamera.DataSource = dt;
        }

        private void AnalyzeQuery(string sqlQuery)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.InfoMessage += (s, e) => MessageBox.Show(e.Message, "STATISTICS INFO");
                conn.Open();
                var wrapped = $@"
                SET STATISTICS IO ON;
                SET STATISTICS TIME ON;
                {sqlQuery};
                SET STATISTICS IO OFF;
                SET STATISTICS TIME OFF;";

                using (var cmd = new SqlCommand(wrapped, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }



        private void btnTambah_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Anda yakin ingin menambahkan data ini?",
                "Konfirmasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        using (SqlCommand command = new SqlCommand("sp_InsertKamera", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@Merk_Kamera", txtMerkKamera.Text);
                            command.Parameters.AddWithValue("@Model", txtModel.Text);
                            command.Parameters.AddWithValue("@Status", cmbStatus.Text);
                            command.Parameters.AddWithValue("@Lokasi", txtLokasi.Text);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Data kamera berhasil ditambahkan.");
                        RefreshDataGrid();
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Gagal menambahkan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }


        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (dgvKamera.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show(
                    "Apakah Anda yakin ingin mengupdate data ini?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvKamera.CurrentRow.Cells["ID_Kamera"].Value);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlTransaction transaction = connection.BeginTransaction();

                        try
                        {
                            using (SqlCommand command = new SqlCommand("sp_UpdateKamera", connection, transaction))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.AddWithValue("@ID_Kamera", id);
                                command.Parameters.AddWithValue("@Merk_Kamera", txtMerkKamera.Text);
                                command.Parameters.AddWithValue("@Model", txtModel.Text);
                                command.Parameters.AddWithValue("@Status", cmbStatus.Text);
                                command.Parameters.AddWithValue("@Lokasi", txtLokasi.Text);

                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("Data kamera berhasil diperbarui.");
                            RefreshDataGrid();
                            ClearFields();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Gagal memperbarui data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Update dibatalkan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
                        connection.Open();
                        SqlTransaction transaction = connection.BeginTransaction();

                        try
                        {
                            using (SqlCommand command = new SqlCommand("sp_DeleteKamera", connection, transaction))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@ID_Kamera", id);

                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("Data kamera berhasil dihapus.");
                            RefreshDataGrid();
                            ClearFields();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Gagal menghapus data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
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

                txtMerkKamera.Text = row.Cells["Merk_Kamera"].Value?.ToString() ?? "";
                txtModel.Text = row.Cells["Model"].Value?.ToString() ?? "";
                cmbStatus.Text = row.Cells["Status"].Value?.ToString() ?? "";
                txtLokasi.Text = row.Cells["Lokasi"].Value?.ToString() ?? "";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // kosongkan saja atau isi sesuai kebutuhan
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); // Tutup form Teknisi
        }

        private void lblMessages_Click(object sender, EventArgs e)
        {

        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            AnalyzeQuery("SELECT * FROM Kamera WHERE Merk_Kamera LIKE '%Canon%'");
        }
    }
}
