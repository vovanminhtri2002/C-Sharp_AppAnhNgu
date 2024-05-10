using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace _BLL
{
    public class XyLyThoiKhoaBieu
    {
        private AnhNguDataContext ThoiKhoaBieuContext = new AnhNguDataContext();
        public class ThoiKhoaBieuViewModel
        {
            public string MaThoiKhoaBieu { get; set; }
            public string TenGiangVien { get; set; }
            public string TenHocVien { get; set; }
            public string TenPhongHoc { get; set; }
            public string TenLop { get; set; }
            public int? Thu { get; set; }
            public int? TietBatDau { get; set; }
            public int? TietKetThuc { get; set; }
            public string DiaDiem { get; set; }
            public string CaHoc { get; set; }
            public DateTime ?NgayHoc { get; set; }
        }
        public List<ThoiKhoaBieuViewModel> GetThoiKhoaBieu()
        {
            return ThoiKhoaBieuContext.ThoiKhoaBieus
                .Select(tkb => new ThoiKhoaBieuViewModel
                {
                    MaThoiKhoaBieu = tkb.IDThoiKhoaBieu,
                    TenGiangVien = tkb.GiangVien.HoTen,
                    TenHocVien = tkb.HocVien.HoTen,
                    TenPhongHoc = tkb.PhongHoc.TenPhongHoc,
                    TenLop = tkb.LopHoc.TenLop,
                    Thu = tkb.Thu,
                    TietBatDau = tkb.TietBatDau,
                    TietKetThuc = tkb.TietKetThuc,
                    DiaDiem = tkb.DiaDiem,
                    CaHoc = tkb.CaHoc,
                    NgayHoc = tkb.NgayHoc
                })
                .ToList();
        }

        public List<string> GetMaHocVien()
        {
            return ThoiKhoaBieuContext.HocViens.Select((HocVien hv) => $"{hv.MaHocVien} - {hv.HoTen}").ToList();
        }

        public List<string> GetMaGiangVien()
        {
            return ThoiKhoaBieuContext.GiangViens.Select((GiangVien gv) => $"{gv.MaGiangVien} - {gv.HoTen}").ToList();
        }

        public List<string> GetMaPhong()
        {
            return ThoiKhoaBieuContext.PhongHocs.Select((PhongHoc ph) => $"{ph.MaPhongHoc} - {ph.TenPhongHoc}").ToList();
        }

        public List<string> GetMaLop()
        {
            return ThoiKhoaBieuContext.LopHocs.Select((LopHoc l) => $"{l.MaLopHoc} - {l.TenLop}").ToList();
        }
        public string GenerateRandomID(string prefix)
        {
            string guid = Guid.NewGuid().ToString();
            string randomPart = guid.Substring(guid.Length - 4);
            string newID = $"{prefix}{randomPart}";
            return newID;
        }
        public void ThemThoiKhoaBieu(ThoiKhoaBieu thoiKhoaBieu)
        {
            ThoiKhoaBieuContext.ThoiKhoaBieus.InsertOnSubmit(thoiKhoaBieu);
            ThoiKhoaBieuContext.SubmitChanges();
        }
        public string GetLastInsertedID()
        {
            // Giả sử bảng ThoiKhoaBieus có một cột identity là IDThoiKhoaBieu
            var lastRecord = ThoiKhoaBieuContext.ThoiKhoaBieus.OrderByDescending(tkb => tkb.IDThoiKhoaBieu).FirstOrDefault();
            return lastRecord?.IDThoiKhoaBieu;
        }

      
        public void XoaThoiKhoaBieu(string idThoiKhoaBieu)
        {
            ThoiKhoaBieu thoiKhoaBieu = ThoiKhoaBieuContext.ThoiKhoaBieus.FirstOrDefault((ThoiKhoaBieu tkb) => tkb.IDThoiKhoaBieu == idThoiKhoaBieu);
            if (thoiKhoaBieu != null)
            {
                ThoiKhoaBieuContext.ThoiKhoaBieus.DeleteOnSubmit(thoiKhoaBieu);
                ThoiKhoaBieuContext.SubmitChanges();
            }
        }
   

       
        public void SuaThoiKhoaBieu(string idThoiKhoaBieu, ThoiKhoaBieu thoiKhoaBieu, string maGiangVien, string maHocVien)
        {
            // Your existing implementation...
            ThoiKhoaBieu thoiKhoaBieu2 = ThoiKhoaBieuContext.ThoiKhoaBieus.SingleOrDefault(t => t.IDThoiKhoaBieu == idThoiKhoaBieu);

            if (thoiKhoaBieu2 != null)
            {
                // Cập nhật thông tin mới của ThoiKhoaBieu
                thoiKhoaBieu2.MaGiangVien = thoiKhoaBieu.MaGiangVien;
                thoiKhoaBieu2.MaHocVien = thoiKhoaBieu.MaHocVien;
                thoiKhoaBieu2.MaPhongHoc = thoiKhoaBieu.MaPhongHoc;
                thoiKhoaBieu2.MaLopHoc = thoiKhoaBieu.MaLopHoc;
                thoiKhoaBieu2.Thu = thoiKhoaBieu.Thu;
                thoiKhoaBieu2.TietBatDau = thoiKhoaBieu.TietBatDau;
                thoiKhoaBieu2.TietKetThuc = thoiKhoaBieu.TietKetThuc;
                thoiKhoaBieu2.DiaDiem = thoiKhoaBieu.DiaDiem;

                // SubmitChanges cho ThoiKhoaBieuContext
                ThoiKhoaBieuContext.SubmitChanges();

                // Lấy ra IDThoiKhoaBieu mới
            
            }
        }

        private void CapNhatMalopHocMaHocVienDiemDanh(ThoiKhoaBieu thoiKhoaBieu)
        {
            List<DiemDanh> list = ThoiKhoaBieuContext.DiemDanhs.Where((DiemDanh dd) => dd.MaHocVien == thoiKhoaBieu.MaHocVien && dd.MaLopHoc == thoiKhoaBieu.MaLopHoc).ToList();
            foreach (DiemDanh item in list)
            {
                item.MaLopHoc = thoiKhoaBieu.MaLopHoc;
                item.MaHocVien = thoiKhoaBieu.MaHocVien;
            }

            ThoiKhoaBieuContext.SubmitChanges();
        }

        public List<ThoiKhoaBieu> TimKiemThoiKhoaBieu(string tuKhoa)
        {
            IQueryable<ThoiKhoaBieu> source = ThoiKhoaBieuContext.ThoiKhoaBieus.AsQueryable();
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                source = source.Where((ThoiKhoaBieu tkb) => tkb.IDThoiKhoaBieu.Contains(tuKhoa) || tkb.MaGiangVien.Contains(tuKhoa) || tkb.MaHocVien.Contains(tuKhoa) || tkb.MaPhongHoc.Contains(tuKhoa) || tkb.MaLopHoc.Contains(tuKhoa) || ((object)tkb.Thu).ToString().Contains(tuKhoa) || ((object)tkb.TietBatDau).ToString().Contains(tuKhoa) || ((object)tkb.TietKetThuc).ToString().Contains(tuKhoa) || tkb.DiaDiem.Contains(tuKhoa));
            }

            return source.ToList();
        }

        public List<LopHoc> LayDanhSachLopHocDaDangKy(string maHocVien)
        {
            var query = from dangKy in ThoiKhoaBieuContext.DangKyKhoaHocs
                        where dangKy.MaHocVien == maHocVien
                        join dk_kh in ThoiKhoaBieuContext.DangKyKhoaHoc_KhoaHocs on dangKy.MaDangKy equals dk_kh.MaDangKy
                        join khoaHoc in ThoiKhoaBieuContext.KhoaHocs on dk_kh.MaKhoaHoc equals khoaHoc.MaKhoaHoc
                        join lopHoc in ThoiKhoaBieuContext.LopHocs on khoaHoc.MaKhoaHoc equals lopHoc.MaKhoaHoc
                        select lopHoc;

            return query.Distinct().ToList();
        }


        public List<string> KiemTraTrungTruocKhiThem(int? thu, string maPhongHoc, int? tietBatDau, int? tietKetThuc, string diaDiem, string caHoc, DateTime? ngayHoc)
        {
            List<string> errorMessages = new List<string>();

            // Kiểm tra trùng lịch học
            var existingSchedules = ThoiKhoaBieuContext.ThoiKhoaBieus
                .Where(tkb => tkb.Thu == thu && tkb.MaPhongHoc == maPhongHoc)
                .ToList();

            foreach (var schedule in existingSchedules)
            {
                // Kiểm tra trùng tiết học
                if ((tietBatDau.HasValue && tietKetThuc.HasValue &&
                    (tietBatDau.Value >= schedule.TietBatDau && tietBatDau.Value <= schedule.TietKetThuc ||
                    tietKetThuc.Value >= schedule.TietBatDau && tietKetThuc.Value <= schedule.TietKetThuc)))
                {
                    errorMessages.Add($"Lịch học trùng với tiết học của lịch có ID {schedule.IDThoiKhoaBieu}.");
                }

                // Kiểm tra trùng địa điểm
                if (diaDiem == schedule.DiaDiem)
                {
                    errorMessages.Add($"Lịch học trùng với địa điểm của lịch có ID {schedule.IDThoiKhoaBieu}.");
                }

                // Kiểm tra trùng ca học
                if (caHoc == schedule.CaHoc)
                {
                    errorMessages.Add($"Lịch học trùng với ca học của lịch có ID {schedule.IDThoiKhoaBieu}.");
                }

                // Kiểm tra trùng ngày học
                if (ngayHoc.HasValue && ngayHoc.Value.Date == schedule.NgayHoc)
                {
                    errorMessages.Add($"Lịch học trùng với ngày học của lịch có ID {schedule.IDThoiKhoaBieu}.");
                }
            }

            return errorMessages;
        }

    }
}