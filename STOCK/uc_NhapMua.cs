using BussinessLayer;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using DataLayer;
using DevExpress.XtraGrid;
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
    public partial class uc_NhapMua : UserControl
    {
        private Timer _timer;
        private string userRole;

        public uc_NhapMua(string role)

        {

            InitializeComponent();
            userRole = role;
            //_timer = new Timer();
            //_timer.Interval = 5000; // 5000 ms = 5 seconds
            //_timer.Tick += Timer_Tick; // Gán sự kiện cho Timer
        }
        bool _them;
        bool _sua = false;
        string err = "";
        NHACC _nhaCC;
        HANGHOA _hanghoa;
        NHANVIEN _nhanvien;
        PHIEUNHAP _phieunhap;
        CT_PHIEUNHAP _ctphieunhap;
        BindingSource _bdPhieuNhapCT;
        BindingSource _bdPhieuNhap;
        List<tb_PHIEUNHAP> _lstPhieuNhap;
        string _mapn;
        List<string> _lstMahh;

    
        
        private void uc_NhapMua_Load(object sender, EventArgs e)
        {
            if (userRole == "Nhân viên")
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                gvChiTiet.OptionsBehavior.Editable = false;
                contextMenuChiTiet.Enabled = false;
            }

            _nhaCC = new NHACC();
            _hanghoa = new HANGHOA();
            _nhanvien = new NHANVIEN();
            _phieunhap = new PHIEUNHAP();
            _ctphieunhap = new CT_PHIEUNHAP();
            _bdPhieuNhap = new BindingSource();
            _bdPhieuNhapCT = new BindingSource();
            _bdPhieuNhap.PositionChanged += _bdPhieuNhap_PositionChanged;
            _lstPhieuNhap = new List<tb_PHIEUNHAP>();
            _lstMahh = new List<string>();
            _lstPhieuNhap = _phieunhap.getAll();
            _bdPhieuNhap.DataSource = _lstPhieuNhap;
            gcDanhSach.DataSource = _bdPhieuNhap;
            gvDanhSach.OptionsBehavior.Editable = false;
            showHideControl(true);
            _enable(false);
            loadNhaCC();
            loadNhanVien();



            gvChiTiet.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            _bdPhieuNhap.ResetBindings(false);
            xuatThongTin();
            gcChiTiet.ContextMenuStrip = contextMenuChiTiet;
            contextMenuChiTiet.Enabled = false;
            ToolStripMenuItem deleteRowMenuItem = new ToolStripMenuItem("Xóa dòng");
            deleteRowMenuItem.Click += DeleteRowMenuItem_Click;

            // Khai báo và khởi tạo ToolStripMenuItem cho "Xóa tất cả"
            ToolStripMenuItem deleteAllRowsMenuItem = new ToolStripMenuItem("Xóa tất cả");
            deleteAllRowsMenuItem.Click += DeleteAllRowsMenuItem_Click;

            // Thêm ToolStripMenuItems vào contextMenuChiTiet
            contextMenuChiTiet.Items.Add(deleteRowMenuItem);
            contextMenuChiTiet.Items.Add(deleteAllRowsMenuItem);

            gvChiTiet.OptionsView.ShowFooter = true;
          //  _timer.Start();

        }
       
        private void DeleteRowMenuItem_Click(object sender, EventArgs e)
        {
            if (gvChiTiet.FocusedRowHandle >= 0)
            {
                // Xóa dòng được chọn
                gvChiTiet.DeleteRow(gvChiTiet.FocusedRowHandle);
            }
        }

        private void DeleteAllRowsMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = gvChiTiet.RowCount - 1; i >= 0; i--)
            {
                gvChiTiet.DeleteRow(i);
            }
        }

        private void _bdPhieuNhap_PositionChanged(object sender, EventArgs e)
        {
            if (!_them)
            {
                xuatThongTin();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            loadNhaCC(); // Cập nhật danh sách Nhà cung cấp
            loadNhanVien(); // Cập nhật danh sách Nhân viên
        }
        void loadNhaCC()
        {
            cboNhaCC.DataSource = _nhaCC.getAll();
            cboNhaCC.DisplayMember = "TENNCC";
            cboNhaCC.ValueMember = "MANCC";
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
            txtMaPN.Enabled = t;
            txtGhiChu.Enabled = t;
            cboNhanVien.Enabled = t;
            cboNhaCC.Enabled = t;
            dtNgayNhap.Enabled = t;
        }
        void _reset()
        {
            txtMaPN.Text = "";
            txtGhiChu.Text = "";
        }
        void xuatThongTin()
        {
            tb_PHIEUNHAP current = (tb_PHIEUNHAP)_bdPhieuNhap.Current;
            if (current != null)
            {
                // Kiểm tra NGAYNHAP có null không trước khi gán
                if (current.NGAYNHAP.HasValue)
                {
                    dtNgayNhap.Value = current.NGAYNHAP.Value;
                }
                else
                {
                    // Nếu NGAYNHAP là null, có thể gán giá trị mặc định hoặc ngày hôm nay
                    dtNgayNhap.Value = DateTime.Today;
                }

                txtMaPN.Text = current.MAPN.ToString();
                txtGhiChu.Text = current.GHICHU;

                if (!string.IsNullOrEmpty(current.MANCC))
                {
                    cboNhaCC.SelectedValue = current.MANCC;
                }
                if (!string.IsNullOrEmpty(current.MANV))
                {
                    cboNhanVien.SelectedValue = current.MANV;
                }

                _bdPhieuNhapCT.DataSource = _ctphieunhap.getListByKhoaFull(current.MAPN);
                gcChiTiet.DataSource = _bdPhieuNhapCT;

                for (int i = 0; i < gvChiTiet.RowCount; i++)
                {
                    gvChiTiet.SetRowCellValue(i, "STT", i + 1);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            _bdPhieuNhapCT.DataSource = _ctphieunhap.getListByKhoaFull(_mapn);
            gcChiTiet.DataSource = _bdPhieuNhapCT;
            gvChiTiet.AddNewRow();
            tabChungTu.SelectedTabPage = pageChiTiet;
            gvChiTiet.OptionsBehavior.Editable = true;
            contextMenuChiTiet.Enabled = true;
            _them = true;
            _sua = false;
            showHideControl(false);
            _enable(true);
            _reset();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            tb_PHIEUNHAP current = (tb_PHIEUNHAP)_bdPhieuNhap.Current;

            if (current == null)
            {
                MessageBox.Show("Không có phiếu nhập nào để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _them = false;
            _sua = true;
            showHideControl(false);
            _enable(true);

            tabChungTu.SelectedTabPage = pageChiTiet;
            tabChungTu.TabPages[0].PageEnabled = false;

            gvChiTiet.OptionsBehavior.Editable = true;
            contextMenuChiTiet.Enabled = true;

            // Hiển thị lại thông tin phiếu nhập
            xuatThongTin();
        }
        private string GenerateMAPN()
        {
            // Lấy ngày hiện tại
            string currentDate = DateTime.Now.ToString("ddMMyyyy");

            // Lấy số thứ tự lớn nhất của ngày hiện tại
            var lastPN = _lstPhieuNhap
                .Where(x => x.MAPN.StartsWith("PN" + currentDate))
                .OrderByDescending(x => x.MAPN)
                .FirstOrDefault();

            // Tính số thứ tự mới
            int nextNumber = 1; // Mặc định là 001 nếu không có phiếu nào
            if (lastPN != null)
            {
                string lastNumber = lastPN.MAPN.Substring(10); // Lấy 3 ký tự cuối cùng
                nextNumber = int.Parse(lastNumber) + 1;
            }

            // Tạo mã phiếu nhập
            return $"PN{currentDate}{nextNumber:000}"; // Định dạng số thứ tự thành 3 chữ số
        }
        void PhieuNhap_Info(tb_PHIEUNHAP phieunhap)
        {
            double _TONGCONG = 0;
            int _TONGSOLUONG = 0; // Khởi tạo biến tổng số lượng

            if (_them)
            {
                phieunhap.MAPN = GenerateMAPN();
                phieunhap.NGAYNHAP = DateTime.Now;
                phieunhap.MANV = cboNhanVien.SelectedValue?.ToString(); // Kiểm tra null trước khi sử dụng
            }
            phieunhap.GHICHU = txtGhiChu.Text;
            phieunhap.MANCC = cboNhaCC.Text;

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

            phieunhap.TONGTIEN = _TONGCONG;
            phieunhap.SOLUONG = _TONGSOLUONG; // Gán tổng số lượng vào phiếu nhập
            phieunhap.NGAYNHAP = DateTime.Now; // Đã gán ngay khi thêm mới ở trên, có thể không cần thiết
            phieunhap.MANV = cboNhanVien.SelectedValue?.ToString();
            phieunhap.MANCC = cboNhaCC.SelectedValue?.ToString();
        }
        void PhieuNhapCT_Info(tb_PHIEUNHAP phieunhap)
        {
            _ctphieunhap.deleteAll(phieunhap.MAPN);
            for (int i = 0; i < gvChiTiet.RowCount; i++)
            {
                if (gvChiTiet.GetRowCellValue(i, "TENHH") == null ||
                    string.IsNullOrWhiteSpace(gvChiTiet.GetRowCellValue(i, "TENHH").ToString()))
                {
                    gvChiTiet.DeleteRow(i);
                    continue;
                }

                tb_CT_PHIEUNHAP _ct = new tb_CT_PHIEUNHAP();
                string stt = (i + 1).ToString("D3"); // Số thứ tự 3 chữ số (001, 002,...)
                _ct.MA_CT_PN = $"CT{phieunhap.MAPN}{stt}"; // Tạo mã chi tiết phiếu nhập
                _ct.MAPN = phieunhap.MAPN;
                _ct.STT = i + 1;
                _ct.MAHH = gvChiTiet.GetRowCellValue(i, "MAHH")?.ToString();
                _ct.TENHH = gvChiTiet.GetRowCellValue(i, "TENHH")?.ToString();
                _ct.DVT = gvChiTiet.GetRowCellValue(i, "DVT")?.ToString(); // Lấy đơn vị tính
                _ct.SOLUONGCT = int.Parse(gvChiTiet.GetRowCellValue(i, "SOLUONGCT").ToString());
                _ct.GIANHAP = double.Parse(gvChiTiet.GetRowCellValue(i, "GIANHAP").ToString());
                _ct.THANHTIEN = double.Parse(gvChiTiet.GetRowCellValue(i, "THANHTIEN").ToString());

                _ctphieunhap.add(_ct);
            }
        }

        private void luuthongtin()
        {
           // DateTime selectedDate = dtNgayNhap.Value;
            err = "";
            tb_PHIEUNHAP pn;
            // Kiểm tra chi tiết phiếu nhập có dữ liệu
            if (gvChiTiet.RowCount == 0)
            {
                err += "Chi tiết phiếu nhập không được rỗng. \r\n ";
                MessageBox.Show("Chi tiết phiếu nhập không được rỗng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (gvChiTiet.RowCount == 1 && (gvChiTiet.GetRowCellValue(0, "MAHH") == null || string.IsNullOrEmpty(gvChiTiet.GetRowCellValue(0, "MAHH").ToString())))
            {
                err += "Chi tiết phiếu nhập không được rỗng. \r\n ";
                MessageBox.Show("Chi tiết phiếu nhập không được rỗng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lưu thông tin phiếu nhập
 
            if (_them)
            {
                pn = new tb_PHIEUNHAP();
                PhieuNhap_Info(pn); // Gán thông tin phiếu nhập từ form
                _phieunhap.add(pn); // Thêm phiếu nhập mới vào cơ sở dữ liệu
                PhieuNhapCT_Info(pn); // Lưu chi tiết phiếu nhập liên quan
                _bdPhieuNhap.Add(pn);
                _bdPhieuNhap.MoveLast();
            }
            else
            {
                pn = (tb_PHIEUNHAP)_bdPhieuNhap.Current;
                pn = _phieunhap.getItem(pn.MAPN); // Lấy đối tượng phiếu nhập hiện tại
                PhieuNhap_Info(pn); // Gán lại thông tin mới từ form
                _phieunhap.update(pn); // Cập nhật phiếu nhập trong cơ sở dữ liệu
                PhieuNhapCT_Info(pn); // Lưu chi tiết phiếu nhập liên quan

                // Cập nhật lại danh sách phiếu nhập sau khi sửa
                _lstPhieuNhap = _phieunhap.getAll(); // Lấy lại danh sách phiếu nhập
                _bdPhieuNhap.DataSource = _lstPhieuNhap;
                gvDanhSach.RefreshData();
                var obj = _lstPhieuNhap.Find(c => c.MAPN == pn.MAPN);
                if (obj != null)
                {
                    _bdPhieuNhap.Position = _lstPhieuNhap.IndexOf(obj);
                }
            }

            // Cập nhật giao diện
            xuatThongTin();
            _them = false;
            tabChungTu.SelectedTabPage = pageDanhSach;
            MessageBox.Show("Lưu thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
          //  UpdateDataSource();
        }

        private void gvChiTiet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "MAHH")
            {
                if (gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "MAHH").ToString().IndexOf(".") == 0)
                {
                    frmDanhMucNhap _popDM = new frmDanhMucNhap(gvChiTiet, gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "MAHH").ToString());
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
                                    gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "DVT", hh.DVT);
                                    gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1);
                                    gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "GIANHAP", hh.GIANHAP);
                                    gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "THANHTIEN", hh.GIANHAP);
                                    gvChiTiet.UpdateTotalSummary();
                                }
                            }
                            else
                            {
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "TENHH", hh.TENHH);
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "DVT", hh.DVT);
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1);
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "GIANHAP", hh.GIANHAP);
                                gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "THANHTIEN", hh.GIANHAP);
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
                if (gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "TENHH") != null)
                {
                    double _soluongct = double.Parse(e.Value.ToString());
                    if (_soluongct != 0)
                    {
                        tb_HANGHOA hh = _hanghoa.getItem(gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "MAHH").ToString());
                        if (gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "GIANHAP") != null)
                        {
                            double _trigiaTT = double.Parse(gvChiTiet.GetRowCellValue(gvChiTiet.FocusedRowHandle, "GIANHAP").ToString());
                            gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "THANHTIEN", _trigiaTT * _soluongct);
                        }
                        else
                        {
                            gvChiTiet.SetRowCellValue(gvChiTiet.FocusedRowHandle, "THANHTIEN", 0);
                        }
                        gvChiTiet.UpdateTotalSummary();
                    }
                    else
                    {
                        MessageBox.Show("Số lượng không thể bằng 0", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    return;
                }
                gvChiTiet.RefreshData();
            }

            // Sau khi xử lý xong dữ liệu và tính toán "THANHTIEN", gọi hàm TinhTongTien để cập nhật tổng tiền
            TinhTongTien();
        }

        // Hàm tính tổng tiền
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn hủy phiếu nhập này?", "Xác nhân", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                tb_PHIEUNHAP current = (tb_PHIEUNHAP)_bdPhieuNhap.Current;
                int index = _bdPhieuNhap.Position;
                _phieunhap.delete(current.MAPN); // Gọi phương thức delete chỉ với một tham số 

                _lstPhieuNhap = _phieunhap.getAll(); // Lấy danh sách phiếu nhập sau khi xóa
                _bdPhieuNhap.DataSource = _lstPhieuNhap; // Cập nhật nguồn dữ liệu cho BindingSource
                gcDanhSach.DataSource = _bdPhieuNhap; // Cập nhật nguồn dữ liệu cho GridControl
                gvDanhSach.RefreshData(); // Làm mới dữ liệu trên GridView
                gvDanhSach.MoveBy(index);
            }
            else
                return;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Lưu thông tin phiếu nhập
                luuthongtin();

                _them = false;
                _sua = false;

                gvChiTiet.OptionsBehavior.Editable = false;
                contextMenuChiTiet.Enabled = false;

                tabChungTu.TabPages[0].PageEnabled = true;

                showHideControl(true);
                _enable(false);

                // Cập nhật lại danh sách phiếu nhập sau khi lưu
                _lstPhieuNhap = _phieunhap.getAll(); // Lấy danh sách phiếu nhập từ cơ sở dữ liệu
                _bdPhieuNhap.DataSource = _lstPhieuNhap; // Gán lại vào BindingSource
                gcDanhSach.DataSource = _bdPhieuNhap; // Cập nhật GridControl
                gvDanhSach.RefreshData();

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi lưu thông tin: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        }

   
    }
}