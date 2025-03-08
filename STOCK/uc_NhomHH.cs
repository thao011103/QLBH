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
    public partial class uc_NhomHH : UserControl
    {
        public uc_NhomHH()
        {
            InitializeComponent();
        }
        BussinessLayer.NHOMHH _nhomhh;
        bool _them;
        string _manhom;

        private void uc_NhomHH_Load(object sender, EventArgs e)
        {
            _nhomhh = new BussinessLayer.NHOMHH();
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
        }
        void _reset()
        {
            txtMa.Text = "";
            txtTen.Text = "";
        }
        void loadData()
        {
            gcDanhSach.DataSource = _nhomhh.getAll();
            gvDanhSach.OptionsBehavior.Editable = false;
        }
        private void btnThem_Click_1(object sender, EventArgs e)
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
            if (!string.IsNullOrEmpty(_manhom))
            {
                if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _nhomhh.delete(_manhom);
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
                tb_NHOMHH nhomhh = new tb_NHOMHH();
                nhomhh.MANHOM = txtMa.Text;
                nhomhh.TENNHOM = txtTen.Text;
                _nhomhh.add(nhomhh);
            }
            else
            {
                tb_NHOMHH nhomhh = _nhomhh.getItem(_manhom);
                nhomhh.TENNHOM = txtTen.Text;
                _nhomhh.update(nhomhh);
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
                _manhom = gvDanhSach.GetFocusedRowCellValue("MANHOM").ToString();
                txtMa.Text = gvDanhSach.GetFocusedRowCellValue("MANHOM").ToString();
                txtTen.Text = gvDanhSach.GetFocusedRowCellValue("TENNHOM").ToString();

            }
        }
    }
}
