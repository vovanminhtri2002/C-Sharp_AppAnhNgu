using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _BLL
{
    public class XyLyTaiLieu
    {
        AnhNguDataContext TaiLieuContext = new AnhNguDataContext();

        public XyLyTaiLieu()
        {

        }

        public List<TaiLieu> GetTaiLieu()
        {
            return TaiLieuContext.TaiLieus.Select(tl => tl).ToList<TaiLieu>();
        }

        public List<string> GetMaKhoaHoc()
        {
            var hocvien = TaiLieuContext.KhoaHocs.Select(kh => $"{kh.MaKhoaHoc} - {kh.TenKhoaHoc}").ToList();
            return hocvien;
        }

        public void ThemTaiLieu(TaiLieu taiLieu)
        {
            TaiLieuContext.TaiLieus.InsertOnSubmit(taiLieu);
            TaiLieuContext.SubmitChanges();
        }

        public void XoaTaiLieu(string maTaiLieu)
        {
            var taiLieuToRemove = TaiLieuContext.TaiLieus.FirstOrDefault(tl => tl.MaTaiLieu == maTaiLieu);

            if (taiLieuToRemove != null)
            {
                TaiLieuContext.TaiLieus.DeleteOnSubmit(taiLieuToRemove);
                TaiLieuContext.SubmitChanges();
            }
        }

        public void SuaTaiLieu(TaiLieu taiLieu)
        {
            TaiLieu tl = TaiLieuContext.TaiLieus.SingleOrDefault(t => t.MaTaiLieu == taiLieu.MaTaiLieu);
            if (tl != null)
            {
                tl.TenTaiLieu = taiLieu.TenTaiLieu;
              
                tl.MaKhoaHoc = taiLieu.MaKhoaHoc;
                tl.SoLuongTaiLieu = taiLieu.SoLuongTaiLieu;
                TaiLieuContext.SubmitChanges();
            }
        }

        public List<TaiLieu> TimKiemTaiLieu(string tuKhoa)
        {
            var query = TaiLieuContext.TaiLieus.AsQueryable();

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(tl =>
                    tl.MaTaiLieu.Contains(tuKhoa) ||
                    tl.TenTaiLieu.Contains(tuKhoa) ||
                  
                    tl.MaKhoaHoc.Contains(tuKhoa) ||
                    tl.SoLuongTaiLieu.ToString().Contains(tuKhoa)
                );
            }

            return query.ToList();
        }
    }
}

