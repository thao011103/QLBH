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
    public partial class uc_HangHoa : UserControl
    {
        public uc_HangHoa()
        {
            InitializeComponent();
        }

        private BussinessLayer.HANGHOA _hanghoa;
        private NHACC _nhacc;
        private NHOMHH _nhomhh;

        private bool _them;
        private string _mahh;

        private void uc_HangHoa_Load(object sender, EventArgs e)
        {
            _hanghoa = new BussinessLayer.HANGHOA();
            _nhacc = new NHACC();

            _nhomhh = new NHOMHH();

            loadData();
            loadNhom();
            loadDVT();
            loadNuocSX();
            showHideControl(true);
            _enable(false);
            txtMa.Enabled = false;
        }
        private void loadNuocSX()
        {
            // Ví dụ danh sách quốc gia sản xuất
            var dsNuocSX = new[] {
         new { TenNuoc = "Việt Nam" },
        new { TenNuoc = "Hàn Quốc" },
        new { TenNuoc = "Nhật Bản" },
        new { TenNuoc = "Pháp" },
        new { TenNuoc = "Mỹ" },
        new { TenNuoc = "Đức" },
        new { TenNuoc = "Trung Quốc" }
              };

            // Gán nguồn dữ liệu cho ComboBox
            cboNuocSX.DataSource = dsNuocSX;
            cboNuocSX.DisplayMember = "TenNuoc";  // Hiển thị tên nước
            cboNuocSX.ValueMember = "TenNuoc";    // Giá trị lưu trữ trong ComboBox (có thể dùng Text nếu không cần mã nước)
            //cboNuocSX.SelectedIndex = -1;
        }
        private void loadDVT()
        {
            // Danh sách đơn vị tính (có thể tùy chỉnh)
            var dsDVT = new[] {
        new { DVT = "Cái" },
        new { DVT = "Hộp" },
        new { DVT = "Lọ" },
        new { DVT = "Chai" },
        new { DVT = "Túi" }
    };

            // Gán dữ liệu vào ComboBox
            cboDVT.DataSource = dsDVT;
            cboDVT.DisplayMember = "DVT";
            cboDVT.ValueMember = "DVT";
        }
        private void loadNhom()
        {
            cboNhom.DataSource = _nhomhh.getAll();
            cboNhom.DisplayMember = "TENNHOM";
            cboNhom.ValueMember = "MANHOM";
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
            txtTen.Enabled = t;
            txtMoTa.Enabled = t;
            cboDVT.Enabled = t;
            cboNhom.Enabled = t;
            spGiaNhap.Enabled = t;
            spGiaBan.Enabled = t;
            cboNuocSX.Enabled = t;
        }

        private void _reset()
        {
            txtMa.Text = "";
            txtTen.Text = "";
            spGiaNhap.Value = 0;  // Sử dụng Value cho NumericUpDown
            spGiaBan.Value = 0;    // Sử dụng Value cho NumericUpDown
            txtMoTa.Text = "";
            //cboNuocSX.SelectedIndex = -1;
        }

        private void loadData()
        {
            gcDanhSach.DataSource = _hanghoa.getAll();
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
            if (string.IsNullOrEmpty(_mahh))
            {
                MessageBox.Show("Vui lòng chọn một hàng hóa để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _them = false;
            _enable(true);
            txtMa.Enabled = false;
            showHideControl(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_mahh))
            {
                if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _hanghoa.delete(_mahh);
                    loadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng hóa để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (_hanghoa == null)
            {
                MessageBox.Show("Đối tượng HANGHOA chưa được khởi tạo.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            {
                MessageBox.Show("Mã hàng hóa không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Tên hàng hóa không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboNhom.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhóm hàng hóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(cboDVT.Text))
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo đối tượng hàng hóa
            tb_HANGHOA hh = new tb_HANGHOA
            {
                MAHH = txtMa.Text.Trim(),
                TENHH = txtTen.Text.Trim(),
                MOTA = txtMoTa.Text.Trim(),
                MANHOM = cboNhom.SelectedValue?.ToString(),
                DVT = cboDVT.Text.Trim(),
                NUOCSX = cboNuocSX.SelectedValue?.ToString(),
                GIABAN = (float)spGiaBan.Value,
                GIANHAP = (float)spGiaNhap.Value
            };

            try
            {
                if (_them)
                {
                    // Thêm mới hàng hóa
                    if (_hanghoa.checkExist(hh.MAHH))
                    {
                        MessageBox.Show($"Mã hàng hóa '{hh.MAHH}' đã tồn tại. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _hanghoa.add(hh);
                    MessageBox.Show("Thêm hàng hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Kiểm tra xem hàng hóa có tồn tại không
                    tb_HANGHOA existingHH = _hanghoa.getItem(_mahh);
                    if (existingHH == null)
                    {
                        MessageBox.Show("Không tìm thấy hàng hóa để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Cập nhật thông tin hàng hóa
                    existingHH.TENHH = txtTen.Text.Trim();
                    existingHH.MOTA = txtMoTa.Text.Trim();
                    existingHH.MANHOM = cboNhom.SelectedValue?.ToString();
                    existingHH.DVT = cboDVT.Text.Trim();
                    existingHH.NUOCSX = cboNuocSX.SelectedValue?.ToString();
                    existingHH.GIANHAP = (float)spGiaNhap.Value;
                    existingHH.GIABAN = (float)spGiaBan.Value;

                    _hanghoa.update(existingHH);
                    MessageBox.Show("Cập nhật hàng hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Sau khi lưu xong, reset trạng thái
                _them = false;
                loadData(); // Load lại dữ liệu
                _enable(false); // Vô hiệu hóa các control nhập liệu
                showHideControl(true); // Hiển thị các nút chức năng chính
                txtMa.Enabled = false; // Không cho chỉnh sửa mã hàng hóa
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra trong quá trình lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                // Sử dụng dấu ? để kiểm tra null và đảm bảo việc ép kiểu đúng
                _mahh = gvDanhSach.GetFocusedRowCellValue("MAHH")?.ToString(); // Sử dụng ToString() an toàn
                txtMa.Text = _mahh;
                txtTen.Text = gvDanhSach.GetFocusedRowCellValue("TENHH")?.ToString(); // Sử dụng ToString() an toàn


                // Tương tự kiểm tra MANHOM
                var manhom = gvDanhSach.GetFocusedRowCellValue("MANHOM");
                if (manhom != null)
                {
                    cboNhom.SelectedValue = manhom.ToString(); // Ép kiểu khi chắc chắn không null
                }

                // Tương tự kiểm tra DVT
                var dvt = gvDanhSach.GetFocusedRowCellValue("DVT")?.ToString();
                if (dvt != null)
                {
                    cboDVT.Text = dvt; // Sử dụng Text thay vì SelectedValue
                }


                txtMoTa.Text = gvDanhSach.GetFocusedRowCellValue("MOTA")?.ToString(); // Sử dụng ToString() an toàn
                var nuocsx = gvDanhSach.GetFocusedRowCellValue("NUOCSX")?.ToString();
                if (nuocsx != null)
                {
                    // Gán giá trị NUOCSX vào ComboBox
                    cboNuocSX.SelectedValue = nuocsx; // Sử dụng SelectedValue thay vì Text
                }
                // Sử dụng Convert.ToDecimal() an toàn với giá trị có thể null
                spGiaNhap.Value = Convert.ToDecimal(gvDanhSach.GetFocusedRowCellValue("GIANHAP") ?? 0);
                spGiaBan.Value = Convert.ToDecimal(gvDanhSach.GetFocusedRowCellValue("GIABAN") ?? 0);
            }
        }

        private void txtExport_Click(object sender, EventArgs e)
        {
            // TODO: Implement export functionality if needed
        }

    }
}
