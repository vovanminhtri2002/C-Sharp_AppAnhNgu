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

namespace Do_An_Chuyen_Nganh.GUI
{
    public partial class fQuanLyPhongHoc : Form
    {
        private XyLyPhongHoc phongHocProcessor = new XyLyPhongHoc();
        public fQuanLyPhongHoc()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            dataPhongHoc.DataSource = phongHocProcessor.LayDanhSachPhongHoc();
           
        }

        private void AutoSizeColumns()
        {
            foreach (DataGridViewColumn column in dataPhongHoc.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        private void XulyCotTiengViet()
        {
            dataPhongHoc.Columns["MaPhongHoc"].HeaderText = "Mã Phòng Học";
            dataPhongHoc.Columns["TenPhongHoc"].HeaderText = "Tên Phòng Học";
            dataPhongHoc.Columns["SoLuongToiDa"].HeaderText = "Số Lượng Tối Đa";
            dataPhongHoc.Columns["SoLuongDaDangKy"].HeaderText = "Số Lượng Đã Đăng Ký";
            dataPhongHoc.Columns["TrangThai"].HeaderText = "Trạng Thái";
        }
        private void fQuanLyPhongHoc_Load(object sender, EventArgs e)
        {
            LoadData();
            AutoSizeColumns();
            XulyCotTiengViet();
        }

        private void dataPH_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataPhongHoc.CurrentRow;
            if (selectedRow != null)
            {
                string maPhongHoc = selectedRow.Cells["MaPhongHoc"].Value.ToString();
                string tenPhongHoc = selectedRow.Cells["TenPhongHoc"].Value.ToString();
                int soLuongToiDa = Convert.ToInt32(selectedRow.Cells["SoLuongToiDa"].Value);
                int soLuongDaDangKy = Convert.ToInt32(selectedRow.Cells["SoLuongDaDangKy"].Value);
                string trangThai = selectedRow.Cells["TrangThai"].Value.ToString();

                txtMa.Text = maPhongHoc;
                txtTenP.Text = tenPhongHoc;
                txtSl.Text = soLuongToiDa.ToString();
                txtSlDk.Text = soLuongDaDangKy.ToString();
                txtTT.Text = trangThai;
            }
        }
        private Random random = new Random();
        private string SinhMaPhongHoc()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomPart = new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return "PH" + randomPart;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            PhongHoc phongHoc = new PhongHoc
            {
                MaPhongHoc = SinhMaPhongHoc(),
                TenPhongHoc = txtTenP.Text,
                SoLuongToiDa = Convert.ToInt32(txtSl.Text),
                SoLuongDaDangKy = Convert.ToInt32(txtSlDk.Text),
                TrangThai = txtTT.Text
            };

            phongHocProcessor.ThemPhongHoc(phongHoc);
            LoadData();
            ClearInputFields();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataPhongHoc.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataPhongHoc.SelectedRows[0];
                string maPhongHoc = selectedRow.Cells["MaPhongHoc"].Value.ToString();

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa phòng học có mã " + maPhongHoc + " không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    phongHocProcessor.XoaPhongHoc(maPhongHoc);
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phòng học để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataPhongHoc.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataPhongHoc.SelectedRows[0].Index;
                DataGridViewRow selectedRow = dataPhongHoc.Rows[selectedRowIndex];
                string maPhongHoc = selectedRow.Cells["MaPhongHoc"].Value.ToString();
                PhongHoc existingPhongHoc = new PhongHoc
                {
                    MaPhongHoc = maPhongHoc,
                    TenPhongHoc = txtTenP.Text,
                    SoLuongToiDa = Convert.ToInt32(txtSl.Text),
                    SoLuongDaDangKy = Convert.ToInt32(txtSlDk.Text),
                    TrangThai = txtTT.Text
                };

                phongHocProcessor.SuaPhongHoc(existingPhongHoc);
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phòng học để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTk.Text.Trim();
            List<PhongHoc> ketQuaTimKiem = phongHocProcessor.TimKiemPhongHoc(tuKhoa);
            dataPhongHoc.DataSource = ketQuaTimKiem;
        }
        private void ClearInputFields()
        {
            txtMa.Clear();
            txtTenP.Clear();
            txtSl.Clear();
            txtSlDk.Clear();
            txtTT.Clear();
        }
    }
}
