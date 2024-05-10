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

namespace Do_An_Chuyen_Nganh.GUI
{
    public partial class fSapXepLopHoc : Form
    {
        private XyLyQuanLyLopHocVien xyLyQuanLyLopHocVien = new XyLyQuanLyLopHocVien();
        private XuLyXepLop xuLyXepLop = new XuLyXepLop();
        private Random random = new Random();
        public fSapXepLopHoc()
        {
            InitializeComponent();
        }
        private void LoadDataQL()
        {
            dataQuanLyLopHocVien.DataSource = xyLyQuanLyLopHocVien.GetDanhSachQuanLyLopHocVien();
            dataQuanLyLopHocVien.Columns["HocVien"].Visible = false;
            dataQuanLyLopHocVien.Columns["LopHoc"].Visible = false;
            comboMaLop.DataSource = xyLyQuanLyLopHocVien.GetMaLop();
            comHocvien.DataSource = xyLyQuanLyLopHocVien.GetMaHocVien();
        }
        private string SinhMaQL()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomPart = new string(Enumerable.Repeat(chars, 2)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return "QLLH" + randomPart;
        }
        private void ClearInputFieldsQL()
        {
            txtID.Clear();
            comboMaLop.SelectedIndex = -1;
            comHocvien.SelectedIndex = -1;
            txtDiem.Clear();
            dateNgayNhan.Value = DateTime.Now;
            txtSoluong.Clear();
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            string[] MaLop = comboMaLop.Text.Split('-');
            string malop = MaLop[0].Trim();
            string[] Mahv = comHocvien.Text.Split('-');
            string mahv = Mahv[0].Trim();

            float diem;
            if (float.TryParse(txtDiem.Text, out diem))
            {
                // Chuyển đổi thành công, sử dụng giá trị diem.
            }
            else
            {
                // Không thể chuyển đổi thành công, có thể là giá trị null hoặc giá trị mặc định khác.
                diem = 0; // Hoặc giá trị mặc định khác tùy thuộc vào yêu cầu của bạn.
            }

            DateTime ngayNhanTaiLieu = dateNgayNhan.Value;

            int soLuongTaiLieuDaNhan;
            if (int.TryParse(txtSoluong.Text, out soLuongTaiLieuDaNhan))
            {
                // Chuyển đổi thành công, sử dụng giá trị soLuongTaiLieuDaNhan.
            }
            else
            {
                // Không thể chuyển đổi thành công, có thể là giá trị null hoặc giá trị mặc định khác.
                soLuongTaiLieuDaNhan = 0; // Hoặc giá trị mặc định khác tùy thuộc vào yêu cầu của bạn.
            }

            XepLopHocVien XepLopHocVien = new XepLopHocVien
            {
                IDQuanLy = SinhMaQL(),
                MaLopHoc = malop,
                MaHocVien = mahv,
               
            };

            xyLyQuanLyLopHocVien.ThemQuanLyLopHocVien(XepLopHocVien);
            LoadDataQL();
            ClearInputFieldsQL();
        }
        private void LoadComboBoxLopHoc(string maHocVien)
        {
            comboMaLop.DataSource = null;
            comboMaLop.Items.Clear();

            XyLyQuanLyLopHocVien quanLyLopHocVienBLL = new XyLyQuanLyLopHocVien();

            List<LopHoc> danhSachLopHoc = quanLyLopHocVienBLL.LayDanhSachLopHocDaDangKy(maHocVien);

            // Thêm items mới
            foreach (LopHoc lopHoc in danhSachLopHoc)
            {
                comboMaLop.Items.Add($"{lopHoc.MaLopHoc} - {lopHoc.TenLop}");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataQuanLyLopHocVien.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataQuanLyLopHocVien.SelectedRows[0].Index;
                DataGridViewRow selectedRow = dataQuanLyLopHocVien.Rows[selectedRowIndex];
                string idQuanLy = selectedRow.Cells["IDQuanLy"].Value.ToString();

                string[] MaLop = comboMaLop.Text.Split('-');
                string malop = MaLop[0].Trim();
                string[] Mahv = comHocvien.Text.Split('-');
                string mahv = Mahv[0].Trim();
                XepLopHocVien existingQuanLy = new XepLopHocVien
                {
                    IDQuanLy = idQuanLy,
                    MaLopHoc = malop,
                    MaHocVien = mahv,
                  
                };

                xuLyXepLop.SuaQuanLyLopHocVien(existingQuanLy);
                LoadDataQL();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một quản lý để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataQuanLyLopHocVien.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataQuanLyLopHocVien.SelectedRows[0];
                string idQuanLy = selectedRow.Cells["IDQuanLy"].Value.ToString();

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa quản lý lớp học viên có ID " + idQuanLy + " không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    xuLyXepLop.XoaQuanLyLopHocVien(idQuanLy);
                    LoadDataQL();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một quản lý lớp học viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTkHV.Text.Trim();
            List<XepLopHocVien> ketQuaTimKiem = xyLyQuanLyLopHocVien.TimKiemQuanLyLopHocVien(tuKhoa);
            dataQuanLyLopHocVien.DataSource = ketQuaTimKiem;
        }

        private void comHocvien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comHocvien.SelectedItem != null)
            {
                string maHocVien = comHocvien.SelectedItem.ToString().Split('-')[0].Trim();
                LoadComboBoxLopHoc(maHocVien);
            }
            else
            {
                // Xử lý khi comHocvien.SelectedItem là null.
            }
        }

        private void dataQuanLyLopHocVien_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataQuanLyLopHocVien.CurrentRow;
            if (selectedRow != null)
            {
                string idQuanLy = selectedRow.Cells["IDQuanLy"].Value?.ToString();
                string maLopHoc = selectedRow.Cells["MaLopHoc"].Value?.ToString();
                string maHocVien = selectedRow.Cells["MaHocVien"].Value?.ToString();
                string diem = selectedRow.Cells["Diem"].Value?.ToString();
                string ngayNhanTaiLieu = selectedRow.Cells["NgayNhanTaiLieu"].Value?.ToString();
                string soLuongTaiLieuDaNhan = selectedRow.Cells["SoLuongTaiLieuDaNhan"].Value?.ToString();

                txtID.Text = idQuanLy;
                comboMaLop.Text = maLopHoc;
                comHocvien.Text = maHocVien;
                txtDiem.Text = diem;
                dateNgayNhan.Value = DateTime.Parse(ngayNhanTaiLieu);
                txtSoluong.Text = soLuongTaiLieuDaNhan;
            }
        }
        private void XulyCotTiengVietQL()
        {
            dataQuanLyLopHocVien.Columns["IDQuanLy"].HeaderText = "ID Quản Lý";
            dataQuanLyLopHocVien.Columns["MaLopHoc"].HeaderText = "Mã Lớp Học";
            dataQuanLyLopHocVien.Columns["MaHocVien"].HeaderText = "Mã Học Viên";
            dataQuanLyLopHocVien.Columns["Diem"].HeaderText = "Điểm";
            dataQuanLyLopHocVien.Columns["NgayNhanTaiLieu"].HeaderText = "Ngày Nhận Tài Liệu";
            dataQuanLyLopHocVien.Columns["SoLuongTaiLieuDaNhan"].HeaderText = "Số Lượng Tài Liệu Đã Nhận";
        }

        private void AutoSizeColumnsQL()
        {
            foreach (DataGridViewColumn column in dataQuanLyLopHocVien.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void fSapXepLopHoc_Load(object sender, EventArgs e)
        {
            LoadDataQL();
            AutoSizeColumnsQL();
            XulyCotTiengVietQL();
        }

        private void btnThemLop_Click(object sender, EventArgs e)
        {
            fQuanLyLop QlLop = new fQuanLyLop();
            QlLop.StartPosition = FormStartPosition.CenterScreen;
            QlLop.ShowDialog();
        }
    }
}
