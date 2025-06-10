using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Windows.Forms;

namespace CRUDUcp1
{
    public partial class Maintenance : Form
    {
        private string connectionString = "Data Source=LAPTOP-DBS9EP5T\\RAEHANARJUN;Initial Catalog=RentalKamera;Integrated Security=True";

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        private const string CacheKey = "MaintenanceData";

        public Maintenance()
        {
            InitializeComponent();
            dgvMaintenance.CellClick += dgvMaintenance_CellClick;

            // Initial loads on form creation
            EnsureIndexes(); // Pastikan indeks dibuat
            LoadKamera();    // Muat data kamera
            LoadTeknisi();   // Muat data teknisi
            LoadData();      // Muat data awal dengan caching dan timing
        }

        private void LoadKamera()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ID_Kamera, Merk_Kamera + ' ' + Model AS NamaKamera FROM Kamera";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Tambahkan baris default "Pilih Kamera"
                    DataRow defaultRow = dt.NewRow();
                    defaultRow["ID_Kamera"] = DBNull.Value; // Using DBNull.Value for consistency with your previous code and safer for non-integer IDs
                    defaultRow["NamaKamera"] = "Pilih Kamera"; // The display text
                    dt.Rows.InsertAt(defaultRow, 0); // Insert at the beginning

                    cmbKamera.DataSource = dt;
                    cmbKamera.DisplayMember = "NamaKamera";
                    cmbKamera.ValueMember = "ID_Kamera";
                    cmbKamera.SelectedIndex = 0; // Select the default item
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data kamera: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadTeknisi()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Pastikan koneksi dibuka sebelum digunakan.
                    string query = "SELECT ID_Teknisi, Nama_Teknisi FROM Teknisi";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Tambahkan baris default "Pilih Teknisi"
                    DataRow defaultRow = dt.NewRow();
                    defaultRow["ID_Teknisi"] = DBNull.Value; // Atau nilai yang tidak valid, misal -1
                    defaultRow["Nama_Teknisi"] = "Pilih Teknisi";
                    dt.Rows.InsertAt(defaultRow, 0); // Masukkan di awal DataTable

                    cmbTeknisi.DataSource = dt;
                    cmbTeknisi.DisplayMember = "Nama_Teknisi"; // Teks yang ditampilkan
                    cmbTeknisi.ValueMember = "ID_Teknisi";     // Value used
                    cmbTeknisi.SelectedIndex = 0;              // Set default item terpilih ke "Pilih Teknisi"
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data teknisi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            DataTable dt;
            if (_cache.Contains(CacheKey))
            {
                dt = _cache.Get(CacheKey) as DataTable;
                lblMessages.Text = "Data dimuat dari cache.";
            }
            else
            {
                var stopwatch = Stopwatch.StartNew();

                dt = new DataTable();
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        // Lebih baik melakukan JOIN untuk menampilkan nama kamera dan teknisi langsung
                        string query = @"
                            SELECT
                                RM.ID_Riwayat,
                                K.Merk_Kamera + ' ' + K.Model AS NamaKamera,
                                T.Nama_Teknisi,
                                RM.Tanggal_Maintenance,
                                RM.Keterangan,
                                RM.ID_Kamera, -- Tetap sertakan ID untuk pembaruan/penghapusan
                                RM.ID_Teknisi  -- Tetap sertakan ID untuk pembaruan/penghapusan
                            FROM Riwayat_Maintainence RM
                            JOIN Kamera K ON RM.ID_Kamera = K.ID_Kamera
                            JOIN Teknisi T ON RM.ID_Teknisi = T.ID_Teknisi";

                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        adapter.Fill(dt);
                    }

                    _cache.Add(CacheKey, dt, _policy);
                    stopwatch.Stop();
                    lblMessages.Text = $"Data dimuat dari database dalam {stopwatch.ElapsedMilliseconds} ms.";
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    MessageBox.Show("Gagal memuat data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblMessages.Text = "Gagal memuat data.";
                }
            }
            dgvMaintenance.DataSource = dt;
            // Sembunyikan kolom ID jika tidak perlu ditampilkan secara langsung di DGV
            if (dgvMaintenance.Columns.Contains("ID_Kamera"))
                dgvMaintenance.Columns["ID_Kamera"].Visible = false;
            if (dgvMaintenance.Columns.Contains("ID_Teknisi"))
                dgvMaintenance.Columns["ID_Teknisi"].Visible = false;
        }

        private void LoadMaintenanceWithTiming()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Gunakan query dengan JOIN untuk tampilan yang lebih informatif
                    string query = @"
                        SELECT
                            RM.ID_Riwayat,
                            K.Merk_Kamera + ' ' + K.Model AS NamaKamera,
                            T.Nama_Teknisi,
                            RM.Tanggal_Maintenance,
                            RM.Keterangan,
                            RM.ID_Kamera,
                            RM.ID_Teknisi
                        FROM Riwayat_Maintainence RM
                        JOIN Kamera K ON RM.ID_Kamera = K.ID_Kamera
                        JOIN Teknisi T ON RM.ID_Teknisi = T.ID_Teknisi";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvMaintenance.DataSource = dt;

                    // Sembunyikan kolom ID jika tidak perlu ditampilkan secara langsung di DGV
                    if (dgvMaintenance.Columns.Contains("ID_Kamera"))
                        dgvMaintenance.Columns["ID_Kamera"].Visible = false;
                    if (dgvMaintenance.Columns.Contains("ID_Teknisi"))
                        dgvMaintenance.Columns["ID_Teknisi"].Visible = false;
                }

                stopwatch.Stop();
                lblMessages.Text = $"Waktu eksekusi: {stopwatch.ElapsedMilliseconds} ms";
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                MessageBox.Show("Gagal memuat data: " + ex.Message);
                lblMessages.Text = "Gagal menghitung waktu.";
            }
        }


        private void EnsureIndexes()
        {
            string script = @"
            IF NOT EXISTS (
                SELECT name FROM sys.indexes 
                WHERE name = 'IX_Riwayat_ID_Kamera'
                  AND object_id = OBJECT_ID('Riwayat_Maintainence')
            )
            BEGIN
                CREATE NONCLUSTERED INDEX IX_Riwayat_ID_Kamera
                ON Riwayat_Maintainence (ID_Kamera);
            END

            IF NOT EXISTS (
                SELECT name FROM sys.indexes 
                WHERE name = 'IX_Riwayat_ID_Teknisi'
                  AND object_id = OBJECT_ID('Riwayat_Maintainence')
            )
            BEGIN
                CREATE NONCLUSTERED INDEX IX_Riwayat_ID_Teknisi
                ON Riwayat_Maintainence (ID_Teknisi);
            END

            IF NOT EXISTS (
                SELECT name FROM sys.indexes 
                WHERE name = 'IX_Riwayat_Tanggal_Maintenance'
                  AND object_id = OBJECT_ID('Riwayat_Maintainence')
            )
            BEGIN
                CREATE NONCLUSTERED INDEX IX_Riwayat_Tanggal_Maintenance
                ON Riwayat_Maintainence (Tanggal_Maintenance);
            END";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(script, conn))
            {
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMessages.Text = "Indeks berhasil diperiksa/dibuat.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal membuat indeks: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RefreshDataGrid()
        {
            // Saat refresh, kita ingin memuat ulang dari database dan membersihkan cache
            _cache.Remove(CacheKey);
            LoadData();

            // Pastikan kolom-kolom ComboBox kembali ke default setelah refresh,
            // atau jika ada data yang dipilih, set ulang berdasarkan data yang dipilih.
            cmbKamera.SelectedIndex = 0;
            cmbTeknisi.SelectedIndex = 0;
            txtKeterangan.Text = string.Empty; // Kosongkan keterangan setelah operasi CRUD
            dtpMaintenance.Value = DateTime.Now; // Atur tanggal kembali ke tanggal saat ini atau tanggal default yang diinginkan
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // Periksa jika "Pilih Kamera" atau "Pilih Teknisi" yang terpilih
            if (cmbKamera.SelectedValue == DBNull.Value || cmbTeknisi.SelectedValue == DBNull.Value || string.IsNullOrWhiteSpace(txtKeterangan.Text))
            {
                MessageBox.Show("Semua kolom (Kamera, Teknisi, Keterangan) harus diisi dengan pilihan yang valid.", "Input Tidak Lengkap", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tanggal = dtpMaintenance.Value;

            DialogResult result = MessageBox.Show(
                "Apakah Anda ingin menambahkan data ini?",
                "Konfirmasi Penambahan", // Ubah judul konfirmasi
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertRiwayatMaintainence", conn, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ID_Kamera", cmbKamera.SelectedValue);
                        cmd.Parameters.AddWithValue("@ID_Teknisi", cmbTeknisi.SelectedValue);
                        cmd.Parameters.AddWithValue("@Tanggal_Maintenance", tanggal);
                        cmd.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Data berhasil ditambahkan!");
                    RefreshDataGrid(); // Panggil LoadData() lagi (dengan cache invalidation)
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Terjadi kesalahan saat menambahkan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Tambahkan ikon error
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Periksa jika "Pilih Kamera" atau "Pilih Teknisi" yang terpilih
            if (cmbKamera.SelectedValue == DBNull.Value || cmbTeknisi.SelectedValue == DBNull.Value || string.IsNullOrWhiteSpace(txtKeterangan.Text))
            {
                MessageBox.Show("Semua kolom (Kamera, Teknisi, Keterangan) harus diisi dengan pilihan yang valid.", "Input Tidak Lengkap", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvMaintenance.CurrentRow != null &&
                dgvMaintenance.CurrentRow.Cells["ID_Riwayat"].Value != null &&
                int.TryParse(dgvMaintenance.CurrentRow.Cells["ID_Riwayat"].Value.ToString(), out int idRiwayat))
            {
                DateTime tanggal = dtpMaintenance.Value;

                DialogResult result = MessageBox.Show(
                    "Apakah Anda yakin ingin mengupdate data ini?",
                    "Konfirmasi Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("sp_UpdateRiwayatMaintainence", conn, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ID_Riwayat", idRiwayat);
                        cmd.Parameters.AddWithValue("@ID_Kamera", cmbKamera.SelectedValue);
                        cmd.Parameters.AddWithValue("@ID_Teknisi", cmbTeknisi.SelectedValue);
                        cmd.Parameters.AddWithValue("@Tanggal_Maintenance", tanggal);
                        cmd.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);

                        // Gunakan SqlParameter untuk ReturnValue
                        SqlParameter returnValue = cmd.Parameters.Add("@RowCount", SqlDbType.Int); // Nama parameter yang lebih deskriptif
                        returnValue.Direction = ParameterDirection.ReturnValue;

                        try
                        {
                            cmd.ExecuteNonQuery();
                            int rowCount = (int)returnValue.Value; // Ambil nilai dari parameter ReturnValue

                            if (rowCount > 0)
                            {
                                transaction.Commit();
                                MessageBox.Show("Data berhasil diupdate.");
                                RefreshDataGrid();
                            }
                            else
                            {
                                transaction.Rollback();
                                MessageBox.Show("Gagal mengupdate data. Data tidak ditemukan.", "Update Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("SQL Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih data yang ingin diupdate.", "Pilih Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvMaintenance.CurrentRow != null &&
           dgvMaintenance.CurrentRow.Cells["ID_Riwayat"].Value != null &&
           int.TryParse(dgvMaintenance.CurrentRow.Cells["ID_Riwayat"].Value.ToString(), out int idRiwayat))
            {
                DialogResult result = MessageBox.Show(
                    "Yakin ingin menghapus data ini?",
                    "Konfirmasi Penghapusan", // Ubah judul konfirmasi
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlTransaction transaction = conn.BeginTransaction();

                        using (SqlCommand cmd = new SqlCommand("sp_DeleteRiwayatMaintainence", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Parameter input
                            cmd.Parameters.AddWithValue("@ID_Riwayat", idRiwayat);

                            // Parameter output
                            SqlParameter outputParam = new SqlParameter("@RowsAffected", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(outputParam);

                            try
                            {
                                cmd.ExecuteNonQuery();
                                int rowsAffected = (int)outputParam.Value;

                                if (rowsAffected > 0)
                                {
                                    transaction.Commit();
                                    MessageBox.Show("Data berhasil dihapus.");
                                    RefreshDataGrid();
                                }
                                else
                                {
                                    transaction.Rollback();
                                    MessageBox.Show("Gagal menghapus data. Data tidak ditemukan.", "Penghapusan Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih data yang ingin dihapus.", "Pilih Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void dgvMaintenance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Pastikan klik pada baris dan kolom yang valid
            {
                DataGridViewRow row = dgvMaintenance.Rows[e.RowIndex];

                // Isi textbox keterangan
                txtKeterangan.Text = row.Cells["Keterangan"].Value?.ToString();

                // Isi tanggal maintenance
                if (DateTime.TryParse(row.Cells["Tanggal_Maintenance"].Value?.ToString(), out DateTime tanggal))
                {
                    dtpMaintenance.Value = tanggal;
                }
                else
                {
                    MessageBox.Show("Format tanggal tidak valid pada baris yang dipilih.");
                    // Opsional: reset dtpMaintenance.Value ke tanggal default atau kosongkan jika tidak valid
                }

                // Ambil nilai ID dari DataGrid, lalu set SelectedValue ComboBox berdasarkan ID
                // Penting: Pastikan kolom ID_Kamera dan ID_Teknisi masih tersedia (walaupun mungkin tidak terlihat)
                // di DataTable yang diikat ke DataGridView.
                object idKameraValue = row.Cells["ID_Kamera"].Value;
                object idTeknisiValue = row.Cells["ID_Teknisi"].Value;

                if (idKameraValue != DBNull.Value && idKameraValue != null)
                {
                    cmbKamera.SelectedValue = idKameraValue;
                }
                else
                {
                    cmbKamera.SelectedIndex = 0; // Pilih item default jika ID_Kamera null/DBNull
                }

                if (idTeknisiValue != DBNull.Value && idTeknisiValue != null)
                {
                    cmbTeknisi.SelectedValue = idTeknisiValue;
                }
                else
                {
                    cmbTeknisi.SelectedIndex = 0; // Pilih item default jika ID_Teknisi null/DBNull
                }
            }
        }

        private void dgvMaintenance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Event ini biasanya tidak digunakan untuk mengisi kontrol, CellClick lebih umum.
            // Biarkan kosong atau hapus jika tidak ada fungsionalitas spesifik yang diperlukan.
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            // Gunakan query yang sama dengan LoadData() untuk analisis yang relevan
            AnalyzeQuery(@"
                SELECT
                    RM.ID_Riwayat,
                    K.Merk_Kamera + ' ' + K.Model AS NamaKamera,
                    T.Nama_Teknisi,
                    RM.Tanggal_Maintenance,
                    RM.Keterangan
                FROM Riwayat_Maintainence RM
                JOIN Kamera K ON RM.ID_Kamera = K.ID_Kamera
                JOIN Teknisi T ON RM.ID_Teknisi = T.ID_Teknisi");
        }

        private void AnalyzeQuery(string sqlQuery)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                // Event InfoMessage menangani pesan dari SET STATISTICS IO/TIME
                conn.InfoMessage += (s, e) => MessageBox.Show(e.Message, "INFORMASI STATISTIK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Open();
                var wrapped = $@"
                SET STATISTICS IO ON;
                SET STATISTICS TIME ON;
                {sqlQuery};
                -- Pastikan untuk mematikan statistik setelah query dieksekusi
                SET STATISTICS IO OFF;
                SET STATISTICS TIME OFF;";

                using (var cmd = new SqlCommand(wrapped, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery(); // Gunakan ExecuteNonQuery karena kita hanya tertarik pada pesan statistik, bukan data hasil
                        // Data hasil query bisa di-load ke DGV jika diinginkan, tapi untuk analisis saja, ExecuteNonQuery cukup.
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error selama analisis query: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void btnLaporan_Click(object sender, EventArgs e)
        {
            // Membuka form laporan (Report)
            Report laporanForm = new Report();
            laporanForm.ShowDialog(); // Tampilkan sebagai form modal
        }

        // Event handler yang tidak digunakan, dapat dihapus jika tidak ada fungsionalitas
        private void label1_Click(object sender, EventArgs e) { }
        private void txtIDKamera_TextChanged(object sender, EventArgs e) { }
    }
}