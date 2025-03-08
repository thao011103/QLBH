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
    public partial class frmDanhMucNhap : Form
    {
        public frmDanhMucNhap()
        {
            InitializeComponent();
        }
        public frmDanhMucNhap(DevExpress.XtraGrid.Views.Grid.GridView gvChiTiet, string chuoi)
        {
            InitializeComponent();
            this._chuoi = chuoi;
            this._gvChiTiet = gvChiTiet;
        }
        string _chuoi;
        DevExpress.XtraGrid.Views.Grid.GridView _gvChiTiet;
        CT_PHIEUNHAP _phieunhapct;
        HANGHOA _hanghoa;
        private void frmDanhMuc_Load(object sender, EventArgs e)
        {
            _phieunhapct = new CT_PHIEUNHAP();
            _hanghoa = new HANGHOA();
            if (_chuoi.Trim().Length == 1)
                gcDanhSach.DataSource = _hanghoa.getAll();
            else
                gcDanhSach.DataSource = _hanghoa.getListByKeyword(_chuoi.Substring(1, _chuoi.Length - 1).TrimStart().ToString());
            gvDanhSach.OptionsBehavior.Editable = false;
        }
        void Insert()
        {
            int[] _selectRow = gvDanhSach.GetSelectedRows();
            List<string> _selected = new List<string>();
            foreach (int item in _selectRow)
            {
                _selected.Add(gvDanhSach.GetRowCellValue(item, "MAHH").ToString());
            }

            foreach (string _item in _selected)
            {
                tb_HANGHOA hh = _hanghoa.getItem(_item);

                // Thêm một hàng mới hoặc chỉnh sửa hàng hiện tại
                if (_gvChiTiet.RowCount > 1)
                {
                    AddOrUpdateRow(hh);
                }
                else
                {
                    if (_gvChiTiet.RowCount == 0)
                        _gvChiTiet.AddNewRow();

                    AddOrUpdateRow(hh);
                }
            }

            // Thêm hàng trống mới sau khi hoàn thành
            _gvChiTiet.AddNewRow();
            _gvChiTiet.SelectRow(_gvChiTiet.RowCount - 1);
            _gvChiTiet.DeleteSelectedRows();
            _gvChiTiet.RefreshData();
            this.Close();
        }

        private void AddOrUpdateRow(tb_HANGHOA hh)
        {
            int mautin = _gvChiTiet.RowCount;
            _gvChiTiet.SelectRow(mautin - 1);

            if (_gvChiTiet.GetRowCellValue(_gvChiTiet.FocusedRowHandle, "TENHH") == null)
            {
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "STT", mautin);
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "MAHH", hh.MAHH);
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "TENHH", hh.TENHH);
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "DVT", hh.DVT); // Đặt DVT ở đây
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1);
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "GIANHAP", hh.GIANHAP);
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "THANHTIEN", hh.GIANHAP);
            }
            else
            {
                _gvChiTiet.AddNewRow();
                _gvChiTiet.SelectRow(mautin);
                mautin++;
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "STT", mautin);
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "MAHH", hh.MAHH);
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "TENHH", hh.TENHH);
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "DVT", hh.DVT); // Đặt DVT ở đây
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "SOLUONGCT", 1);
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "GIANHAP", hh.GIANHAP);
                _gvChiTiet.SetRowCellValue(_gvChiTiet.FocusedRowHandle, "THANHTIEN", hh.GIANHAP);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Insert();
        }

        private void gcDanhSach_Click(object sender, EventArgs e)
        {

        }
    }
}