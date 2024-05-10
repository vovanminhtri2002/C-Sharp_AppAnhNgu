using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _BLL
{
    public class XyLyDangKyKhoaHoc
    {
        private AnhNguDataContext DangKyKhoaHocContext = new AnhNguDataContext();

        public XyLyDangKyKhoaHoc()
        {

        }

        public class DangKyKhoaHocViewModel
        {
            public string MaDangKy { get; set; }
            public string TenHocVien { get; set; }
            public string TenKhoaHoc { get; set; }
            public DateTime? NgayDangKy { get; set; }
            public string TrangThai { get; set; }
            public bool Dathanhtoan { get; set; }
            public string HinhThucThanhToan { get; set; }
        }

        public List<DangKyKhoaHocViewModel> GetDangKyKhoaHoc()
        {
            return DangKyKhoaHocContext.DangKyKhoaHocs
                .Join(DangKyKhoaHocContext.HocViens, dk => dk.MaHocVien, hv => hv.MaHocVien, (dk, hv) => new { dk, hv })
                .Join(DangKyKhoaHocContext.DangKyKhoaHoc_KhoaHocs, j => j.dk.MaDangKy, kh => kh.MaDangKy, (j, kh) => new { j, kh })
                .Join(DangKyKhoaHocContext.KhoaHocs, jk => jk.kh.MaKhoaHoc, kh => kh.MaKhoaHoc, (jk, kh) => new DangKyKhoaHocViewModel
                {
                    MaDangKy = jk.j.dk.MaDangKy,
                    TenHocVien = jk.j.hv.HoTen,
                    TenKhoaHoc = kh.TenKhoaHoc,
                    NgayDangKy = jk.j.dk.NgayDangKy,
                    TrangThai = jk.j.dk.TrangThai,
                    Dathanhtoan = (bool)jk.j.dk.DaThanhToan,
                    HinhThucThanhToan = jk.j.dk.HinhThucThanhToan
                })
                .ToList();
        }

        public List<string> GetMaHocVien()
        {
            var hocvien = DangKyKhoaHocContext.HocViens.Select(hv => $"{hv.MaHocVien} - {hv.HoTen}").ToList();
            return hocvien;
        }

        public List<string> GetMaKhoaHoc()
        {
            var khoahoc = DangKyKhoaHocContext.KhoaHocs
                .Select(kh => $"{kh.MaKhoaHoc.Trim()} - {kh.TenKhoaHoc}")
                .ToList();
            return khoahoc;
        }

        public void ThemDangKyKhoaHoc_KhoaHoc(DangKyKhoaHoc_KhoaHoc dangKyKhoaHoc_KhoaHoc)
        {
            DangKyKhoaHocContext.DangKyKhoaHoc_KhoaHocs.InsertOnSubmit(dangKyKhoaHoc_KhoaHoc);
            DangKyKhoaHocContext.SubmitChanges();
        }
        public void ThemDangKyKhoaHoc(DangKyKhoaHoc dangKyKhoaHoc)
        {
            DangKyKhoaHocContext.DangKyKhoaHocs.InsertOnSubmit(dangKyKhoaHoc);
            DangKyKhoaHocContext.SubmitChanges();
        }
        public void ThemPhieuThu(PhieuThu phieuthu)
        {
            DangKyKhoaHocContext.PhieuThus.InsertOnSubmit(phieuthu);
            DangKyKhoaHocContext.SubmitChanges();
        }
        public void XoaDangKyKhoaHoc(string maDangKy)
        {
            var dangKyToRemove = DangKyKhoaHocContext.DangKyKhoaHocs.FirstOrDefault(dk => dk.MaDangKy == maDangKy);
            var dkre = DangKyKhoaHocContext.DangKyKhoaHoc_KhoaHocs.FirstOrDefault(dk => dk.MaDangKy == maDangKy);
            if (dangKyToRemove != null)
            {
                DangKyKhoaHocContext.DangKyKhoaHocs.DeleteOnSubmit(dangKyToRemove);
                DangKyKhoaHocContext.SubmitChanges();
                DangKyKhoaHocContext.DangKyKhoaHoc_KhoaHocs.DeleteOnSubmit(dkre);
                DangKyKhoaHocContext.SubmitChanges();
            }
        }

        public void SuaDangKyKhoaHoc(DangKyKhoaHoc dangKyKhoaHoc)
        {
            DangKyKhoaHoc dk = DangKyKhoaHocContext.DangKyKhoaHocs.SingleOrDefault(d => d.MaDangKy == dangKyKhoaHoc.MaDangKy);
            if (dk != null)
            {
                dk.MaHocVien = dangKyKhoaHoc.MaHocVien;
                dk.NgayDangKy = dangKyKhoaHoc.NgayDangKy;
                dk.TrangThai = dangKyKhoaHoc.TrangThai;
                dk.DaThanhToan = dangKyKhoaHoc.DaThanhToan;
                dk.HinhThucThanhToan = dangKyKhoaHoc.HinhThucThanhToan;
                DangKyKhoaHocContext.SubmitChanges();
            }
        }

        public List<DangKyKhoaHoc> TimKiemDangKyKhoaHoc(string tuKhoa)
        {
            var query = DangKyKhoaHocContext.DangKyKhoaHocs.AsQueryable();

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(dk =>
                    dk.MaDangKy.Contains(tuKhoa) ||
                    dk.MaHocVien.Contains(tuKhoa) ||
                    dk.NgayDangKy.ToString().Contains(tuKhoa) ||
                    dk.TrangThai.Contains(tuKhoa) ||
                    dk.DaThanhToan.ToString().Contains(tuKhoa) ||
                    dk.HinhThucThanhToan.Contains(tuKhoa)
                );
            }

            return query.ToList();
        }

        public bool KiemTraHocVienDaDangKy(string soDienThoai)
        {
            var trangThaiDangKy = from hocVien in DangKyKhoaHocContext.HocViens
                                  join taiKhoan in DangKyKhoaHocContext.TaiKhoans on hocVien.MaTaiKhoan equals taiKhoan.MaTaiKhoan
                                  join dangKy in DangKyKhoaHocContext.DangKyKhoaHocs on hocVien.MaHocVien equals dangKy.MaHocVien into gj
                                  from subDangKy in gj.DefaultIfEmpty()
                                  where hocVien.SoDienThoai == soDienThoai
                                  select subDangKy;
            bool hocVienDaDangKy = trangThaiDangKy.Any(subDangKy => subDangKy != null);
            return hocVienDaDangKy;
        }
        public string LayMaHocVienTheoSoDienThoai(string soDienThoai)
        {
            var hocVien = DangKyKhoaHocContext.HocViens.FirstOrDefault(hv => hv.SoDienThoai == soDienThoai);

            if (hocVien != null)
            {
                return hocVien.MaHocVien;
            }
            else
            {
                return null;
            }
        }
        public decimal LayTongTienKhoaHoc(string maKhoaHoc)
        {
            decimal? tongTien = (decimal?)DangKyKhoaHocContext.KhoaHocs
                .Where(kh => kh.MaKhoaHoc == maKhoaHoc)
                .Sum(kh => kh.HocPhi);
            return tongTien ?? 0m;
        }
        public decimal LayDonGiaKhoaHoc(string maKhoaHoc)
        {
            double? hocPhi = DangKyKhoaHocContext.KhoaHocs
                .Where(kh => kh.MaKhoaHoc == maKhoaHoc)
                .Sum(kh => kh.HocPhi);

            return (decimal?)hocPhi ?? 0m;
        }


    }
}
