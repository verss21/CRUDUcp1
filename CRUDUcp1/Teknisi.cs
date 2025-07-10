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
using System.Diagnostics;
using System.Runtime.Caching;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;


namespace CRUDUcp1
{
    public partial class Teknisi : Form
    {
        koneksi kn = new koneksi();
        string strKonek = "";

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        private const string CacheKey = "TeknisiData";

        public Teknisi()
        {
            InitializeComponent();
          
        }

        private void FormTeknisi_Load(object sender, EventArgs e)
        {
            EnsureIndexes();
            LoadData();
            LoadTeknisi();
        }

        private void EnsureIndexes()
        {
            string indexScript = @"
IF OBJECT_ID('dbo.Teknisi', 'U') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Teknisi_Nama')
        CREATE NONCLUSTERED INDEX IX_Teknisi_Nama ON dbo.Teknisi(Nama_Teknisi);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Email')
        CREATE NONCLUSTERED INDEX IX_Email ON dbo.Teknisi(Email);

END";

            using (var conn = new SqlConnection(kn.connectionString()))
            using (var cmd = new SqlCommand(indexScript, conn))
            {
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("SQL Error saat membuat indeks: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error umum saat membuat indeks: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                using (var conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT ID_Teknisi, Nama_Teknisi, No_Telepon, Email FROM Teknisi", conn))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }

                _cache.Add(CacheKey, dt, _policy);
                stopwatch.Stop();
                lblMessages.Text = $"Data loaded from database in {stopwatch.ElapsedMilliseconds} ms";
            }

            DataGridViewTeknisi.AutoGenerateColumns = true;
            DataGridViewTeknisi.DataSource = dt;
        }

        private void AnalyzeQuery(string sqlQuery)
        {
            using (var conn = new SqlConnection(kn.connectionString()))
            {
                conn.InfoMessage += (s, e) =>
                {
                    // Bisa dikumpulkan semua pesan ke satu string jika banyak info
                    MessageBox.Show(e.Message, "STATISTICS INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                conn.Open();

                // Gunakan semicolon jika belum pasti query-nya menutup dengan titik koma
                var wrappedQuery = $@"
            SET STATISTICS IO ON;
            SET STATISTICS TIME ON;
            {sqlQuery};
            SET STATISTICS TIME OFF;
            SET STATISTICS IO OFF;
        ";

                using (var cmd = new SqlCommand(wrappedQuery, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"SQL Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadTeknisi()
        {
            using (SqlConnection conn = new SqlConnection(kn.connectionString()))
            {
                string query = "SELECT ID_Teknisi, Nama_Teknisi, No_Telepon, Email FROM Teknisi";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataGridViewTeknisi.DataSource = dt;
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // Validasi input tidak boleh kosong
            if (string.IsNullOrWhiteSpace(txtNamaTeknisi.Text) ||
                string.IsNullOrWhiteSpace(txtNoTelp.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Kolom tidak boleh kosong.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!txtNamaTeknisi.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                MessageBox.Show("Nama teknisi hanya boleh berisi huruf dan spasi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi nomor telepon hanya angka
            if (!txtNoTelp.Text.All(char.IsDigit))
            {
                MessageBox.Show("Nomor telepon hanya boleh berisi angka.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi nomor telepon harus dimulai dari 08
            if (!txtNoTelp.Text.StartsWith("08"))
            {
                MessageBox.Show("Nomor telepon harus dimulai dari '08'.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi panjang nomor telepon
            if (txtNoTelp.Text.Length < 11 || txtNoTelp.Text.Length > 13)
            {
                MessageBox.Show("Panjang nomor telepon harus antara 11 hingga 13 digit.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi email harus mengandung '@' dan diakhiri '.com'
            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.EndsWith(".com"))
            {
                MessageBox.Show("Format email tidak valid. Email harus mengandung '@' dan diakhiri dengan '.com'.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Konfirmasi penambahan data
            DialogResult result = MessageBox.Show(
                "Anda yakin ingin menambahkan data ini?",
                "Konfirmasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(kn.connectionString()))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Cek duplikasi No_Telepon atau Email
                        string cekQuery = "SELECT COUNT(*) FROM Teknisi WHERE No_Telepon = @NoTelp OR Email = @Email";
                        using (SqlCommand cekCommand = new SqlCommand(cekQuery, connection, transaction))
                        {
                            cekCommand.Parameters.AddWithValue("@NoTelp", txtNoTelp.Text.Trim());
                            cekCommand.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                            int count = (int)cekCommand.ExecuteScalar();
                            if (count > 0)
                            {
                                MessageBox.Show("Nomor telepon atau email sudah digunakan.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                transaction.Rollback();
                                return;
                            }
                        }

                        // Insert data
                        using (SqlCommand command = new SqlCommand("sp_InsertTeknisi", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Nama_Teknisi", txtNamaTeknisi.Text.Trim());
                            command.Parameters.AddWithValue("@No_Telepon", txtNoTelp.Text.Trim());
                            command.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        ClearFields();
                        MessageBox.Show("Data teknisi berhasil ditambahkan.");
                        RefreshDataGrid();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Gagal menambahkan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Penambahan data dibatalkan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnHapus_Click(object sender, EventArgs e)
        {
            // Validasi: Tidak ada baris yang benar-benar dipilih
            if (DataGridViewTeknisi.SelectedRows.Count == 0 || DataGridViewTeknisi.CurrentRow == null)
            {
                MessageBox.Show("Silakan pilih data teknisi yang ingin dihapus dari tabel.", "Tidak Ada Data Terpilih", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = DataGridViewTeknisi.CurrentRow;

            // Validasi apakah kolom yang dibutuhkan kosong
            if (selectedRow.Cells["ID_Teknisi"].Value == null ||
                string.IsNullOrWhiteSpace(selectedRow.Cells["ID_Teknisi"].Value.ToString()) ||
                selectedRow.Cells["Nama_Teknisi"].Value == null ||
                string.IsNullOrWhiteSpace(selectedRow.Cells["Nama_Teknisi"].Value.ToString()))
            {
                MessageBox.Show("Data teknisi tidak lengkap atau ada kolom kosong. Tidak dapat menghapus.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(selectedRow.Cells["ID_Teknisi"].Value);
            string nama = selectedRow.Cells["Nama_Teknisi"].Value.ToString();

            var confirm = MessageBox.Show($"Yakin ingin menghapus teknisi '{nama}' dengan ID {id}?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(kn.connectionString()))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        using (SqlCommand command = new SqlCommand("sp_DeleteTeknisi", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ID_Teknisi", id);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                MessageBox.Show("Data teknisi berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                RefreshDataGrid();
                            }
                            else
                            {
                                transaction.Rollback();
                                MessageBox.Show("Data teknisi tidak ditemukan.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        ClearFields();
                        connection.Close();
                    }
                }
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
            // Validasi baris benar-benar dipilih
            if (DataGridViewTeknisi.SelectedRows.Count == 0 || DataGridViewTeknisi.CurrentRow == null)
            {
                MessageBox.Show("Silakan pilih baris teknisi yang ingin diperbarui terlebih dahulu.", "Tidak Ada Data Terpilih", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(DataGridViewTeknisi.CurrentRow.Cells["ID_Teknisi"].Value);
            string namaTeknisi = txtNamaTeknisi.Text.Trim();
            string noTelp = txtNoTelp.Text.Trim();
            string email = txtEmail.Text.Trim();

            // Ambil nilai lama dari baris yang dipilih
            string oldNama = DataGridViewTeknisi.CurrentRow.Cells["Nama_Teknisi"].Value != DBNull.Value
                ? DataGridViewTeknisi.CurrentRow.Cells["Nama_Teknisi"].Value.ToString().Trim()
                : "";

            string oldNoTelp = DataGridViewTeknisi.CurrentRow.Cells["No_Telepon"].Value != DBNull.Value
                ? DataGridViewTeknisi.CurrentRow.Cells["No_Telepon"].Value.ToString().Trim()
                : "";

            string oldEmail = DataGridViewTeknisi.CurrentRow.Cells["Email"].Value != DBNull.Value
                ? DataGridViewTeknisi.CurrentRow.Cells["Email"].Value.ToString().Trim()
                : "";

            // Cek apakah data berubah
            if (namaTeknisi == oldNama && noTelp == oldNoTelp && email == oldEmail)
            {
                MessageBox.Show("Data harus diubah terlebih dahulu sebelum diperbarui.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi tidak boleh kosong
            if (string.IsNullOrWhiteSpace(namaTeknisi) ||
                string.IsNullOrWhiteSpace(noTelp) ||
                string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Semua field wajib diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi nama hanya huruf dan spasi
            if (!namaTeknisi.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                MessageBox.Show("Nama teknisi hanya boleh berisi huruf dan spasi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi nomor telepon hanya angka
            if (!noTelp.All(char.IsDigit))
            {
                MessageBox.Show("Nomor telepon hanya boleh berisi angka.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!noTelp.StartsWith("08"))
            {
                MessageBox.Show("Nomor telepon harus dimulai dari '08'.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (noTelp.Length < 11 || noTelp.Length > 13)
            {
                MessageBox.Show("Panjang nomor telepon harus antara 11 hingga 13 digit.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!email.Contains("@") || !email.EndsWith(".com"))
            {
                MessageBox.Show("Email harus mengandung '@' dan diakhiri dengan '.com'.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Yakin ingin mengupdate data teknisi ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(kn.connectionString()))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Cek duplikat nomor atau email
                        string cekQuery = "SELECT COUNT(*) FROM Teknisi WHERE (No_Telepon = @NoTelp OR Email = @Email) AND ID_Teknisi != @ID";
                        using (SqlCommand cekCommand = new SqlCommand(cekQuery, connection, transaction))
                        {
                            cekCommand.Parameters.AddWithValue("@NoTelp", noTelp);
                            cekCommand.Parameters.AddWithValue("@Email", email);
                            cekCommand.Parameters.AddWithValue("@ID", id);

                            int count = (int)cekCommand.ExecuteScalar();
                            if (count > 0)
                            {
                                MessageBox.Show("Nomor telepon atau email sudah digunakan oleh teknisi lain.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                transaction.Rollback();
                                return;
                            }
                        }

                        // Jalankan stored procedure
                        using (SqlCommand command = new SqlCommand("sp_UpdateTeknisi", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ID_Teknisi", id);
                            command.Parameters.AddWithValue("@Nama_Teknisi", namaTeknisi);
                            command.Parameters.AddWithValue("@No_Telepon", noTelp);
                            command.Parameters.AddWithValue("@Email", email);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Data teknisi berhasil diperbarui.");
                        RefreshDataGrid();
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Terjadi kesalahan saat update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
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
            using (SqlConnection connection = new SqlConnection(kn.connectionString()))
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // Tutup form Teknisi
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            // Query by teknisi name
            AnalyzeQuery("SELECT * FROM Teknisi WHERE Nama_Teknisi LIKE '%Budi%'");

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
        



