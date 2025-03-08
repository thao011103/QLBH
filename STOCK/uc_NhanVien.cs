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
    public partial class uc_NhanVien : UserControl
    {
        public uc_NhanVien()
        {
            InitializeComponent();
        }
        BussinessLayer.NHANVIEN _nv;
        bool _them;
        string _manv;
        private void uc_NhanVien_Load(object sender, EventArgs e)
        {
            _nv = new BussinessLayer.NHANVIEN();
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
            cboGioiTinh.Text="";
            txtDiaChi.Text = "";
        }
        void loadData()
        {
            gcDanhSach.DataSource = _nv.getAll();
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
            if (!string.IsNullOrEmpty(_manv))
            {
                if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _nv.delete(_manv);
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
                // Kiểm tra ràng buộc: Mã và tên không được để trống
                if (string.IsNullOrWhiteSpace(txtMa.Text))
                {
                    MessageBox.Show("Mã nhân viên không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Tên nhân viên không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    // Tạo đối tượng nhân viên mới
                    tb_NHANVIEN nv = new tb_NHANVIEN
                    {
                        MANV = txtMa.Text.Trim(),
                        TENNV = txtTen.Text.Trim(),
                        DIACHI = txtDiaChi.Text.Trim(),
                        SDT = txtDienThoai.Text.Trim(),
                        EMAIL = txtEmail.Text.Trim(),
                        GIOITINH = cboGioiTinh.Text
                    };

                    // Gọi phương thức thêm mới
                    _nv.add(nv);
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Hiển thị thông báo khi mã nhân viên bị trùng hoặc lỗi khác
                    if (ex.Message.Contains("đã tồn tại"))
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Có lỗi xảy ra khi thêm nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                // Kiểm tra ràng buộc: Tên không được để trống
                if (string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Tên nhân viên không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    // Lấy đối tượng nhân viên từ cơ sở dữ liệu
                    tb_NHANVIEN nv = _nv.getItem(_manv);
                    if (nv == null)
                    {
                        MessageBox.Show("Không tìm thấy nhân viên để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Cập nhật thông tin nhân viên
                    nv.TENNV = txtTen.Text.Trim();
                    nv.DIACHI = txtDiaChi.Text.Trim();
                    nv.SDT = txtDienThoai.Text.Trim();
                    nv.EMAIL = txtEmail.Text.Trim();
                    nv.GIOITINH = cboGioiTinh.Text;

                    _nv.update(nv);
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra khi cập nhật nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Sau khi thêm hoặc cập nhật, đặt lại trạng thái
            _them = false;
            loadData(); // Load lại dữ liệu hiển thị
            _enable(false); // Vô hiệu hóa các control nhập liệu
            showHideControl(true); // Hiển thị lại các nút chức năng chính
            txtMa.Enabled = false; // Không cho chỉnh sửa mã nhân viên

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
                _manv = gvDanhSach.GetFocusedRowCellValue("MANV").ToString();
                txtMa.Text = gvDanhSach.GetFocusedRowCellValue("MANV").ToString();
                txtTen.Text = gvDanhSach.GetFocusedRowCellValue("TENNV").ToString();
                txtDienThoai.Text = gvDanhSach.GetFocusedRowCellValue("SDT").ToString();
                txtEmail.Text = gvDanhSach.GetFocusedRowCellValue("EMAIL").ToString();
                txtDiaChi.Text = gvDanhSach.GetFocusedRowCellValue("DIACHI").ToString();
                cboGioiTinh.Text = gvDanhSach.GetFocusedRowCellValue("GIOITINH").ToString();
            }
        }
    }
}
