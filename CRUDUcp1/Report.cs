using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDUcp1
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            SetupReportViewer(); // panggil method utama
        }

        private void SetupReportViewer()
        {
            string connectionString = "Data Source=LAPTOP-DBS9EP5T\\RAEHANARJUN;Initial Catalog=RentalKamera;Integrated Security=True";

            string query = @"
                SELECT 
                    rm.ID_Riwayat,
                    k.Merk_Kamera,
                    k.Model,
                    k.Status,
                    t.Nama_Teknisi,
                    t.No_Telepon,
                    t.Email,
                    rm.Tanggal_Maintenance,
                    rm.Keterangan
                FROM 
                    Riwayat_Maintainence rm
                JOIN 
                    Kamera k ON rm.ID_Kamera = k.ID_Kamera
                JOIN 
                    Teknisi t ON rm.ID_Teknisi = t.ID_Teknisi
                ORDER BY 
                    rm.Tanggal_Maintenance DESC";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dt); // pastikan sama dengan nama dataset di file .rdlc

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.LocalReport.ReportPath = "D:\\Pengembangan Aplikasi Basis Data\\CRUDUcp1\\CRUDUcp1\\RiwayatMaintenanceReport.rdlc"; // file harus ada di root proyek atau sesuaikan path

            reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            // kosongkan, tidak perlu isi apapun di sini
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // tutup form report dan kembali ke form sebelumnya
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
