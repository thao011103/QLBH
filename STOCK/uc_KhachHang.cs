using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STOCK
{
    public partial class uc_KhachHang : UserControl
    {
        public uc_KhachHang()
        {
            InitializeComponent();
        }
        BussinessLayer.KHACHHANG _kh;
        bool _them;
        string _makh;
        private void uc_KhachHang_Load(object sender, EventArgs e)
        {
            _kh = new BussinessLayer.KHACHHANG();
            loadData();
            showHideControl(true);
            _enable(false);
            txtMa.Enabled = false;
        }
        void showHideControl(bool t)
        {
            btnThem.Visible = t;
            btnSua.Visible = t;
            btnXoa.Visible = t;
            btnLuu.Visible = !t;
            btnBoQua.Visible = !t;
        }
        void _enable(bool t)
        {
            txtTen.Enabled = t;
            cboGioiTinh.Enabled = t;
            txtDienThoai.Enabled = t;
            txtEmail.Enabled = t;
            txtDienThoai.Enabled = t;
            txtDiaChi.Enabled = t;

        }
        void _reset()
        {
            txtMa.Text = "";
            txtTen.Text = "";
            txtDienThoai.Text = "";
            txtEmail.Text = "";
            cboGioiTinh.Text = "";
            txtDiaChi.Text = "";
        }
        void loadData()
        {
            gcDanhSach.DataSource = _kh.getAll();
            gvDanhSach.OptionsBehavior.Editable = false;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            _them = true;
            txtMa.Enabled = true;
            showHideControl(false);
            _enable(true);
            _reset();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            _them = false;
            _enable(true);
            txtMa.Enabled = false;
            showHideControl(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_makh))
            {
                if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _kh.delete(_makh);
                    loadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (_them)
            {
                tb_KHACHHANG kh = new tb_KHACHHANG();
                kh.MAKH = txtMa.Text;
                kh.TENKH = txtTen.Text;
                kh.DIACHI = txtDiaChi.Text;
                kh.SDT = txtDienThoai.Text;
                kh.EMAIL = txtEmail.Text;
                kh.GIOITINH = cboGioiTinh.Text;
                _kh.add(kh);
            }
            else
            {
                tb_KHACHHANG kh = _kh.getItem(_makh);
                kh.TENKH = txtTen.Text;
                kh.DIACHI = txtDiaChi.Text;
                kh.SDT = txtDienThoai.Text;
                kh.EMAIL = txtEmail.Text;
                kh.GIOITINH = cboGioiTinh.Text;
                _kh.update(kh);
            }
            _them = false;
            loadData();
            _enable(false);
            showHideControl(true);
            txtMa.Enabled = false;
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            _them = false;
            txtMa.Enabled = false;
            _enable(false);
            showHideControl(true);
        }

        private void gvDanhSach_Click(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                _makh = gvDanhSach.GetFocusedRowCellValue("MAKH").ToString();
                txtMa.Text = _makh;
                txtMa.Text = gvDanhSach.GetFocusedRowCellValue("MAKH").ToString();
                txtTen.Text = gvDanhSach.GetFocusedRowCellValue("TENKH").ToString();
                txtDienThoai.Text = gvDanhSach.GetFocusedRowCellValue("SDT").ToString();
                txtEmail.Text = gvDanhSach.GetFocusedRowCellValue("EMAIL").ToString();
                txtDiaChi.Text = gvDanhSach.GetFocusedRowCellValue("DIACHI").ToString();
                cboGioiTinh.Text = gvDanhSach.GetFocusedRowCellValue("GIOITINH").ToString();

            }
        }

        private void btnLichSu_Click(object sender, EventArgs e)
        {
            // Lấy mã khách hàng từ txtMa
            string maKhachHang = txtMa.Text;

            // Kiểm tra nếu txtMa không rỗng thì mở form frmLichSuMuaHang
            if (!string.IsNullOrEmpty(maKhachHang))
            {
                frmLichSuMuaHang frmLichSu = new frmLichSuMuaHang(maKhachHang);
                frmLichSu.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng trước khi xem lịch sử mua hàng.", "Thông báo");
            }
        }
    }
}
