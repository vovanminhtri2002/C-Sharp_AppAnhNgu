using _BLL;
using DevExpress.XtraEditors.Design;
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
    public partial class fLichDayGV : Form
    {
    
        private XuLyXemThoiKhoaBieu quanLyLichHocBLL = new XuLyXemThoiKhoaBieu();
        private XuLyDiemDanhHocVien xuLyDiemDanhHocVien = new XuLyDiemDanhHocVien();
 
        private string maGiangVien;
        private string hoTen;
        public fLichDayGV(string hoTen, string maGiangVien)
        {
            this.hoTen = hoTen;
            this.maGiangVien = maGiangVien;
            InitializeComponent();
            dataTKB.Columns.Add("TenLop", "Tên Lớp");
            dataTKB.Columns.Add("TenPhongHoc", "Tên Phòng Học");
            dataTKB.Columns.Add("Thu", "Thứ");
            dataTKB.Columns.Add("TietBatDau", "Tiết Bắt Đầu");
            dataTKB.Columns.Add("TietKetThuc", "Tiết Kết Thúc");
            dataTKB.Columns.Add("CaHoc", "Ca Học");
            dataTKB.Columns.Add("NgayHoc", "Ngày Học");
        }
        private void HienThiLichDay()
        {
            List<XuLyXemThoiKhoaBieu.ThongTinLopHoc> thongTinLopHocList = quanLyLichHocBLL.LayLichDayCuaGiangVien(maGiangVien);

            foreach (var thongTinLopHoc in thongTinLopHocList)
            {
                if (thongTinLopHoc != null)
                {
                    int rowIndex = dataTKB.Rows.Add();

                    dataTKB.Rows[rowIndex].Cells["TenLop"].Value = thongTinLopHoc.TenLop;
                    dataTKB.Rows[rowIndex].Cells["TenPhongHoc"].Value = thongTinLopHoc.TenPhongHoc;
                    dataTKB.Rows[rowIndex].Cells["Thu"].Value = thongTinLopHoc.Thu;
                    dataTKB.Rows[rowIndex].Cells["TietBatDau"].Value = thongTinLopHoc.TietBatDau;
                    dataTKB.Rows[rowIndex].Cells["TietKetThuc"].Value = thongTinLopHoc.TietKetThuc;
                    dataTKB.Rows[rowIndex].Cells["CaHoc"].Value = thongTinLopHoc.Cahoc;
                    if (thongTinLopHoc.NgayHoc.HasValue)
                    {
                        dataTKB.Rows[rowIndex].Cells["NgayHoc"].Value = thongTinLopHoc.NgayHoc.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        dataTKB.Rows[rowIndex].Cells["NgayHoc"].Value = DBNull.Value;
                    }
                }
            }
        }

        

        private void dataTKB_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string tenLopHoc = dataTKB.Rows[e.RowIndex].Cells["TenLop"].Value?.ToString();

                if (!string.IsNullOrEmpty(tenLopHoc))
                {
                    string maLopHoc = quanLyLichHocBLL.LayMaLopHocTheoTenLop(tenLopHoc);

                    if (!string.IsNullOrEmpty(maLopHoc))
                    {
                        DateTime ngayHoc = DateTime.MinValue;
                        object ngayHocCellValue = dataTKB.Rows[e.RowIndex].Cells["NgayHoc"].Value;

                        if (ngayHocCellValue != null && ngayHocCellValue != DBNull.Value)
                        {
                            ngayHoc = Convert.ToDateTime(ngayHocCellValue);
                        }

                        string tenphong = dataTKB.Rows[e.RowIndex].Cells["TenPhongHoc"].Value?.ToString();
                        string maPhong = quanLyLichHocBLL.LayMaPhongHocTheoTenPhong(tenphong);
                        int thu = Convert.ToInt32(dataTKB.Rows[e.RowIndex].Cells["Thu"].Value);
                        int tietBatDau = Convert.ToInt32(dataTKB.Rows[e.RowIndex].Cells["TietBatDau"].Value);
                        int tietKetThuc = Convert.ToInt32(dataTKB.Rows[e.RowIndex].Cells["TietKetThuc"].Value);
                        string caHoc = dataTKB.Rows[e.RowIndex].Cells["CaHoc"].Value?.ToString();

                        var danhSachHocVien = xuLyDiemDanhHocVien.LayDanhSachHocVienTheoThoiKhoaBieu(maLopHoc, maPhong, thu, tietBatDau, tietKetThuc, caHoc, ngayHoc);

                        fDiemDanhHocVien f = new fDiemDanhHocVien(maLopHoc, ngayHoc, maPhong, thu, tietBatDau, tietKetThuc, caHoc,danhSachHocVien);
                        f.StartPosition = FormStartPosition.CenterScreen;
                        f.Show();
                    }
                }
            }
        }

        private void fThoiKhoaBieuGV_Load(object sender, EventArgs e)
        {
            dataTKB.Rows.Clear();
            HienThiLichDay();
        }
    }
}
