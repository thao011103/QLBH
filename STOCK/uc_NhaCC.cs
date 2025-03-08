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
    public partial class uc_NhaCC : UserControl
    {
        public uc_NhaCC()
        {
            InitializeComponent();
        }
        BussinessLayer.NHACC _nhacc;
        bool _them;
        string _mancc;

        private void uc_NhaCC_Load(object sender, EventArgs e)
        {
            _nhacc = new BussinessLayer.NHACC();
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
            txtDienThoai.Enabled = t;
            txtEmail.Enabled = t;
            txtDiaChi.Enabled = t;
            txtGhiChu.Enabled = t;
            txtMST.Enabled = t; // Bật/tắt textbox MASOTHUE
        }

        void _reset()
        {
            txtMa.Text = "";
            txtTen.Text = "";
            txtDienThoai.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";
            txtGhiChu.Text = "";
            txtMST.Text = ""; // Đặt lại textbox MASOTHUE
        }

        void loadData()
        {
            gcDanhSach.DataSource = _nhacc.getAll();
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
            if (!string.IsNullOrEmpty(_mancc))
            {
                if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _nhacc.delete(_mancc);
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
                tb_NHACC ncc = new tb_NHACC();
                ncc.MANCC = txtMa.Text;
                ncc.TENNCC = txtTen.Text;
                ncc.DIACHI = txtDiaChi.Text;
                ncc.SDT = txtDienThoai.Text;
                ncc.EMAIL = txtEmail.Text;
                ncc.GHI_CHU = txtGhiChu.Text;
                ncc.MASOTHUE = txtMST.Text; // Thêm MASOTHUE
                _nhacc.add(ncc);
            }
            else
            {
                tb_NHACC ncc = _nhacc.getItem(_mancc);
                ncc.TENNCC = txtTen.Text;
                ncc.DIACHI = txtDiaChi.Text;
                ncc.SDT = txtDienThoai.Text;
                ncc.EMAIL = txtEmail.Text;
                ncc.GHI_CHU = txtGhiChu.Text;
                ncc.MASOTHUE = txtMST.Text; // Cập nhật MASOTHUE
                _nhacc.update(ncc);
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
                _mancc = gvDanhSach.GetFocusedRowCellValue("MANCC")?.ToString(); // Sử dụng toán tử null conditional
                txtMa.Text = gvDanhSach.GetFocusedRowCellValue("MANCC")?.ToString();
                txtTen.Text = gvDanhSach.GetFocusedRowCellValue("TENNCC")?.ToString();
                txtDienThoai.Text = gvDanhSach.GetFocusedRowCellValue("SDT")?.ToString();
                txtEmail.Text = gvDanhSach.GetFocusedRowCellValue("EMAIL")?.ToString();
                txtDiaChi.Text = gvDanhSach.GetFocusedRowCellValue("DIACHI")?.ToString();
                txtGhiChu.Text = gvDanhSach.GetFocusedRowCellValue("GHI_CHU")?.ToString();
                txtMST.Text = gvDanhSach.GetFocusedRowCellValue("MASOTHUE")?.ToString();
            }
        }
    }
}
