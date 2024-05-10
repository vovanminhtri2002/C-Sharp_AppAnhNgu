using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _BLL
{
    public class XuLyXepLop
    {
        AnhNguDataContext Xeplop = new AnhNguDataContext();
        public void ThemQuanLyLopHocVien(XepLopHocVien quanLyLopHocVien)
        {
            // Lấy thông tin lớp học
            var lopHoc = Xeplop.LopHocs.SingleOrDefault(lop => lop.MaLopHoc == quanLyLopHocVien.MaLopHoc);

            if (lopHoc != null)
            {
                if (lopHoc.SoLuongHocVienHienTai < lopHoc.SoLuongHocVienToiDa)
                {
                    Xeplop.XepLopHocViens.InsertOnSubmit(quanLyLopHocVien);
                    Xeplop.SubmitChanges();
                    lopHoc.SoLuongHocVienHienTai++;
                    Xeplop.SubmitChanges();
                }
                else
                {
                    Console.WriteLine("Lớp đã đầy, không thể thêm học viên.");
                }
            }
        }
        public void SuaQuanLyLopHocVien(XepLopHocVien quanLyLopHocVien)
        {
            XepLopHocVien ql = Xeplop.XepLopHocViens.SingleOrDefault(q => q.IDQuanLy == quanLyLopHocVien.IDQuanLy);
            if (ql != null)
            {
                var lopHocCu = Xeplop.LopHocs.SingleOrDefault(lop => lop.MaLopHoc == ql.MaLopHoc);
                var lopHocMoi = Xeplop.LopHocs.SingleOrDefault(lop => lop.MaLopHoc == quanLyLopHocVien.MaLopHoc);

                if (lopHocCu != null)
                {
                    lopHocCu.SoLuongHocVienHienTai--; // Giảm số lượng học viên cũ
                    Xeplop.SubmitChanges();
                }

                ql.MaLopHoc = quanLyLopHocVien.MaLopHoc;
                ql.MaHocVien = quanLyLopHocVien.MaHocVien;
              

                Xeplop.SubmitChanges();

                if (lopHocMoi != null)
                {
                    lopHocMoi.SoLuongHocVienHienTai++; // Tăng số lượng học viên mới
                    Xeplop.SubmitChanges();
                }
            }
        }
        public void XoaQuanLyLopHocVien(string idQuanLy)
        {
            var quanLyToRemove = Xeplop.XepLopHocViens.FirstOrDefault(ql => ql.IDQuanLy == idQuanLy);

            if (quanLyToRemove != null)
            {
                // Lấy thông tin lớp học
                var lopHoc = Xeplop.LopHocs.SingleOrDefault(lop => lop.MaLopHoc == quanLyToRemove.MaLopHoc);

                if (lopHoc != null)
                {
                    // Giảm số lượng học viên trong lớp
                    lopHoc.SoLuongHocVienHienTai--;

                    // Xóa quản lý lớp học viên
                    Xeplop.XepLopHocViens.DeleteOnSubmit(quanLyToRemove);
                    Xeplop.SubmitChanges();

                    // Cập nhật số lượng học viên trong lớp
                    Xeplop.SubmitChanges();
                }
            }
        }

    }
}
