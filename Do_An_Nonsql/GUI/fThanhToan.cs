using _BLL;
using DevExpress.ClipboardSource.SpreadsheetML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VietQRPaymentAPI;
using static Do_An_Chuyen_Nganh.fDangNhap;


namespace Do_An_Chuyen_Nganh
{
    public partial class fThanhToan : Form
    {
        private string mapt;
        private AnhNguDataContext PhieuThuContext = new AnhNguDataContext();
        private XyLyPhieuThu xuLyPhieuThu = new XyLyPhieuThu();
        private Random random = new Random();
        public fThanhToan(string maPT)
        {
            InitializeComponent();
            this.mapt = maPT;
            txtMaPhieuThu.Text = maPT;
        }
        public string HocVien
        {
            get { return txtMaHocVien.Text; }
            set { txtMaHocVien.Text = value; }
        }


        public string TrangThai
        {
            get { return txtTrangThai.Text; }
            set { txtTrangThai.Text = value; }
        }

        public string TongTien
        {
            get { return txtTongTien.Text; }
            set { txtTongTien.Text = value; }
        }
        private void fThanhToan_Load(object sender, EventArgs e)
        {
            LoadDataThanhToan();
        }
    
       

    private void XulyCotTiengViet()
        {
            dataThanhToan.Columns["mapheiu"].HeaderText = "Mã Phiếu Thu";
            dataThanhToan.Columns["tenhocvine"].HeaderText = "Mã Học Viên";
            dataThanhToan.Columns["ngaylap"].HeaderText = "Ngày Lập";
            dataThanhToan.Columns["tongtien"].HeaderText = "Tổng Tiền";
            dataThanhToan.Columns["tennv"].HeaderText = "Mã Nhân Viên";
            dataThanhToan.Columns["trangthai"].HeaderText = "Trạng Thái";
        }

        private void AutoSizeColumns()
        {
            foreach (DataGridViewColumn column in dataThanhToan.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        private void LoadDataThanhToan()
        {
            dataThanhToan.DataSource = xuLyPhieuThu.Getphieuthu();
          //  dataThanhToan.Columns["HocVien"].Visible = false;
          //  dataThanhToan.Columns["NhanVien"].Visible = false;
            AutoSizeColumns();
            XulyCotTiengViet();
        }

        private void btnThanhToanQR_Click(object sender, EventArgs e)
        {
            fThanhToanQR a = new fThanhToanQR();
            a.StartPosition = FormStartPosition.CenterScreen;
            a.ShowDialog(); 
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTuKhoa.Text.Trim();
            List<PhieuThu> ketQuaTimKiem = xuLyPhieuThu.TimKiemPhieuThu(tuKhoa);
            dataThanhToan.DataSource = ketQuaTimKiem;
        }


        private void dataThanhToan_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataThanhToan.CurrentRow; // Lấy dòng đầu tiên được chọn
            {
                if (selectedRow != null)
                {
                    // Lấy dữ liệu từ các cột của dòng được chọn
                    string maPhieuThu = selectedRow.Cells["mapheiu"].Value?.ToString();
                    string maHocVien = selectedRow.Cells["tenhocvine"].Value?.ToString();
                    string ngayLapStr = selectedRow.Cells["ngaylap"].Value?.ToString();
                    string tongTien = selectedRow.Cells["tongtien"].Value?.ToString();
                    string maNhanVien = selectedRow.Cells["tennv"].Value?.ToString();
                    string trangThai = selectedRow.Cells["trangthai"].Value?.ToString();
                    try
                    {
                        dateNgayLap.Value = DateTime.Parse(ngayLapStr);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Ngày lập không hợp lệ.");
                    }
                    txtMaPhieuThu.Text = maPhieuThu;
                    txtMaHocVien.Text = maHocVien;
                    txtTongTien.Text = tongTien;
                    txtMaNhanVien.Text = maNhanVien;
                    txtTrangThai.Text = trangThai;
                }
            }
        }

        private void btnInPhieuThu_Click(object sender, EventArgs e)
        {
            string maPhieuThu = txtMaPhieuThu.Text;

            if (!string.IsNullOrEmpty(maPhieuThu))
            {
                xuLyPhieuThu.CapNhatTrangThaiDaThanhToan(maPhieuThu);
                MessageBox.Show("Thanh toán thành công!");
                LoadDataThanhToan();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã phiếu thu để cập nhật trạng thái.");
            }
        }

        private void txtMaPhieuThu_TextChanged(object sender, EventArgs e)
        {
            string maPhieuThu = txtMaPhieuThu.Text;
            string tenKhoaHoc = xuLyPhieuThu.TenKhoaHocDaDangKy(maPhieuThu);
            txtTenKhoaHocDaDangKy.Multiline = true;
            txtTenKhoaHocDaDangKy.ScrollBars = ScrollBars.Vertical; // Hoặc ScrollBars.Both
            txtTenKhoaHocDaDangKy.Text = tenKhoaHoc;
        }
    }
}
