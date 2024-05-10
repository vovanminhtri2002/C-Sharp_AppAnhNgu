using _BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Do_An_Chuyen_Nganh
{
    public partial class fQuanLyKhoaHoc : Form
    {
        private XyLyKhoaHoc khoaHocProcessor = new XyLyKhoaHoc();
        public fQuanLyKhoaHoc()
        {
            InitializeComponent();
        }

        private void AutoSizeColumns()
        {
            foreach (DataGridViewColumn column in dataKH.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        private void LoadData()
        {
            dataKH.DataSource = khoaHocProcessor.GetKhoaHoc();
            AutoSizeColumns();
            XulyCotTiengViet();
        }
        private void XulyCotTiengViet()
        {
            // Đổi tên cột để phản ánh thông tin đúng với khóa học
            dataKH.Columns["MaKhoaHoc"].HeaderText = "Mã Khóa Học";
            dataKH.Columns["TenKhoaHoc"].HeaderText = "Tên Khóa Học";
            dataKH.Columns["MoTa"].HeaderText = "Mô Tả";
            dataKH.Columns["HocPhi"].HeaderText = "Học Phí";
            dataKH.Columns["ThoiGianBatDau"].HeaderText = "Thời Gian Bắt Đầu";
            dataKH.Columns["ThoiGianKetThuc"].HeaderText = "Thời Gian Kết Thúc";
            dataKH.Columns["SoLuongHocVien"].HeaderText = "Số Lượng Học Viên";
            dataKH.Columns["DiaDiemHoc"].HeaderText = "Địa Điểm Học";
            dataKH.Columns["TrangThai"].HeaderText = "Trạng Thái";
        }

        private void fQuanLyKhoaHoc_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataKH_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataKH.CurrentRow;
            if (selectedRow != null)
            {
                string maKhoaHoc = selectedRow.Cells["MaKhoaHoc"].Value.ToString();
                string tenKhoaHoc = selectedRow.Cells["TenKhoaHoc"].Value.ToString();
                string moTa = selectedRow.Cells["MoTa"].Value.ToString();
                string hocPhi = selectedRow.Cells["HocPhi"].Value.ToString();
                DateTime thoiGianBatDau = Convert.ToDateTime(selectedRow.Cells["ThoiGianBatDau"].Value);
                DateTime thoiGianKetThuc = Convert.ToDateTime(selectedRow.Cells["ThoiGianKetThuc"].Value);
                string soLuongHocVien = selectedRow.Cells["SoLuongHocVien"].Value.ToString();
                string diaDiemHoc = selectedRow.Cells["DiaDiemHoc"].Value.ToString();
                string trangThai = selectedRow.Cells["TrangThai"].Value.ToString();

                // Thay thế các tên các trường tương ứng
                txtMakh.Text = maKhoaHoc;
                txtTen.Text = tenKhoaHoc;
                txtMoTa.Text = moTa;
                txtHocphi.Text = hocPhi;
                databd.Value = thoiGianBatDau;
                datekt.Value = thoiGianKetThuc;
                txtSl.Text = soLuongHocVien;
                txtDiaDiem.Text = diaDiemHoc;
                txtTt.Text = trangThai;
            }
        }
        private Random random = new Random();
        private string SinhMaKhoaHoc()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomPart = new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return "KH" + randomPart;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            KhoaHoc khoaHoc = new KhoaHoc
            {
                MaKhoaHoc = SinhMaKhoaHoc(),
                TenKhoaHoc = txtTen.Text,
                MoTa = txtMoTa.Text,
                HocPhi = float.Parse(txtHocphi.Text),
                ThoiGianBatDau = databd.Value,
                ThoiGianKetThuc = datekt.Value,
                SoLuongHocVien = int.Parse(txtSl.Text),
                DiaDiemHoc = txtDiaDiem.Text,
                TrangThai = txtTt.Text
            };

            khoaHocProcessor.ThemKhoaHoc(khoaHoc);
            LoadData();
            ClearInputFields();
        }
        private void ClearInputFields()
        {
            txtMakh.Clear();
            txtMakh.Clear();
            txtMoTa.Clear();
            txtHocphi.Clear();
            databd.Value = DateTime.Now;
            datekt.Value = DateTime.Now;
            txtSl.Clear();
            txtDiaDiem.Clear();
            txtTt.Clear();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataKH.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataKH.SelectedRows[0];
                string maKhoaHoc = selectedRow.Cells["MaKhoaHoc"].Value.ToString();

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khóa học có mã " + maKhoaHoc + " không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    khoaHocProcessor.XoaKhoaHoc(maKhoaHoc);
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khóa học để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataKH.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataKH.SelectedRows[0].Index;
                DataGridViewRow selectedRow = dataKH.Rows[selectedRowIndex];
                string maKhoaHoc = selectedRow.Cells["MaKhoaHoc"].Value.ToString();
                KhoaHoc existingKhoaHoc = new KhoaHoc
                {
                    MaKhoaHoc = maKhoaHoc,
                    TenKhoaHoc = txtTen.Text,
                    MoTa = txtMoTa.Text,
                    HocPhi = float.Parse(txtHocphi.Text),
                    ThoiGianBatDau = databd.Value,
                    ThoiGianKetThuc = datekt.Value,
                    SoLuongHocVien = int.Parse(txtSl.Text),
                    DiaDiemHoc = txtDiaDiem.Text,
                    TrangThai = txtTt.Text
                };
                khoaHocProcessor.SuaKhoaHoc(existingKhoaHoc);
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khóa học để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTK.Text.Trim();
            List<KhoaHoc> ketQuaTimKiem = khoaHocProcessor.TimKiemKhoaHoc(tuKhoa);
            dataKH.DataSource = ketQuaTimKiem;
        }
    }
}
