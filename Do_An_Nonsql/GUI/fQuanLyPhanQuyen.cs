using _BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Do_An_Chuyen_Nganh
{
  
    public partial class fQuanLyPhanQuyen : Form
    {
        private XuLyQuyenTruyCap quyenTruyCapProcessor = new XuLyQuyenTruyCap();
        private XuLyTaiKhoan taiKhoanProcessor = new XuLyTaiKhoan();
        public fQuanLyPhanQuyen()
        {
            InitializeComponent();
        }

        private void fQuanLyPhanQuyen_Load(object sender, EventArgs e)
        {
            LoadData();
            AutoSizeColumns();
            XulyCotTiengViet();
        }
        private void XulyCotTiengViet()
        {
            // Đổi tên cột để phản ánh thông tin đúng với tài khoản
            dataQuyenTruyCap.Columns["MaQuyenTruyCap"].HeaderText = "Mã Quyền Truy Cập";
            dataQuyenTruyCap.Columns["TenQuyenTruyCap"].HeaderText = "Tên Quyền Truy Cập";

            dataTaiKhoan.Columns["MaTaiKhoan"].HeaderText = "Mã Tài Khoản";
            dataTaiKhoan.Columns["TenDangNhap"].HeaderText = "Tên Đăng Nhập";
            dataTaiKhoan.Columns["MatKhau"].HeaderText = "Mật Khẩu";
            dataTaiKhoan.Columns["MaQuyenTruyCap"].HeaderText = "Mã Quyền Truy Cập";
        }
         private void LoadData()
        {
            dataQuyenTruyCap.DataSource = quyenTruyCapProcessor.GetQuyenTruyCap();
            dataTaiKhoan.DataSource = taiKhoanProcessor.GetTaiKhoan();
            dataTaiKhoan.Columns["QuyenTruyCap"].Visible = false;
            comboQ.DataSource = taiKhoanProcessor.GetMaQuyenTruyCap();
        }
        private void AutoSizeColumns()
        {
            foreach (DataGridViewColumn column in dataQuyenTruyCap.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            foreach (DataGridViewColumn column in dataTaiKhoan.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void btnThemQ_Click(object sender, EventArgs e)
        {
            QuyenTruyCap quyenTruyCap = new QuyenTruyCap
            {
                MaQuyenTruyCap = txtMaQ.Text,
                TenQuyenTruyCap = txtTenQ.Text
            };

            quyenTruyCapProcessor.ThemQuyenTruyCap(quyenTruyCap);
            LoadData();
            ClearInputFieldsQuyenTruyCap();
        }

        private void btnXoaQ_Click(object sender, EventArgs e)
        {
            if (dataQuyenTruyCap.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataQuyenTruyCap.SelectedRows[0];
                string maQuyenTruyCap = selectedRow.Cells["MaQuyenTruyCap"].Value.ToString();

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa quyền truy cập có mã " + maQuyenTruyCap + " không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    quyenTruyCapProcessor.XoaQuyenTruyCap(maQuyenTruyCap);
                    dataQuyenTruyCap.DataSource = quyenTruyCapProcessor.GetQuyenTruyCap();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một quyền truy cập để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSuaQ_Click(object sender, EventArgs e)
        {
            if (dataQuyenTruyCap.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataQuyenTruyCap.SelectedRows[0];
                string maQuyenTruyCap = selectedRow.Cells["MaQuyenTruyCap"].Value.ToString();

                QuyenTruyCap quyenTruyCap = new QuyenTruyCap
                {
                    MaQuyenTruyCap = maQuyenTruyCap,
                    TenQuyenTruyCap = txtTenQ.Text
                };

                quyenTruyCapProcessor.SuaQuyenTruyCap(quyenTruyCap);
                LoadData();
                ClearInputFieldsQuyenTruyCap();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một quyền truy cập để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

      


       

        private void btnThemTK_Click(object sender, EventArgs e)
        {
            TaiKhoan taiKhoan = new TaiKhoan
            {
                MaTaiKhoan = txtMaTk.Text,
                TenDangNhap = txtTenDn.Text,
                MatKhau = txtMk.Text,
                MaQuyenTruyCap = comboQ.Text
            };

            taiKhoanProcessor.ThemTaiKhoan(taiKhoan);
            LoadData();
            ClearInputFieldsTaiKhoan();
        }

        private void btnXoaTk_Click(object sender, EventArgs e)
        {
            if (dataTaiKhoan.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataTaiKhoan.SelectedRows[0];
                string maTaiKhoan = selectedRow.Cells["MaTaiKhoan"].Value.ToString();

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản có mã " + maTaiKhoan + " không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    taiKhoanProcessor.XoaTaiKhoan(maTaiKhoan);
                    dataTaiKhoan.DataSource = taiKhoanProcessor.GetTaiKhoan();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ClearInputFieldsQuyenTruyCap()
        {
            txtMaQ.Clear();
            txtTenQ.Clear();
        }

      
        private void ClearInputFieldsTaiKhoan()
        {
            txtMaTk.Clear();
            txtTenDn.Clear();
            txtMk.Clear();
            comboQ.SelectedIndex = -1;
        }

        private void btnSuaTK_Click(object sender, EventArgs e)
        {
            if (dataTaiKhoan.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataTaiKhoan.SelectedRows[0];
                string maTaiKhoan = selectedRow.Cells["MaTaiKhoan"].Value.ToString();

                TaiKhoan taiKhoan = new TaiKhoan
                {
                    MaTaiKhoan = maTaiKhoan,
                    TenDangNhap = txtTenDn.Text,
                    MatKhau = txtMk.Text,
                    MaQuyenTruyCap = comboQ.Text
                };

                taiKhoanProcessor.SuaTaiKhoan(taiKhoan);
                LoadData();
                ClearInputFieldsTaiKhoan();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTKQ_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTkQ.Text.Trim();
            List<QuyenTruyCap> ketQuaTimKiem = quyenTruyCapProcessor.TimKiemQuyenTruyCap(tuKhoa);
            dataQuyenTruyCap.DataSource = ketQuaTimKiem;
        }

      

        private void btnTKTK_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTKTK.Text.Trim();
            List<TaiKhoan> ketQuaTimKiem = taiKhoanProcessor.TimKiemTaiKhoan(tuKhoa);
            dataTaiKhoan.DataSource = ketQuaTimKiem;

        }

        private void dataQuyenTruyCap_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataQuyenTruyCap.CurrentRow;
            if (selectedRow != null)
            {
                string maQuyenTruyCap = selectedRow.Cells["MaQuyenTruyCap"].Value.ToString();
                string tenQuyenTruyCap = selectedRow.Cells["TenQuyenTruyCap"].Value.ToString();

                // Hiển thị thông tin tương ứng với dòng được chọn
                txtMaQ.Text = maQuyenTruyCap;
                txtTenQ.Text = tenQuyenTruyCap;
            }
        }

      

        private void dataTaiKhoan_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataTaiKhoan.CurrentRow;
            if (selectedRow != null)
            {
                string maTaiKhoan = selectedRow.Cells["MaTaiKhoan"].Value.ToString();
                string tenDangNhap = selectedRow.Cells["TenDangNhap"].Value.ToString();
                string matKhau = selectedRow.Cells["MatKhau"].Value.ToString();
                string maQuyenTruyCap = selectedRow.Cells["MaQuyenTruyCap"].Value.ToString();

                // Hiển thị thông tin tương ứng với dòng được chọn
                txtMaTk.Text = maTaiKhoan;
                txtTenDn.Text = tenDangNhap;
                txtMk.Text = matKhau;
                comboQ.Text = maQuyenTruyCap;
            }
        }
    }
}
