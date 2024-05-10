using _BLL;
using DevExpress.XtraEditors.Filtering.Templates;
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
    public partial class fQuanLyLichHoc : Form
    {
        private XyLyThoiKhoaBieu thoiKhoaBieuProcessor = new XyLyThoiKhoaBieu();
     

        public fQuanLyLichHoc()
        {
            InitializeComponent();
        }

     
        private void fQuanLyLichHoc_Load(object sender, EventArgs e)
        {
            LoadData();
            AutoSizeColumns();
            XulyCotTiengViet();

        }
        private void XulyCotTiengViet()
        {
            // Xử lý tên cột để phản ánh thông tin đúng với ThoiKhoaBieu
            dataThoiKhoaBieu.Columns["MaThoiKhoaBieu"].HeaderText = "ID Thời Khóa Biểu";
            dataThoiKhoaBieu.Columns["TenGiangVien"].HeaderText = "Tên Giảng Viên";
            dataThoiKhoaBieu.Columns["TenHocVien"].HeaderText = "Tên Học Viên";
            dataThoiKhoaBieu.Columns["TenPhongHoc"].HeaderText = "Tên Phòng Học";
            dataThoiKhoaBieu.Columns["TenLop"].HeaderText = "Tên Lớp Học";
            dataThoiKhoaBieu.Columns["Thu"].HeaderText = "Thứ";
            dataThoiKhoaBieu.Columns["TietBatDau"].HeaderText = "Tiết Bắt Đầu";
            dataThoiKhoaBieu.Columns["TietKetThuc"].HeaderText = "Tiết Kết Thúc";
            dataThoiKhoaBieu.Columns["DiaDiem"].HeaderText = "Địa Điểm";
            dataThoiKhoaBieu.Columns["CaHoc"].HeaderText = "Ca Học";
            dataThoiKhoaBieu.Columns["NgayHoc"].HeaderText = "Ngày Học";
        }
        private void LoadData()
        {
            dataThoiKhoaBieu.DataSource = thoiKhoaBieuProcessor.GetThoiKhoaBieu();
            combogv.DataSource = thoiKhoaBieuProcessor.GetMaGiangVien();
            combohv.DataSource = thoiKhoaBieuProcessor.GetMaHocVien();
            combolop.DataSource = thoiKhoaBieuProcessor.GetMaLop();
            comphong.DataSource = thoiKhoaBieuProcessor.GetMaPhong();
        }

      
        private void btnThemTKB_Click(object sender, EventArgs e)
        {
            string[] magv = combogv.Text.Split('-');
            string Magv = magv[0].Trim();
            string[] mahv = combohv.Text.Split('-');
            string Mahv = mahv[0].Trim();
            string[] maph = comphong.Text.Split('-');
            string Maph = maph[0].Trim();
            string[] malop = combolop.Text.Split('-');
            string Malop = malop[0].Trim();

            ThoiKhoaBieu thoiKhoaBieu = new ThoiKhoaBieu
            {
                IDThoiKhoaBieu = txtIDTKB.Text,
                MaGiangVien = Magv,
                MaHocVien = Mahv,
                MaPhongHoc = Maph,
                MaLopHoc = Malop,
                Thu = Convert.ToInt32(txtThu.Text),
                TietBatDau = Convert.ToInt32(txtTietBD.Text),
                TietKetThuc = Convert.ToInt32(txtTietKT.Text),
                DiaDiem = txtDiaDiem.Text,
                CaHoc = txtcahoc.Text,
                NgayHoc = dateTimePicker1.Value,
            };

                // Nếu không có lịch học nào bị trùng, thêm vào cơ sở dữ liệu
                thoiKhoaBieuProcessor.ThemThoiKhoaBieu(thoiKhoaBieu);
                LoadData();
                ClearInputFieldsThoiKhoaBieu();

                MessageBox.Show("Thêm Thời khóa biểu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
      
        }
        private void ClearInputFieldsThoiKhoaBieu()
        {
            txtIDTKB.Clear();
            // Gán giá trị mặc định cho ComboBoxes
            combogv.SelectedIndex = -1;
            combohv.SelectedIndex = -1;
            comphong.SelectedIndex = -1;
            combolop.SelectedIndex = -1;
            txtThu.Clear();
            txtTietBD.Clear();
            txtTietKT.Clear();
            txtDiaDiem.Clear();
            txtcahoc.Clear();
        }
        private void AutoSizeColumns()
        {
            foreach (DataGridViewColumn column in dataThoiKhoaBieu.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void btnXoaTKB_Click(object sender, EventArgs e)
        {
            if (dataThoiKhoaBieu.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataThoiKhoaBieu.SelectedRows[0];
                string idThoiKhoaBieu = selectedRow.Cells["MaThoiKhoaBieu"].Value.ToString();

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa thời khóa biểu có ID " + idThoiKhoaBieu + " không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    thoiKhoaBieuProcessor.XoaThoiKhoaBieu(idThoiKhoaBieu);
                    dataThoiKhoaBieu.DataSource = thoiKhoaBieuProcessor.GetThoiKhoaBieu();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một thời khóa biểu để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSuaTKB_Click(object sender, EventArgs e)
        {
            if (dataThoiKhoaBieu.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataThoiKhoaBieu.SelectedRows[0];
                string idThoiKhoaBieu = selectedRow.Cells["MaThoiKhoaBieu"].Value.ToString();
                string[] magv = combogv.Text.Split('-');
                string Magv = magv[0].Trim();
                string[] mahv = combohv.Text.Split('-');
                string Mahv = mahv[0].Trim();
                string[] maph = comphong.Text.Split('-');
                string Maph = maph[0].Trim();
                string[] malop = combolop.Text.Split('-');
                string Malop = malop[0].Trim();

                ThoiKhoaBieu thoiKhoaBieu = new ThoiKhoaBieu
                {
                    IDThoiKhoaBieu = txtIDTKB.Text,
                    MaGiangVien = Magv,
                    MaHocVien = Mahv,
                    MaPhongHoc = Maph,
                    MaLopHoc = Malop,
                    Thu = Convert.ToInt32(txtThu.Text),
                    TietBatDau = Convert.ToInt32(txtTietBD.Text),
                    TietKetThuc = Convert.ToInt32(txtTietKT.Text),
                    DiaDiem = txtDiaDiem.Text,
                    CaHoc = txtcahoc.Text,
                    NgayHoc = dateTimePicker1.Value,
                };
                string maGiangVien = Magv;
                string maHocVien = Mahv;

                thoiKhoaBieuProcessor.SuaThoiKhoaBieu(idThoiKhoaBieu, thoiKhoaBieu, maGiangVien, maHocVien);
                LoadData();
                ClearInputFieldsThoiKhoaBieu();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một thời khóa biểu để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnTimTKB_Click(object sender, EventArgs e)
        {
            string keyword = txtTk.Text.Trim();
            List<ThoiKhoaBieu> ketQuaTimKiem = thoiKhoaBieuProcessor.TimKiemThoiKhoaBieu(keyword);
            dataThoiKhoaBieu.DataSource = ketQuaTimKiem;
        }
        private void dataThoiKhoaBieu_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataThoiKhoaBieu.CurrentRow;
            if (selectedRow != null)
            {
                string idThoiKhoaBieu = selectedRow.Cells["MaThoiKhoaBieu"].Value.ToString();
                string maGiangVien = selectedRow.Cells["TenGiangVien"].Value.ToString();
                string maHocVien = selectedRow.Cells["TenHocVien"].Value.ToString();
                string maPhongHoc = selectedRow.Cells["TenPhongHoc"].Value.ToString();
                string maLopHoc = selectedRow.Cells["TenLop"].Value.ToString();
                string thu = selectedRow.Cells["Thu"].Value.ToString();
                string tietBatDau = selectedRow.Cells["TietBatDau"].Value.ToString();
                string tietKetThuc = selectedRow.Cells["TietKetThuc"].Value.ToString();
                string diaDiem = selectedRow.Cells["DiaDiem"].Value.ToString();
                string ca = selectedRow.Cells["CaHoc"].Value.ToString();
                string ngayhoc = selectedRow.Cells["NgayHoc"].Value.ToString();
                // Hiển thị thông tin tương ứng với dòng được chọn
                txtIDTKB.Text = idThoiKhoaBieu;
                combogv.Text = maGiangVien;
                combohv.Text = maHocVien;
                comphong.Text = maPhongHoc;
                combolop.Text = maLopHoc;
                txtThu.Text = thu;
                txtTietBD.Text = tietBatDau;
                txtTietKT.Text = tietKetThuc;
                txtDiaDiem.Text = diaDiem;
                txtcahoc.Text = ca;
                DateTime parsedDate;
                if (DateTime.TryParse(ngayhoc, out parsedDate))
                {
                    dateTimePicker1.Value = parsedDate;
                }
                else
                {
                 
                    MessageBox.Show("Lỗi Format!");
                }
            }
        }
        private void LoadComboBoxLopHoc(string maHocVien)
        {
            combolop.DataSource = null;
            combolop.Items.Clear();
            XyLyThoiKhoaBieu xulymaloptheohocvien = new XyLyThoiKhoaBieu();
            List<LopHoc> danhSachLopHoc = xulymaloptheohocvien.LayDanhSachLopHocDaDangKy(maHocVien);
            foreach (LopHoc lopHoc in danhSachLopHoc)
            {
                combolop.Items.Add($"{lopHoc.MaLopHoc} - {lopHoc.TenLop}");
            }
        }
        private void combohv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combohv.SelectedItem != null)
            {
                string maHocVien = combohv.SelectedItem.ToString().Split('-')[0].Trim();
                LoadComboBoxLopHoc(maHocVien);
            }
            else
            {
                MessageBox.Show("Lỗi Thông tin học viên!");
            }
        }
    }
}
