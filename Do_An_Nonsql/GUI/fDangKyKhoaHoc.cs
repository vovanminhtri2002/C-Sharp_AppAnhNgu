using _BLL;
using Do_An_Chuyen_Nganh.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Do_An_Chuyen_Nganh.fDangNhap;

namespace Do_An_Chuyen_Nganh
{
    public partial class fDangKyKhoaHoc : Form
    {
        private XyLyDangKyKhoaHoc dangKyProcessor = new XyLyDangKyKhoaHoc();
        private bool isLoadingData = false;
        private Random random = new Random();
        private XuLyXepLop xuLyXepLop = new XuLyXepLop();
        private string test;
        public fDangKyKhoaHoc()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            dataDangKy.DataSource = dangKyProcessor.GetDangKyKhoaHoc();
            //  dataDangKy.Columns["HocVien"].Visible = false;
            //  dataDangKy.Columns["KhoaHoc"].Visible = false;
            comboMaHocVien.DataSource = dangKyProcessor.GetMaHocVien();





        }
        public class KhoaHocCheckItem
        {
            public string MaKhoaHoc { get; set; }
            public bool IsChecked { get; set; }
            public decimal DonGia { get; set; }
        }




        private string NhapSoDienThoai()
        {
            string soDienThoai = "";

            using (fKtraDK formNhapSoDienThoai = new fKtraDK())
            {
                formNhapSoDienThoai.StartPosition = FormStartPosition.CenterScreen;

                if (formNhapSoDienThoai.ShowDialog() == DialogResult.OK)
                {
                    soDienThoai = formNhapSoDienThoai.SoDienThoaiNhap;
                }
            }

            return soDienThoai;
        }


        private void AutoSizeColumns()
        {
            foreach (DataGridViewColumn column in dataDangKy.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void ClearInputFields()
        {
            txtma.Clear();
            comboMaHocVien.SelectedIndex = -1;
            comboMaKhoaHoc.SelectedIndex = -1;
            dateNgayDK.Value = DateTime.Now;
            txtTrangThai.Clear();
            check.Checked = false;
            txtHinhthuc.Clear();
        }
        private void XulyCotTiengViet()
        {

            dataDangKy.Columns["MaDangKy"].HeaderText = "Mã Đăng Ký";
            dataDangKy.Columns["TenHocVien"].HeaderText = "Tên Học Viên";
            dataDangKy.Columns["TenKhoaHoc"].HeaderText = "Tên Khóa Học";
            dataDangKy.Columns["NgayDangKy"].HeaderText = "Ngày Đăng Ký";
            dataDangKy.Columns["TrangThai"].HeaderText = "Trạng Thái";
            dataDangKy.Columns["Dathanhtoan"].HeaderText = "Đã Thanh Toán";
            dataDangKy.Columns["HinhThucThanhToan"].HeaderText = "Hình Thức Thanh Toán";
        }
        private void dataDKKH_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataDangKy.CurrentRow;
            if (selectedRow != null)
            {
                string maDangKy = selectedRow.Cells["MaDangKy"].Value.ToString();
                string maHocVien = selectedRow.Cells["TenHocVien"].Value.ToString();
                string maKhoaHoc = selectedRow.Cells["TenKhoaHoc"].Value.ToString();
                DateTime ngayDangKy = Convert.ToDateTime(selectedRow.Cells["NgayDangKy"].Value);
                string trangThai = selectedRow.Cells["TrangThai"].Value.ToString();
                bool daThanhToan = Convert.ToBoolean(selectedRow.Cells["Dathanhtoan"].Value);
                object hinhThucThanhToanObj = selectedRow.Cells["HinhThucThanhToan"].Value;

                string hinhThucThanhToan = hinhThucThanhToanObj != null ? hinhThucThanhToanObj.ToString() : null;

                txtma.Text = maDangKy;
                comboMaHocVien.Text = maHocVien;
                comboMaKhoaHoc.Text = maKhoaHoc;
                dateNgayDK.Value = ngayDangKy;
                txtTrangThai.Text = trangThai;
                check.Checked = daThanhToan;
                txtHinhthuc.Text = hinhThucThanhToan;
            }
        }

        private void fDangKy_Load(object sender, EventArgs e)
        {
            LoadData();
            AutoSizeColumns();
            XulyCotTiengViet();
        }

        private string SinhMaDangKy()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomPart = new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return "DK" + randomPart;
        }
        private string SinhMaPhieuThu()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomPart = new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return "PT" + randomPart;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            string[] mahv = comboMaHocVien.Text.Split('-');
            string mahvpasrt = mahv[0].Trim();

            // Tạo một mã đăng ký mới cho học viên
            string maDangKy = SinhMaDangKy();

            // Tạo một danh sách để lưu các mã khóa học đã được chọn
            List<string> maKhoaHocList = new List<string>();

            // Lặp qua tất cả các mục trong checkedListBox1
            for (int i = 0; i < checkKhoaHoc.Items.Count; i++)
            {
                // Kiểm tra nếu mục thứ i đã được chọn
                if (checkKhoaHoc.GetItemChecked(i))
                {
                    // Kiểm tra kiểu dữ liệu của mục và chuyển đổi thành kiểu phù hợp
                    if (checkKhoaHoc.Items[i] is KhoaHocCheckItem)
                    {
                        KhoaHocCheckItem khoaHocItem = (KhoaHocCheckItem)checkKhoaHoc.Items[i];
                        string[] makh = khoaHocItem.MaKhoaHoc.Split('-');
                        string Makh = makh[0].Trim();
                        // Thêm mã khóa học vào danh sách
                        maKhoaHocList.Add(Makh);

                        // Hiển thị thông báo hoặc thực hiện các thao tác khác nếu cần
                        MessageBox.Show("Đã chọn khóa học: " + Makh, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            // Thêm thông tin đăng ký vào cơ sở dữ liệu cho mỗi khóa học đã chọn
            DangKyKhoaHoc dangKy = new DangKyKhoaHoc
            {
                MaDangKy = maDangKy,
                MaHocVien = mahvpasrt,
                NgayDangKy = dateNgayDK.Value,
                TrangThai = txtTrangThai.Text,
                DaThanhToan = check.Checked,
                HinhThucThanhToan = txtHinhthuc.Text
            };

            dangKyProcessor.ThemDangKyKhoaHoc(dangKy);

            // Thêm thông tin đăng ký vào bảng liên kết DangKyKhoaHoc_KhoaHoc
            foreach (string maKhoaHoc in maKhoaHocList)
            {
                DangKyKhoaHoc_KhoaHoc dk_kh = new DangKyKhoaHoc_KhoaHoc
                {
                    MaDangKy = maDangKy,
                    MaKhoaHoc = maKhoaHoc
                };

                dangKyProcessor.ThemDangKyKhoaHoc_KhoaHoc(dk_kh);
            }
            string manv = UserInfo.MaNhanVien;
            decimal tongTien;
            if (decimal.TryParse(label7.Text, out tongTien))
            {
                double? tongTienDouble = Convert.ToDouble(tongTien);
                test = SinhMaPhieuThu();
                PhieuThu phieuthu = new PhieuThu
                {
                    MaPhieuThu = test,
                    MaHocVien = mahvpasrt,
                    NgayLap = dateNgayDK.Value,
                    TongTien = tongTienDouble,
                    MaNhanVien = manv,
                    TrangThai = "Chưa thanh toán"
                };
                dangKyProcessor.ThemPhieuThu(phieuthu);

            }
            else
            {

                MessageBox.Show("Giá trị đơn giá không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            LoadData();
            ClearInputFields();

            // Hiển thị thông báo thanh toán nếu cần
            DialogResult result = MessageBox.Show("Bạn muốn thanh toán cho các đăng ký đã thêm không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                fThanhToan thanhToanForm = new fThanhToan(test);
                //thanhToanForm.ComboBoxValue = maDangKy; // Truyền mã đăng ký chung
                thanhToanForm.StartPosition = FormStartPosition.CenterScreen;
                thanhToanForm.ShowDialog();
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataDangKy.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataDangKy.SelectedRows[0];
                string maDangKy = selectedRow.Cells["MaDangKy"].Value.ToString();

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa đăng ký có mã " + maDangKy + " không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dangKyProcessor.XoaDangKyKhoaHoc(maDangKy);
                    dataDangKy.DataSource = dangKyProcessor.GetDangKyKhoaHoc();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đăng ký để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            //if (dataDangKy.SelectedRows.Count > 0)
            //{
            //    int selectedRowIndex = dataDangKy.SelectedRows[0].Index;
            //    DataGridViewRow selectedRow = dataDangKy.Rows[selectedRowIndex];
            //    string maDangKy = selectedRow.Cells["MaDangKy"].Value.ToString();

            //    string[] mahv = comboMaHocVien.Text.Split('-');
            //    string mahvpasrt = mahv[0].Trim();
            //    string[] makh = comboMaKhoaHoc.Text.Split('-');
            //    string makhpasrt = makh[0].Trim();
            //    DangKyKhoaHoc dangKy = new DangKyKhoaHoc
            //    {
            //        MaDangKy = txtma.Text,
            //        MaHocVien = mahvpasrt,
            //        MaKhoaHoc = makhpasrt,
            //        NgayDangKy = dateNgayDK.Value,
            //        TrangThai = txtTrangThai.Text,
            //        DaThanhToan = check.Checked,
            //        HinhThucThanhToan = txtHinhthuc.Text
            //    };

            //    dangKyProcessor.SuaDangKyKhoaHoc(dangKy);
            //    dataDangKy.DataSource = dangKyProcessor.GetDangKyKhoaHoc();
            //}
            //else
            //{
            //    MessageBox.Show("Vui lòng chọn một đăng ký để sửa chữa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTk.Text.Trim();
            List<DangKyKhoaHoc> ketQuaTimKiem = dangKyProcessor.TimKiemDangKyKhoaHoc(tuKhoa);
            dataDangKy.DataSource = ketQuaTimKiem;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XuLyTimKiemHocVien();
        }
        public void XuLyTimKiemHocVien()
        {
            while (true)
            {
                string soDienThoai = NhapSoDienThoai();
                bool hocVienDaDangKy = dangKyProcessor.KiemTraHocVienDaDangKy(soDienThoai);

                if (hocVienDaDangKy)
                {
                    DialogResult dialogResult = MessageBox.Show($"Học viên có số điện thoại {soDienThoai} đã đăng ký khóa học. Bạn có muốn đăng ký khóa học không?", "Tiếp tục tìm kiếm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.Yes)
                    {
                        string maHocVien = dangKyProcessor.LayMaHocVienTheoSoDienThoai(soDienThoai);

                        if (!string.IsNullOrEmpty(maHocVien))
                        {
                            dataDangKy.SelectionChanged -= dataDKKH_SelectionChanged;
                            comboMaHocVien.SelectedItem = maHocVien;
                            foreach (DataGridViewRow row in dataDangKy.Rows)
                            {
                                if (row.Cells["MaHocVien"].Value.ToString() == maHocVien)
                                {
                                    row.Selected = true;
                                    break;
                                }
                            }
                            dataDangKy.SelectionChanged += dataDKKH_SelectionChanged;
                            LoadData();
                            AutoSizeColumns();
                            XulyCotTiengViet();

                            break;
                        }
                    }
                    MessageBox.Show("Đã hủy tìm kiếm.");
                    break;
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show($"Học viên có số điện thoại {soDienThoai} chưa đăng ký khóa học. Bạn có muốn đăng ký mới không?", "Đăng ký mới", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.Yes)
                    {
                        fDangKy_Load(this, EventArgs.Empty);
                        break;
                    }
                    else
                    {
                        DialogResult continueSearchResult = MessageBox.Show("Đã hủy đăng ký mới. Bạn có muốn tiếp tục tìm kiếm không?", "Tiếp tục tìm kiếm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (continueSearchResult == DialogResult.No)
                        {
                            fDangKy_Load(this, EventArgs.Empty);
                            break;
                        }

                    }
                }
            }
        }

        private void comboMaKhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedText = comboMaKhoaHoc.Text;
            string maKhoaHoc = string.IsNullOrEmpty(selectedText) ? string.Empty : selectedText.Split('-')[0].Trim();

            if (!string.IsNullOrEmpty(maKhoaHoc))
            {
                decimal tongTien = dangKyProcessor.LayTongTienKhoaHoc(maKhoaHoc);
                lableTien.Text = $"Tổng tiền:  {tongTien.ToString("C")}";
            }
            else
            {
                lableTien.Text = "Chưa chọn mã khóa học.";
            }
        }
        private Form customForm; // Biến toàn cục để lưu trữ form tùy chỉnh
        private void btnchonKhoaHoc_Click(object sender, EventArgs e)
        {
            List<KhoaHocCheckItem> danhSachKhoaHoc = dangKyProcessor.GetMaKhoaHoc()
     .Select(maKhoaHoc => new KhoaHocCheckItem
     {
         MaKhoaHoc = maKhoaHoc,
         IsChecked = false,
         DonGia = 0 // Thêm dòng này để lấy đơn giá
     })
     .ToList();

            checkKhoaHoc.DataSource = danhSachKhoaHoc;
            checkKhoaHoc.DisplayMember = "MaKhoaHoc";
            checkKhoaHoc.ValueMember = "IsChecked";
            if (checkKhoaHoc.Items.Count > 0)
            {

                if (customForm == null || customForm.IsDisposed)
                {
                    customForm = new Form();
                    customForm.Text = "Chọn khóa học";
                    customForm.StartPosition = FormStartPosition.CenterScreen;

                    checkKhoaHoc.Dock = DockStyle.Fill;

                    var okButton = new Button();
                    okButton.Text = "OK";
                    okButton.DialogResult = DialogResult.OK;

                    customForm.Controls.Add(checkKhoaHoc);
                    customForm.Controls.Add(okButton);

                    okButton.Click += (s, args) => customForm.Close();
                }
                if (customForm.ShowDialog() == DialogResult.OK)
                {

                    StringBuilder selectedKhoaHoc = new StringBuilder();
                    selectedKhoaHoc.AppendLine("Các khóa học đã chọn:");

                    foreach (var item in checkKhoaHoc.CheckedItems)
                    {
                        selectedKhoaHoc.AppendLine(item.ToString());
                    }

                    MessageBox.Show(selectedKhoaHoc.ToString(), "Thông tin khóa học đã chọn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Bạn đã huỷ việc chọn khóa học.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu khóa học để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private List<string> selectedCourses = new List<string>();


        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            List<KhoaHocCheckItem> danhSachKhoaHoc = dangKyProcessor.GetMaKhoaHoc()
               .Select(maKhoaHoc => new KhoaHocCheckItem
               {
                   MaKhoaHoc = maKhoaHoc,
                   IsChecked = false
               })
               .ToList();

            checkKhoaHoc.DataSource = danhSachKhoaHoc;
            checkKhoaHoc.DisplayMember = "MaKhoaHoc";
            checkKhoaHoc.ValueMember = "IsChecked";

            decimal DonGia = 0m; // Khai báo biến DonGia ở phạm vi bên ngoài

            if (e.Index >= 0 && e.Index < checkKhoaHoc.Items.Count)
            {
                KhoaHocCheckItem selectedItem = (KhoaHocCheckItem)checkKhoaHoc.Items[e.Index];
                string maKhoaHoc = string.IsNullOrEmpty(selectedItem.MaKhoaHoc) ? string.Empty : selectedItem.MaKhoaHoc.Split('-')[0].Trim();

                if (e.NewValue == CheckState.Checked)
                {
                    // Nếu người dùng chọn, thêm mã khóa học vào danh sách
                    selectedCourses.Add(maKhoaHoc);
                }
                else
                {
                    // Nếu người dùng bỏ chọn, loại bỏ mã khóa học khỏi danh sách
                    selectedCourses.Remove(maKhoaHoc);
                }

                // Tính tổng đơn giá của các khóa học đã chọn
                foreach (string selectedCourse in selectedCourses)
                {
                    DonGia += dangKyProcessor.LayDonGiaKhoaHoc(selectedCourse);
                }

                // Update the label with the new total price
                label7.Text = DonGia.ToString();
            }


        }

        private void btnThemHocVien_Click(object sender, EventArgs e)
        {
            fQuanLyHocVien a = new fQuanLyHocVien();
            a.StartPosition = FormStartPosition.CenterScreen;
            a.ShowDialog();
            fDangKy_Load(this, EventArgs.Empty);
        }
    }
}