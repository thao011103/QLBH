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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics; // Để mở file PDF sau khi tạo xong

namespace STOCK
{
    public partial class uc_Xuat : UserControl
    {
        private string userRole;
        private Entities db;
        public uc_Xuat(string role)
        {
            InitializeComponent();
            userRole = role;
            db = Entities.CreateEntities();
        }
        bool _them;
        bool _sua = false;
        string err = "";
        KHACHHANG _khachhang;
        HANGHOA _hanghoa;
        NHANVIEN _nhanvien;
        HOADON _hoadon;
        CT_HOADON _cthoadon;
        BindingSource _bdHoaDonCT;
        BindingSource _bdHoaDon;
        List<tb_HOADON> _lstHoaDon;
        string _mahd;
        List<string> _lstMahh;
        private void uc_Xuat_Load(object sender, EventArgs e)
        {
            if (userRole == "Nhân viên")
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                gvChiTiet.OptionsBehavior.Editable = false;
                contextMenuChiTiet.Enabled = false;
            }
            _khachhang = new KHACHHANG();
            _hanghoa = new HANGHOA();
            _nhanvien = new NHANVIEN();
            _hoadon = new HOADON();
            _cthoadon = new CT_HOADON();
            _bdHoaDon = new BindingSource();
            _bdHoaDonCT = new BindingSource();
            _bdHoaDon.PositionChanged += _bdHoaDon_PositionChanged;
            _lstHoaDon = new List<tb_HOADON>();
            _lstMahh = new List<string>();
            _lstHoaDon = _hoadon.getAll();
            _lstHoaDon = _lstHoaDon.OrderByDescending(hd => hd.NGAYHD).ToList();
            _bdHoaDon.DataSource = _lstHoaDon;
            gcDanhSach.DataSource = _bdHoaDon;
            gvDanhSach.OptionsBehavior.Editable = false;
            showHideControl(true);
            _enable(false);
            loadKhachHang();
            loadNhanVien();
            gvChiTiet.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            _bdHoaDon.ResetBindings(false);
            xuatThongTin();
            gcChiTiet.ContextMenuStrip = contextMenuChiTiet;
            contextMenuChiTiet.Enabled = false;
            ToolStripMenuItem deleteRowMenuItem = new ToolStripMenuItem("Xóa dòng");
            deleteRowMenuItem.Click += deleteRowMenuItem_Click;

            // Khai báo và khởi tạo ToolStripMenuItem cho "Xóa tất cả"
            ToolStripMenuItem deleteAllRowsMenuItem = new ToolStripMenuItem("Xóa tất cả");
            deleteAllRowsMenuItem.Click += deleteAllRowsMenuItem_Click;

            // Thêm ToolStripMenuItems vào contextMenuChiTiet
            contextMenuChiTiet.Items.Add(deleteRowMenuItem);
            contextMenuChiTiet.Items.Add(deleteAllRowsMenuItem);

        }

        private void _bdHoaDon_PositionChanged(object sender, EventArgs e)
        {
            if (!_them)
            {
                xuatThongTin();
            }
        }

        private void deleteAllRowsMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = gvChiTiet.RowCount - 1; i >= 0; i--)
            {
                gvChiTiet.DeleteRow(i);
            }
        }

        private void deleteRowMenuItem_Click(object sender, EventArgs e)
        {
            if (gvChiTiet.FocusedRowHandle >= 0)
            {
                // Xóa dòng được chọn
                gvChiTiet.DeleteRow(gvChiTiet.FocusedRowHandle);
            }
        }
 
        void loadKhachHang()
        {
            cboKhachHang.DataSource = _khachhang.getAll();
            cboKhachHang.DisplayMember = "TENKH";
            cboKhachHang.ValueMember = "MAKH";
        }
        void loadNhanVien()
        {
            cboNhanVien.DataSource = _nhanvien.getAll();
            cboNhanVien.DisplayMember = "TENNV";
            cboNhanVien.ValueMember = "MANV";
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
           txtMaHD.Enabled = t;
            txtGhiChu.Enabled = t;
            cboNhanVien.Enabled = t;
            cboKhachHang.Enabled = t;
            dtNgayHD.Enabled = t;
        }
        void _reset()
        {
            txtMaHD.Text = "";
            txtGhiChu.Text = "";
        }

        void xuatThongTin()
        {
            tb_HOADON current = (tb_HOADON)_bdHoaDon.Current;
            if (current != null)
            {
                dtNgayHD.Value = current.NGAYHD.Value;
                txtMaHD.Text = current.MAHD.ToString();
                txtGhiChu.Text = current.GHICHU;


                // Kiểm tra nếu MANCC không null trước khi chuyển đổi sang số nguyên
                if (!string.IsNullOrEmpty(current.MAKH))
                {
                    cboKhachHang.SelectedValue = current.MAKH;
                }
                if (!string.IsNullOrEmpty(current.MANV))
                {
                    cboNhanVien.SelectedValue = current.MANV;
                }

                _bdHoaDonCT.DataSource = _cthoadon.getListByKhoaFull(current.MAHD);
                gcChiTiet.DataSource = _bdHoaDonCT;

                for (int i = 0; i < gvChiTiet.RowCount; i++)
                {
                    gvChiTiet.SetRowCellValue(i, "STT", i + 1);
                }
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            btnXuatHD.Visible = false;
            _bdHoaDonCT.DataSource = _cthoadon.getListByKhoaFull(_mahd);
            gcChiTiet.DataSource = _bdHoaDonCT;
            gvChiTiet.AddNewRow();
            tabChungTu.SelectedTabPage = pageChiTiet;
            gvChiTiet.OptionsBehavior.Editable = true;
            contextMenuChiTiet.Enabled = true;
            _them = true;
            _sua = false;
            showHideControl(false);
            _enable(true);
            _reset();
            UpdateDataSource();
        }
        private void UpdateDataSource()
        {
            _lstHoaDon = _hoadon.getAll();
            _lstHoaDon = _lstHoaDon.OrderByDescending(hd => hd.NGAYHD).ToList(); // Sắp xếp lại
            _bdHoaDon.DataSource = _lstHoaDon; // Cập nhật BindingSource
            _bdHoaDon.ResetBindings(false); // Đặt lại bindings
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn hủy phiếu nhập này?", "Xác nhân", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                tb_HOADON current = (tb_HOADON)_bdHoaDon.Current;
                int index = _bdHoaDon.Position;
                _hoadon.delete(current.MAHD); // Gọi phương thức delete chỉ với một tham số 

                _lstHoaDon = _hoadon.getAll(); // Lấy danh sách phiếu nhập sau khi xóa
                _bdHoaDon.DataSource = _lstHoaDon; // Cập nhật nguồn dữ liệu cho BindingSource
                gcDanhSach.DataSource = _bdHoaDon; // Cập nhật nguồn dữ liệu cho GridControl
                gvDanhSach.RefreshData(); // Làm mới dữ liệu trên GridView
                gvDanhSach.MoveBy(index);
            }
            else
                return;
            UpdateDataSource();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnXuatHD.Visible = false;
            tb_HOADON current = (tb_HOADON)_bdHoaDon.Current;
            _them = false;
            _sua = true;
            showHideControl(false);
            _enable(true);
            tabChungTu.SelectedTabPage = pageChiTiet;
            tabChungTu.TabPages[0].PageEnabled = false;
            gvChiTiet.OptionsBehavior.Editable = true;
            contextMenuChiTiet.Enabled = true;
            xuatThongTin();
            UpdateDataSource();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            luuthongtin();
            _them = false;
            _sua = false;
            gvChiTiet.OptionsBehavior.Editable = false;
            contextMenuChiTiet.Enabled = false;
            tabChungTu.TabPages[0].PageEnabled = true;
            showHideControl(true);
            _enable(false);

            _lstHoaDon = _hoadon.getAll(); // Lấy danh sách phiếu nhập sau khi lưu
            _bdHoaDon.DataSource = _lstHoaDon; // Cập nhật nguồn dữ liệu cho BindingSource
            gcDanhSach.DataSource = _bdHoaDon; // Cập nhật nguồn dữ liệu cho GridControl
            gvDanhSach.RefreshData();
            UpdateDataSource();
            btnXuatHD.Visible = true;
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            _them = false;
            _sua = false;
            showHideControl(true);
            _reset();
            _enable(false);
            xuatThongTin();
            tabChungTu.TabPages[0].PageEnabled = true;
            tabChungTu.SelectedTabPage = pageDanhSach;
            gvChiTiet.OptionsBehavior.Editable = false;
            contextMenuChiTiet.Enabled = false;
            UpdateDataSource();
        }
        private string GenerateMAHD()
        {
            // Lấy ngày hiện tại
            string currentDate = DateTime.Now.ToString("ddMMyyyy");

            // Lấy số thứ tự lớn nhất của ngày hiện tại
            var lastHD = db.tb_HOADON
                .Where(x => x.MAHD.StartsWith("HD" + currentDate))
                .OrderByDescending(x => x.MAHD)
                .FirstOrDefault();

            // Tính số thứ tự mới
            int nextNumber = 1; // Mặc định là 001 nếu không có hóa đơn nào
            if (lastHD != null)
            {
                string lastNumber = lastHD.MAHD.Substring(10); // Lấy 3 ký tự cuối cùng
                nextNumber = int.Parse(lastNumber) + 1;
            }

            // Tạo mã hóa đơn
            return $"HD{currentDate}{nextNumber:000}"; // Định dạng số thứ tự thành 3 chữ số
        }

        void HoaDon_Info(tb_HOADON hoadon)
        {
            double _TONGCONG = 0;
            int _TONGSOLUONG = 0; // Khởi tạo biến tổng số lượng

            if (_them)
            {
                hoadon.MAHD = GenerateMAHD();
                hoadon.NGAYHD = DateTime.Now;
                hoadon.MANV = cboNhanVien.SelectedValue?.ToString(); // Kiểm tra null trước khi sử dụng
            }
            hoadon.GHICHU = txtGhiChu.Text;
            hoadon.MAKH = cboKhachHang.Text;

            // Kiểm tra xem gvChiTiet có giá trị không null trước khi sử dụng
            if (gvChiTiet != null)
            {
                // Tạo danh sách các hàng cần xóa
                List<int> rowsToDelete = new List<int>();
                for (int i = 0; i < gvChiTiet.RowCount; i++)
                {
                    if (gvChiTiet.GetRowCellValue(i, "TENHH") == null)
                    {
                        rowsToDelete.Add(i);
                    }
                    else
                    {
                        var thanhtienCellValue = gvChiTiet.GetRowCellValue(i, gvChiTiet.Columns["THANHTIEN"]);
                        var soluongCellValue = gvChiTiet.GetRowCellValue(i, gvChiTiet.Columns["SOLUONGCT"]); // Lấy giá trị số lượng chi tiết
                        if (thanhtienCellValue != null && soluongCellValue != null)
                        {
                            _TONGCONG += double.Parse(thanhtienCellValue.ToString());
                            _TONGSOLUONG += int.Parse(soluongCellValue.ToString()); // Cộng dồn số lượng chi tiết vào tổng số lượng
                        }
                        else
                        {
                            MessageBox.Show("Dòng " + (i + 1) + " có giá trị thành tiền hoặc số lượng là null.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                // Xóa các hàng đã được đánh dấu
                foreach (int rowIndex in rowsToDelete)
                {
                    gvChiTiet.DeleteRow(rowIndex);
                }
            }
            else
            {
                MessageBox.Show("Dữ liệu chi tiết phiếu nhập không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            hoadon.TONGTIEN = _TONGCONG;
            hoadon.SOLUONG = _TONGSOLUONG; // Gán tổng số lượng vào phiếu nhập
            hoadon.NGAYHD = DateTime.Now; // Đã gán ngay khi thêm mới ở trên, có thể không cần thiết
            hoadon.MANV = cboNhanVien.SelectedValue?.ToString();
            hoadon.MAKH = cboKhachHang.SelectedValue?.ToString();
        }
        void HoaDonCT_Info(tb_HOADON hoadon)
        {
            _cthoadon.deleteAll(hoadon.MAHD);
            for (int i = 0; i < gvChiTiet.RowCount; i++)
            {
                if (gvChiTiet.GetRowCellValue(i, "TENHH") == null)
                    gvChiTiet.DeleteRow(i);
                else
                {
                    tb_CT_HOADON _ct = new tb_CT_HOADON();
                    string stt = (i + 1).ToString("D3"); // Tạo số thứ tự 3 chữ số (001, 002,...)
                    _ct.MA_CT_HD = $"CT{hoadon.MAHD}{stt}";
                    _ct.MAHD = hoadon.MAHD;
                    _ct.STT = i + 1;
                    _ct.MAHH = gvChiTiet.GetRowCellValue(i, "MAHH").ToString();
                    _ct.DVT = gvChiTiet.GetRowCellValue(i, "DVT")?.ToString(); // Lấy đơn vị tính
                    _ct.SOLUONGCT = int.Parse(gvChiTiet.GetRowCellValue(i, "SOLUONGCT").ToString());
                    _ct.GIABAN = double.Parse(gvChiTiet.GetRowCellValue(i, "GIABAN").ToString());
                    _ct.THANHTIEN = double.Parse(gvChiTiet.GetRowCellValue(i, "THANHTIEN").ToString());
                    _cthoadon.add(_ct);
                }
            }
        }
        private void luuthongtin()
        {
            err = "";
            tb_HOADON hd;
            if (gvChiTiet.RowCount == 0)
            {
                err += "Chi tiết phiếu nhập không được rỗng. \r\n ";
                MessageBox.Show("Chi tiết phiếu nhập không được rỗng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (gvChiTiet.RowCount == 1 && gvChiTiet.GetRowCellValue(0, "MAHH") == null)
            {
                err += "Chi tiết phiếu nhập không được rỗng. \r\n ";
                MessageBox.Show("Chi tiết phiếu nhập không được rỗng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_them)
            {
                hd = new tb_HOADON();
                HoaDon_Info(hd);
                _hoadon.add(hd); // Gọi phương thức mà không gán kết quả trả về
                HoaDonCT_Info(hd); // Sử dụng đối tượng pn trực tiếp
                _bdHoaDon.Add(hd);
                _bdHoaDon.MoveLast();
            }
            else
            {
                hd = (tb_HOADON)_bdHoaDon.Current;
                hd = _hoadon.getItem(hd.MAHD);
                HoaDon_Info(hd);
                _hoadon.update(hd); // Gọi phương thức mà không gán kết quả trả về
                HoaDonCT_Info(hd); // Sử dụng đối tượng pn trực tiếp

                _lstHoaDon = null;
                _bdHoaDon.DataSource = _lstHoaDon;
                gvDanhSach.ClearSorting();
                gvDanhSach.RefreshData();
                var obj = _bdHoaDon.List.OfType<tb_HOADON>().ToList().Find(c => c.MAHD == hd.MAHD);
                _bdHoaDon.Position = _bdHoaDon.IndexOf(obj);
            }
            xuatThongTin();
            _them = false;
            //tabChungTu.SelectedTabPage = pageDanhSach;
            MessageBox.Show("Lưu thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDataSource();
        }

        private void gvChiTiet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "MAHH")
            {
                if (gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "MAHH").ToString().IndexOf(".") == 0)
                {
                    frmDanhMucBan _popDM = new frmDanhMucBan(gvChiTiet, gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "MAHH").ToString());
                    _popDM.ShowDialog();
                }
                else
                {
                    tb_HANGHOA hh = _hanghoa.getItem(e.Value.ToString());
                    if (hh != null)
                    {
                        if (_hanghoa.checkExist(hh.MAHH))
                        {
                            List<string> s = new List<string>();
                            if (gvChiTiet.RowCount > 1)
                            {
                                for (int i = 0; i < gvChiTiet.RowCount - 1; i++)
                                {
                                    s.Add(gvChiTiet.GetRowCellValue(i, "MAHH").ToString());
                                }
                                if (s.Find(x => x.Equals(e.Value.ToString())) != null)
                                {
                                    MessageBox.Show("Mã này đã có trong lưới nhập liệu.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                else
                                {
                                    gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "TENHH", hh.TENHH);
                                    gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "DVT", hh.DVT); // Đảm bảo lấy DVT từ bảng HANGHOA
                                    gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1);
                                    gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "GIABAN", hh.GIABAN);
                                    gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "THANHTIEN", hh.GIABAN);
                                    gvChiTiet.UpdateTotalSummary();
                                }
                            }
                            else
                            {
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "TENHH", hh.TENHH);
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "DVT", hh.DVT); // Đảm bảo lấy DVT từ bảng HANGHOA
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1);
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "GIABAN", hh.GIABAN);
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "THANHTIEN", hh.GIABAN);
                                gvChiTiet.UpdateTotalSummary();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Mã tài sản này đã được nhập", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã tài sản không chính xác. Kiểm tra lại. ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    gvChiTiet.RefreshData();
                }
            
        }
            if (e.Column.FieldName == "SOLUONGCT")
            {
                // Kiểm tra xem tên hàng hóa có khác null không
                if (gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "TENHH") != null)
                {
                    double _soluongct;

                    // Kiểm tra nếu giá trị nhập vào không null và chuyển đổi sang double
                    if (double.TryParse(e.Value.ToString(), out _soluongct))
                    {
                        // Sử dụng phương thức getItemWithLG_TON để lấy thông tin hàng hóa bao gồm LG_TON
                        var hh = _hanghoa.getItemWithLG_TON(gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "MAHH").ToString());

                        // Kiểm tra nếu hh khác null
                        if (hh != null)
                        {
                            int lgTon = hh.LG_TON; // Sử dụng GetValueOrDefault()

                            // Kiểm tra điều kiện số lượng
                            if (_soluongct <= 0)
                            {
                                MessageBox.Show("Số lượng không hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1); // Đặt lại giá trị ô số lượng về 1
                                return;
                            }
                            else if (_soluongct > lgTon)
                            {
                                MessageBox.Show("Số lượng nhập không được lớn hơn số lượng tồn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1); // Đặt lại giá trị ô số lượng về 1
                                return;
                            }

                            // Kiểm tra và lấy giá bán
                            if (gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "GIABAN") != null)
                            {
                                double _trigiaTT = double.Parse(gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "GIABAN").ToString());

                                // Tính thành tiền
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "THANHTIEN", _trigiaTT * _soluongct);
                            }
                            else
                            {
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "THANHTIEN", 0);
                            }

                            gvChiTiet.UpdateTotalSummary(); // Cập nhật tổng
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin hàng hóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Giá trị không hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1); // Đặt lại giá trị ô số lượng về 1
                        return;
                    }
                }
                else
                {
                    return;
                }

                gvChiTiet.RefreshData(); // Làm mới dữ liệu
            }


            TinhTongTien();
        }
        private void TinhTongTien()
        {
            decimal tongTien = 0;

            // Duyệt qua tất cả các hàng trong GridView
            for (int i = 0; i < gvChiTiet.RowCount; i++)
            {
                // Lấy giá trị của cột "THANHTIEN" trong mỗi hàng
                object value = gvChiTiet.GetRowCellValue(i, "THANHTIEN");

                if (value != null && value != DBNull.Value)
                {
                    // Cộng dồn tổng tiền
                    tongTien += Convert.ToDecimal(value);
                }
            }

            // Hiển thị tổng tiền vào txtTongTien
            txtTongTien.Text = tongTien.ToString("n0");
        }
        private void gvDanhSach_DoubleClick(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                tabChungTu.SelectedTabPage = pageChiTiet;
                xuatThongTin();
                TinhTongTien();
            }
        }

        private void tabChungTu_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (_sua == false && tabChungTu.SelectedTabPage == pageChiTiet)
            {
                gvChiTiet.OptionsBehavior.Editable = false;
            }
        }

        private void gvChiTiet_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!gvChiTiet.IsGroupRow(e.RowHandle))
            {
                if (e.Info.IsRowIndicator)
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1;
                        e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    }
                    SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                    Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                    //  BeginInvoke(new MethodInvoker(delegate { cal(_Width, gcChiTiet); }));
                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));
                SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                //  BeginInvoke(new MethodInvoker(delegate { cal(_Width, gcChiTiet); }));

            }
        }

        private void btnXuatHD_Click(object sender, EventArgs e)
        {
            luuthongtin();

            // Xuất hóa đơn ra file PDF
            XuatHoaDonPDF();
            _them = false;
            _sua = false;
            gvChiTiet.OptionsBehavior.Editable = false;
            contextMenuChiTiet.Enabled = false;
            tabChungTu.TabPages[0].PageEnabled = true;
            showHideControl(true);
            _enable(false);

            // Cập nhật danh sách hóa đơn
            _lstHoaDon = _hoadon.getAll(); // Lấy danh sách hóa đơn sau khi lưu
            _bdHoaDon.DataSource = _lstHoaDon; // Cập nhật nguồn dữ liệu cho BindingSource
            gcDanhSach.DataSource = _bdHoaDon; // Cập nhật nguồn dữ liệu cho GridControl
            gvDanhSach.RefreshData();
        }
        private void XuatHoaDonPDF()
        {
            // Đường dẫn nơi file PDF sẽ được lưu
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HoaDon_" + txtMaHD.Text + ".pdf");

            // Đường dẫn tới file font Roboto
            string robotoRegularPath = @"C:\Users\ADMIN\Downloads\Roboto\Roboto-Regular.ttf";
            string robotoBoldPath = @"C:\Users\ADMIN\Downloads\Roboto\Roboto-Bold.ttf";



            // Tạo document PDF với khổ giấy nhỏ (khổ in nhiệt)
            int baseHeight = 300; // Chiều cao cơ bản cho phần tiêu đề và thông tin hóa đơn
            int rowHeight = 20;   // Chiều cao trung bình của mỗi dòng trong chi tiết hóa đơn
            int totalHeight = baseHeight + (gvChiTiet.RowCount * rowHeight);

            // Tạo document PDF với chiều cao động
            Document doc = new Document(new iTextSharp.text.Rectangle(350, totalHeight), 10f, 10f, 5f, 5f); // 80mm x totalHeight
            
            try
            {
                // Khởi tạo writer để ghi vào PDF
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

                // Mở document để bắt đầu viết
                doc.Open();

                // Nhúng font từ file Roboto
                BaseFont robotoRegular = BaseFont.CreateFont(robotoRegularPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont robotoBold = BaseFont.CreateFont(robotoBoldPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                // Tạo các đối tượng Font với kích thước mong muốn
                iTextSharp.text.Font titleFont = new iTextSharp.text.Font(robotoBold, 18f, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font normalFont = new iTextSharp.text.Font(robotoRegular, 10f, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font shopFont = new iTextSharp.text.Font(robotoBold, 12f, iTextSharp.text.Font.BOLD); // Font cho tên cửa hàng
                iTextSharp.text.Font italicFont = new iTextSharp.text.Font(robotoRegular, 9f, iTextSharp.text.Font.ITALIC);

                doc.Add(new Paragraph("Tip Xinh Cosmetic", shopFont) { Alignment = Element.ALIGN_LEFT });
                doc.Add(new Paragraph("Địa chỉ: Số 01, Ngõ 102, Trần Phú, Mỗ Lao, Hà Đông, Hà Nội", normalFont) { Alignment = Element.ALIGN_LEFT });
              

                // Tạo đối tượng Paragraph cho tiêu đề
                iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("HÓA ĐƠN BÁN HÀNG", titleFont);
                title.Alignment = Element.ALIGN_CENTER;

                // Thêm tiêu đề vào tài liệu
                doc.Add(title);
                doc.Add(new Paragraph(" ")); // Thêm khoảng trắng

                // Thêm thông tin hóa đơn
                doc.Add(new Paragraph("Mã hóa đơn: " + txtMaHD.Text, normalFont));
                doc.Add(new Paragraph("Thời gian: " + dtNgayHD.Value.ToString("dd/MM/yyyy"), normalFont));
                doc.Add(new Paragraph("Nhân viên: " + cboNhanVien.Text, normalFont));
                doc.Add(new Paragraph("Khách hàng: " + cboKhachHang.Text, normalFont));
                doc.Add(new Paragraph("Ghi chú: " + txtGhiChu.Text, normalFont));

            

                doc.Add(new Paragraph("Đơn vị: Đồng", normalFont) { Alignment = Element.ALIGN_RIGHT });
                doc.Add(new Paragraph(" "));
                // Tạo bảng cho chi tiết hóa đơn
                PdfPTable table = new PdfPTable(6); // Số cột
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 10, 35, 10, 10, 20, 20 }); // Thiết lập tỷ lệ cột

                // Thêm tiêu đề các cột
                table.AddCell(new Phrase("STT", normalFont));
                table.AddCell(new Phrase("Tên hàng hóa", normalFont));
                table.AddCell(new Phrase("Đơn vị tính", normalFont));
                table.AddCell(new Phrase("Số lượng", normalFont));
                table.AddCell(new Phrase("Đơn giá", normalFont));
                table.AddCell(new Phrase("Thành tiền", normalFont));

                // Lấy dữ liệu từ gvChiTiet và thêm vào bảng
                for (int i = 0; i < gvChiTiet.RowCount; i++)
                {
                    // Tên hàng hóa
                    var tenhh = gvChiTiet.GetRowCellValue(i, "TENHH")?.ToString() ?? "N/A";

                    // Kiểm tra nếu tên hàng hóa không phải là "N/A"
                    if (tenhh != "N/A")
                    {
                        table.AddCell(new Phrase((i + 1).ToString(), normalFont)); // STT
                        table.AddCell(new Phrase(tenhh, normalFont)); // Tên hàng hóa

                        var dvt = gvChiTiet.GetRowCellValue(i, "DVT")?.ToString() ?? "0";
                        table.AddCell(new Phrase(dvt, normalFont));
                        // Số lượng
                        var soluong = gvChiTiet.GetRowCellValue(i, "SOLUONGCT")?.ToString() ?? "0";
                        table.AddCell(new Phrase(soluong, normalFont));

                        // Đơn giá
                        var giaban = gvChiTiet.GetRowCellValue(i, "GIABAN")?.ToString() ?? "0";
                        var formattedGiaBan = String.Format("{0:N0}", decimal.Parse(giaban)); // Định dạng số
                        PdfPCell giaBanCell = new PdfPCell(new Phrase(formattedGiaBan, normalFont))
                        {
                            HorizontalAlignment = Element.ALIGN_RIGHT // Căn trái
                        };
                        table.AddCell(giaBanCell);

                        // Thành tiền
                        var thanhtien = gvChiTiet.GetRowCellValue(i, "THANHTIEN")?.ToString() ?? "0";
                        var formattedThanhTien = String.Format("{0:N0}", decimal.Parse(thanhtien)); // Định dạng số
                        PdfPCell thanhTienCell = new PdfPCell(new Phrase(formattedThanhTien, normalFont))
                        {
                            HorizontalAlignment = Element.ALIGN_RIGHT // Căn trái
                        };
                        table.AddCell(thanhTienCell);
                    }
                }

                // Thêm bảng vào document
                doc.Add(table);


                // Thêm tổng tiền
                // Lấy tổng tiền từ txtTongTien và thêm vào hóa đơn
                var tongtien = double.Parse(txtTongTien.Text).ToString("N0"); // Thêm dấu phẩy
                doc.Add(new Paragraph(" "));
                PdfPTable totalTable = new PdfPTable(2);
                totalTable.WidthPercentage = 100;
                PdfPCell totalLabelCell = new PdfPCell(new Phrase("Tổng ", new iTextSharp.text.Font(robotoBold, 12f, iTextSharp.text.Font.BOLD)));
                totalLabelCell.Border = PdfPCell.NO_BORDER;
                totalLabelCell.HorizontalAlignment = Element.ALIGN_LEFT;
                totalTable.AddCell(totalLabelCell);
                PdfPCell totalAmountCell = new PdfPCell(new Phrase(tongtien, new iTextSharp.text.Font(robotoBold, 12f, iTextSharp.text.Font.BOLD)));
                totalAmountCell.Border = PdfPCell.NO_BORDER;
                totalAmountCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalTable.AddCell(totalAmountCell);
                doc.Add(totalTable);

                doc.Add(new Paragraph(" ")); // Thêm khoảng trắng

                doc.Add(new Paragraph("Xin cảm ơn quý khách - Hẹn gặp lại!", italicFont) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph("Quý khách vui lòng giữ hóa đơn, hàng được phép đổi", italicFont) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph("trả trong 3 ngày (Điều kiện đủ tem mác nguyên trên sản phẩm)", italicFont) { Alignment = Element.ALIGN_CENTER });
                // Đóng document sau khi hoàn tất
                doc.Close();

                // Mở file PDF sau khi tạo xong
                Process.Start(filePath);

                MessageBox.Show("Hóa đơn đã được xuất ra file PDF!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file PDF: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Đóng document trong trường hợp có lỗi
                if (doc.IsOpen())
                {
                    doc.Close();
                }
            }
        }

    }
}
