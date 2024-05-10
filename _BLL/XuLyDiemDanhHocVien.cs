using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _BLL
{
    public  class XuLyDiemDanhHocVien
    {  private AnhNguDataContext DiemDanhContext = new AnhNguDataContext();
        public class ThongTinHocVienDiemDanh
        {
            public string MaHocVien { get; set; }
            public string TenHocVien { get; set; }
            public DateTime NgayDiemDanh { get; set; } 
            public string CoDiHoc { get; set; }
            public string maphong { get; set; }
            public int? Thu { get; set; }
            public int? TietBatDau { get; set; }
            public int? TietKetThuc { get; set; }
            public string CaHoc { get; set; }
        }

        public List<ThongTinHocVienDiemDanh> LayDanhSachHocVienTheoThoiKhoaBieu(string maLopHoc, string maPhong, int thu, int tietBatDau, int tietKetThuc, string caHoc, DateTime ngayHoc)
        {
            List<ThongTinHocVienDiemDanh> danhSachHocVien = new List<ThongTinHocVienDiemDanh>();

            var thoiKhoaBieuQuery = DiemDanhContext.ThoiKhoaBieus
                .Where(tkb => tkb.MaLopHoc == maLopHoc && tkb.MaPhongHoc == maPhong && tkb.Thu == thu && tkb.CaHoc == caHoc && tkb.TietBatDau >= tietBatDau && tkb.TietKetThuc <= tietKetThuc && tkb.NgayHoc == ngayHoc)
                .ToList();

            foreach (var thoiKhoaBieu in thoiKhoaBieuQuery)
            {
                var hocVienList = DiemDanhContext.XepLopHocViens
                    .Where(qlhv => qlhv.MaLopHoc == maLopHoc)
                    .Select(qlhv => qlhv.HocVien)
                    .ToList();

                foreach (var hocVien in hocVienList)
                {
                    // Kiểm tra xem học viên đã có trong danh sách chưa dựa trên MaHocVien
                    var existingStudent = danhSachHocVien.FirstOrDefault(s => s.MaHocVien == hocVien.MaHocVien);

                    if (existingStudent != null)
                    {
                        // Nếu học viên đã có trong danh sách, cập nhật thông tin
                        existingStudent.NgayDiemDanh = thoiKhoaBieu.NgayHoc ?? DateTime.MinValue;
                        existingStudent.maphong = thoiKhoaBieu.PhongHoc.TenPhongHoc;
                        existingStudent.Thu = thoiKhoaBieu.Thu;
                        existingStudent.TietBatDau = thoiKhoaBieu.TietBatDau;
                        existingStudent.TietKetThuc = thoiKhoaBieu.TietKetThuc;
                        existingStudent.CaHoc = thoiKhoaBieu.CaHoc;

                        // Cập nhật trạng thái điểm danh tùy thuộc vào yêu cầu của bạn
                        existingStudent.CoDiHoc = LayTrangThaiDiemDanh(hocVien.MaHocVien, maLopHoc, ngayHoc);
                    }
                    else
                    {
                        // Nếu học viên chưa có trong danh sách, thêm một bản ghi mới
                        var thongTinHocVien = new ThongTinHocVienDiemDanh
                        {
                            MaHocVien = hocVien.MaHocVien,
                            TenHocVien = hocVien.HoTen,
                            NgayDiemDanh = thoiKhoaBieu.NgayHoc ?? DateTime.MinValue,
                            maphong = thoiKhoaBieu.PhongHoc.TenPhongHoc,
                            Thu = thoiKhoaBieu.Thu,
                            TietBatDau = thoiKhoaBieu.TietBatDau,
                            TietKetThuc = thoiKhoaBieu.TietKetThuc,
                            CaHoc = thoiKhoaBieu.CaHoc,

                            // Cập nhật trạng thái điểm danh tùy thuộc vào yêu cầu của bạn
                            CoDiHoc = LayTrangThaiDiemDanh(hocVien.MaHocVien, maLopHoc, ngayHoc)
                        };

                        danhSachHocVien.Add(thongTinHocVien);
                    }
                }
            }

            return danhSachHocVien;
        }

        public string LayTrangThaiDiemDanh(string maHocVien, string maLopHoc, DateTime ngayHoc)
        {
         
            var diemDanhHocVien = DiemDanhContext.DiemDanhs
                .FirstOrDefault(dd => dd.MaHocVien == maHocVien && dd.MaLopHoc == maLopHoc && dd.NgayDiemDanh == ngayHoc);
            if (diemDanhHocVien != null)
            {
                return diemDanhHocVien.TrangThaiDiemDanh; 
            }
            return "Vắng";
        }


        public string GenerateIDDiemDanh()
        {
            string prefix = "IDDN";
            string randomSuffix = Guid.NewGuid().ToString("N").Substring(0, 4);
            string generatedID = $"{prefix}_{randomSuffix}";

            return generatedID;
        }
        public void CapNhatTrangThaiDiemDanh(string maHocVien, string maLopHoc, DateTime ngayHoc, string coDiHoc)
        {
            // Lấy thông tin ThoiKhoaBieu tương ứng với mã lớp học, mã học viên, và ngày học
            var thoiKhoaBieu = DiemDanhContext.ThoiKhoaBieus
                .FirstOrDefault(tkb => tkb.MaLopHoc == maLopHoc
                                        && tkb.MaHocVien == maHocVien
                                        && tkb.NgayHoc == ngayHoc
                );

            if (thoiKhoaBieu != null)
            {
                // Kiểm tra xem đã có thông tin điểm danh cho buổi học chưa
                var diemDanh = DiemDanhContext.DiemDanhs
                    .FirstOrDefault(dd => dd.MaHocVien == maHocVien
                                            && dd.MaLopHoc == maLopHoc
                    );

                if (diemDanh != null)
                {
                    // Nếu đã có, cập nhật trạng thái điểm danh
                    diemDanh.TrangThaiDiemDanh = coDiHoc;
                }
                else
                {
                    // Nếu chưa có, tạo mới và thêm vào cơ sở dữ liệu
                    diemDanh = new DiemDanh
                    {
                        IDDiemDanh = GenerateIDDiemDanh(),
                        MaHocVien = maHocVien,
                        MaLopHoc = maLopHoc,
                        NgayDiemDanh = ngayHoc,
                        TrangThaiDiemDanh = coDiHoc
                    };

                    DiemDanhContext.DiemDanhs.InsertOnSubmit(diemDanh);
                }

                DiemDanhContext.SubmitChanges();

            }
        }

       


       
    }

}

