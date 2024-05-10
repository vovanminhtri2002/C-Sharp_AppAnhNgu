using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _BLL
{
    public class XyLyQuanLyLopHocVien
    {
        private AnhNguDataContext QuanLyLopHocVienContext = new AnhNguDataContext();

        public List<XepLopHocVien> GetDanhSachQuanLyLopHocVien()
        {
            return QuanLyLopHocVienContext.XepLopHocViens.ToList();
        }

        //public void ThemQuanLyLopHocVien(XepLopHocVien XepLopHocVien)
        //{
        //    QuanLyLopHocVienContext.XepLopHocViens.InsertOnSubmit(XepLopHocVien);
        //    QuanLyLopHocVienContext.SubmitChanges();
        //}
        public void ThemQuanLyLopHocVien(XepLopHocVien XepLopHocVien)
        {
            // Lấy thông tin lớp học
            var lopHoc = QuanLyLopHocVienContext.LopHocs.SingleOrDefault(lop => lop.MaLopHoc == XepLopHocVien.MaLopHoc);

            if (lopHoc != null)
            {
                // Kiểm tra số lượng học viên hiện tại và tối đa trong lớp
                if (lopHoc.SoLuongHocVienHienTai < lopHoc.SoLuongHocVienToiDa)
                {
                    QuanLyLopHocVienContext.XepLopHocViens.InsertOnSubmit(XepLopHocVien);
                    QuanLyLopHocVienContext.SubmitChanges();
                    lopHoc.SoLuongHocVienHienTai++;
                    QuanLyLopHocVienContext.SubmitChanges();
                }
                else
                {
                    Console.WriteLine("Lớp đã đầy, không thể thêm học viên.");
                }
            }
        }
        public List<string> GetMaLop()
        {
            var lophoc = QuanLyLopHocVienContext.LopHocs.Select(lop => $"{lop.MaLopHoc} - {lop.TenLop}").ToList();
            return lophoc;
        }
        public List<string> GetMaHocVien()
        {
            var hocvien = QuanLyLopHocVienContext.HocViens.Select(hv => $"{hv.MaHocVien} - {hv.HoTen}").ToList();
            return hocvien;
        }
        public void XoaQuanLyLopHocVien(string idQuanLy)
        {
            var quanLyToRemove = QuanLyLopHocVienContext.XepLopHocViens.FirstOrDefault(ql => ql.IDQuanLy == idQuanLy);

            if (quanLyToRemove != null)
            {
                QuanLyLopHocVienContext.XepLopHocViens.DeleteOnSubmit(quanLyToRemove);
                QuanLyLopHocVienContext.SubmitChanges();
            }
        }

        public void SuaQuanLyLopHocVien(XepLopHocVien XepLopHocVien)
        {
            XepLopHocVien ql = QuanLyLopHocVienContext.XepLopHocViens.SingleOrDefault(q => q.IDQuanLy == XepLopHocVien.IDQuanLy);
            if (ql != null)
            {
                ql.MaLopHoc = XepLopHocVien.MaLopHoc;
                ql.MaHocVien = XepLopHocVien.MaHocVien;
             
                QuanLyLopHocVienContext.SubmitChanges();
            }
        }

        public List<XepLopHocVien> TimKiemQuanLyLopHocVien(string tuKhoa)
        {
            var query = QuanLyLopHocVienContext.XepLopHocViens.AsQueryable();

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(ql =>
                    ql.IDQuanLy.Contains(tuKhoa) ||
                    ql.MaLopHoc.Contains(tuKhoa) ||
                    ql.MaHocVien.Contains(tuKhoa )
                );
            }

            return query.ToList();
        }


        public List<LopHoc> LayDanhSachLopHocDaDangKy(string maHocVien)
        {
            var query = from dangKy in QuanLyLopHocVienContext.DangKyKhoaHocs
                        where dangKy.MaHocVien == maHocVien
                        join dk_kh in QuanLyLopHocVienContext.DangKyKhoaHoc_KhoaHocs on dangKy.MaDangKy equals dk_kh.MaDangKy
                        join khoaHoc in QuanLyLopHocVienContext.KhoaHocs on dk_kh.MaKhoaHoc equals khoaHoc.MaKhoaHoc
                        join lopHoc in QuanLyLopHocVienContext.LopHocs on khoaHoc.MaKhoaHoc equals lopHoc.MaKhoaHoc
                        select lopHoc;

            return query.Distinct().ToList();
        }

    }
}

