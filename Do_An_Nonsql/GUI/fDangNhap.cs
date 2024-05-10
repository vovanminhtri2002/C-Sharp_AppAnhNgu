using _BLL;
using Do_An_Chuyen_Nganh.GUI;
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

namespace Do_An_Chuyen_Nganh
{
    public partial class fDangNhap : Form
    {
        private fTrangChu fTrangChuInstance;
        private string magv;
        private string mahv;
        public fDangNhap()
        {
            InitializeComponent();

            txtMatKhau.PasswordChar = '*';
        }
        public static class UserInfo
        {
            public static string MaNhanVien { get; set; }
        }
        private void lblQuenMK_Click(object sender, EventArgs e)
        {
            this.Hide();
            fDoiMatKhau f = new fDoiMatKhau();
            f.ShowDialog();
            this.Close();
        }
        public string LayHoTen(string username, string password)
        {
            string hoTen = string.Empty;

            try
            {
                hoTen = TaiKhoanContext.LayHoTen(username, password); // Sửa thành phương thức lấy họ tên
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy thông tin họ tên: " + ex.Message);
            }

            return hoTen;
        }
        private XuLyTaiKhoan TaiKhoanContext = new XuLyTaiKhoan();
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string username = txtTenDN.Text;
            string password = txtMatKhau.Text;

            string hoTen = LayHoTen(username, password); // Lấy thông tin họ tên từ hàm LayHoTen

            string role = CheckLogin(username, password);

            if (!string.IsNullOrEmpty(role))
            {
                this.Hide();
                MessageBox.Show($"Đăng nhập thành công với quyền: {role}");
                UserInfo.MaNhanVien = TaiKhoanContext.LayMaNhanVien(username, password);
                OpenRelevantForm(role, hoTen); // Truyền thông tin hoTen vào hàm OpenRelevantForm
            }
            else
            {
                MessageBox.Show("Đăng nhập không thành công. Vui lòng kiểm tra lại thông tin.");
            }
        }
        public string CheckLogin(string username, string password)
        {
            string role = string.Empty;

            try
            {
              
                role = TaiKhoanContext.CheckLogin(username, password);
              
                if (role.Equals("Giảng viên"))
                {
                    magv = TaiKhoanContext.LayMaGiangVien(username);
                }
                if (role.Equals("Học viên"))
                {
                    mahv = TaiKhoanContext.LayMaHocVien(username);
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi kiểm tra thông tin đăng nhập: " + ex.Message);
            }

            return role;
        }

        

        private void OpenRelevantForm(string role,string hoTen)
        {
            switch (role)
            {
                case "Quản trị hệ thống":

                   fTrangChuInstance = new fTrangChu(hoTen, magv,mahv);
                    fTrangChuInstance.Show();

                    break;
                case "Giảng viên":
                    fTrangChuInstance = new fTrangChu(hoTen, magv, mahv);
                    fTrangChuInstance.Show();
                    fTrangChuInstance.HienThiGiangVien(); // Gọi phương thức từ instance của fTrangChu đã tạo trước đó
                    //FLichDayGV a = new FLichDayGV();
                    //a.maGiangVien = magv;
                    //a.Show();
                    break;
                case "Học viên":
                    fTrangChuInstance = new fTrangChu(hoTen, magv, mahv);
                    fTrangChuInstance.Show();
                    fTrangChuInstance.HienThiHocVien();
                    //fThoiKhoaBieuHV b = new fThoiKhoaBieuHV();
                    //b.mahocvien = mahv;
                    //b.Show();
                    break;
                case "Nhân viên văn phòng":
                    fTrangChuInstance = new fTrangChu(hoTen, magv, mahv);
                    fTrangChuInstance.Show();
                    fTrangChuInstance.HienThiNhanVien();
                    break;
                default:
                    break;
            }
        }

        private void ckbHienThiMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbHienThiMatKhau.Checked)
            {
                txtMatKhau.PasswordChar = '\0'; // Hiển thị mật khẩu
            }
            else
            {
                txtMatKhau.PasswordChar = '*'; // Ẩn mật khẩu bằng ký tự '*'
            }
        }
    }
}
