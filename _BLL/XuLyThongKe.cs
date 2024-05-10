using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace _BLL
{
    public  class XuLyThongKe
    {
        private AnhNguDataContext thongke = new AnhNguDataContext();

        public Dictionary<string, int> ThongKeHocVienTheoKhoaHoc()
        {
            var query = from dangKy in thongke.DangKyKhoaHoc_KhoaHocs
                        join khoaHoc in thongke.KhoaHocs on dangKy.MaKhoaHoc equals khoaHoc.MaKhoaHoc
                        group dangKy by khoaHoc.TenKhoaHoc into g
                        select new { KhoaHoc = g.Key, SoLuong = g.Count() };

            return query.ToDictionary(item => item.KhoaHoc, item => item.SoLuong);
        }

        public double LayTongDoanhThu()
        {
            double? tongDoanhThu = thongke.PhieuThus.Sum(pt => pt.TongTien);
            return tongDoanhThu ?? 0;

        }
        public Dictionary<string, int> ThongKeKetQuaCuoiKy()
        {
                var query = from diemCuoiKy in thongke.DiemCuoiKies
                            group diemCuoiKy by
                                diemCuoiKy.DiemCuoiKy1 >= 8.0 ? "Đậu" : "Rớt" into g
                            select new { KetQua = g.Key, SoLuong = g.Count() };

                return query.ToDictionary(item => item.KetQua, item => item.SoLuong);
        }
        public int LayTongSoLuongHocVienDangKy()
        {
            int tongSoLuongHocVienDangKy = thongke.DangKyKhoaHocs.Count();
            return tongSoLuongHocVienDangKy;
        }
        public Dictionary<string, int> ThongKeSoBuoiVang()
        {
            var query = from hocVien in thongke.HocViens
                        join thoiKhoaBieu in thongke.ThoiKhoaBieus on hocVien.MaHocVien equals thoiKhoaBieu.MaHocVien
                        let leftJoin = from diemDanh in thongke.DiemDanhs
                                       where diemDanh.MaHocVien == hocVien.MaHocVien && diemDanh.MaLopHoc == thoiKhoaBieu.MaLopHoc && diemDanh.NgayDiemDanh == thoiKhoaBieu.NgayHoc
                                       select diemDanh
                        where leftJoin.FirstOrDefault() == null || leftJoin.First().TrangThaiDiemDanh == "Vắng"
                        group hocVien by new { hocVien.MaHocVien, hocVien.HoTen } into g
                        select new {  HoTen = g.Key.HoTen, SoBuoiVang = g.Count() };

            return query.ToDictionary(item => $"{item.HoTen}", item => item.SoBuoiVang);
        }
        public Dictionary<string, int> ThongKeSoLuongLopDay()
        {
            var query = from giangVien in thongke.GiangViens
                        join thoiKhoaBieu in thongke.ThoiKhoaBieus on giangVien.MaGiangVien equals thoiKhoaBieu.MaGiangVien
                        group giangVien by giangVien.HoTen into g
                        select new { HoTen = g.Key, SoLuongLopDay = g.Count() };

            return query.ToDictionary(item => item.HoTen, item => item.SoLuongLopDay);
        }

    }
}
