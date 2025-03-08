using BussinessLayer;
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
    public partial class uc_TaiKhoan : UserControl
    {
        private BussinessLayer.TAIKHOAN _taikhoan;
        private NHANVIEN _nhanvien;

        private bool _them;
        private string _matk;
        public uc_TaiKhoan()
        {
            InitializeComponent();
        }


        private void uc_TaiKhoan_Load(object sender, EventArgs e)
        {
            _taikhoan = new BussinessLayer.TAIKHOAN();
            _nhanvien = new NHANVIEN();

            loadData();
            loadNV();

            showHideControl(true);
            _enable(false);
            txtMaTK.Enabled = false;
        }

        private void loadNV()
        {
            cboMaNV.DataSource = _nhanvien.getAll();
            cboMaNV.DisplayMember = "TENNV";
            cboMaNV.ValueMember = "MANV";
        }

        private void showHideControl(bool t)
        {
            btnThem.Visible = t;
            btnSua.Visible = t;
            btnXoa.Visible = t;
            btnLuu.Visible = !t;
            btnBoQua.Visible = !t;
        }

        private void _enable(bool t)
        {
            txtTDN.Enabled = t;
            txtMatKhau.Enabled = t;
            cboVaiTro.Enabled = t;
            cboMaNV.Enabled = t;
        }

        private void _reset()
        {
            txtMaTK.Text = "";
            txtTDN.Text = "";
            txtMatKhau.Text = "";
            cboVaiTro.SelectedIndex = -1;
        }

        private void loadData()
        {
            gcDanhSach.DataSource = _taikhoan.getAll();
            gvDanhSach.OptionsBehavior.Editable = false;
        }



        private void btnThem_Click_1(object sender, EventArgs e)
        {
            _them = true;
            txtMaTK.Enabled = true;
            showHideControl(false);
            _enable(true);
            _reset();
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_matk))
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _them = false;
            _enable(true);
            txtMaTK.Enabled = false;
            showHideControl(false);
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_matk))
            {
                if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _taikhoan.delete(_matk);
                    loadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLuu_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTK.Text) || string.IsNullOrWhiteSpace(txtTDN.Text))
            {
                MessageBox.Show("Mã tài khoản và tên đăng nhập không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tb_TAIKHOAN tk = new tb_TAIKHOAN
            {
                MATK = txtMaTK.Text,
                MANV = cboMaNV.SelectedValue?.ToString(),
                TENDANGNHAP = txtTDN.Text,
                MATKHAU = txtMatKhau.Text,
                VAITRO = cboVaiTro.SelectedItem?.ToString()
            };

            if (_them)
            {
                _taikhoan.add(tk);
            }
            else
            {
                tk = _taikhoan.getItem(_matk);
                if (tk != null)
                {
                    tk.TENDANGNHAP = txtTDN.Text;
                    tk.MATKHAU = txtMatKhau.Text;
                    tk.VAITRO = cboVaiTro.SelectedItem?.ToString();
                    tk.MANV = cboMaNV.SelectedValue?.ToString();
                    _taikhoan.update(tk);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tài khoản để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            _them = false;
            loadData();
            _enable(false);
            showHideControl(true);
            txtMaTK.Enabled = false;
        }

        private void btnBoQua_Click_1(object sender, EventArgs e)
        {
            _them = false;
            txtMaTK.Enabled = false;
            _enable(false);
            showHideControl(true);
        }

        private void gvDanhSach_Click_1(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                _matk = gvDanhSach.GetFocusedRowCellValue("MATK")?.ToString();
                txtMaTK.Text = _matk;
                txtTDN.Text = gvDanhSach.GetFocusedRowCellValue("TENDANGNHAP")?.ToString();
                txtMatKhau.Text = gvDanhSach.GetFocusedRowCellValue("MATKHAU")?.ToString();
                cboVaiTro.SelectedItem = gvDanhSach.GetFocusedRowCellValue("VAITRO")?.ToString();
                cboMaNV.SelectedValue = gvDanhSach.GetFocusedRowCellValue("MANV")?.ToString();
            }
        }
    }
}
