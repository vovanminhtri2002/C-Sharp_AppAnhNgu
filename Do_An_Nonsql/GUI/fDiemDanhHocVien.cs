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
    public partial class fDiemDanhHocVien : Form
    {
        private XuLyDiemDanhHocVien xuLyDiemDanhHocVien = new XuLyDiemDanhHocVien();
        private AnhNguDataContext DiemDanhContext = new AnhNguDataContext();
        private string maLopHoc;
        private DateTime ngayHoc;
        private string maphong;
        private int thu;
        private int tietBatDau;
        private int tietKetThuc;
        private string caHoc;
        private List<XuLyDiemDanhHocVien.ThongTinHocVienDiemDanh> danhSachHocVien;

        public fDiemDanhHocVien(string maLopHoc, DateTime ngayHoc, string maPhong, int thu, int tietBatDau, int tietKetThuc, string cahoc, List<XuLyDiemDanhHocVien.ThongTinHocVienDiemDanh> danhSachHocVien)
        {
            InitializeComponent();
            this.maLopHoc = maLopHoc;
            this.ngayHoc = ngayHoc;
            this.maphong = maPhong;
            this.thu = thu;
            this.tietBatDau = tietBatDau;
            this.tietKetThuc = tietKetThuc;
            this.caHoc = cahoc;
            this.danhSachHocVien = danhSachHocVien;
            LoadDanhSachHocVien();
        }
        private void LoadDanhSachHocVien()
        {

            var danhSachHocVienLocal = xuLyDiemDanhHocVien.LayDanhSachHocVienTheoThoiKhoaBieu(maLopHoc, maphong, thu, tietBatDau, tietKetThuc, caHoc, ngayHoc);

            // Hiển thị danh sách học viên trên DataGridView
            dataDiemDanh.DataSource = danhSachHocVienLocal;

            // Tự động tạo checkbox column cho trạng thái điểm danh
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "Đi học";
            checkBoxColumn.Name = "colDiHoc";
            dataDiemDanh.Columns.Add(checkBoxColumn);

            // Xử lý sự kiện trước khi cột được vẽ
            dataDiemDanh.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dataDiemDanh.Columns["colDiHoc"].Index)
                {
                    var trangThaiDiemDanh = dataDiemDanh.Rows[e.RowIndex].Cells["CoDiHoc"].Value.ToString();

                 
                    e.Value = trangThaiDiemDanh == "Vắng";

                    // Vô hiệu hóa ô Checkbox nếu trạng thái là "Vắng"
                    if (trangThaiDiemDanh == "Đã điểm danh")
                    {
                        dataDiemDanh.Rows[e.RowIndex].Cells["colDiHoc"].ReadOnly = true;
                        dataDiemDanh.Rows[e.RowIndex].Cells["colDiHoc"].ToolTipText = "Học viên vắng";
                    }
                }
            };

            dataDiemDanh.CellContentClick += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dataDiemDanh.Columns["colDiHoc"].Index)
                {
                    // Nếu ô Checkbox được click
                    if (dataDiemDanh.Rows[e.RowIndex].Cells["colDiHoc"] is DataGridViewCheckBoxCell cell)
                    {
                        // Kiểm tra trạng thái của ô Checkbox
                        if (cell.Value == null || !(bool)cell.Value)
                        {
                            // Nếu ô Checkbox chưa được check, chuyển trạng thái thành "Vắng"
                            dataDiemDanh.Rows[e.RowIndex].Cells["CoDiHoc"].Value = "Đã điểm danh";
                            cell.Value = true; // Cập nhật giá trị của ô Checkbox
                        }
                        else
                        {
                            // Nếu ô Checkbox đã được check, chuyển trạng thái thành "Đã điểm danh"
                            dataDiemDanh.Rows[e.RowIndex].Cells["CoDiHoc"].Value = "Vắng";
                            cell.Value = false; // Cập nhật giá trị của ô Checkbox
                        }
                    }
                }
            };
        }


        private void btnLuudiemdanh_Click(object sender, EventArgs e)
        {

            try
            {
                string trangThaiDiemDanh;
                string maHocVien;

                // Lấy danh sách học viên đã được chọn để điểm danh
                List<string> maHocViens = new List<string>();
                foreach (DataGridViewRow row in dataDiemDanh.Rows)
                {
                    // Kiểm tra nếu dòng không phải là dòng header và có đối tượng cell
                    if (row.Cells["colDiHoc"] is DataGridViewCheckBoxCell cell && cell.Value != null)
                    {
                        // Kiểm tra giá trị không phải là null trước khi thêm vào danh sách
                        if (row.Cells["MaHocVien"].Value != null)
                        {
                            maHocVien = row.Cells["MaHocVien"].Value.ToString();
                            maHocViens.Add(maHocVien);
                        }
                    }
                }
                foreach (DataGridViewRow row in dataDiemDanh.Rows)
                {
                    if (row.Cells["colDiHoc"] is DataGridViewCheckBoxCell cell && cell.Value != null)
                    {
                         maHocVien = row.Cells["MaHocVien"].Value.ToString();
                        bool isChecked = (bool)cell.Value;

                        // Lấy buổi học hiện tại
                        DateTime ngayDiemDanh = ngayHoc;

                        // Chuyển giá trị bool thành string
                         trangThaiDiemDanh = isChecked ? "Đã điểm danh" : "Vắng";

                        // Cập nhật giá trị trong cơ sở dữ liệu
                        xuLyDiemDanhHocVien.CapNhatTrangThaiDiemDanh(maHocVien, maLopHoc, ngayDiemDanh, trangThaiDiemDanh);

                        // Cập nhật giá trị trạng thái trên DataGridView
                        row.Cells["CoDiHoc"].Value = trangThaiDiemDanh;
                    }
                }

                // Hiển thị thông báo sau khi lưu điểm danh
                MessageBox.Show("Đã điểm danh thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

