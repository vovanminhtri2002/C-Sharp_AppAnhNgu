using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _BLL
{
    public  class XuLyXemThoiKhoaBieu
    {
        private AnhNguDataContext XulyTKB = new AnhNguDataContext();

        public List<ThongTinLopHoc> LayThongTinLopHoc(string maHocVien)
        {
            var thongTinLopHocList = new List<ThongTinLopHoc>();

            var lopHocList = XulyTKB.ThoiKhoaBieus
                .Where(tkb => tkb.MaHocVien == maHocVien)
                .ToList();

            foreach (var lopHoc in lopHocList)
            {
                var thongTinLopHoc = new ThongTinLopHoc
                {
                    TenLop = GetTenLop(lopHoc.MaLopHoc),
                    TenPhongHoc = GetTenPhongHoc(lopHoc.MaPhongHoc),
                    Thu = lopHoc.Thu.ToString(),
                    TietBatDau = lopHoc.TietBatDau ?? 0,
                    TietKetThuc = lopHoc.TietKetThuc ?? 0,
                    TenGiangVien = GetTenGiangVien(lopHoc.MaGiangVien),
                    Cahoc = lopHoc.CaHoc,
                    NgayHoc = lopHoc.NgayHoc,
                };

                thongTinLopHocList.Add(thongTinLopHoc);
            }

            return thongTinLopHocList;
        }
        private string GetTenLop(string Malop)
        {
            var lophoc = XulyTKB.LopHocs
                .FirstOrDefault(l => l.MaLopHoc == Malop);
            return lophoc?.TenLop ?? "N/A";
        }
        private string GetTenPhongHoc(string maPhongHoc)
        {
            var phongHoc = XulyTKB.PhongHocs
                .FirstOrDefault(ph => ph.MaPhongHoc == maPhongHoc);
            return phongHoc?.TenPhongHoc ?? "N/A";
        }

        private string GetTenGiangVien(string maGiangVien)
        {
            var giangVien = XulyTKB.GiangViens
                .FirstOrDefault(gv => gv.MaGiangVien == maGiangVien);

            return giangVien?.HoTen ?? "N/A";
        }

        public class ThongTinLopHoc
        {
            public string TenLop { get; set; }
            public string TenPhongHoc { get; set; }
            public string Thu { get; set; }
            public int TietBatDau { get; set; }
            public int TietKetThuc { get; set; }
            public string TenGiangVien { get; set; }
            public string Cahoc { get; set; }
            public DateTime? NgayHoc { get; set; }
        }
        public List<ThongTinLopHoc> LayLichDayCuaGiangVien(string maGiangVien)
        {
            var thongTinLopHocList = new List<ThongTinLopHoc>();

            var lopHocList = XulyTKB.ThoiKhoaBieus
                .Where(tkb => tkb.MaGiangVien == maGiangVien)
                .ToList();

            var ngayHocDistinct = lopHocList.Select(tkb => tkb.NgayHoc).Distinct();

            foreach (var ngayHoc in ngayHocDistinct)
            {
                // Lọc lại danh sách lớp học theo ngày học
                var lopHocTheoNgay = lopHocList.Where(tkb => tkb.NgayHoc == ngayHoc).FirstOrDefault();

                var thongTinLopHoc = new ThongTinLopHoc
                {
                    TenLop = GetTenLop(lopHocTheoNgay.MaLopHoc),
                    TenPhongHoc = GetTenPhongHoc(lopHocTheoNgay.MaPhongHoc),
                    Thu = lopHocTheoNgay.Thu.ToString(),
                    TietBatDau = lopHocTheoNgay.TietBatDau ?? 0,
                    TietKetThuc = lopHocTheoNgay.TietKetThuc ?? 0,
                    Cahoc = lopHocTheoNgay.CaHoc,
                    NgayHoc = ngayHoc,
                };

                thongTinLopHocList.Add(thongTinLopHoc);
            }

            return thongTinLopHocList;
        }
        public string LayMaLopHocTheoTenLop(string tenLopHoc)
        {
            var lopHoc = XulyTKB.LopHocs.FirstOrDefault(lop => lop.TenLop == tenLopHoc);

            if (lopHoc != null)
            {
                return lopHoc.MaLopHoc;
            }
            return string.Empty;
        }
        public string LayMaPhongHocTheoTenPhong(string tenphonghoc)
        {
            
            var phonghoc = XulyTKB.PhongHocs.FirstOrDefault(lop => lop.TenPhongHoc == tenphonghoc);

            if (phonghoc != null)
            {
                return phonghoc.MaPhongHoc;
            }
            return string.Empty;
        }

    }
}

